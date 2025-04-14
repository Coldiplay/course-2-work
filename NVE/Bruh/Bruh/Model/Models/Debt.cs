using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruh.Model.Models
{
    public class Debt : IModel
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public decimal Summ { get; set; }
        public short AnnualInterest { get; set; }
    }
}
