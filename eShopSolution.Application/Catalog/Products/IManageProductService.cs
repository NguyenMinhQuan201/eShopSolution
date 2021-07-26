using eShopSolution.ViewMoldes.Catalog.ProductImage;
using eShopSolution.ViewMoldes.Catalog.ProductImages;
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
        Task<ProductViewModle> GetById(int productId,string langugeId);
        Task<bool> UpdatePrice(int ProductId,decimal newPrice);
        Task<bool> UpdateStock(int productId,int addedQuantity);
        Task AddViewCount(int productId);
        Task<PagedResult<ProductViewModle>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> AddImage(int productId, ProductIamgeCreateRequest request);
        Task<int> RemoveImage( int imageId);
        Task<int> UpdateImage(int imageId, ProductIamgeUpdateRequest request);
        Task<List<ProductImageViewModle>> GetListImages(int productId);
        Task<ProductImageViewModle> GetImageByID(int imageId);
    }
}
