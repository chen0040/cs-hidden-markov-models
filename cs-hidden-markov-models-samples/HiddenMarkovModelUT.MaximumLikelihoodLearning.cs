using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.Learning.Supervised;

namespace HiddenMarkovModels.UT
{
    public partial class HiddenMarkovModelUT
    {
        public static void MaximumLikelihoodLearning()
        {
            int[][] observations = 
             {
                 new int[] { 0,0,0,1,0,0 }, 
                 new int[] { 1,0,0,1,0,0 },
                 new int[] { 0,0,1,0,0,0 },
                 new int[] { 0,0,0,0,1,0 },
                 new int[] { 1,0,0,0,1,0 },
                 new int[] { 0,0,0,1,1,0 },
                 new int[] { 1,0,0,0,0,0 },
                 new int[] { 1,0,1,0,0,0 },
             };

            // Now those are the visible states associated with each observation in each 
            // observation sequence above. Note that there is always one state assigned
            // to each observation, so the lengths of the sequence of observations and 
            // the sequence of states must always match.

            int[][] paths = 
             {
                 new int[] { 0,0,1,0,1,0 },
                 new int[] { 1,0,1,0,1,0 },
                 new int[] { 1,0,0,1,1,0 },
                 new int[] { 1,0,1,1,1,0 },
                 new int[] { 1,0,0,1,0,1 },
                 new int[] { 0,0,1,0,0,1 },
                 new int[] { 0,0,1,1,0,1 },
                 new int[] { 0,1,1,1,0,0 },
             };

            // Create our Markov model with two states (0, 1) and two symbols (0, 1)
            HiddenMarkovModel model = new HiddenMarkovModel(state_count: 2, symbol_count: 2);

            // Now we can create our learning algorithm
            MaximumLikelihoodLearning teacher = new MaximumLikelihoodLearning(model)
            {
                // Set some options
                UseLaplaceRule = false
            };

            // and finally learn a model using the algorithm
            double logLikelihood = teacher.Run(observations, paths);


            // To check what has been learned, we can extract the emission
            // and transition matrices, as well as the initial probability
            // vector from the HMM to compare against expected values:

            double[] pi = model.ProbabilityVector; // { 0.5, 0.5 }
            double[,] A = model.TransitionMatrix;    // { { 7/20, 13/20 }, { 14/20, 6/20 } }
            double[,] B = model.EmissionMatrix;      // { { 17/25, 8/25 }, { 19/23, 4/23 } }

            Console.WriteLine("pi: {{{0}}}", string.Join(", ", pi));
            Console.WriteLine("A: {0}", ToString(A));
            Console.WriteLine("B: {0}", ToString(B));
        }

        private static string ToString(double[,] matrix)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.Append("{ ");
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    if (j == 0)
                    {
                        sb.AppendFormat("{0}", matrix[i, j]);
                    }
                    else
                    {
                        sb.AppendFormat(", {0}", matrix[i, j]);
                    }
                }
                sb.Append(" }");
            }
            sb.Append(" }");

            return sb.ToString();
        }

    }
}
