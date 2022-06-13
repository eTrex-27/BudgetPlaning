using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Controllers
{
    public class Balance
    {
        public static double GetBalance()
        {
            var balance = 0.00;

            try
            {
                var connection = Connection.ConnectToDB();

                connection.Open();

                Connection.CreateTableIfNotExists(connection);

                balance = GetSumm("Доход") - GetSumm("Расход");

                connection.Close();
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
                var connection = Connection.ConnectToDB();

                connection.Open();

                var query = $"SELECT Summ, Type FROM Information WHERE Type = '{Type}'";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.CommandTimeout = 10;

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var value = Connection.ParseDouble(reader[0].ToString());
                            if (value != 0.00)
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
