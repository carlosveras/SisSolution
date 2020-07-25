using SisSolutions.Entities;
using SisSolutions.Entities.Enums;
using SisSolutions.Exceptions.Course.Entities.Exceptions;
using System;
using System.Globalization;

namespace SisSolutions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Valor Emprestimo ...............: ");
            double vlrEmprestimo = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            Console.Write("Quantas Parcelas ...............: ");
            int qtParcelas = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
          
            Console.WriteLine();

            Console.WriteLine("Tipo Credito ...................:");
            foreach (int i in Enum.GetValues(typeof(TipoCredito)))
            {
                Console.Write($"{Enum.GetName(typeof(TipoCredito), i)}");
                Console.WriteLine($" {i}");
            }

            Console.WriteLine();

            Console.Write("Informe o Tipo Credito .........: ");
            TipoCredito tipoCredito = (TipoCredito)Enum.Parse(typeof(TipoCredito), Console.ReadLine());
            Console.WriteLine();

            Console.Write("Primeiro Vencimento (DD/MM/YYYY): ");
            DateTime primVcto = DateTime.Parse(Console.ReadLine());

            CalculaCredito cCredito = new CalculaCredito();

            try
            {
                double valor = cCredito.Calcular(vlrEmprestimo, tipoCredito, qtParcelas, primVcto);
            }
            catch (DomainException e)
            {
                Console.WriteLine();
                Console.WriteLine("Mensagem .......................: " + e.Message);
            }

            Console.WriteLine(cCredito);
            Console.ReadKey();
        }
    }
}
