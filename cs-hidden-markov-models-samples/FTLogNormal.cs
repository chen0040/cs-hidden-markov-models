using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathUtils.Distribution;

namespace HiddenMarkovModels.MathUtils.FT
{
    public class FTLogNormal
    {
        public static void Run()
        {
            LogNormal ln = new LogNormal(5.13, 0.17);
            Gaussian normal_distribution = ln.ToNormal();
            Console.WriteLine("Geometric Mean: {0} Geometric Standard Deviation: {1}", ln.GeometricMean, ln.GeometricStdDev);
            Console.WriteLine("Normal: ({0}, {1})", normal_distribution.Mean, normal_distribution.StdDev);

        }
    }
}
