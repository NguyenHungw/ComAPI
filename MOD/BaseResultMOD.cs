﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD
{
    public class BaseResultMOD
    {
        public int Status { get; set; } = 0;
        public string Message { get; set; } = "";
        public object Data { get; set; }
        public int TotalRow { get; set; }
    }
}
