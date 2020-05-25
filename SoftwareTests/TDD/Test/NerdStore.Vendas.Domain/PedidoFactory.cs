using System;

namespace Nerd.Store.Vendas.Domain
{
    public partial class Pedido
    {
        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido{
                    ClienteId = clienteId,
                };
                pedido.TornarRascunho();
                return pedido;
            }
        }
        
    }


}