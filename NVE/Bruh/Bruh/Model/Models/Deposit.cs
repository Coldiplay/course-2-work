using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruh.Model.Models
{
    public class Deposit : IModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal InitalSumm { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public bool Capitalization { get; set; }
        public int InterestRate { get; set; }
        public string PeriodicityOfPayment { get; set; }
        public int BankID { get; set; }
        public int TypeOfDeposit { get; set; }
        public int CurrencyID { get; set; }
    }
}
