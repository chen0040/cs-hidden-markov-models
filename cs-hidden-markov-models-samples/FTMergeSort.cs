using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathHelpers;

namespace HiddenMarkovModels.MathHelpers.FT
{
    public class FTMergeSort
    {
        public static void RunExample()
        {
            double[] a = new double[] { 4, 2, 6, 3, 5, 6, 1, 10, 10, 11, 32, 12 };
            for (int i = 0; i < a.Length; ++i)
            {
                Console.Write("{0} ", a[i]);
            }
            Console.WriteLine();
            MergeSort.Sort(a);
            for (int i = 0; i < a.Length; ++i)
            {
                Console.Write("{0} ", a[i]);
            }
            Console.WriteLine();
        }
    }
}
