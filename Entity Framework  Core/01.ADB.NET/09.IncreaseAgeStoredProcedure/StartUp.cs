using Microsoft.Data.SqlClient;
using System;

namespace _09.IncreaseAgeStoredProcedure
{
    public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("EXEC usp_GetOlder @id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", int.Parse(Console.ReadLine()));
            SqlDataReader result = sqlCommand.ExecuteReader();
            result.Read();
            Console.WriteLine($"{result["Name"]} – {result["Age"]} years old");


        }
    }
}
