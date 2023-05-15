using System.Security.Claims;
using Mag.Common.Models;

namespace Mag.Common.Interfaces;

public interface IAccountService<T> where T : class
{
    public Task<Response<T>> AddUser(UserRegisterDto model);
    public Task<Response<T>> LoginUser(UserLoginDto model);
    public Task<Response<T>> GetUserDetail(ClaimsPrincipal claimsPrincipal);
    public Task<Response<T>> ExitFromAccount();
    public Task<Response<T>> Edit(UserEditDto model, ClaimsPrincipal claimsPrincipal);
}