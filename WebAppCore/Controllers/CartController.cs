using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Enums;
using WebAppCore.Extensions;
using WebAppCore.Models;
using WebAppCore.Services;
using WebAppCore.SignalR;
using WebAppCore.Utilities.Constants;

namespace WebAppCore.Controllers
{
	public class CartController:Controller
	{
		IProductService _productService;
		IBillService _billService;
		IViewRenderService _viewRenderService;
		IConfiguration _configuration;
		IEmailSender _emailSender;
		private readonly ILogger _logger;
		private IAnnouncementService _announcementService;
		private readonly IHubContext<SignalRHub> _hubContext;
		private readonly IUserService _IUserService;
		private IAnnouncementUserService _announcementUserService;

		public CartController(IProductService productService,
			IEmailSender emailSender,IViewRenderService viewRenderService,
			IConfiguration configuration,IBillService billService,ILogger<CartController> logger,
			IAnnouncementService announcementService,
			IHubContext<SignalRHub> hubContext,
			IUserService IUserService,
			IAnnouncementUserService announcementUserService)
		{
			_productService = productService;
			_billService = billService;
			_viewRenderService = viewRenderService;
			_configuration = configuration;
			_emailSender = emailSender;
			_logger = logger;
			_announcementService = announcementService;
			_hubContext = hubContext;
			_IUserService = IUserService;
			_announcementUserService = announcementUserService;
		}
		[Route("cart.html",Name = "Cart")]
		public IActionResult Index()
		{
			return View();
		}

		[Route("checkout.html",Name = "Checkout")]
		[HttpGet]
		public IActionResult Checkout()
		{
			var model = new CheckoutViewModel();
			var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
			//if(session.Any(x => x.Color == null))
			//{
			//	_logger.LogWarning("Bạn vui lòng chọn màu.");
			//	return Redirect("/cart.html");
			//}

			model.Carts = session;
			return View(model);
		}
		[Route("checkout.html",Name = "Checkout")]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Checkout(CheckoutViewModel model)
		{
			var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

			if(ModelState.IsValid)
			{
				if(session != null)
				{
					var details = new List<BillDetailViewModel>();
					foreach(var item in session)
					{
						details.Add(new BillDetailViewModel() {
							Product = item.Product,
							Price = item.Price,
							//ColorId = item.Color.Id,
							//SizeId = item.Size.Id,
							Quantity = item.Quantity,
							ProductId = item.Product.Id
						});
					}
					var billViewModel = new BillViewModel() {
						CustomerMobile = model.CustomerMobile,
						BillStatus = BillStatus.New,
						CustomerAddress = model.CustomerAddress,
						CustomerName = model.CustomerName,
						CustomerMessage = model.CustomerMessage,
						BillDetails = details,
						DateCreated = DateTime.UtcNow
						//CustomerId = User.Identity.IsAuthenticated == true? ((ClaimsIdentity)User.Identity).GetSpecificClaim("UserId"):     
					};
					if(User.Identity.IsAuthenticated == true)
					{
						billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
					}
					_billService.Create(billViewModel);
					try
					{

						_billService.Save();

						var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail",billViewModel);
						//Send mail
						await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"],"New bill from Panda Shop",content);

						ViewData["Success"] = true;
						HttpContext.Session.Remove(CommonConstants.CartSession);
						//add annoucment
						var user = User.GetSpecificClaim("UserName");
						var userId = Guid.Parse(User.GetSpecificClaim("UserId"));
						var avatar = User.GetSpecificClaim("Avatar");
						var data = await _IUserService.GetUserWithRole("Admin");
						var notificationId = Guid.NewGuid().ToString();
						var userNameOrNew = string.IsNullOrEmpty(user) ? "mới" : user;
						var announcement = new AnnouncementViewModel() {
							Title = $"Đơn hàng từ khác hàng {userNameOrNew}",
							DateCreated = DateTime.UtcNow,
							Content = $"Đơn hàng {DateTime.UtcNow} mới đang chờ xử lý",
							Id = notificationId,
							UserId = User.GetUserId() == Guid.Empty || User.GetUserId() == null ? Guid.Empty : User.GetUserId(),
							Avartar = string.IsNullOrEmpty(avatar) ? "/admin-side/images/user.png" : avatar
						};
						
						await _announcementService.AnnounSendUser(userId,announcement,data);
						foreach(var item in data)
						{
							if(item.Id != userId)
							{
								await _hubContext.Clients.User(item.Id.ToString()).SendAsync("ReceiveMessage",announcement);
							}
						}
					} catch(Exception ex)
					{
						ViewData["Success"] = false;
						ModelState.AddModelError("",ex.Message);
					}

				}
			}
			model.Carts = session;
			return View(model);
		}
		#region AJAX Request
		/// <summary>
		/// Get list item
		/// </summary>
		/// <returns></returns>
		public IActionResult GetCart()
		{
			var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
			if(session == null)
				session = new List<ShoppingCartViewModel>();
			return new OkObjectResult(session);
		}

