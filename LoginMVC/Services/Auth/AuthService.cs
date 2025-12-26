using System;
using System.Threading.Tasks;
using LoginMVC.Models;
using LoginMVC.Models.Api;
using LoginMVC.Models.Dtos;
using LoginMVC.Services.Api;

namespace LoginMVC.Services.Auth
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        Task<LoginResponseDto> LoginAsync(UserModel credentials);
        Task<UserDto> GetUserProfileAsync();
        void Logout();
    }

    public sealed class AuthService : IAuthService
    {
        private const string TokenKey = "token";
        private const string UserKey = "user";

        private readonly ISecApiClient _api;
        private readonly ISessionService _session;

        public AuthService(ISecApiClient api, ISessionService session)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_session.Get<string>(TokenKey));

        public async Task<LoginResponseDto> LoginAsync(UserModel credentials)
        {
            var result = await _api.LoginAsync(credentials).ConfigureAwait(false);

            if (result == null || !result.IsSuccess || string.IsNullOrWhiteSpace(result.Token))
                throw new InvalidOperationException(result?.ErrorMessage ?? "Login failed.");

            _session.Set(TokenKey, result.Token);
            _session.Set(UserKey, result.User);

            return result;
        }

        public async Task<UserDto> GetUserProfileAsync()
        {
            var token = _session.Get<string>(TokenKey);
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var me = await _api.GetMeAsync(token).ConfigureAwait(false);

            if (me != null)
                _session.Set(UserKey, me);

            return me;
        }

        public void Logout()
        {
            _session.Remove(TokenKey);
            _session.Remove(UserKey);
        }
    }
}