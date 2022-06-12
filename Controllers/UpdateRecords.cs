using BudgetPlaning.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Controllers
{
    public class UpdateRecords
    {
        public static void Update(int id, string columnName, string value)
        {
            try
            {
                var connection = Connection.ConnectToDB();

                connection.Open();

                if (columnName.Equals("Summ")) value = value.Replace("$", "").Replace(".00", "");

                switch (columnName)
                {
                    case "Date": ValdationDate(value); break;
                    case "Summ": ValdationSumm(value); break;
                    case "Type": ValdationType(value); break;
                }

                var query = $"UPDATE Information SET {columnName} = '{value}' WHERE ID = {id + 1}";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void ValdationType(string value)
        {
            try
            {
                if (!OperationTypes.GetTypes().Contains(value))
                {
                    throw new Exception("Тип может быть только \"Доход\" или \"Расход\"");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void ValdationDate(string value)
        {
            try
            {
                if (!DateTime.TryParseExact(value, "dd.MM.yyyy, HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
                {
                    throw new Exception($"Значение даты {value} некорректно, введите в формате \"день.месяц.год, часы:минуты\"");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void ValdationSumm(string value)
        {
            try
            {
                var validate = Validation.ValidateSumField(value);
                if (!validate.Item1 && !string.IsNullOrEmpty(validate.Item2))
                {
                    throw new Exception(validate.Item2);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
