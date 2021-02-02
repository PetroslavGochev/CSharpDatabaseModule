using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace _08.IncreaseMinionAge
{
    public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        static void Main(string[] args)
        {
            
            int[] listOfId = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string query =
                @"  UPDATE Minions
                    SET Name = CONCAT(UPPER(Left(Name,1)),SUBSTRING(Name,2,Len(Name))),
                    Age += 1
                    WHERE Id = @id";
            for (int i = 0; i < listOfId.Length; i++)
            {
                int id = listOfId[i];
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.ExecuteNonQuery();
            }
            PrintResult(sqlConnection);
            sqlConnection.Close();
        }
        private static void PrintResult(SqlConnection sqlConnection)
        {
            string query =
                @"SELECT * FROM Minions";
            SqlCommand sqlCommand = new SqlCommand(query,sqlConnection);
            SqlDataReader result = sqlCommand.ExecuteReader();
            while (result.Read())
            {
                Console.WriteLine($"{result["Name"]} {result["Age"]}");
            }
        }
      
    }
}
