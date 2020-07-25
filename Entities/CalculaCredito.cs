using SisSolutions.Entities.Enums;
using SisSolutions.Exceptions.Course.Entities.Exceptions;
using System;
using System.Globalization;

namespace SisSolutions.Entities
{
    class CalculaCredito
    {
        const double MaxEmprestimo = 1000000;
        private double taxa;
        private string anoMes;

        public double VlrEmprestimo { get; set; }

        public int QtParcelas { get; set; }

        public TipoCredito TpCredito { get; set; }

        public DateTime PrimVcto { get; set; }

        public double VlrTotalComJuros { get; set; }

        public double VlrJuros { get; set; }

        public string StatusAprovacao { get; set; }

        public CalculaCredito()
        {
        }

        public CalculaCredito(double vlrEmprestimo, TipoCredito tpCredito, int qtParcelas, DateTime prVcto)
        {
            VlrEmprestimo = vlrEmprestimo;
            QtParcelas = qtParcelas;
            TpCredito = tpCredito;
            PrimVcto = prVcto;
        }

        public double Calcular(double vlrEmprestimo, TipoCredito tpCredito, int qtParcelas, DateTime prVcto)
        {
            Validacoes(qtParcelas, prVcto);

            switch (tpCredito)
            {
                case TipoCredito.CD:
                    taxa = 2;
                    anoMes = "M";
                    VlrTotalComJuros = Calcula(taxa, vlrEmprestimo, qtParcelas, tpCredito, anoMes);
                    break;
                case TipoCredito.CC:
                    anoMes = "M";
                    taxa = 1;
                    VlrTotalComJuros = Calcula(taxa, vlrEmprestimo, qtParcelas, tpCredito, anoMes);
                    break;
                case TipoCredito.PJ:
                    anoMes = "M";
                    taxa = 5;
                    VlrTotalComJuros = Calcula(taxa, vlrEmprestimo, qtParcelas, tpCredito, anoMes);
                    break;
                case TipoCredito.PF:
                    anoMes = "M";
                    taxa = 3;
                    VlrTotalComJuros = Calcula(taxa, vlrEmprestimo, qtParcelas, tpCredito, anoMes);
                    break;
                default:
                    anoMes = "A";
                    taxa = 9;
                    VlrTotalComJuros = Calcula(taxa, vlrEmprestimo, qtParcelas, tpCredito, anoMes);
                    break;
            }

            return VlrTotalComJuros;
        }

        private double Calcula(double taxa, double vlrEmprestimo, int qtParcelas, TipoCredito tpCredito, string anoMes)
        {
            StatusAprovacao = "Aprovado.";

            double taxaR = (taxa / 100);
            double taxaC = taxaR;
            double dividendo = 1;
            double divisor = 12;
            double potencia = (dividendo / divisor);

            if (anoMes.Equals("A"))
                taxaC = Math.Pow(1 + taxaR, potencia) - 1;

            VlrTotalComJuros = vlrEmprestimo * Math.Pow(1 + taxaC, qtParcelas);

            VlrJuros = VlrTotalComJuros - vlrEmprestimo;

            if (VlrTotalComJuros > MaxEmprestimo)
                StatusAprovacao = "Emprestimo reprovado.";

            if (tpCredito == TipoCredito.PJ && VlrTotalComJuros < 15000)
            {
                StatusAprovacao = "Emprestimo reprovado.";
                throw new DomainException("Valor minimo a ser liberado nao alcancado");
            }

            if (VlrTotalComJuros > MaxEmprestimo)
            {
                StatusAprovacao = "Emprestimo reprovado.";
                throw new DomainException("Credito excedeu o limite");
            }

            return VlrTotalComJuros;
        }

        public override string ToString()
        {
            return "\r\nStatus da Aprovacao.............: "
                + StatusAprovacao
                + "\r\nValor do Juros..................: "
                + VlrJuros.ToString("F2", CultureInfo.InvariantCulture)
                + "\r\nValor total com juros...........: "
                + VlrTotalComJuros.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void Validacoes(int qtParcelas, DateTime prVcto)
        {
            TimeSpan date = (prVcto - DateTime.Now);
            int Dias = date.Days;

            if (Dias < 5 || Dias > 40)
            {
                throw new DomainException("Dia do 1* Vencimento inconsistente!!!");
            }

            if (qtParcelas < 5 || qtParcelas > 72)
            {
                throw new DomainException("Quantidades de Parcelas inconsistente!!!");
            }
        }

    }
}
