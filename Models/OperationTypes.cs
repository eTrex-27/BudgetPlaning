using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Models
{
    public class OperationTypes
    {
        public static List<string> GetTypes()
        {
            return new List<string> { "Доход", "Расход" };
        }
    }
}
