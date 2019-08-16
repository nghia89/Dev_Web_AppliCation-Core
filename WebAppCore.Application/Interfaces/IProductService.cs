using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize,string sortBy,int? sortPrice);

        Task<PagedResult<ProductViewModel>> PagingAsync(int? categoryId, string keyword, int page, int pageSize,string sortBy);

        ProductViewModel Add(ProductViewModel productVm);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void ImportExcel(string filePath, int categoryId);

        void Save();

        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(int productId);

        void AddImages(int productId, string[] images);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);

        List<ProductViewModel> GetLastest(int top);

        Task<List<ProductViewModel>> GetProductNew(int top);

        Task<List<ProductViewModel>> GetHotProduct(int top);

        Task<List<ProductViewModel>> BuyALotProducts(int top);

        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<ProductViewModel> GetUpsellProducts(int top);

        List<TagViewModel> GetProductTags(int productId);   

        bool CheckAvailability(int productId, int size, int color);

    }
}