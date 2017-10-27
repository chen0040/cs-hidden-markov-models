# cs-hidden-markov-models

Hidden Markov Models using C#. Hidden Markov Models for sequence generation, event sequence prediction, supervised learning on temporal sequence. The library is built on .NET 4.5.2.

# Install

```bash
Install-Package cs-hidden-markov-models -Version 1.0.1
```

# Usage

## Generate / forecast sequence

The sample code below shows how to use HMM to generate a sequence based on historical data:

```cs
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
```

## Evaluate the likelihood of discrete event sequence

The sample codes below evaluate the likelihood of discrete event sequences based on historical data.

```cs 
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

```

## Evaluate the likelihood of continuous value sequence

The sample codes below evaluate the likelihood of continuous value sequence based on historical data. 

```cs
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
```

## HMM Classifier

The following sample codes shows how to train and predict temporal sequence using HMM as a supervised classifier:

```cs
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
```



