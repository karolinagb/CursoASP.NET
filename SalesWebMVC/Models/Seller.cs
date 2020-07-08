using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /*Transformando o e-mail em um link de e-mail:*/
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /*Com o anotation display eu posso utilizar o nome que eu quise nos labels:*/
        [Display (Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate  { get; set; }

        [Display (Name = "Base Salary")]
        //Definindo o formato dos dados: "{0:F2} - define duas casas decimais"
        [DisplayFormat (DataFormatString = "{0:F2}")]
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
