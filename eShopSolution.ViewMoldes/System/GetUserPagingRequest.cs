using eShopSolution.ViewMoldes.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.System
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string KeyWord { get; set; }
    }
}
