using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.Product;

namespace WebAppCore.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm);

        void Update(ProductCategoryViewModel productCategoryVm);

        void Delete(int id);

        Task<List<ProductCategoryViewModel>> GetAll();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        Task<ProductCategoryViewModel> GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);

        Task<List<ProductCategoryViewModel>> GetHomeCategories(int top);

        void Save();
    }
}