using System.Linq;
using System;
using Nerd.Store.Vendas.Domain;
using Xunit;
using NerdStore.Core.DominObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {

        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
        //Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2 , 100);
        
        //Act
        pedido.AdicionarItem(pedidoItem);

        //Assert
        Assert.Equal(200, pedido.ValorTotal);
        }


        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - PedidoTests")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2 , 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1 , 100);

            //Act
            pedido.AdicionarItem(pedidoItem2);

            //Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);

        }

        [Fact(DisplayName = "Adicionar Item Pedido Acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1 , 100);
            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
            
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente Acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1 , 100);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM , 100);
            pedido.AdicionarItem(pedidoItem);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 1 , 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2 , 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5 , 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            //Act 
            pedido.AtualizarItem(pedidoItemAtualizado);

            //Assert
            Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto XPTO", 2 , 100);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3 , 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5 , 15);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario + 
                                pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

            //Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            //Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
            
        }

        [Fact(DisplayName = "Atualizar Item Pedido Qunatidade Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AtualizarItemPedido_ItensUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            //Arange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 15);

            //Act & Assert

            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));

        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));


        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            //Arange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto XPTO", 2 , 100);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3 , 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario;

            //Act
            pedido.RemoverItem(pedidoItemExistente2);

            //Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher Válido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {

            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
           TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);

        }

        [Fact(DisplayName = "Aplicar voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarErros()
        {

            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1,
           TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.False(result.IsValid);

        }

        [Fact(DisplayName = "Aplicar voucher tipo Valor Desconto")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AplicarVucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto XPTO", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
           TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher tipo Porcentagem Desconto")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AplicarVucher_VoucherTipoPorcentualDesconto_DeveDescontarDoValorTotal()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto XPTO", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1,
           TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

            var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotalComDesconto = pedido.ValorTotal - valorDesconto;

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(valorTotalComDesconto, pedido.ValorTotal);
        }


        [Fact(DisplayName = "Aplicar Voucher desconto excede valor total")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto XPTO", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var voucher = new Voucher("PROMO-15-REAIS", null, 300, 1,
           TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(0, pedido.ValorTotal);
        }


        [Fact(DisplayName = "Aplicar Voucher recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido Tests")]
        public void AplicarVoucher_ModificarItemPedido_DeveCalcularDescontoValorTotal()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto XPTO", 2, 50);
            pedido.AdicionarItem(pedidoItemExistente1);

            var voucher = new Voucher("PROMO-15-REAIS", null, 50, 1,
           TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);
            pedido.AplicarVoucher(voucher);

            var pedidoItemExistente2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 4, 25);

            //Act
            pedido.AdicionarItem(pedidoItemExistente2);

            //Assert
            var totalEsperado = pedido.PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, pedido.ValorTotal);
        }
    }
}
