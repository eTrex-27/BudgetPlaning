using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Models
{
    public  class CategoriesExpense : Categories
    {
        public override List<string> GetCategories()
        {
            return new List<string> { "Супермаркеты",
                                      "Развлечения",
                                      "Еда",
                                      "Транспорт",
                                      "Коммунальные платежи",
                                      "Кафе и рестораны",
                                      "Прочие расходы"};
        }
    }
}
