using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathUtils.Statistics;

namespace HiddenMarkovModels.MathUtils.FT
{
    public class FTMedian
    {
        public static void RunExample()
        {
            double[] a = new double[] { 4, 2, 6, 3, 5, 6, 1, 10, 10, 11, 32, 12 };
            Console.WriteLine("Median: {0}", Median.GetMedian(a));
        }
    }
}
