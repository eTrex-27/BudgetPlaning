using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Controllers
{
    public class Validation
    {
        public static (bool, string) ValidateSumField(string field)
        {
            string errorText = "Проверьте корректность суммы";
            bool valid = false;
            if (string.IsNullOrEmpty(field))
            {
                errorText = "Поле обязательно для заполнения";
                return (valid, errorText);
            }
            else if (field.StartsWith("-"))
            {
                errorText = "Введите положительное значение и выбирете Тип операции \"Расход\"";
                return (valid, errorText);
            }
            else if (field.StartsWith(".") || field.StartsWith(","))
            {
                errorText = "Сумма не может начинаться со знаков разделителей";
                return (valid, errorText);
            }
            else if (!ValidationSymbols(field) || !CheckRepeatSymbolsOfSplit(field))
            {
                errorText = "Поле содержит недопустимый формат";
                return (valid, errorText);
            }
            else if (field.Equals("0"))
            {
                errorText = "Сумма не может быть равна 0";
                return (valid, errorText);
            }
            else if (field.StartsWith("0"))
            {
                errorText = "Сумма не может начинаться с 0";
                return (valid, errorText);
            }
            else
            {
                valid = true;
                errorText = "";
                return (valid, errorText);
            }
        }

        private static bool ValidationSymbols(string field)
        {
            foreach (char symbol in field)
            {
                if (!"0123456789.,".Contains(symbol))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckRepeatSymbolsOfSplit(string field)
        {
            var splitSymbols = 0;
            foreach (char symbol in field)
            {
                if (".".Contains(symbol) || ",".Contains(symbol)) splitSymbols++;
                else splitSymbols = 0;

                if (splitSymbols > 1) return false;
            }
            return true;
        }
    }
}
