using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace _04.Add_Minion
{
    public class StartUp
    {
        private const string ADDING_TOWN = "Town {0} was added to the database.";
        private const string ADDING_VILLAIN = "Villain {0} was added to the database.";
        private const string ADDING_SERVANT = "Successfully added {0} to be minion of {1}.";
        private const string ERROR_TOWN_INSERT = "An error occurred while inserting town to the database";
        private const string ERROR_VILLAIN_INSERT = "An error occurred while inserting villain to the database";
        private const string connectionString = @"Server=.;Database=MinionsDB;Integrated Security = true";

        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string minionName = string.Empty;
            string villain = string.Empty;
            try
            {
                minionName = ReadInformationAboutMinion(sqlConnection);
                villain = ReadInformationAboutVillian(sqlConnection);
            }
            finally 
            {
                AddSeverantOfVillian(sqlConnection,minionName, villain);
            }
            sqlConnection.Close();
            
        }

        private static string ReadInformationAboutMinion(SqlConnection sqlConnection)
        {
            string[] minionArgs = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string minionName = minionArgs[1];
            string age = minionArgs[2];
            string town = minionArgs[3];
            string query =
                @"
                SELECT Id FROM Towns AS T
                WHERE T.Name = @town";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@town", town);
            string townId = sqlCommand.ExecuteScalar()?.ToString();
            if (townId == null)
            {
                InsertTownIntoTownTable(sqlConnection, town);
            }
            return minionName;
        }
        private static void InsertTownIntoTownTable(SqlConnection sqlConnection, string townName)
        {
            string query =
                @"INSERT INTO Towns (Name)
                 VALUES(@name)";
            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@name", townName);
            try
            {
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(String.Format(ADDING_TOWN, townName));
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ERROR_TOWN_INSERT);
            }
            
           
        }
        private static string ReadInformationAboutVillian(SqlConnection sqlConnection)
        {
            string[] villianArgs = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string villain = villianArgs[1];
            string query =
                @"SELECT Name FROM Villains AS v
                  WHERE v.Name = @name";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@name", villain);
            string name = sqlCommand.ExecuteScalar()?.ToString();
            if (name == null)
            {
                InsertVillianIntoVillianTable(sqlConnection, villain);
            }
            return villain;
        }
        private static void InsertVillianIntoVillianTable(SqlConnection sqlConnection, string villain)
        {
            const string evilnessFactors = "Evil";
            string query =
                @"  INSERT INTO Villains (Name,EvilnessFactorId)
                    VALUES 
                    (@villainName,
                    (SELECT Id FROM EvilnessFactors 
                    WHERE EvilnessFactors.Name =@evilness))";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@evilness", evilnessFactors);
            sqlCommand.Parameters.AddWithValue("@villainName", villain);
            try
            {
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(String.Format(ADDING_VILLAIN, villain));
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine(ERROR_VILLAIN_INSERT);
            }
            
        }

        private static void AddSeverantOfVillian(SqlConnection sqlConnection,string minionName,string villainName)
        {
            string query =
                @"  INSERT INTO MinionsVillains (MinionId,VillainId)
                    Values (
                    (SELECT Id FROM Minions AS m
                    WHERE m.Name = @minionName),
                    (SELECT Id FROM Villains AS v
                    WHERE V.Name = @villainName))";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@minionName",minionName);
            sqlCommand.Parameters.AddWithValue( "@villainName",villainName);
            Console.WriteLine(String.Format(ADDING_SERVANT, minionName, villainName));
        }
    }
}




