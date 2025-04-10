using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruh.Model.Models
{
    public class Operation
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public DateTime TransactDate { get; set; }
        public DateTime DateOfCreate { get; set; }
        public bool Income { get; set; }
        public int CategoryID { get; set; }
        public int BankAccountID { get; set; }
        public string? Description { get; set; }
        public int? DebtID { get; set; }
        public int? PeriodicityID { get; set; }

    }
}
