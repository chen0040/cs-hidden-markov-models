using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiddenMarkovModels.Topology
{
    public interface ITopology
    {
        int Create(out double[,] logTransitionMatrix, out double[] logInitialState);
    }
}
