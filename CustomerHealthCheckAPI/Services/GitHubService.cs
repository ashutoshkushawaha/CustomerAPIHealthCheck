using Azure.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace CustomerHealthCheckAPI.Services
{
    public class GitHubService:IGitHubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GitHubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> IsValidGitHubUser(string username)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GitHub");
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");
                var productInfo = new ProductInfoHeaderValue("myApp", "v1.1");
                client.DefaultRequestHeaders.UserAgent.Add(productInfo);
                var response = await client.GetAsync($"/users/{username}");
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    var responseBody = await response.Content.ReadFromJsonAsync<JsonObject>();
                    var message = responseBody!["message"]!.ToString();
                    throw new HttpRequestException(message);
                }
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
    }
}
