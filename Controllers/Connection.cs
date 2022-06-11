using BudgetPlaning.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BudgetPlaning.Controllers
{
    public class Connection
    {
        public static SqliteConnection ConnectToDB()
        {
            try
            {
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Database.db");
                using (var connection = new SqliteConnection($"Data Source={dbPath};Mode=ReadWriteCreate;"))
                {
                    return connection;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateTableIfNotExists(SqliteConnection connection)
        {
            try
            {
                var queryIfNotExist = "CREATE TABLE IF NOT EXISTS Information(Date TEXT NOT NULL, Summ TEXT NOT NULL, Type TEXT NOT NULL, Category TEXT NOT NULL, Comment TEXT)";

                using (SqliteCommand command = new SqliteCommand(queryIfNotExist, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Record> GetData(SqliteConnection connection)
        {
            List<Record> records = new List<Record>();

            try
            {
                var query = "SELECT Date as 'Дата', Summ as 'Сумма', Type as 'Тип', Category as 'Категория', Comment as 'Комментарий' FROM Information";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            records.Add(new Record(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), !string.IsNullOrEmpty(reader[4].ToString()) ? reader[4].ToString() : "--"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return records;
        }

        public static void AddRecord(string date, string summ, string type, string category, string comment)
        {
            try
            {
                var connection = ConnectToDB();

                connection.Open();

                var query = $"INSERT INTO Information (Date, Summ, Type, Category, Comment) VALUES ('{date}', '{summ}', '{type}', '{category}', '{comment}')";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static double GetBalance()
        {
            var balance = 0.00;

            try
            {
                balance = GetSumm("Доход") - GetSumm("Расход");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return balance;
        }

        private static double GetSumm(string Type)
        {
            var summ = 0.00;

            try
            {
                var connection = ConnectToDB();

                connection.Open();

                var query = $"SELECT Summ, Type FROM Information WHERE Type = '{Type}'";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (double.TryParse(reader[0].ToString().Replace("$", ""), out var value))
                            {
                                summ += value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return summ;
        }
    }
}
