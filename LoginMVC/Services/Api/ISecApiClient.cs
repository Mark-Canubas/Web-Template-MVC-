using System.Threading.Tasks;
using LoginMVC.Models;
using LoginMVC.Models.Api;
using LoginMVC.Models.Dtos;

namespace LoginMVC.Services.Api
{
    public interface ISecApiClient
    {
        Task<LoginResponseDto> LoginAsync(UserModel credentials);
        Task<UserDto> GetMeAsync(string bearerToken);
    }
}