using BudgetPlaning.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                InitializeDatbase();

                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Database.db");

                using (var connection = new SqliteConnection($"Filename={dbPath};Mode=ReadWriteCreate;"))
                {
                    return connection;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async static void InitializeDatbase()
        {
            try
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync("Database.db", CreationCollisionOption.OpenIfExists);
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
                var queryIfNotExist = "CREATE TABLE IF NOT EXISTS Information(ID INTEGER PRIMARY KEY, Date TEXT NOT NULL, Summ TEXT NOT NULL, Type TEXT NOT NULL, Category TEXT NOT NULL, Comment TEXT)";

                using (SqliteCommand command = new SqliteCommand(queryIfNotExist, connection))
                {
                    command.CommandTimeout = 10;

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
                    command.CommandTimeout = 10;

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var number = ParseDouble(reader[1].ToString());

                            records.Add(new Record(reader[0].ToString(), number.ToString("C", CultureInfo.GetCultureInfo("en-US")), reader[2].ToString(), reader[3].ToString(), !string.IsNullOrEmpty(reader[4].ToString()) ? reader[4].ToString() : "--"));
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

        public static double ParseDouble(string value)
        {
            var number = 0.00;

            if (double.TryParse(value, out number) || double.TryParse(value.Replace(".", ","), out number) || double.TryParse(value.Replace(",", "."), out number))
                return number;
            else throw new Exception("Сумма имеет неправильный формат");
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
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch
            {
                throw new Exception("Не удалось добавить операцию, проверьте заполненные данные");
            }
        }

        public static void CleareRecords()
        {
            try
            {
                var connection = ConnectToDB();

                connection.Open();

                var query = "DELETE FROM Information";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.CommandTimeout = 10;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch
            {
                throw new Exception("Не удалось очистить историю");
            }
        }
    }
}
