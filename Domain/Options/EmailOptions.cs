﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Options
{
    public class EmailOptions
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToEmail { get; set; }
    }
}
