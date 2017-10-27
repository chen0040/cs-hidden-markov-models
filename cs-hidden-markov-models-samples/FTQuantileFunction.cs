using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathUtils.Statistics;
using HiddenMarkovModels.MathUtils.SpecialFunctions;
using HiddenMarkovModels.MathUtils.Distribution;

namespace HiddenMarkovModels.MathUtils.FT
{
    public class FTQuantileFunction
    {
        public static void RunExample()
        {
            double prob = 0.95;
            double prob1 = (1 - prob) / 2;
            double prob2 = 1 - prob1;
            double z1 = Gaussian.GetQuantile(prob1);
            double z2 = Gaussian.GetQuantile(prob2);

            Console.WriteLine("{0:0.000}", z1);
            Console.WriteLine("{0:0.000}", z2);
        }
    }
}
