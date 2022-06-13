using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Controllers
{
    public class Statistics
    {
        public static (string, string) GetStatistics(string filter)
        {
            var income = "";
            var expense = "";

            try
            {
                var connection = Connection.ConnectToDB();

                connection.Open();

                Connection.CreateTableIfNotExists(connection);

                income = GetSummIncome(connection, filter);

                expense = GetSummExpense(connection, filter);

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (income, expense);
        }

        private static string GetSummExpense(SqliteConnection connection, string filter)
        {
            var expense = "";

            try
            {
                var query = "";

                switch (filter)
                {
                    case "Week": query = "SELECT SUM(Summ) FROM Information WHERE Type = 'Расход' AND Date >= Date('now', '-6 days')"; break;
                    case "Month": query = "SELECT SUM(Summ) FROM Information WHERE Type = 'Расход' AND Date >= Date('now', '-1 month')"; break;
                    case "Year": query = "SELECT SUM(Summ) FROM Information WHERE Type = 'Расход' AND Date >= Date('now', '-1 year')"; break;
                }

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.CommandTimeout = 10;

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var number = Connection.ParseDouble(reader[0].ToString());

                            expense = number.ToString("C", CultureInfo.GetCultureInfo("en-US"));
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Не удалось сформировать статистику");
            }

            return expense;
        }

        private static string GetSummIncome(SqliteConnection connection, string filter)
        {
            var income = "";

            try
            {
                var query = "";

                switch (filter)
                {
                    case "Week": query = "SELECT SUM(Summ) FROM Information WHERE Type = 'Доход' AND Date >= Date('now', '-6 days')"; break;
                    case "Month": query = "SELECT SUM(Summ) FROM Information WHERE Type = 'Доход' AND Date >= Date('now', '-1 month')"; break;
                    case "Year": query = "SELECT SUM(Summ) FROM Information WHERE Type = 'Доход' AND Date >= Date('now', '-1 year')"; break;
                }

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.CommandTimeout = 10;

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            var number = Connection.ParseDouble(reader[0].ToString());

                            income = number.ToString("C", CultureInfo.GetCultureInfo("en-US"));
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Не удалось сформировать статистику");
            }

            return income;
        }
    }
}
