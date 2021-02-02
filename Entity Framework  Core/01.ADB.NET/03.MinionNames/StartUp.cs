using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _03.MinionNames
{
    public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        private const string NOT_EXIST_VILLIAN = "No villain with ID {0} exists in the database.";
        private const string NO_MINION = "(no minion)";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            int villianId = int.Parse(Console.ReadLine());
            Console.WriteLine(GetVillianWithId(sqlConnection, villianId));
            Console.WriteLine(GetMinionWithId(sqlConnection, villianId));
            sqlConnection.Close();
        }

        private static string GetVillianWithId(SqlConnection sqlConnection, int villianId)
        {
            StringBuilder sb = new StringBuilder();
            string query =
                @"SELECT [Name] FROM Villains
                  WHERE Id = @villianId";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villianId", villianId);
            string sqlReader = sqlCommand.ExecuteScalar()?.ToString();
            if (String.IsNullOrEmpty(sqlReader))
            {
                sb.Append(String.Format(NOT_EXIST_VILLIAN, villianId));
            }
            else
            {
                sb.Append($"Viillian: ");
                sb.Append(sqlReader);
            }
            return sb.ToString();
        }
        private static string GetMinionWithId(SqlConnection sqlConnection,int villianId)
        {
            StringBuilder sb = new StringBuilder();
            string query =
                    @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY m.Name) AS Number,
                    m.Name AS [Name],
                    m.Age AS Age
                    FROM Minions AS m
                    JOIN MinionsVillains AS mv ON mv.MinionId = m.Id
                    WHERE mv.VillainId = @villianId";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue ("@villianId",villianId);
            using SqlDataReader result = sqlCommand.ExecuteReader();
            if(result == null)
            {
                sb.Append(NO_MINION);
            }
            else
            {
                while (result.Read())
                {
                    sb.AppendLine($"{result["Number"]}. {result["Name"]} {result["Age"]}");
                }
            }
            return sb.ToString();
        }
    }

}
