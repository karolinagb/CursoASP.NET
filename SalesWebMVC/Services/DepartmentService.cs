using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMVCContext _context;

        public DepartmentService(SalesWebMVCContext context)
        {
            _context = context;
        }

        //Retonando todos os departamentos:
        public async Task<List<Department>> FindAllAsync()
        {
            /*ToList provoca a execução da expressão Lambda, porém precisamos que essa operação seja assíncrona então
             vamos usar o ToListAsync.
            Temos que avisar o compilador que isso é uma chamada assíncrona*/
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
