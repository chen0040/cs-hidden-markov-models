using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiddenMarkovModels.MathUtils.Statistics;

namespace HiddenMarkovModels.MathUtils.FT
{
    public class FTANCOVA
    {
        public static void RunExample()
        {
            Tuple<double, double, int>[] data = new Tuple<double, double, int>[]{
                Tuple.Create(5.0, 20.0, 1),
                Tuple.Create(10.0, 23.0, 1),
                Tuple.Create(12.0, 30.0, 1),
                Tuple.Create(9.0, 25.0, 1),
                Tuple.Create(23.0, 34.0, 1),
                Tuple.Create(21.0, 40.0, 1),
                Tuple.Create(14.0, 27.0, 1),
                Tuple.Create(18.0, 38.0, 1),
                Tuple.Create(6.0, 24.0, 1),
                Tuple.Create(13.0, 31.0, 1),
                Tuple.Create(7.0, 19.0, 2),
                Tuple.Create(12.0, 26.0, 2),
                Tuple.Create(27.0, 33.0, 2),
                Tuple.Create(24.0, 35.0, 2),
                Tuple.Create(18.0, 30.0, 2),
                Tuple.Create(22.0, 31.0, 2),
                Tuple.Create(26.0, 34.0, 2),
                Tuple.Create(21.0, 28.0, 2),
                Tuple.Create(14.0, 23.0, 2),
                Tuple.Create(9.0, 22.0, 2),
            };

            double[] x = new double[data.Length];
            double[] y = new double[data.Length];
            int[] grpCat = new int[data.Length];

            for(int i=0; i < data.Length; ++i)
            {
                x[i]=data[i].Item1;
                y[i]=data[i].Item2;
                grpCat[i]=data[i].Item3;
            }

            ANCOVA output = null;
            ANCOVA.RunANCOVA(x, y, grpCat, out output);
            Console.WriteLine(output.Summary);

        }
    }
}
