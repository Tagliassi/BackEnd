using System;
using System.Net;

namespace BACK_END_PROJECT.API.errors
{
    // Exceção personalizada para NotFound
    public class NotFoundException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        // Construtor com mensagem de erro e causa opcional
        public NotFoundException(string message, Exception? cause = null)
            : base(message, cause)
        {
            StatusCode = HttpStatusCode.NotFound; // Define o código de status HTTP como 404
        }
    }
}
