﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebAppCore.Areas.Admin.Controllers
{

	public class UploadController:BaseController
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		public UploadController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost]
		public async Task UploadImageForCKEditor(IList<IFormFile> upload,string CKEditorFuncNum,string CKEditor,string langCode)
		{
			DateTime now = DateTime.Now;
			if(upload.Count == 0)
			{
				await HttpContext.Response.WriteAsync("Yêu cầu nhập ảnh");
			}
			else
			{
				var file = upload[0];
				var filename = ContentDispositionHeaderValue
									.Parse(file.ContentDisposition)
									.FileName
									.Trim('"');

				var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

				string folder = _hostingEnvironment.WebRootPath + imageFolder;

				if(!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
				string filePath = Path.Combine(folder,filename);
				using(FileStream fs = System.IO.File.Create(filePath))
				{
					file.CopyTo(fs);
					fs.Flush();
				}
				await HttpContext.Response.WriteAsync("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", '" + Path.Combine(imageFolder,filename).Replace(@"\",@"/") + "');</script>");
			}
		}
		/// <summary>
		/// Upload image for form
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult UploadImage()
		{
			DateTime now = DateTime.Now;
			var files = Request.Form.Files;
			if(files.Count == 0)
			{
				return new BadRequestObjectResult(files);
			}
			else
			{
				var file = files[0];
				var filename = ContentDispositionHeaderValue
									.Parse(file.ContentDisposition)
									.FileName
									.Trim('"');

				var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

				string folder = _hostingEnvironment.WebRootPath + imageFolder;

				if(!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
				var linkFullFile = folder + @"\" + filename;
				if(System.IO.File.Exists(linkFullFile))
				{
					var status = true;
					var fileName = Path.Combine(imageFolder,filename).Replace(@"\",@"/");
					return new OkObjectResult(new { Status = status,FileName = fileName});
				}
				string filePath = Path.Combine(folder,filename);
				using(FileStream fs = System.IO.File.Create(filePath))
				{
					file.CopyTo(fs);
					fs.Flush();
				}
				return new OkObjectResult(Path.Combine(imageFolder,filename).Replace(@"\",@"/"));
			}
		}

		[HttpPost]
		public IActionResult UploadImageSlide()
		{
			DateTime now = DateTime.Now;
			var files = Request.Form.Files;
			if(files.Count == 0)
			{
				return new BadRequestObjectResult(files);
			}
			else
			{
				var file = files[0];
				var filename = ContentDispositionHeaderValue
									.Parse(file.ContentDisposition)
									.FileName
									.Trim('"');

				var imageFolder = $@"\client-side\images\{now.ToString("yyyyMMdd")}";

				string folder = _hostingEnvironment.WebRootPath + imageFolder;

				if(!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
				string filePath = Path.Combine(folder,filename);
				using(FileStream fs = System.IO.File.Create(filePath))
				{
					file.CopyTo(fs);
					fs.Flush();
				}
				return new OkObjectResult(Path.Combine(imageFolder,filename).Replace(@"\",@"/"));
			}
		}

		[HttpPost]
		public IActionResult UploadLogo()
		{
			DateTime now = DateTime.Now;
			var files = Request.Form.Files;
			if(files.Count == 0)
			{
				return new BadRequestObjectResult(files);
			}
			else
			{
				var file = files[0];
				var filename = ContentDispositionHeaderValue
									.Parse(file.ContentDisposition)
									.FileName
									.Trim('"');

				var imageFolder = $@"\client-side\logo\{now.ToString("yyyyMMdd")}";

				string folder = _hostingEnvironment.WebRootPath + imageFolder;

				if(!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
				string filePath = Path.Combine(folder,filename);
				using(FileStream fs = System.IO.File.Create(filePath))
				{
					file.CopyTo(fs);
					fs.Flush();
				}
				return new OkObjectResult(Path.Combine(imageFolder,filename).Replace(@"\",@"/"));
			}
		}
	}
}
