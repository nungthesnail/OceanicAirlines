using System.Net.Http.Json;

namespace NetworkClient
{
    public class InterserviceSenderClient
    {
        private static HttpClient _httpClient = new HttpClient();

        private string _apiToken = "";

        public InterserviceSenderClient()
        { }

        public InterserviceSenderClient(string apiToken)
        {
            _apiToken = apiToken;
        }

        public void SetApiToken(string token)
        {
            _apiToken = token;
        }

        public async Task<HttpResponseMessage> MakeGetRequestAsync(string url)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            return await MakeRequestAsync(request);
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(HttpRequestMessage request)
        {
            AddApiTokenHeader(request);

            var result = await SendHttpRequestAsync(request);

            return result;
        }

        private void AddApiTokenHeader(HttpRequestMessage request)
        {
            request.Headers.Add("token", _apiToken);
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> MakePostRequestJsonAsync(string url, object? obj)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            
            AddJsonContent(request, obj);

            return await MakeRequestAsync(request);
        }

        private void AddJsonContent(HttpRequestMessage request, object? obj)
        {
            request.Content = JsonContent.Create(obj);
        }

        public async Task<HttpResponseMessage> MakePutRequestJsonAsync(string url, object? obj)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

            AddJsonContent(request, obj);

            return await MakeRequestAsync(request);
        }

        public async Task<HttpResponseMessage> MakeDeleteRequestAsync(string url)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);

            return await MakeRequestAsync(request);
        }
    }
}
