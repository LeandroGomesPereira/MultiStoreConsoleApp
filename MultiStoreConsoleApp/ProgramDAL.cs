using Microsoft.Data.SqlClient;
using System.Data;

namespace MultiStoreConsoleApp
{
    public class ProgramDAL
    {
        private readonly string _connectionString = "Server=localhost;Integrated security=SSPI;database=AvaliacaoMTP;TrustServerCertificate=True";

        public void ExcluiTabela()
        {
            string query = @$"DROP TABLE stage.MultiStore";

            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlComando = new SqlCommand(query, sqlConnection);

            try
            {
                sqlConnection.Open();
                sqlComando.ExecuteNonQuery();
                Console.WriteLine("Command executed correctly!");
            }
            catch (Exception ex)
            {
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

        public void CriaTabela(List<string> nomeColunas, bool usaConfiguracaoPadrao)
        {
            string query = @$"CREATE TABLE stage.Teste";
            string queryColunas;

            if (usaConfiguracaoPadrao)
                queryColunas = "( " + string.Join(" varchar(100), ", nomeColunas) + " varchar(100));";
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
	                	ProductName varchar(50),
	                	Sales decimal(18, 2),
	                	Quantity int,
	                	Discount int,
	                	Profit decimal(18, 4)
	                );";
            }

            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlComando = new SqlCommand(query + queryColunas, sqlConnection);

            try
            {
                sqlConnection.Open();
                sqlComando.ExecuteNonQuery();
                Console.WriteLine("Command executed correctly!");
            }
            catch (Exception ex)
            {
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
    }
}
