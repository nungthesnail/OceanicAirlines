using System.Text.Json;
using System.Text;
using System.Net;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Connectors
{
    /// <summary>
    /// Базовый класс коннектора микросервиса. Производит создание запросов к микросервису, их отправку и обработку возникающих ошибок
    /// </summary>
    public abstract class Connector
    {
        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly HttpClientHandler _httpHandler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,

            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
        };

        protected static HttpClient _httpClient = new HttpClient(_httpHandler);

        protected InterserviceCommunicator _communicator = null!;
        protected ConnectorSettings _settings = null!;

        protected async Task<HttpResponseMessage> Send(HttpMethod httpMethod, string route, object? data = null)
        {
            var httpRequest = CreateHttpRequestMessage();

            SetHttpMethodToRequest(ref httpRequest, httpMethod);

            BuildUrlAndSetToHttpRequest(ref httpRequest, route);

            SetHttpRequestContentIfNotNull(ref httpRequest, data);

            return await Send(httpRequest);
        }

        private HttpRequestMessage CreateHttpRequestMessage()
        {
            return new HttpRequestMessage();
        }

        private void SetHttpMethodToRequest(ref HttpRequestMessage httpRequest, HttpMethod httpMethod)
        {
            httpRequest.Method = httpMethod;
        }

        private void BuildUrlAndSetToHttpRequest(ref HttpRequestMessage request, string route)
        {
            request.RequestUri = BuildUrl(route);
        }

        private Uri BuildUrl(string route)
        {
            var result = $"http://{_settings.ServiceUrl}/{route}";

            return new Uri(result);
        }

        private void SetHttpRequestContentIfNotNull(ref HttpRequestMessage request, object? data)
        {
            if (data != null)
            {
                request.Content = new StringContent
                (
                    content: JsonSerializer.Serialize(data, jsonOptions),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"
                );
            };
        }

		/// <summary>
		/// Производит отправку межсервисного запроса, обрабатывая требования авторизации
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Результат выполнения запроса</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		private async Task<HttpResponseMessage> Send(HttpRequestMessage request)
        {
            try
            {
                // Установка http-заголовка с токеном авторизации
                SetTokenHeaderToHttpRequest(ref request);

                // Первая попытка отправки запроса
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return response; // Успешное выполнение запроса
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Попытка провести авторизацию и вторую попытку запроса
                    request = DuplicateRequest(request);

                    SetTokenHeaderToHttpRequest(ref request);

                    response = await TryToAuthorizeAndRepeatRequest(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return response; // Успешное выполнение запроса
                    }
                }

                // Выбрасывание исключения в соответствии с http-статусом ответа от вызываемого сервиса в случае ошибки
                await ThrowRequestErrorException(response);

                // Недостижимый выброс исключения для подавления ошибки анализатора
                throw new RequestFailedException();
            }
            catch (HttpRequestException exc)
            {
                throw new InterserviceCommunicationException(exc.Message);
            }
        }
        private void SetTokenHeaderToHttpRequest(ref HttpRequestMessage request)
        {
            var token = _communicator.RequestAuthorizationToken();

            var tokenHeaderKeyValuePair = new KeyValuePair<string, IEnumerable<string>>
            (
                key: "token",
                value: [token]
            );

            _httpClient.DefaultRequestHeaders.Remove("token");
            _httpClient.DefaultRequestHeaders.Add("token", token);
        }

        private HttpRequestMessage DuplicateRequest(HttpRequestMessage request)
        {
            return new HttpRequestMessage()
            {
                RequestUri = request.RequestUri,
                Content = request.Content,
                Method = request.Method
            };
        }

        private async Task<HttpResponseMessage> TryToAuthorizeAndRepeatRequest(HttpRequestMessage request)
        {
            await _communicator.RequestAuthorization();

            SetTokenHeaderToHttpRequest(ref request);

            return await _httpClient.SendAsync(request);
        }

        private async Task ThrowRequestErrorException(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnathorizedException();

                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException();

                case HttpStatusCode.NotFound:
                    throw new NotFoundException();

                case HttpStatusCode.BadRequest:
                    throw new BadRequestException();

                default:
                    throw new RequestFailedException($"Status Code: {response.StatusCode}; Message: {await response.Content.ReadAsStringAsync()};");
            }
        }

        protected async Task<TResult?> DeserializeHttpContent<TResult>(HttpContent content)
        {
            return JsonSerializer.Deserialize<TResult>(await content.ReadAsStringAsync(), jsonOptions);
        }
    }
}
