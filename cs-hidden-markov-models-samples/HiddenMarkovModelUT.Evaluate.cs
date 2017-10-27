using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiddenMarkovModels.UT
{
    public partial class HiddenMarkovModelUT
    {
        public static void Evaluate()
        {
            double[,] transition =
            {
                {0.7, 0.3},
                {0.4, 0.6}
            };

            double[,] emission =
            {
                { 0.1, 0.4, 0.5},
                { 0.6, 0.3, 0.1}
            };

            double[] initial = { 0.6, 0.4 };

            HiddenMarkovModel hmm = new HiddenMarkovModel(transition, emission, initial);
            int[] sequence = new int[] { 0, 1, 2 };

            double logLikeliHood = hmm.Evaluate(sequence);

            // At this point, the log-likelihood of the sequence
            // occurring within the model is -3.3928721329161653.
            Console.WriteLine("logLikeliHood: {0}", logLikeliHood);
        }

        public static void Decode()
        {
            double[,] transition =
            {
                {0.7, 0.3},
                {0.4, 0.6}
            };

            double[,] emission =
            {
                { 0.1, 0.4, 0.5},
                { 0.6, 0.3, 0.1}
            };

            double[] initial = { 0.6, 0.4 };

            HiddenMarkovModel hmm = new HiddenMarkovModel(transition, emission, initial);
            int[] sequence = new int[] { 0, 1, 2 };

            // At this point, the state path will be 1-0-0 and the
            // log-likelihood will be -4.3095199438871337
            double logLikelihood;
            int[] path = hmm.Decode(sequence, out logLikelihood);

            Console.WriteLine("Path: {0}", string.Join("-", path));
            Console.WriteLine("logLikelihood: {0}", logLikelihood);
        }
    }
}
