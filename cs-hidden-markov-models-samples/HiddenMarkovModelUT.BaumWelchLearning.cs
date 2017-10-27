using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.Learning.Unsupervised;
using HiddenMarkovModels.MathUtils.Distribution;
using HiddenMarkovModels.Topology;

namespace HiddenMarkovModels.UT
{
    public partial class HiddenMarkovModelUT
    {
        public static void BaumWelchLearning()
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
            var teacher = new BaumWelchLearning(hmm) { Tolerance = 0.0001, Iterations = 0 };
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

        public static void BaumWelchLearningContinuous()
        {
            // Create continuous sequences. In the sequences below, there
            //  seems to be two states, one for values between 0 and 1 and
            //  another for values between 5 and 7. The states seems to be
            //  switched on every observation.
            double[][] sequences = new double[][] 
            {
                new double[] { 0.1, 5.2, 0.3, 6.7, 0.1, 6.0 },
                new double[] { 0.2, 6.2, 0.3, 6.3, 0.1, 5.0 },
                new double[] { 0.1, 7.0, 0.1, 7.0, 0.2, 5.6 },
            };
             
                         
            // Specify a initial normal distribution for the samples.
            Gaussian density = new Gaussian();
             
            // Creates a continuous hidden Markov Model with two states organized in a forward
            //  topology and an underlying univariate Normal distribution as probability density.
            var model = new HiddenMarkovModel(new Ergodic(2), density);
             
            // Configure the learning algorithms to train the sequence classifier until the
            // difference in the average log-likelihood changes only by as little as 0.0001
            var teacher = new BaumWelchLearning(model)
            {
                Tolerance = 0.0001,
                Iterations = 0,
            };
             
            // Fit the model
            double logLikelihood = teacher.Run(sequences);
             
            // See the log-probability of the sequences learned
            double a1 = model.Evaluate(new double[] { 0.1, 5.2, 0.3, 6.7, 0.1, 6.0 }); // -0.12799388666109757
            double a2 = model.Evaluate(new double[] { 0.2, 6.2, 0.3, 6.3, 0.1, 5.0 }); // 0.01171157434400194
            Console.WriteLine("a1 = {0}", a1);
            Console.WriteLine("a2 = {0}", a2);
            
            // See the log-probability of an unrelated sequence
            double a3 = model.Evaluate(new[] { 1.1, 2.2, 1.3, 3.2, 4.2, 1.0 }); // -298.7465244473417
            Console.WriteLine("a3 = {0}", a3);
            
            // We can transform the log-probabilities to actual probabilities:
            double likelihood = System.Math.Exp(logLikelihood);
            a1 = System.Math.Exp(a1); // 0.879
            a2 = System.Math.Exp(a2); // 1.011
            a3 = System.Math.Exp(a3); // 0.000
            Console.WriteLine("a1 = {0}", a1);
            Console.WriteLine("a2 = {0}", a2);
            Console.WriteLine("a3 = {0}", a3);
               
            // We can also ask the model to decode one of the sequences. After
            // this step the state variable will contain: { 0, 1, 0, 1, 0, 1 }

            double lll;
            int[] states = model.Decode(new double[] { 0.1, 5.2, 0.3, 6.7, 0.1, 6.0 }, out lll);
            Console.WriteLine("states: {{{0}}}", string.Join(", ", states));
            Console.WriteLine("lll: {0}", lll);
        }
    }
}
