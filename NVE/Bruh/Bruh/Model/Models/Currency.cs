﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruh.Model.Models
{
    public class Currency : IModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public char Symbol { get; set; }
    }
}
