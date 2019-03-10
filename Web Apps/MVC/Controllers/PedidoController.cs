﻿using MVC.Models.ViewModels;
using MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Model.Redis;
using MVC.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class PedidoController : BaseController
    {
        private readonly IIdentityParser<ApplicationUser> appUserParser;
        private readonly IPedidoService pedidoService;

        public PedidoController(
            IIdentityParser<ApplicationUser> appUserParser,
            IPedidoService pedidoService,
            ILogger<PedidoController> logger,
            IUserRedisRepository repository)
            : base(logger, repository)
        {
            this.appUserParser = appUserParser;
            this.pedidoService = pedidoService;
        }

        [HttpGet("{clienteId}")]
        public async Task<ActionResult> Historico(string clienteId)
        {
            await CheckUserCounterData();

            List<PedidoDTO> model = await pedidoService.GetAsync(clienteId);
            return base.View(model);
        }

        [Route("Notificacoes")]
        public async Task<ActionResult> Notificacoes()
        {
            string clienteId = GetUserId();
            UserCounterData userCounterData = await userRedisRepository.GetUserCounterDataAsync(clienteId);
            await userRedisRepository.MarkAllAsReadAsync(clienteId);
            await CheckUserCounterData();
            return View(userCounterData);
        }
    }
}
