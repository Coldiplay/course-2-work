﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruh.Model.Models
{
    public class Bank : IModel
    {
        public int ID { get; set; }
        public string Title { get; set; }

        //public override bool Equals(object? obj)
        //{
        //    return obj != null && this.ID == (obj as Bank).ID;
        //}
    }
}
