using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System;
using NerdStore.Core.DominObjects;
using FluentValidation.Results;

namespace Nerd.Store.Vendas.Domain
{
    public partial class Pedido : Entity, IAggregateRoot
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; private set; }
        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItens;
        public PedidoStatus PedidoStatus { get; set; }
        public bool VoucherUtilizado { get; private set; }
        public Voucher Voucher { get; private set; }
        public decimal Desconto { get; set; }


        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }


        public void AdicionarItem(PedidoItem pedidoItem)
        {

            ValidarQuantidadeItemPermitida(pedidoItem);

            if (PedidoItemExistente(pedidoItem))
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

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoInexistente(pedidoItem);
            ValidarQuantidadeItemPermitida(pedidoItem);

            var itemExistente = PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

            _pedidoItens.Remove(itemExistente);
            _pedidoItens.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoInexistente(pedidoItem);
            _pedidoItens.Remove(pedidoItem);
            CalcularValorPedido();
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var quantidadeItens = item.Quantidade;
            if(PedidoItemExistente(item))
            {
                var itemExistente =  _pedidoItens.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                quantidadeItens += itemExistente.Quantidade;
            }
            if(quantidadeItens > MAX_UNIDADES_ITEM) 
                throw new DomainException($"Número Máximo de {MAX_UNIDADES_ITEM} unidades por produto");
        }

        public bool PedidoItemExistente(PedidoItem pedidoItem)
        {
            return _pedidoItens.Any(p => p.ProdutoId == pedidoItem.ProdutoId);
        }

        private void ValidarPedidoInexistente (PedidoItem pedidoItem)
        {
            if(!PedidoItemExistente(pedidoItem)) throw new DomainException($"O Item não existe no pedido");
        }

        private void CalcularValorPedido()
        {
            ValorTotal = _pedidoItens.Sum(i => i.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public ValidationResult AplicarVoucher (Voucher voucher)
        {

            var result = voucher.ValidarSeAplicavel();

            if (!result.IsValid) return result;
            
            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorTotalDesconto();
            
            return result;
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if(Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor)
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.PercentualDesconto.HasValue)
                {
                    desconto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }
        
    }


}