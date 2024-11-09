using System;
using System.Net;

namespace BACK_END_PROJECT.API.errors
{
    // Exceção personalizada para BadRequest
    public class BadRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        // Construtor com mensagem de erro e causa opcional
        public BadRequestException(string message, Exception? cause = null)
            : base(message, cause)
        {
            StatusCode = HttpStatusCode.BadRequest; // Define o código de status HTTP como 400
        }
    }
}
