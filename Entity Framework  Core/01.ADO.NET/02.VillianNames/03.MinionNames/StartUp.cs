using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _03.MinionNames
{
    public class StartUp
    {
       private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            Console.WriteLine(GetVillianWithId(sqlConnection, 1));
            
        }

        public static string GetVillianWithId(SqlConnection sqlConnection,int villianId)
        {
            StringBuilder sb = new StringBuilder();
            string query = 
                @"SELECT [Name] FROM Villains
                  WHERE Id = @villianId";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villianId", villianId);
            string sqlReader = sqlCommand.ExecuteScalar()?.ToString();
            sb.Append($"Viillian: ");
            sb.Append(sqlReader);
            return sb.ToString();
        } 
    }
}
