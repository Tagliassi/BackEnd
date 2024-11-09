using System;
using System.Net;

namespace BACK_END_PROJECT.API.errors
{
    // Exceção personalizada para Forbidden
    public class ForbiddenException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        // Construtor com mensagem de erro e causa opcional
        public ForbiddenException(string message = "Forbidden", Exception? cause = null)
            : base(message, cause)
        {
            StatusCode = HttpStatusCode.Forbidden; // Define o código de status HTTP como 403
        }
    }
}
