using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static alglib;

namespace _2017_5A_AL2_MyFirstNativeDllForUnity
{
    public static class SourceMulti
    {
        public class MLP
        {
            public double[][][] weights;
            public double[][] ComputedOutputs;
            public double[][] ComputedSum;
            public double[][] Deltas;
            public int numLayers;
            public int[] npl;

            public MLP(int numLayers, int[] npl)
            {
                this.numLayers = numLayers;
                this.npl = npl;
                weights = new double[numLayers][][];
                Random random = new Random();
                for(int l=0; l<numLayers;l++)
                {
                    weights[l] = new double[npl[l-1]][];
                    for (int i=0;i < npl[l-1];i++)
                    {
                        weights[l][i] = new double[npl[l]];
                        for(int j=0; j<npl[l];j++)
                        {
                            weights[l][i][j] = random.NextDouble() * 2 - 1;
                        }
                    }
                }
            }

            public int FitRegressionBackDrop(double[,] inputs)
            {
                return 0;
            }

            public int FitClassificationBackDrop(double[,] inputs)
            {
                return 0;
            }

            public double[] Classify(double[] input)
            {

                int size = input.Length;
                double[][] x = new double[size][];
                x[0] = new double[size+1];
                x[0][0] = 1;
                for(int j = 0; j< size; j++)
                {
                    x[0][j] = input[j];
                }

                for (int l = 0; l<numLayers;  l++)
                {
                    x[1][0] = 1;
                    for(int j = 1; j<  ; j++)
                    {
                        for(int i = 0; i < l -1; i++)
                        {
                            //double Sum += weights[l][i][j] * x[]
                        }

                        x[l][j] = Math.Tanh())
                    }

                }
                return null;
            }

            public double[] Predict(double[] input)
            {
                return null;
            }
        }

        public static MLP mlp_create_model(int numLayers, int[] npl)
        {
            MLP result = new MLP(numLayers, npl);
            return result;
        }
        
        public static int mlp_fit_regression_backdrop(MLP model, double[,] inputs) { return model.FitRegressionBackDrop(inputs); } // **** Params manquants ?
        public static int mlp_fit_classification_backdrop(MLP model, double[,] inputs) { return model.FitClassificationBackDrop(inputs); } // **** Params manquants ?
        public static double[] mlp_classify(MLP model, double[] input) { return model.Classify(input); }
        public static double[] mlp_predict(MLP model, double[] input) { return model.Predict(input); }

    }
}
