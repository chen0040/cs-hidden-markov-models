using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathUtils.LinearAlgebra;

namespace HiddenMarkovModels.MathUtils.FT
{
    public class FTMatrix
    {
        public static void RunExample()
        {
            double[][] A = new double[4][]{
                new double[4] { 1, 0, 2, -1},
                new double[4] { 3, 0, 0, 5},
                new double[4] { 2, 1, 4, -3},
                new double[4] { 1, 0, 5, 0}
            };

            RunExample(A);

            A = new double[3][]{
                new double[3] { -2, 2, -3},
                new double[3] { -1, 1, 3},
                new double[3] { 2, 0, -1}
            };

            RunExample(A);

            A = new double[3][]{
                new double[3] { 1, 2, 3},
                new double[3] { 4, 5, 6},
                new double[3] { 7, 8, 2}
            };

            RunExample(A);
        }

        public static void RunExample(double[][] A)
        {
            Console.WriteLine("A = {0}", MatrixOp.Summary(A));

            double[][] C = MatrixOp.GetUpperTriangularMatrix(A);
            Console.WriteLine("C = {0}", MatrixOp.Summary(C));

            double detA = MatrixOp.GetDeterminant(A);

            Console.WriteLine("det(A) = {0}", detA);
        }
    }
}
