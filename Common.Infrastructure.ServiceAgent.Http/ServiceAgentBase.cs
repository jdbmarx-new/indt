using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.ServiceAgent.Http
{
    public abstract class ServiceAgentBase
    {
        protected ILogger Logger { get; }
        protected HttpClient Client { get; }

        private readonly JsonSerializerOptions _serializerOptions;

        protected ServiceAgentBase(ILogger logger, HttpClient client, bool enumAsString = false)
        {
            Logger = logger;
            Client = client;
            _serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true};

            if (enumAsString)
            {
                _serializerOptions.Converters.Add(new JsonStringEnumConverter());
            }
        }

        protected static int ThreadId => Environment.CurrentManagedThreadId;

        protected Task PutAsync(string path, object? requisicao = null, string? operationException = null, Action<HttpResponseMessage, string?>? handleValid = null, Action<HttpResponseMessage>? handleInvalid = null, [CallerMemberName] string? caller = null)
        {
            return PutAsync<object>(path, requisicao, operationException, handleValid is null ? null : (hrm, s) => { handleValid(hrm, s); return null!; }, handleInvalid is null ? null : hrm => { handleInvalid(hrm); return null!; }, caller);
        }

        protected Task<T?> PutAsync<T>(string path, object? requisicao = null, string? operationException = null, Func<HttpResponseMessage, string?, T>? handleValid = null, Func<HttpResponseMessage, T>? handleInvalid = null, [CallerMemberName] string? caller = null)
        {
            return SendAsync(HttpMethod.Put, path, requisicao, operationException, caller, handleValid, handleInvalid);
        }

        protected Task<T?> PostAsync<T>(string path, object? requisicao = null, string? operationException = null, Func<HttpResponseMessage, string?, T>? handleValid = null, Func<HttpResponseMessage, T>? handleInvalid = null, [CallerMemberName] string? caller = null)
        {
            return SendAsync(HttpMethod.Post, path, requisicao, operationException, caller, handleValid, handleInvalid);
        }

        protected Task<T?> GetAsync<T>(string path, string? operationException = null, Func<HttpResponseMessage, string?, T>? handleValid = null, Func<HttpResponseMessage, T>? handleInvalid = null, [CallerMemberName] string? caller = null)
        {
            return SendAsync(HttpMethod.Get, path, null, operationException, caller, handleValid, handleInvalid);
        }

        private async Task<T?> SendAsync<T>(HttpMethod method, string path, object? requisicao, string? operationException, string? caller, Func<HttpResponseMessage, string?, T>? handleValid, Func<HttpResponseMessage, T>? handleInvalid = null)
        {
            string body = string.Empty;
            if (requisicao is not null)
            {
                body = JsonSerializer.Serialize(requisicao, _serializerOptions);
            }

            string logRadical = $"[{GetType().Name}][{caller}][{ThreadId}]";
            LogDebug($"{logRadical} Inicio Requisicao = {body}");

            HttpResponseMessage? respostaHttp = null;

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            string? result = null;
            try
            {
                string headers = string.Join("\n\t", Client.DefaultRequestHeaders.Select(h => $"{h.Key}:'{string.Join("','", h.Value.Select(v => v))}'"));

                Logger?.LogDebug($"{logRadical} Efetuando request:{method.Method} {Client.BaseAddress?.OriginalString}{path}\nHeaders:\n\t{headers}\nBody:\n{body}");

                using var req = new HttpRequestMessage(method, path);
                req.Content = content;
                respostaHttp = await ProcessRequestAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogError($"{logRadical} Ocorreu um erro ao realizar a chamada ao serviço exception = \r\n {ex}", ex);
                throw new InvalidOperationException(operationException);
            }
            finally
            {
                if (respostaHttp is not null)
                {
                    result = await respostaHttp.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

                LogDebug($"{logRadical} Response Status: '{respostaHttp?.StatusCode}'");
                LogDebug($"{logRadical} Response Body recebido: '{result}'");
            }

            T? resposta = default;
            if (respostaHttp is not null && CheckSuccess(respostaHttp))
            {
                if (handleValid is not null)
                {
                    resposta = handleValid(respostaHttp, result);
                }
                else if (!string.IsNullOrWhiteSpace(result))
                {
                    resposta = JsonSerializer.Deserialize<T?>(result, _serializerOptions);
                }
            }
            else
            {
                LogWarning($"{logRadical} Ocorreu um erro ao realizar a chamada ao serviço");
                if (handleInvalid is not null)
                {
                    resposta = handleInvalid(respostaHttp!);
                }
            }

            LogDebug($"{logRadical} Fim = {resposta}");

            respostaHttp?.Dispose();

            return resposta;
        }

        protected virtual async Task<HttpResponseMessage> ProcessRequestAsync(HttpRequestMessage req)
        {
            return await Client.SendAsync(req).ConfigureAwait(false);
        }

        protected virtual bool CheckSuccess(HttpResponseMessage respostaHttp)
        {
            return respostaHttp.IsSuccessStatusCode;
        }

        protected virtual void LogDebug(string message)
        {
            Logger?.LogDebug(message);
        }

        protected virtual void LogWarning(string message)
        {
            Logger?.LogWarning(message);
        }

        protected virtual void LogError(string message, Exception ex)
        {
            Logger?.LogError(ex, message);
        }
    }
}