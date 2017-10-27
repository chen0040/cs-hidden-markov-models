using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiddenMarkovModels.MathUtils.SpecialFunctions
{
    public class ClampFunction
    {
        public static double Clamp(double value, double lower, double upper)
        {
            return System.Math.Min(upper, System.Math.Max(value, lower));
        }
    }
}