		/// <summary>
		/// Remove all products in cart
		/// </summary>
		/// <returns></returns>
		public IActionResult ClearCart()
		{
			HttpContext.Session.Remove(CommonConstants.CartSession);
			return new OkObjectResult("OK");
		}

		/// <summary>
		/// Add product to cart
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult AddToCart(int productId,int quantity,int color,int size)
		{
			//Get product detail
			var product = _productService.GetById(productId);

			//Get session with item list from cart
			var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
			if(session != null)
			{
				//Convert string to list object
				bool hasChanged = false;

				//Check exist with item product id
				if(session.Any(x => x.Product.Id == productId))
				{
					foreach(var item in session)
					{
						//Update quantity for product if match product id
						if(item.Product.Id == productId)
						{
							item.Quantity += quantity;
							item.Price = product.PromotionPrice ?? product.Price;
							hasChanged = true;
						}
					}
				}
				else
				{
					session.Add(new ShoppingCartViewModel() {
						Product = product,
						Quantity = quantity,
						Color = _billService.GetColor(color),
						Size = _billService.GetSize(size),
						Price = product.PromotionPrice ?? product.Price
					});
					hasChanged = true;
				}

				//Update back to cart
				if(hasChanged)
				{
					HttpContext.Session.Set(CommonConstants.CartSession,session);
				}
			}
			else
			{
				//Add new cart
				var cart = new List<ShoppingCartViewModel>();
				cart.Add(new ShoppingCartViewModel() {
					Product = product,
					Quantity = quantity,
					Color = _billService.GetColor(color),
					Size = _billService.GetSize(size),
					Price = product.PromotionPrice ?? product.Price
				});
				HttpContext.Session.Set(CommonConstants.CartSession,cart);
			}
			return new OkObjectResult(productId);
		}

		/// <summary>
		/// Remove a product
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		public IActionResult RemoveFromCart(int productId)
		{
			var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
			if(session != null)
			{
				bool hasChanged = false;
				foreach(var item in session)
				{
					if(item.Product.Id == productId)
					{
						session.Remove(item);
						hasChanged = true;
						break;
					}
				}
				if(hasChanged)
				{
					HttpContext.Session.Set(CommonConstants.CartSession,session);
				}
				return new OkObjectResult(productId);
			}
			return new EmptyResult();
		}

		/// <summary>
		/// Update product quantity
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		public IActionResult UpdateCart(int productId,int quantity,int color,int size)
		{
			var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
			if(session != null)
			{
				bool hasChanged = false;
				foreach(var item in session)
				{
					if(item.Product.Id == productId)
					{
						var product = _productService.GetById(productId);
						item.Product = product;
						item.Size = _billService.GetSize(size);
						item.Color = _billService.GetColor(color);
						item.Quantity = quantity;
						item.Price = product.PromotionPrice ?? product.Price;
						hasChanged = true;
					}
				}
				if(hasChanged)
				{
					HttpContext.Session.Set(CommonConstants.CartSession,session);
				}
				return new OkObjectResult(productId);
			}
			return new EmptyResult();
		}

		[HttpGet]
		public IActionResult GetColors()
		{
			var colors = _billService.GetColors();
			return new OkObjectResult(colors);
		}

		[HttpGet]
		public IActionResult GetSizes()
		{
			var sizes = _billService.GetSizes();
			return new OkObjectResult(sizes);
		}
		#endregion
	}
}