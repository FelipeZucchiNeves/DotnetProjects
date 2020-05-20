using System.Linq;
using System;
using Nerd.Store.Vendas.Domain;
using Xunit;
using NerdStore.Core.DominObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Adicionar Item Pedido com unidades abaixo do permitido")]
        [Trait("Categoria", "Vendas - Pedido Item Tests")]
        public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(()=>new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1 , 100));
            
        }

    }
}