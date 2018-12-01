﻿using CasaDoCodigo.OrdemDeCompra.Controllers;
using CasaDoCodigo.OrdemDeCompra.Models;
using CasaDoCodigo.OrdemDeCompra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrdemDeCompra.UnitTests
{
    public class OrdemDeCompraControllerTest
    {
        Mock<IPedidoRepository> pedidoRepositoryMock;

        public OrdemDeCompraControllerTest()
        {
            pedidoRepositoryMock = new Mock<IPedidoRepository>();
        }

        [Theory]
        [InlineData("", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678")]
        [InlineData("clienteId", "", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678")]
        [InlineData("clienteId", "clienteNome", "", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678")]
        [InlineData("clienteId", "clienteNome", "cliente@email.com", "", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678")]
        [InlineData("clienteId", "clienteNome", "cliente@email.com", "fone", "", "complemento", "bairro", "municipio", "uf", "12345-678")]
        [InlineData("clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "", "municipio", "uf", "12345-678")]
        [InlineData("clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "", "uf", "12345-678")]
        [InlineData("clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "", "12345-678")]
        [InlineData("clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "")]
        public async Task Post_Invalid_Pedido(string clienteId, string clienteNome, string clienteEmail, string clienteTelefone, string clienteEndereco, string clienteComplemento, string clienteBairro, string clienteMunicipio, string clienteUF, string clienteCEP)
        {
            //arrange
            List<ItemPedido> itens = new List<ItemPedido> {
                new ItemPedido("001", "produto 001", 1, 12.34m)
            };
            Pedido pedido = new Pedido(itens, clienteId, clienteNome, clienteEmail, clienteTelefone, clienteEndereco, clienteComplemento, clienteBairro, clienteMunicipio, clienteUF, clienteCEP);
            var controller = new OrdemDeCompraController(pedidoRepositoryMock.Object);
            controller.ModelState.AddModelError("cliente", "Required");
            //act
            IActionResult actionResult = await controller.Post(pedido);

            //assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Post_Invalid_Pedido_No_Items()
        {
            //arrange
            Pedido pedido = new Pedido(new List<ItemPedido>(), "clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678");
            var controller = new OrdemDeCompraController(pedidoRepositoryMock.Object);
            controller.ModelState.AddModelError("cliente", "Required");
            //act
            IActionResult actionResult = await controller.Post(pedido);

            //assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Post_Invalid_Pedido_Items_Null()
        {
            //arrange
            Pedido pedido = new Pedido(null, "clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678");
            var controller = new OrdemDeCompraController(pedidoRepositoryMock.Object);
            controller.ModelState.AddModelError("cliente", "Required");
            //act
            IActionResult actionResult = await controller.Post(pedido);

            //assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task Post_Invalid_Pedido_Success()
        {
            //arrange
            List<ItemPedido> itens = new List<ItemPedido> {
                new ItemPedido("001", "produto 001", 1, 12.34m)
            };
            Pedido pedido = new Pedido(itens, "clienteId", "clienteNome", "cliente@email.com", "fone", "endereco", "complemento", "bairro", "municipio", "uf", "12345-678");
            pedido.Id = 123;
            pedidoRepositoryMock
                .Setup(r => r.CreateOrUpdate(It.IsAny<Pedido>()))
                .ReturnsAsync(pedido);
            var controller = new OrdemDeCompraController(pedidoRepositoryMock.Object);
            //act
            IActionResult actionResult = await controller.Post(pedido);

            //assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            Pedido pedidoCriado  = Assert.IsType<Pedido>(okObjectResult.Value);
            Assert.Equal(123, pedidoCriado.Id);
        }
    }
}