﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.Catalog.ProductImages
{
    public class ProductIamgeUpdateRequest
    {
        public int Id { get; set; }



        public string Caption { get; set; }

        public bool IsDefault { get; set; }


        public int SortOrder { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
