using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _05.ChangeTownNameCasing
{
   public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";
        private const string INVALID_COUNTRY = "No town names were affected.";
        private const string COUNT_OF_AFFFECTED_TOWNS = "{0} town names were affected.";
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            UpdateTownFromCountry(sqlConnection, Console.ReadLine());
            sqlConnection.Close();
        }
        private static void UpdateTownFromCountry(SqlConnection sqlConnection,string country)
        {
            string query =
                @"  SELECT COUNT(*) FROM Towns as T
                    JOIN Countries AS c ON c.Id = t.CountryCode
                    WHERE C.Name = @country
                    GROUP BY t.CountryCode";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@country", country);
            string affectedTowns = sqlCommand.ExecuteScalar()?.ToString();
            if(affectedTowns == null)
            {
                Console.WriteLine(INVALID_COUNTRY);
            }
            else
            {
                PrintResult(NameOfAffectedTowns(sqlConnection, country),affectedTowns);
            }
        }
        private static List<string> NameOfAffectedTowns(SqlConnection sqlConnection,string country)
        {
            List<string> towns = new List<string>();
            string query =
                @"  SELECT UPPER(T.Name) AS Name
                    FROM Towns AS T
                    JOIN Countries AS c ON c.Id = t.CountryCode
                    WHERE C.Name = @country";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@country", country);
            using SqlDataReader result = sqlCommand.ExecuteReader();
            while (result.Read())
            {
                towns.Add(result["Name"].ToString());
            }
            return towns;
        }

        private static void PrintResult(List<string> towns,string affectedTowns)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format(COUNT_OF_AFFFECTED_TOWNS, affectedTowns));
            sb.Append("[");
            for (int i = 0; i < towns.Count; i++)
            {
                if(i == towns.Count - 1)
                {
                    sb.AppendLine($"{towns[i]}]");
                    continue;
                }
                sb.Append($"{towns[i]}, ");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
