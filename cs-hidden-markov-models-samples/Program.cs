using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathUtils.FT;
using HiddenMarkovModels.UT;

namespace HiddenMarkovModels.FT
{
    class Program
    {
        static void Main(string[] args)
        {
            //FTLogNormal.Run();
            //FTQuantileFunction.RunExample();
            //FTMedian.RunExample();
            //FTPercentileFunction.RunExample();
            //FTMergeSort.RunExample();
            //FTANCOVA.RunExample();
            FTMatrix.RunExample();

            //HiddenMarkovModelUT.Evaluate();
            //HiddenMarkovModelUT.Decode();

            //HiddenMarkovModelUT.MaximumLikelihoodLearning();
            //HiddenMarkvoModelUT.ViterbiLearning();
            //HiddenMarkvoModelUT.BaumWelchLearning();
            //HiddenMarkovModelUT.BaumWelchLearningContinuous();

            //HiddenMarkovClassifierUT.LearnAndPredict();
            HiddenMarkovClassifierUT.LearnAndPredictContinuous();

            //HiddenMarkovModelUT.Generate();
        }
    }
}
