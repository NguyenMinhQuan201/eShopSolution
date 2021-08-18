using eShopSolution.ViewMoldes.Common;
using eShopSolution.ViewMoldes.System.User;
using System;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.sevice
{
    public interface IUserAPIClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);
        Task<ApiResult<bool>> UpdateUser(Guid Id, UserUpdateRequest request);
        Task<ApiResult<UserVm>> GetById(Guid Id);
        Task<ApiResult<bool>> Delete(Guid Id);

    }
}
