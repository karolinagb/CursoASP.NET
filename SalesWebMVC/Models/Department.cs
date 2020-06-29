using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /*Um departamento pode ter vários vendedores.
         Vamos implementar um ICollection porque é mais genérico.*/
        //Uma classe abstrata ou interface não pode ser instanciada, vamos criar uma instância de lista.
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        /*Como teremos um construtor com argumentos, o framework precisa de um construtor vazio/default*/
        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        //Adicionar ou remove um seller
        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        /*Total de vendas do departamento:*/
        public double TotalSales(DateTime initial, DateTime final)
        {
            /*Para somar as vendas do departamento, tenho que somar as vendas de todos os vendedores naquele departamento*/
            /*Para isso vamos pegar cada vendedor da lista, chamar o TotalSales (passando o período) 
             * do vendedor e somando o resultado*/
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
