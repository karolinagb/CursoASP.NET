using System.Collections.Generic;

namespace SalesWebMVC.Models.ViewModels
{
    public class SellerFormViewModel
    {
        /*Classe que terá os dados necessários para uma tela de cadastro de vendedor:*/

        //Primeiro ter um vededor:
        public Seller Seller { get; set; }

        //Ter uma lista de departamentos para selecionar no cadastro do seller:
        //OBS: UTILIZAR O NOME DOS ATRIBUTOS CORRETOS PARA AJUDAR O FRAMEWORK A RECONHECER OS DADOS:
        //Na hora do framework fazer a conversão dos dados http para objeto ele faz automaticamente:
        public ICollection<Department> Departments { get; set; }
    }
}
