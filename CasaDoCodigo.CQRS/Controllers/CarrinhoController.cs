﻿using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    [Authorize]
    public class CarrinhoController : BaseController
    {
        private readonly IIdentityParser<ApplicationUser> appUserParser;
        private readonly ICatalogoService catalogoService;
        private readonly ICarrinhoService carrinhoService;

        public CarrinhoController(
            IHttpContextAccessor contextAccessor,
            IIdentityParser<ApplicationUser> appUserParser,
            ILogger<CarrinhoController> logger,
            ICatalogoService catalogoService,
            ICarrinhoService carrinhoService)
            : base(logger)
        {
            this.appUserParser = appUserParser;
            this.catalogoService = catalogoService;
            this.carrinhoService = carrinhoService;
        }

        public async Task<IActionResult> Index(string codigo)
        {
            try
            {
                string idUsuario = GetUserId();
                var produto = await catalogoService.GetProduto(codigo);
                ItemCarrinho itemCarrinho = new ItemCarrinho(produto.Codigo, produto.Codigo, produto.Nome, produto.Preco, 1, produto.UrlImagem);
                var carrinho = await carrinhoService.AddItem(idUsuario, itemCarrinho);
                return View(carrinho);
            }
            catch (BrokenCircuitException e)
            {
                logger.LogError(e, e.Message);
                HandleBrokenCircuitException(catalogoService);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                HandleException();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> quantidades, string action)
        {
            try
            {
                var usuario = appUserParser.Parse(HttpContext.User);
                var carrinho = carrinhoService.DefinirQuantidades(usuario, quantidades);
                //if (action == "[ Checkout ]")
                //{
                //    RedirectToAction("Create", "Order");
                //}
            }
            catch (BrokenCircuitException e)
            {
                logger.LogError(e, e.Message);
                HandleBrokenCircuitException(carrinhoService);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                HandleException();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<UpdateQuantidadeOutput> UpdateQuantidade([FromBody]ItemCarrinho itemCarrinho)
        {
            return await carrinhoService.UpdateItem(GetUserId(), itemCarrinho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Cadastro cadastro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var viewModel = new CadastroViewModel(cadastro);
                    await carrinhoService.Checkout(GetUserId(), viewModel);
                    return RedirectToAction("Checkout");
                }
                return RedirectToAction("Index", "Cadastro");
            }
            catch (BrokenCircuitException e)
            {
                logger.LogError(e, e.Message);
                HandleBrokenCircuitException(carrinhoService);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                HandleException();
            }
            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            try
            {
                var usuario = appUserParser.Parse(HttpContext.User);
                return View(new PedidoConfirmado(usuario.Email));
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                HandleException();
            }
            return View();
        }
    }
}