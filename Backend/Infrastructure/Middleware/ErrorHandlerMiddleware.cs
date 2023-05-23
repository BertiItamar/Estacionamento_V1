using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Chama o próximo middleware na cadeia de execução
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        // Erro personalizado da aplicação
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // Erro de item não encontrado
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // Erro não tratado
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                if (error != null)
                {
                    // Serializa a mensagem de erro para JSON
                    var result = System.Text.Json.JsonSerializer.Serialize(new AppException { Message = error.Message }, new JsonSerializerOptions
                    {
                        IgnoreNullValues = true
                    });

                    await response.WriteAsync(result); // Escreve a resposta com a mensagem de erro serializada em JSON
                }
            }
        }
    }
}
