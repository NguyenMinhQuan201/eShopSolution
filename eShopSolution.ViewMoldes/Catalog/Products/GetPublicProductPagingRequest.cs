using eShopSolution.ViewMoldes.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        
        public int? CategoryId { get; set; }
    }
}
