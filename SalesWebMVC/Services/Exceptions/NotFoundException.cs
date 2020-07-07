using System;

namespace SalesWebMVC.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        //base para reutilizar o construtor da ApplicationException
        /*Estamos criando uma exceção personalizada porque vamos querer ter exceções específicas da nossa camada de serviço.
         Quando a gente tem essa exceção personalizada a gente tem a possibilidade de tratar exclusivamente essa exceção.*/
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
