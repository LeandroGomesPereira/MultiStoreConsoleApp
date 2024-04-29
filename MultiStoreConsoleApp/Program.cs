using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Data;
using System.Text.RegularExpressions;

namespace MultiStoreConsoleApp
{
    public class Program
    {
        private static string _connectionString = "Server=localhost;Integrated security=SSPI;database=AvaliacaoMTP;TrustServerCertificate=True";

        private static void Main(string[] args)
        {
            string nomeArquivo = "stage_MultiStore.xlsx";
            string caminho = "C:\\temp\\";
            bool usaConfiguracaoPadrao = true;
            List<List<string>> registrosArquivo = new();
            List<string> nomeColunas = new List<string>();

            ProgramDAL programDAL = new ProgramDAL();

            ExibeMenuInicial(ref caminho, ref nomeArquivo, ref usaConfiguracaoPadrao);

            Console.WriteLine(caminho + nomeArquivo);

            ProcessaArquivoExcel(caminho, nomeArquivo, ref registrosArquivo, ref nomeColunas);
            
            nomeColunas = nomeColunas.Select(nome => Regex.Replace(nome, "[^a-zA-Z0-9_.]+", "")).ToList();

            string diretorioArquivosProcessados = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\arquivos_processados\\" + nomeArquivo;

            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            sqlConnection.Open();
            SqlTransaction transacao = sqlConnection.BeginTransaction();

            try
            {
                programDAL.ExcluiTabela(sqlConnection, transacao);

                programDAL.CriaTabela(nomeColunas, usaConfiguracaoPadrao, sqlConnection, transacao);

                programDAL.InsereDados(nomeColunas, registrosArquivo, sqlConnection, transacao);

                File.Move(caminho + nomeArquivo, diretorioArquivosProcessados, true);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();

                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
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

        public static void ProcessaArquivoExcel(string caminho, string nomeArquivo, ref List<List<string>> registros, ref List<string> nomeColunas)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (caminho[caminho.Length - 1] != '\\')
                caminho += "\\";

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(caminho + nomeArquivo)))
            {
                try
                {
                    ExcelWorksheet myWorksheet = excelPackage.Workbook.Worksheets.First();
                    int totalRows = myWorksheet.Dimension.End.Row;
                    int totalColumns = myWorksheet.Dimension.End.Column;

                    nomeColunas = myWorksheet.Cells[1, 1, 1, totalColumns]
                        .Select(celula => celula.Value == null ? string.Empty : celula.Value.ToString())
                        .ToList();

                    for (int rowNum = 2; rowNum <= totalRows; rowNum++)
                    {
                        List<string?> row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns]
                            .Select(celula => celula.Value == null ? string.Empty : celula.Value.ToString().Replace("'", "''"))
                            .ToList();
                        registros.Add(row);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}