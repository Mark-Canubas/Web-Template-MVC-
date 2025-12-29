using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LoginMVC.Models;
using LoginMVC.Models.Api;
using LoginMVC.Models.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoginMVC.Services.Api
{
    public sealed class SecApiClient : ISecApiClient
    {
        private static readonly HttpClient Http = new HttpClient();

        private readonly string _baseUrl;

        public SecApiClient()
        {
            _baseUrl = ConfigurationManager.AppSettings["ClearingSettlementApi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(_baseUrl))
                throw new ConfigurationErrorsException("Missing appSetting: ClearingSettlementApi:BaseUrl");

            if (!_baseUrl.EndsWith("/", StringComparison.Ordinal))
                _baseUrl += "/";
        }

        private static string TryExtractApiMessage(string jsonOrText)
        {
            if (string.IsNullOrWhiteSpace(jsonOrText)) return null;

            try
            {
                var obj = JObject.Parse(jsonOrText);
                return (string)obj["message"]
                       ?? (string)obj["error"]
                       ?? (string)obj["errorMessage"];
            }
            catch
            {
                return null;
            }
        }

        public async Task<LoginResponseDto> LoginAsync(UserModel credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var url = _baseUrl + "Auth/login";
            var json = JsonConvert.SerializeObject(new { username = credentials.Username, password = credentials.Password });

            using (var req = new HttpRequestMessage(HttpMethod.Post, url))
            {
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var resp = await Http.SendAsync(req).ConfigureAwait(false))
                {
                    var respJson = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!resp.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Trace.TraceWarning("Login API error ({0}): {1}", (int)resp.StatusCode, respJson);

                        var msg = TryExtractApiMessage(respJson);
                        throw new InvalidOperationException(msg ?? "Login failed. Please verify your credentials and try again.");
                    }

                    return JsonConvert.DeserializeObject<LoginResponseDto>(respJson);
                }
            }
        }

        public async Task<UserDto> GetMeAsync(string bearerToken)
        {
            var url = _baseUrl + "Auth/me";

            using (var req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if (!string.IsNullOrWhiteSpace(bearerToken))
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                using (var resp = await Http.SendAsync(req).ConfigureAwait(false))
                {
                    var respJson = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!resp.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Trace.TraceWarning("Me API error ({0}): {1}", (int)resp.StatusCode, respJson);

                        var msg = TryExtractApiMessage(respJson);
                        throw new InvalidOperationException(msg ?? "Unable to load profile.");
                    }

                    return JsonConvert.DeserializeObject<UserDto>(respJson);
                }
            }
        }
    }
}