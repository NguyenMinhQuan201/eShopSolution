using eShopSolution.ViewMoldes.Common;
using eShopSolution.ViewMoldes.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.User
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);

        Task<PagedResult<UserVm>> GetUserPaging(GetUserPagingRequest request);
    }
}
