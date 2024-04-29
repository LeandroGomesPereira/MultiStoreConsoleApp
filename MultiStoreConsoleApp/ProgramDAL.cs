using Microsoft.Data.SqlClient;

namespace MultiStoreConsoleApp
{
    public class ProgramDAL
    {
        public void ExcluiTabela(SqlConnection sqlConnection, SqlTransaction transacao)
        {
            string query = @$"DROP TABLE stage.MultiStore";

            SqlCommand sqlComando = new SqlCommand(query, sqlConnection, transacao);

            sqlComando.ExecuteNonQuery();
            Console.WriteLine("Tabela Excluída com sucesso!");
        }

        public void CriaTabela(List<string> nomeColunas, bool usaConfiguracaoPadrao, SqlConnection sqlConnection, SqlTransaction transacao)
        {
            string query = @$"CREATE TABLE stage.MultiStore";
            string queryColunas;

            if (!usaConfiguracaoPadrao)
                queryColunas = "( " + string.Join(" varchar(300), ", nomeColunas) + " varchar(100));";
            else
            {
                queryColunas = @"
                    (
	                	RowID int PRIMARY KEY,
	                	OrderID varchar(15),
	                	OrderDate int,
	                	ShipDate int,
	                	ShipMode varchar(50),
	                	CustomerID varchar(10),
	                	CustomerName varchar(50),
	                	CustomerAge int,
	                	CustomerBirthday varchar(5),
	                	CustomerState varchar(20),
	                	Segment varchar(20),
	                	Country varchar(50),
	                	City varchar(50),
	                	State varchar(50),
	                	RegionalManagerID varchar(15),
	                	RegionalManager varchar(50),
	                	PostalCode int,
	                	Region varchar(50),
	                	ProductID varchar(15),
	                	Category varchar(50),
	                	SubCategory varchar(50),
	                	ProductName varchar(500),
	                	Sales decimal(18, 2),
	                	Quantity int,
	                	Discount decimal(18, 2),
	                	Profit decimal(18, 4)
	                );";
            }

            SqlCommand sqlComando = new SqlCommand(query + queryColunas, sqlConnection, transacao);
            
            sqlComando.ExecuteNonQuery();
            Console.WriteLine("Tabela Criada com sucesso!");
        }

        public void InsereDados(List<string> nomeColunas, List<List<string>> registros, SqlConnection sqlConnection, SqlTransaction transacao)
        {
            foreach (List<string> row in registros)
            {
                string query = @$"INSERT INTO stage.MultiStore ({string.Join(", ", nomeColunas)}) VALUES ('{string.Join("', '", row)}');";

                SqlCommand sqlComando = new SqlCommand(query, sqlConnection, transacao);

                sqlComando.ExecuteNonQuery();
                Console.WriteLine("Registros Inseridos com sucesso!");
            }

        }
    }
}
