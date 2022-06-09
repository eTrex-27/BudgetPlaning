using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlaning.Models
{
    public class Record
    {
        public string Date { get; set; }
        public string Summ { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Comment { get; set; }

        public Record(string Date, string Summ, string Type, string Category, string Comment)
        {
            this.Date = Date;
            this.Summ = Summ;
            this.Type = Type;
            this.Category = Category;
            this.Comment = Comment;
        }
    }
}
