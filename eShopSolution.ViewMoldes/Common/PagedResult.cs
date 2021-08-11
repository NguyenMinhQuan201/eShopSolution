using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.Common
{
    public class PagedResult<T> : PageResultBase
    {
        public List<T> Items { get; set; }
        
    }
}
