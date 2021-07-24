using eShopSolution.ViewMoldes.Catalog.Products;
using eShopSolution.ViewMoldes.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModle>> GetAllbyCategoryId(GetPublicProductPagingRequest request);
        Task<List<ProductViewModle>> GetAll();
    }
}
