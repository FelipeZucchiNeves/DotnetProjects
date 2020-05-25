using System;
using FluentValidation;
using NerdStore.Core;
using FluentValidation.Results;


namespace Nerd.Store.Vendas.Domain
{
    public class Voucher
    {

        public string Codigo { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int  Quantidade { get;private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }


        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }

        public Voucher( string codigo, decimal? percentualDesconto, decimal? valorDesconto, int quantidade,
        TipoDescontoVoucher tipoDescontoVoucher, DateTime dataValidade, bool ativo, bool utilizado)
        {
            TipoDescontoVoucher = tipoDescontoVoucher;
            Codigo = codigo;
            PercentualDesconto = percentualDesconto;
            ValorDesconto = valorDesconto;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
        }


    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public static string CodigoErroMsg => "Voucher sem código válido.";
        public static string DataValidadeErroMsg => "Este Voucher está expirado.";
        public static string AtivoErroMsg => "Este Voucher não é mais válido.";
        public static string UtilizadoErroMsg => "Este Voucher já foi utilizado.";
        public static string QuantidadeErroMsg => "Este Voucher não está mais disponível.";
        public static string ValorDescontoErroMsg => "O valor do desconto preciser ser superio a 0.";
        public static string PorcentagemDescontoErroMsg => "O valor da porcentagem tem que ser superior a 1%.";


        public VoucherAplicavelValidation()
        {
            RuleFor(c => c.Codigo)
            .NotEmpty()
            .WithMessage(CodigoErroMsg);

            RuleFor(c => c.DataValidade)
            .Must(DataVencimentoSuperiorAtual)
            .WithMessage(DataValidadeErroMsg);

            RuleFor(c => c.Ativo)
            .Equal(true)
            .WithMessage(AtivoErroMsg);

            RuleFor(c => c.Utilizado)
            .Equal(false)
            .WithMessage(UtilizadoErroMsg);

            RuleFor(c => c.Quantidade)
            .GreaterThan(0)
            .WithMessage(QuantidadeErroMsg);

            When(x => x.TipoDescontoVoucher == TipoDescontoVoucher.Valor, () =>{

                RuleFor(x => x.ValorDesconto)
                .NotNull()
                .WithMessage(ValorDescontoErroMsg)
                .GreaterThan(0)
                .WithMessage(ValorDescontoErroMsg);

            });

            When(x => x.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem, () => {
                RuleFor (x => x.PercentualDesconto)
                .NotNull()
                .WithMessage(PorcentagemDescontoErroMsg)
                .GreaterThan(0)
                .WithMessage(PorcentagemDescontoErroMsg);
            });
        }

        protected  static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}