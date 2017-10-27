using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathHelpers;

namespace HiddenMarkovModels.MathUtils.SpecialFunctions
{
    public class GammaFunction
    {
        public static double GetGamma(double x)
        {
            return System.Math.Exp(Gamma.Log(x));
        }
    }
}
