using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.Catalog.Products
{
    public class ProductImageViewModle
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public bool IsDefault { get; set; }
        public long FileSize { get; set; }
    }
}
