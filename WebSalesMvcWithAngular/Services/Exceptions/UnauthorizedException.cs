using System;

namespace WebSalesMvcWithAngular.Services.Exceptions
{

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Acesso negado. Você não está autenticado!")
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
