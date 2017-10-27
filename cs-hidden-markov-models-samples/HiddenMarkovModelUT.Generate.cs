using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.Learning.Unsupervised;
using HiddenMarkovModels.Topology;
using HiddenMarkovModels.Helpers;

namespace HiddenMarkovModels.UT
{
    public partial class HiddenMarkovModelUT
    {
        public static void Generate()
        {
            MathHelper.SetupGenerator(42);

            // Consider some phrases:
            //
            string[][] phrases =
             {
                 new[] { "those", "are", "sample", "words", "from", "a", "dictionary" },
                 new[] { "those", "are", "sample", "words" },
                 new[] { "sample", "words", "are", "words" },
                 new[] { "those", "words" },
                 new[] { "those", "are", "words" },
                 new[] { "words", "from", "a", "dictionary" },
                 new[] { "those", "are", "words", "from", "a", "dictionary" }
             };

            // Let's begin by transforming them to sequence of
            // integer labels using a codification codebook:
            var codebook = new Codification(phrases);

            // Now we can create the training data for the models:
            int[][] sequence = codebook.Translate(phrases);

            // To create the models, we will specify a forward topology,
            // as the sequences have definite start and ending points.
            //
            var topology = new Forward(state_count: 4);
            int symbols = codebook.SymbolCount; // We have 7 different words
            Console.WriteLine("Symbol Count: {0}", symbols);

            // Create the hidden Markov model
            HiddenMarkovModel hmm = new HiddenMarkovModel(topology, symbols);

            // Create the learning algorithm
            BaumWelchLearning teacher = new BaumWelchLearning(hmm);

            // Teach the model about the phrases
            double error = teacher.Run(sequence);

            // Now, we can ask the model to generate new samples
            // from the word distributions it has just learned:
            //
            int[] sample = hmm.Generate(3);

            // And the result will be: "those", "are", "words".
            string[] result = codebook.Translate(sample);

            foreach(string result_word in result)
            {
                Console.WriteLine(result_word);
            }
        }
    }
}
