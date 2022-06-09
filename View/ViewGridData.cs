using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlaning.Models;
using BudgetPlaning.Controllers;
using Microsoft.Data.Sqlite;

namespace BudgetPlaning.View
{
    public class ViewGridData
    {
        public static List<Record> GetRecords()
        {
            List<Record> records = new List<Record>();

            try
            {
                var connection = Connection.ConnectToDB();

                connection.Open();

                Connection.CreateTableIfNotExists(connection);

                records = Connection.GetData(connection);

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return records;
        }
    }
}
