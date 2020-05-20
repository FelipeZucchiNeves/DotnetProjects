using FluentValidation;
using Nerd.Store.Vendas.Domain;
using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public AdicionarItemPedidoCommand(Guid clieteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clieteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }


        }
        public class AdicionarItemPedidoValidation : AbstractValidator<AdicionarItemPedidoCommand>
        {
            public static string IdClienteMsgErro => "Id do cliente inválido";
            public static string IdProdutoMsgErro => "Id do produto inválido";
            public static string NomeMsgErro => "O nome do produto não foi informado";
            public static string QtdMaxMsgErro => $"Quantidade máxima de um item é {Pedido.MAX_UNIDADES_ITEM}";
            public static string QtdMinMsgErro => "Quantidade mínima de um item é 1";
            public static string ValorUnitarioMsgErro => "Valro acima precisa ser maior que 0";

            public AdicionarItemPedidoValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage(IdClienteMsgErro);

                RuleFor(c => c.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage(IdProdutoMsgErro);

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage(NomeMsgErro);

                RuleFor(c => c.Quantidade)
                    .GreaterThan(0)
                    .WithMessage(QtdMinMsgErro)
                    .LessThanOrEqualTo(Pedido.MAX_UNIDADES_ITEM)
                    .WithMessage(QtdMaxMsgErro);

                RuleFor(c => c.ValorUnitario)
                    .GreaterThan(0)
                    .WithMessage(ValorUnitarioMsgErro);

            }

    }
}
