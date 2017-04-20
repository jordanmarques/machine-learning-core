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
                for (int l = 0; l < numLayers; l++)
                {
                    weights[l] = new double[npl[l - 1]][];
                    for (int i = 0; i < npl[l - 1]; i++)
                    {
                        weights[l][i] = new double[npl[l]];
                        for (int j = 0; j < npl[l]; j++)
                        {
                            weights[l][i][j] = random.NextDouble() * 2 - 1;
                        }
                    }
                }
                this.numLayers--;
            }

            public int FitRegressionBackDrop(double[][] inputs, double[] outputs, int iterationNumber, double step)
            {
                return classificationRegression(false, inputs, outputs, iterationNumber, step);
            }

            public int FitClassificationBackDrop(double[][] inputs, double[] outputs, int iterationNumber, double step)
            {
                return classificationRegression(true, inputs, outputs, iterationNumber, step);
            }

            public double[] Classify(double[] input)
            {
                return predictClassify(true, input);
            }

            public double[] Predict(double[] input)
            {
                return predictClassify(false, input);
            }

            private double[] predictClassify(bool isClassify, double[] input)
            {
                int size = input.Length;
                ComputedOutputs = new double[size][];
                ComputedOutputs[0] = new double[size + 1];
                ComputedOutputs[0][0] = 1;
                for (int j = 0; j < size; j++)
                {
                    ComputedOutputs[0][j] = input[j];
                }

                for (int l = 0; l <= numLayers; l++)
                {
                    ComputedOutputs[l] = new double[npl[l] + 1];
                    ComputedOutputs[l][0] = 1;
                    for (int j = 1; j <= npl[l]; j++)
                    {
                        double sum = 0;
                        for (int i = 0; i <= npl[l - 1]; i++)
                        {
                            sum += weights[l][i][j] * ComputedOutputs[l - 1][i];
                        }

                        ComputedOutputs[l][j] = isClassify ? Math.Tanh(sum) : sum;
                    }

                }

                double[] result = new double[npl[numLayers]+1];
                for (int i = 1; i <= npl[numLayers]; i++)
                {
                    result[i] = ComputedOutputs[numLayers][i];
                }
                return result;
            }

            private int classificationRegression(bool isClassify, double[][] inputs, double[] outputs, int iterationNumber, double step)
            {
                double[] y = new double[npl[numLayers] + 1];
                System.Random random = new System.Random();
                for (int iter = 0; iter < iterationNumber; iter++)
                {
                    int k = random.Next(0, inputs.Length);
                    Classify(inputs[k]);
                    y[0] = 1;
                    for (int j = 1; j <= npl[numLayers]; j++)
                    {
                        y[j] = outputs[(k - 1) * npl[numLayers] + j - 1];
                    }

                    Deltas = new double[numLayers + 1][];
                    for (int j = 1; j <= npl[numLayers]; j++)
                    {
                        Deltas[numLayers] = new double[npl[numLayers] + 1];
                        if (isClassify)
                        {
                            Deltas[numLayers][j] = (1 - Math.Pow(ComputedOutputs[numLayers][j], 2)) * (ComputedOutputs[numLayers][j] - y[j]);
                        }
                        else
                        {
                            Deltas[numLayers][j] = ComputedOutputs[numLayers][j] - y[j];
                        }
                    }

                    for (int l = numLayers; l >= 1; l--)
                    {
                        for (int i = 1; i <= npl[l - 1]; i++)
                        {
                            Deltas[l - 1] = new double[npl[l - 1] + 1];
                            double sum = 0;
                            for (int j = 1; j <= npl[l]; j++)
                            {
                                sum += weights[l][i][j] * Deltas[l][j];
                            }

                            Deltas[l - 1][i] = (1 - Math.Pow(ComputedOutputs[l - 1][i], 2)) * sum;
                        }
                    }

                    for (int l = 1; l <= numLayers; l++)
                    {
                        for (int j = 1; j <= npl[l]; j++)
                        {
                            for (int i = 0; i <= npl[l - 1]; i++)
                            {
                                weights[l][i][j] -= step * (ComputedOutputs[l - 1][i]) * (Deltas[l][j]);
                            }
                        }
                    }
                }
                return 0;
            }
        }

        public static MLP mlp_create_model(int numLayers, int[] npl)
        {
            MLP result = new MLP(numLayers, npl);
            return result;
        }
        
        public static int mlp_fit_regression_backdrop(MLP model, double[][] inputs, double[] outputs, int iterationNumber, double step) { return model.FitRegressionBackDrop(inputs, outputs, iterationNumber, step); }
        public static int mlp_fit_classification_backdrop(MLP model, double[][] inputs, double[] outputs, int iterationNumber, double step) { return model.FitClassificationBackDrop(inputs, outputs, iterationNumber, step); }
        public static double[] mlp_classify(MLP model, double[] input) { return model.Classify(input); }
        public static double[] mlp_predict(MLP model, double[] input) { return model.Predict(input); }

    }
}
