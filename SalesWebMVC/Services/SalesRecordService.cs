using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        //O readonly é para definir que essa dependência não seja alterada:
        private readonly SalesWebMVCContext _context;

        //Criando um construtor para que a injeção de dependência possa ocorrer:
        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            /*Vai pegar um objeto do tipo DbSet (SalesRecord) e transformar num
             IQueryble para fazer consultas (permite mais opções de consulta):*/
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(X => X.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            /*Para executar a consulta vamos usar o ToList(), mas vamos acrescentar
             mais coisas:*/
            /*Vamos fazer um join da tabela de vendedor com a de departamento e colocar
             em ordem descrescente por data*/
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            /*Vai pegar um objeto do tipo DbSet (SalesRecord) e transformar num
             IQueryble para fazer consultas (permite mais opções de consulta):*/
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(X => X.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            /*Para executar a consulta vamos usar o ToList(), mas vamos acrescentar
             mais coisas:*/
            /*Vamos fazer um join da tabela de vendedor com a de departamento e colocar
             em ordem descrescente por data*/
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
