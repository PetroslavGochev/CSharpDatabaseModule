using Microsoft.Data.SqlClient;
using System;

namespace _06.RemoveVillain
{
    public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        private const string NOT_FOUND_VILLAIN = "No such villain was found.";
        private const string DELETE_VILLAIN = "{0} was deleted.";
        private const string NUMBER_OF_RELEASE_SEVERANT = "{0} minions were released.";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            int villainId = int.Parse(Console.ReadLine());
            string name = IsVillainIdExist(sqlConnection,villainId);
           
            if(name == null)
            {
                Console.WriteLine(NOT_FOUND_VILLAIN);
            }
            else
            {
                int affectedServent = DeleteServentOfVillain(sqlConnection, villainId);
                DeleteVillain(sqlConnection, villainId);
                Console.WriteLine(String.Format(DELETE_VILLAIN, name));
                Console.WriteLine(String.Format(NUMBER_OF_RELEASE_SEVERANT, affectedServent));
            }
            sqlConnection.Close();
        }
        private static int DeleteServentOfVillain(SqlConnection sqlConnection,int villainId)
        {
            string query =
                @"DELETE FROM MinionsVillains
                  WHERE VillainId = @villainId";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
            string result = sqlCommand.ExecuteNonQuery().ToString();
            return int.Parse(result);
        }
        private static void DeleteVillain(SqlConnection sqlConnection,int villainId)
        {
            string query =
                @"DELETE FROM Villains
                  WHERE Id = @villainId";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
            string result = sqlCommand.ExecuteNonQuery().ToString();
        }

        private static string IsVillainIdExist(SqlConnection sqlConnection,int villainId)
        {
            string query =
                @"SELECT Name From Villains 
                    WHERE Id = @villainId";
            using SqlCommand sqlCommand = new SqlCommand(query,sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
            string name = sqlCommand.ExecuteScalar()?.ToString();
            return name;
        }
    }
}
