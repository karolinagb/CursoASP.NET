using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Services.Exceptions;

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

        public void Insert(Seller obj)
        {
            //Pegando o primeiro department do banco e associando com o seller:
            //OBS: O Entity Framework já entende que o que será associado é o ID:
            //obj.Department = _context.Department.First();

            _context.Add(obj);

            //Para confirmar as inserções acima, temos que chamar o savechanges:
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            /*O include faz um join com o departamento do vendedor para mostrarmos na tela:*/
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            //Find e remove são do dbset do context
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            //Testando se o id já existe no banco a fim de ser feito a atualização:
            //Any = serve para falar se existe algum registro no banco com a condição que for colocada nele:
            if (!(_context.Seller.Any(x => x.Id == obj.Id)))
            {
                throw new DllNotFoundException("Id not found");
            }

            /*Quando você chama a operação de atualizar no banco de dados, o banco de dados pode gerar uma exceção de conflito
             de concorrência. Se esse erro ocorrer no banco de dados, o Entity Framework vai gerar uma excessão chamada
            DbUpdateConcurrecyException. Então vamos colocar um bloco try para tentar atualizar. E um bloco catch para capturar
            uma possível exceção do banco de dados:*/

            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            //Se o Entity Framework lançar essa exceção abaixo:
            catch (DbUpdateConcurrencyException e)
            {
                //Nós vamos lançar outra a nível de serviço:
                throw new DbConcurrencyException(e.Message);
            }
            
        }
    }
}
