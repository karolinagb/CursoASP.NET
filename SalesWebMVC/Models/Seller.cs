using System;
using System.Linq;
using System.Collections.Generic;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate  { get; set; }
        public double BaseSalary { get; set; }

        //O seller possui um department:
        public Department Department { get; set; }

        /*Quando crio uma propriedade com o nome da classe e escrito Id no final,
         o framework entende que eu quero guardar o id nessa outra classe e já cria o banco de dados corretamente.
        O tipo int obriga a não ser nulo:*/
        public int DepartmentId { get; set; }

        /*Um seller possui vários SalesRecords*/
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        /*Métodos Customizados*/

        //Adicionar e remover uma venda:
        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        //Total de vendas
        public double TotalSales(DateTime initial, DateTime final)
        {
            //Para descobrir o total de vendas no período vamos utilizar o Linq:
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
