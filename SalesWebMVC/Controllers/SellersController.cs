using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        /*Dependência para o SellerService:*/
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            //Recebendo a lista de vendedores:
            var list = _sellerService.FindAll();
            //Imprimindo:
            return View(list);
        }

        //IActionResult é o tipo de retorno de todas as ações:
        public IActionResult Create()
        {
            return View();
        }

        //Anotation:
        [HttpPost]
        //Previnindo que aplicação sofra ataque csrf = alguém envia dados maliciosos aproveitando a sua sessao
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);

            /*Redirect retorna para a ação que eu quiser.
             O nameoof ajuda quando eu mudar o nome da ação, pois ai eu não preciso mudar aq tb*/
            return RedirectToAction(nameof(Index));
        }
    }
}
