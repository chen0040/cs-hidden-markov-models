using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiddenMarkovModels.Helpers
{
    public class DiagnosticsHelper
    {
        public static void Assert(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
        }
    }
}
