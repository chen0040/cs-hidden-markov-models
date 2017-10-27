using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.Learning.Supervised;
using HiddenMarkovModels.Learning.Unsupervised;
using HiddenMarkovModels.Topology;
using HiddenMarkovModels.MathUtils.Distribution;

namespace HiddenMarkovModels.UT
{
    public partial class HiddenMarkovClassifierUT
    {
        public static void Learn()
        {
            int[][] inputs = new int[][]
               {
                   new int[] { 0,1,1,0 },   // Class 0
                   new int[] { 0,0,1,0 },   // Class 0
                   new int[] { 0,1,1,1,0 }, // Class 0
                   new int[] { 0,1,0 },     // Class 0
               
                   new int[] { 1,0,0,1 },   // Class 1
                   new int[] { 1,1,0,1 },   // Class 1
                   new int[] { 1,0,0,0,1 }, // Class 1
                   new int[] { 1,0,1 },     // Class 1
               };

            int[] outputs = new int[]
               {
                   0,0,0,0, // First four sequences are of class 0
                   1,1,1,1, // Last four sequences are of class 1
               };


            // We are trying to predict two different classes
            int classes = 2;

            // Each sequence may have up to two symbols (0 or 1)
            int symbols = 2;

            // Nested models will have two states each
            int[] states = new int[] { 2, 2 };

            // Creates a new Hidden Markov Model Sequence Classifier with the given parameters
            HiddenMarkovClassifier classifier = new HiddenMarkovClassifier(classes, states, symbols);

            // Create a new learning algorithm to train the sequence classifier
            var teacher = new HiddenMarkovClassifierLearning(classifier,

                // Train each model until the log-likelihood changes less than 0.001
                modelIndex => new BaumWelchLearning(classifier.Models[modelIndex])
                {
                    Tolerance = 0.001,
                    Iterations = 0
                }
            );

            // Train the sequence classifier using the algorithm
            double likelihood = teacher.Run(inputs, outputs);

            Console.WriteLine("likelihood: {0}", likelihood);
        }

        public static void LearnAndPredict()
        {
            // Suppose we would like to learn how to classify the
            // following set of sequences among three class labels: 

            int[][] inputSequences =
            {
                // First class of sequences: starts and
                // ends with zeros, ones in the middle:
                new[] { 0, 1, 1, 1, 0 },        
                new[] { 0, 0, 1, 1, 0, 0 },     
                new[] { 0, 1, 1, 1, 1, 0 },     

                // Second class of sequences: starts with
                // twos and switches to ones until the end.
                new[] { 2, 2, 2, 2, 1, 1, 1, 1, 1 },
                new[] { 2, 2, 1, 2, 1, 1, 1, 1, 1 },
                new[] { 2, 2, 2, 2, 2, 1, 1, 1, 1 },

                // Third class of sequences: can start
                // with any symbols, but ends with three.
                new[] { 0, 0, 1, 1, 3, 3, 3, 3 },
                new[] { 0, 0, 0, 3, 3, 3, 3 },
                new[] { 1, 0, 1, 2, 2, 2, 3, 3 },
                new[] { 1, 1, 2, 3, 3, 3, 3 },
                new[] { 0, 0, 1, 1, 3, 3, 3, 3 },
                new[] { 2, 2, 0, 3, 3, 3, 3 },
                new[] { 1, 0, 1, 2, 3, 3, 3, 3 },
                new[] { 1, 1, 2, 3, 3, 3, 3 },
            };

                        // Now consider their respective class labels
                        int[] outputLabels =
            {
                /* Sequences  1-3 are from class 0: */ 0, 0, 0,
                /* Sequences  4-6 are from class 1: */ 1, 1, 1,
                /* Sequences 7-14 are from class 2: */ 2, 2, 2, 2, 2, 2, 2, 2
            };


            // We will use a single topology for all inner models, but we 
            // could also have explicit led different topologies for each:

            ITopology forward = new Forward(state_count: 3);

            // Now we create a hidden Markov classifier with the given topology
            HiddenMarkovClassifier classifier = new HiddenMarkovClassifier(class_count: 3, topology: forward, symbol_count: 4);

            // And create a algorithms to teach each of the inner models
            var teacher = new HiddenMarkovClassifierLearning(classifier,

                // We can specify individual training options for each inner model:
                modelIndex => new BaumWelchLearning(classifier.Models[modelIndex])
                {
                    Tolerance = 0.001, // iterate until log-likelihood changes less than 0.001
                    Iterations = 0     // don't place an upper limit on the number of iterations
                });


            // Then let's call its Run method to start learning
            double error = teacher.Run(inputSequences, outputLabels);


            // After training has finished, we can check the 
            // output classificaton label for some sequences. 

            int y1 = classifier.Compute(new[] { 0, 1, 1, 1, 0 });    // output is y1 = 0
            int y2 = classifier.Compute(new[] { 0, 0, 1, 1, 0, 0 }); // output is y1 = 0

            int y3 = classifier.Compute(new[] { 2, 2, 2, 2, 1, 1 }); // output is y2 = 1
            int y4 = classifier.Compute(new[] { 2, 2, 1, 1 });       // output is y2 = 1

            int y5 = classifier.Compute(new[] { 0, 0, 1, 3, 3, 3 }); // output is y3 = 2
            int y6 = classifier.Compute(new[] { 2, 0, 2, 2, 3, 3 }); // output is y3 = 2

            Console.WriteLine("y1 = {0}", y1);
            Console.WriteLine("y2 = {0}", y2);
            Console.WriteLine("y3 = {0}", y3);
            Console.WriteLine("y4 = {0}", y4);
            Console.WriteLine("y5 = {0}", y5);
            Console.WriteLine("y6 = {0}", y6);
        }

        public static void LearnAndPredictContinuous()
        {
            // Create a Continuous density Hidden Markov Model Sequence Classifier
            // to detect a univariate sequence and the same sequence backwards.
            double[][] sequences = new double[][] 
            {
                new double[] { 0,1,2,3,4 }, // This is the first  sequence with label = 0
                new double[] { 4,3,2,1,0 }, // This is the second sequence with label = 1
            };
               
            // Labels for the sequences
            int[] labels = { 0, 1 };
            
            // Creates a new Continuous-density Hidden Markov Model Sequence Classifier
            //  containing 2 hidden Markov Models with 2 states and an underlying Normal
            //  distribution as the continuous probability density.
            Gaussian density = new Gaussian();
            var classifier = new HiddenMarkovClassifier(2, new Ergodic(2), density);
            
            // Create a new learning algorithm to train the sequence classifier
            var teacher = new HiddenMarkovClassifierLearning(classifier,
            
                // Train each model until the log-likelihood changes less than 0.001
                modelIndex => new BaumWelchLearning(classifier.Models[modelIndex])
                {
                    Tolerance = 0.0001,
                    Iterations = 0
                }
            );
               
            // Train the sequence classifier using the algorithm
            teacher.Run(sequences, labels);
               
               
            // Calculate the probability that the given
            //  sequences originated from the model
            double likelihood;
               
            // Try to classify the first sequence (output should be 0)
            int c1 = classifier.Compute(sequences[0], out likelihood);
            Console.WriteLine("c1: {0}", c1);
               
            // Try to classify the second sequence (output should be 1)
            int c2 = classifier.Compute(sequences[1], out likelihood);
            Console.WriteLine("c2: {0}", c2);
        }
    }
}
