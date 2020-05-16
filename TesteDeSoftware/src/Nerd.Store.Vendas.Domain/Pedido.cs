using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System;
using NerdStore.Core.DominObjects;

namespace Nerd.Store.Vendas.Domain
{
    public class Pedido
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; private set; }
        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItens;
        public PedidoStatus PedidoStatus { get; set; }


        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }


        public void AdicionarItem(PedidoItem pedidoItem)
        {

            if(pedidoItem.Quantidade > MAX_UNIDADES_ITEM) 
            throw new DomainException($"Número Máximo de {MAX_UNIDADES_ITEM} unidades por produto");

            if (_pedidoItens.Any(p => p.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemExistente = _pedidoItens
                                    .FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;

                _pedidoItens.Remove(itemExistente);
            }

            _pedidoItens.Add(pedidoItem);
            CalcularValorPedido();
        }

        private void CalcularValorPedido()
        {
            ValorTotal = _pedidoItens.Sum(i => i.CalcularValor());
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }


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




    public enum PedidoStatus
    {
        Rascunho = 0,
        Iniciando = 1,
        Pago = 4,
        Entregue = 5,
        Cancelado = 6
    }






    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }


        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            if(quantidade < Pedido.MIN_UNIDADES_ITEM) 
            throw new DomainException($"Número Mínimo de {Pedido.MIN_UNIDADES_ITEM} unidades por produto");
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        internal void AdicionarUnidades (int unidades)
        {
            Quantidade += unidades;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }


}