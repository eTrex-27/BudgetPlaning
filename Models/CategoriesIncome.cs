using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Models
{
    public class CategoriesIncome : Categories
    {
        public override List<string> GetCategories()
        {
            return new List<string> { "Заработная плата",
                                      "Стипендия",
                                      "Пенсия",
                                      "Возврат долга",
                                      "Дивиденды"};
        }
    }
}
