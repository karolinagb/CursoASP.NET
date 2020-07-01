using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        //O readonly é para definir que essa dependência não seja alterada:
        private readonly SalesWebMVCContext _context;

        //Criando um construtor para que a injeção de dependência possa ocorrer:
        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        //Lista para retornar todos os vendedores do banco de dados:
        public List<Seller> FindAll()
        {
            /*Será acessado toda a base de dados referente a vendedores e converter isso para uma lista:*/
            return _context.Seller.ToList();
        }
    }
}
