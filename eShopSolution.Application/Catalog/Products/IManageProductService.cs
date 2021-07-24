using eShopSolution.ViewMoldes.Catalog.Products;
using eShopSolution.ViewMoldes.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    { 
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<bool> UpdatePrice(int ProductId,decimal newPrice);
        Task<bool> UpdateStock(int productId,int addedQuantity);
        Task AddViewCount(int productId);
        Task<PagedResult<ProductViewModle>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> AddImages(int productId, List<IFormFile> files);
        Task<int> RemoveImages(int imageId);
        Task<int> UpdateImages(int imageId, string caption,bool isdefault);
        Task<List<ProductImageViewModle>> GetListImage(int productId);
    }
}
