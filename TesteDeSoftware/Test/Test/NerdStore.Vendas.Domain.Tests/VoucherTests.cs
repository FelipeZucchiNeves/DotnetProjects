using System.Linq;
using System;
using Nerd.Store.Vendas.Domain;
using Xunit;
using NerdStore.Core.DominObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Unitário")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1,
            TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15),true,false);

            //Act
            var result = voucher.ValidarSeAplicavel();

            //Assert
            Assert.True(result.IsValid);

        }
    }
}
