using LiberacaoCredito.Api.Controllers;
using LiberacaoCredito.Domain.Enums;
using LiberacaoCredito.Domain.Interfaces;
using LiberacaoCredito.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LiberacaoCredito.Test.Controllers
{
    public class CreditoControllerTests
    {
        private readonly Mock<ICreditoService> _creditoServiceMock;
        private readonly Mock<ILogger<CreditoController>> _loggerMock;
        private readonly CreditoController _controller;

        public CreditoControllerTests()
        {
            _creditoServiceMock = new Mock<ICreditoService>();
            _loggerMock = new Mock<ILogger<CreditoController>>();
            _controller = new CreditoController(_creditoServiceMock.Object, _loggerMock.Object);
        }

        
        [Fact]
        public void ProcessarCredito_ShouldReturnOkResultWhenCreditIsApproved()
        {
            // Arrange
            var credito = new Credito
            {
                ValorCredito = 10000,
                QuantidadeParcelas = 12,
                DataPrimeiroVencimento = DateTime.Today.AddDays(20),
                TipoCredito = TipoCredito.CreditoPessoaFisica
            };

            var resultadoAprovado = new ResultadoCredito
            {
                Aprovado = true,
                ValorTotalComJuros = 11000,
                ValorJuros = 1000
            };

            _creditoServiceMock.Setup(service => service.ProcessarCredito(It.IsAny<Credito>()))
                              .Returns(resultadoAprovado);

            // Act
            var result = _controller.ProcessarCredito(credito);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultadoCredito = Assert.IsType<ResultadoCredito>(okResult.Value);
            Assert.Equal(resultadoAprovado, resultadoCredito);
        }

        [Fact]
        public void ProcessarCredito_ShouldReturnBadRequestResultWhenCreditIsNotApproved()
        {
            // Arrange
            var credito = new Credito
            {
                ValorCredito = 10000,
                QuantidadeParcelas = 12,
                DataPrimeiroVencimento = DateTime.Today.AddDays(20),
                TipoCredito = TipoCredito.CreditoPessoaFisica
            };

            var resultadoRejeitado = new ResultadoCredito
            {
                Aprovado = false,
                MensagensErro = new List<string> { "Crédito não aprovado devido a restrições internas." }
            };

            _creditoServiceMock.Setup(service => service.ProcessarCredito(It.IsAny<Credito>()))
                              .Returns(resultadoRejeitado);

            // Act
            var result = _controller.ProcessarCredito(credito);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var resultadoCredito = Assert.IsType<ResultadoCredito>(badResult.Value);
            Assert.Equal(resultadoRejeitado, resultadoCredito);
        }

        [Fact]
        public void ProcessarCredito_ShouldReturnInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            var credito = new Credito
            {
                ValorCredito = 10000,
                QuantidadeParcelas = 12,
                DataPrimeiroVencimento = DateTime.Today.AddDays(20),
                TipoCredito = TipoCredito.CreditoPessoaFisica
            };

            _creditoServiceMock.Setup(service => service.ProcessarCredito(It.IsAny<Credito>()))
                              .Throws(new Exception("Erro interno no serviço de crédito."));

            // Act
            var result = _controller.ProcessarCredito(credito);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
