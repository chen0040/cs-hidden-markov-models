using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.Learning.Unsupervised;

namespace HiddenMarkovModels.UT
{
    public partial class HiddenMarkovModelUT
    {
        public static void ViterbiLearning()
        {
            int[][] sequences = new int[][] 
               {
                   new int[] { 0,1,1,1,1,0,1,1,1,1 },
                   new int[] { 0,1,1,1,0,1,1,1,1,1 },
                   new int[] { 0,1,1,1,1,1,1,1,1,1 },
                   new int[] { 0,1,1,1,1,1         },
                   new int[] { 0,1,1,1,1,1,1       },
                   new int[] { 0,1,1,1,1,1,1,1,1,1 },
                   new int[] { 0,1,1,1,1,1,1,1,1,1 },
               };

            // Creates a new Hidden Markov Model with 3 states for
            //  an output alphabet of two characters (zero and one)
            HiddenMarkovModel hmm = new HiddenMarkovModel(state_count: 3, symbol_count: 2);

            // Try to fit the model to the data until the difference in
            //  the average log-likelihood changes only by as little as 0.0001
            var teacher = new ViterbiLearning(hmm) { Tolerance = 0.0001, Iterations = 0 };
            double ll = teacher.Run(sequences);

            // Calculate the probability that the given
            //  sequences originated from the model
            double l1 = hmm.Evaluate(new int[] { 0, 1 });       // 0.999
            double l2 = hmm.Evaluate(new int[] { 0, 1, 1, 1 }); // 0.916
            Console.WriteLine("l1: {0}", System.Math.Exp(l1));
            Console.WriteLine("l2: {0}", System.Math.Exp(l2));

            // Sequences which do not start with zero have much lesser probability.
            double l3 = hmm.Evaluate(new int[] { 1, 1 });       // 0.000
            double l4 = hmm.Evaluate(new int[] { 1, 0, 0, 0 }); // 0.000
            Console.WriteLine("l3: {0}", System.Math.Exp(l3));
            Console.WriteLine("l4: {0}", System.Math.Exp(l4));

            // Sequences which contains few errors have higher probability
            //  than the ones which do not start with zero. This shows some
            //  of the temporal elasticity and error tolerance of the HMMs.
            double l5 = hmm.Evaluate(new int[] { 0, 1, 0, 1, 1, 1, 1, 1, 1 }); // 0.034
            double l6 = hmm.Evaluate(new int[] { 0, 1, 1, 1, 1, 1, 1, 0, 1 }); // 0.034
            Console.WriteLine("l5: {0}", System.Math.Exp(l5));
            Console.WriteLine("l6: {0}", System.Math.Exp(l6));
        }
    }
}
