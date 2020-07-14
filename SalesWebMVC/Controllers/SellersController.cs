using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        /*Dependência para o SellerService:*/
        private readonly SellerService _sellerService;

        /*Criando dependência com o DepartmentService:*/
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        /*No caso do controlador não vamos trocar o nome da operação pois ela tem que seguir o padrão do framework*/
        public async Task<IActionResult> Index()
        {
            //Recebendo a lista de vendedores:
            var list = await _sellerService.FindAllAsync();
            //Imprimindo:
            return View(list);
        }

        //IActionResult é o tipo de retorno de todas as ações:
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();

            var viewModel = new SellerFormViewModel { Departments = departments};

            return View(viewModel);
        }

        //Anotation:
        [HttpPost]
        //Previnindo que aplicação sofra ataque csrf = alguém envia dados maliciosos aproveitando a sua sessao
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            /*Validação no controller caso o javascript do usuario esteja desabilitado:*/
            //ModelState.IsValid = testa se o modelo foi validado
            if (!(ModelState.IsValid))
            {
                //Antes de retornar a view, vamos recarregar o formulario:
                var departments = await _departmentService.FindAllAsync();

                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                /*Vai voltar na tela de criação do vendedor enquanto não estiver correto:*/
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);

            /*Redirect retorna para a ação que eu quiser.
             O nameoof ajuda quando eu mudar o nome da ação, pois ai eu não preciso mudar aq tb*/
            return RedirectToAction(nameof(Index));
        }


        //GET
        //int? = opcional
        public async Task<IActionResult> Delete(int? id)
        {
            //Se não for digitado o id:
            if(id == null)
            {
                //NotFound instância uma resposta básica:
                /*Utilizando agora o erro personalizado. Para colocar a mensagem vamos instanciar um objeto anônimo:*/
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            //Trazer o objeto que estou querendo deletar:
            //Tem que por o id.Value para pegar o valor dele, caso exista, porque ele é um objeto opcional:
            var obj = await _sellerService.FindByIdAsync(id.Value);

            //Se o id não existir:
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }       
        }

        //GET
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        /*Esse opcional é só para evitar de acontecer algum erro de execução porque na verdade esse id é obrigatório:
         Por isso testamos se o id é igual a nulo:*/
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            //Tenho que recarregar a lista de departamentos para editar o vendedor:
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            /*Validação no controller caso o javascript do usuario esteja desabilitado:*/
            //ModelState.IsValid = testa se o modelo foi validado
            if (!(ModelState.IsValid))
            {
                //Antes de retornar a view, vamos recarregar o formulario:
                var departments = await _departmentService.FindAllAsync();

                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                /*Vai voltar na tela de criação do vendedor enquanto não estiver correto:*/
                return View(viewModel);
            }

            /*O id do vendedor que eu estou atualizando não pode ser diferente do id da url da requisição*/
            if (id != seller.Id)
            {
                //Id não corresponde:
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                /*Como a operação Update pode gerar exceções vamos colocar isso dentro do try:*/
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            /*Como as exeções já carregam uma mensagem na hora de redirecionar para página de erro vamos usar a mensagem
             da exceção:*/
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            
        }

        //A ação de erro não precisa ser assincrona porque ela nao tem nenhuma acesso a dados:
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                //Macete para pegar o id da requisição:
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                /*O current é opcional, se ele for nulo vamos colocar o operador de coalecência nula e do lado o que poderá
                 ser usado como id.*/
            };
            return View(viewModel);
        }
    }
}
