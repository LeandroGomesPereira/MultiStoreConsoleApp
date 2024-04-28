using System.Text.RegularExpressions;

namespace MultiStoreConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string nomeArquivo = "stage_MultiStore.xlsx";
            string caminho = "C:\\\\";
            bool usaConfiguracaoPadrao = true;
            FileInfo arquivo;
            List<string> nomeColunas = new List<string>()
            {
                "RowID",
                "OrderID",
                "Order Date",
                "Ship Date",
                "Ship Mode",
                "CustomerID",
                "Customer Name",
                "Customer Age",
                "Customer Birthday",
                "Customer State",
                "Segment",
                "Country",
                "City",
                "State",
                "Regional ManagerID",
                "Regional Manager",
                "Postal Code",
                "Region",
                "ProductID",
                "Category",
                "Sub-Category",
                "Product Name",
                "Sales",
                "Quantity",
                "Discount",
                "Profit"
            };

            ProgramDAL programDAL = new ProgramDAL();

            ExibeMenuInicial(ref caminho, ref nomeArquivo, ref usaConfiguracaoPadrao);

            Console.WriteLine(caminho + nomeArquivo);
            arquivo = new FileInfo(caminho + nomeArquivo);
            
            nomeColunas = nomeColunas.Select(nome => Regex.Replace(nome, "[^a-zA-Z0-9_.]+", "")).ToList();

            //programDAL.ExcluiTabela();

            programDAL.CriaTabela(nomeColunas, usaConfiguracaoPadrao);
        }

        public static void ExibeMenuInicial(ref string caminho, ref string nomeArquivo, ref bool usaConfiguracaoPadrao)
        {
            Console.WriteLine(@$"Usar Caminho Padrão '{caminho}'? (Y/N)");
            bool usaCaminhoPadrao = Console.ReadLine().ToUpper() == "Y";

            if (!usaCaminhoPadrao)
            {
                Console.WriteLine(@$"Digite o Caminho desejado abaixo:");
                caminho = Console.ReadLine();
            }

            Console.WriteLine(@$"Usar Nome do Arquivo Padrão '{nomeArquivo}'? (Y/N)");
            bool usaNomeArquivoPadrao = Console.ReadLine().ToUpper() == "Y";

            if (!usaNomeArquivoPadrao)
            {
                Console.WriteLine(@$"Digite o Nome do Arquivo desejado abaixo:");
                nomeArquivo = Console.ReadLine();
            }

            Console.WriteLine(@$"Usar Nomenclaturas e Tipos das Colunas Padrões? (Y/N)");
            Console.WriteLine(@$"Caso optar por não usar a configuração padrão por haver mais colunas ou colunas diferentes do padrão,");
            Console.WriteLine(@$"A tabela no banco será criada com os nomes das colunas no documentos e todas com tipo 'varchar(100)'");
            usaConfiguracaoPadrao = Console.ReadLine().ToUpper() == "Y";

            if (string.IsNullOrWhiteSpace(caminho) || string.IsNullOrWhiteSpace(nomeArquivo))
            {
                Console.WriteLine("Caminho e Nome do Arquivo precisam ser definidos!");
                ExibeMenuInicial(ref caminho, ref nomeArquivo, ref usaConfiguracaoPadrao);
            }
        }
    }
}