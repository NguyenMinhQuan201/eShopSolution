using eShopSolution.ViewMoldes.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.System.User
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string KeyWord { get; set; }
    }
}
