using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _07.PrintAllMinionNames
{
    public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            int numberOfId = GetCountOfId(sqlConnection);
            GetFirstName(sqlConnection, numberOfId);
            sqlConnection.Close();
        }
        private static int GetCountOfId(SqlConnection sqlConnection)
        {
            string query =
                @"SELECT COUNT(Id) FROM Minions";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int countOfId = int.Parse(sqlCommand.ExecuteScalar()?.ToString());
            return countOfId;
        }
        private static void GetFirstName(SqlConnection sqlConnection, int numberOfId)
        {
            Queue<string> firstNames = new Queue<string>();
            Stack<string> secondNames = new Stack<string>();
            string query = 
                @"  SELECT 
                    ROW_NUMBER() OVER (ORDEr BY Id) AS Number,
                    Name
                    FROM Minions";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            using SqlDataReader result = sqlCommand.ExecuteReader();
            for (int i = 1; i <= numberOfId / 2; i++)
            {
                result.Read();
                firstNames.Enqueue(result["Name"].ToString());
            }
            for (int i = numberOfId; i > numberOfId/2; i--)
            {
                result.Read();
                secondNames.Push(result["Name"].ToString());
            }
            PrintResult(firstNames, secondNames,numberOfId);
        }
        private static void PrintResult(Queue<string> first,Stack<string> second,int count)
        {
            for (int i = 0; i < count / 2; i++)
            {
                Console.WriteLine(first.Dequeue());
                Console.WriteLine(second.Pop());
            }
            if (count % 2 != 0)
            {
                Console.WriteLine(second.Pop());
            }
            
        }
    }
}
