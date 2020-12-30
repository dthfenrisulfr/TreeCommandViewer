using System;
using System.Collections.Generic;
using System.Text;

namespace TreeService.Builder
{
    public static class BinariCodeEnum
    {
        public static Dictionary<int, string> Code { get; set; }

        public static void Build()
        {
            Code.Add(1, "0");
            Code.Add(1, "car(0)");
            Code.Add(2, "cdr(0)");
            Code.Add(3, "car(car(0))");
            Code.Add(4, "cdr(car(0))");
            Code.Add(5, "car(0)");
            Code.Add(6, "car(0)");
            Code.Add(7, "car(0)");
            Code.Add(8, "car(0)");
            Code.Add(9, "car(0)");
            Code.Add(10, "car(0)");
            Code.Add(11, "car(0)");
            Code.Add(12, "car(0)");
            Code.Add(13, "car(0)");
            Code.Add(14, "car(0)");
            Code.Add(15, "car(0)");
            Code.Add(16, "car(0)");
            Code.Add(17, "car(0)");
            Code.Add(18, "car(0)");
            Code.Add(19, "car(0)");
            Code.Add(20, "car(0)");
            Code.Add(21, "car(0)");
            Code.Add(22, "car(0)");
            Code.Add(23, "car(0)");
            Code.Add(24, "car(0)");
            Code.Add(25, "car(0)");
            Code.Add(26, "car(0)");
            Code.Add(27, "car(0)");
            Code.Add(28, "car(0)");
            Code.Add(29, "car(0)");
            Code.Add(30, "car(0)");
        }
    }
}
