using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static alglib;
using Random = System.Random;

namespace MyDll
{
    public static class Source
    {
        public static double[] linear_create_model(int inputDimension)
        {
            double[] model = new double[inputDimension + 1];
            Random random = new Random();
            for (int i = 0; i <= inputDimension; i++)
            {
                model[i] = random.NextDouble() * 2 - 1;
            }

            return model;
        }

        public static int linear_fit_regression(ref double[] model, double[,] inputs, double[] outputs)
        {
            int i;
            alglib.matinvreport mr;

            double[,] Y = ToRectangular(outputs, 1);
            double[,] X = new double[inputs.GetLength(0), inputs.GetLength(1) + 1];
            for (int j = 0; j < inputs.GetLength(0); j++)
            {
                X[j, 0] = 1;
                for (int k = 0; k < inputs.GetLength(1); k++)
                {
                    X[j, k + 1] = inputs[j, k];
                }
            }

            double[,] Xt = Transpose(X);
            double[,] XtX = MultiplyMatrix(Xt, X);
            rmatrixinverse(ref XtX , out i, out mr);
            double[,] XtXXt = MultiplyMatrix(XtX, Xt);
            double[,] result = MultiplyMatrix(XtXXt, Y);
            double[] linearResult = new double[0];
            ToLinear(ref linearResult, result);
            model = linearResult;

            return 0;
        }

        public static int linear_fit_classification_rosenblatt(ref double[] model, double[,] inputs, double[] outputs, int iterationNumber, double step)
        {
            double[,] X = new double[inputs.GetLength(0), inputs.GetLength(1) + 1];
            for (int j = 0; j < inputs.GetLength(0); j++)
            {
                X[j, 0] = 1;
                for (int k = 0; k < inputs.GetLength(1); k++)
                {
                    X[j, k + 1] = inputs[j, k];
                }
            }

            System.Random random = new System.Random();
            
            int len1 = X.GetLength(1);

            double[,] rndInput = new double[len1, 1];
            double[] flatRndInput = new double[len1 - 1];
            double[,] rndOutput = new double[1, 1];

            for (int i = 0; i < iterationNumber; i++)
            {
                int num = random.Next(0, outputs.Length);

                for (int j = 0; j < len1; j++)
                {
                    rndInput[j, 0] = X[num, j];
                    if (j > 0)
                    {
                        flatRndInput[j-1] = X[num, j];
                    }
                }

                rndOutput[0, 0] = outputs[num];

                double linearClassifyRes = linear_classify(ref model, flatRndInput);

				double[,] sub = new double[1, 1];
				sub[0, 0] = linearClassifyRes;

				double[,] subRes = SubstractMatrix(rndOutput, sub);
				double[,] multiplyRes = MultiplyMatrix(rndInput, subRes);
                double[,] rectModel = ToRectangular(model, 1);
                
                rectModel = AdditionMatrix(rectModel, MultiplyMatrixScalar(step, multiplyRes));
				ToLinear(ref model, rectModel);

			}

            return 0;
        }

        public static double linear_classify(ref double[] model, double[] input)
        {
			int inputSize = input.Length;

			double result = 1 * model[0];

			for (int i = 0; i < inputSize; i++)
			{
				result += model[i + 1] * input[i];
			}
			return Math.Tanh(result);
        }

        public static double linear_predict(ref double[] model, double[] input)
        {
            int inputSize = input.Length;

            double result = 1 * model[0];

            for (int i = 0; i < inputSize; i++)
            {
                result += model[i + 1] * input[i];
            }
            return result;
        }

        public static double[,] Transpose(double[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);
            double[,] result = new double[h, w];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j]; 
                    
                }
            } return result;
        }

        public static double[,] ToRectangular(double[] flatArray, int width)
        {
            int height = (int)Math.Ceiling(flatArray.Length / (double)width);
            double[,] result = new double[height, width];
            int rowIndex, colIndex;

            for (int index = 0; index < flatArray.Length; index++)
            {
                rowIndex = index / width; colIndex = index % width; result[rowIndex, colIndex] = flatArray[index];
            }

            return result;
        }

        public static int ToLinear(ref double[] result, double[,] rectangularArray)
        {
            int lineSize = rectangularArray.GetLength(1);
            result = new double[rectangularArray.GetLength(0) * lineSize];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = rectangularArray[i / lineSize, i % lineSize];
            }

            return lineSize;

        }

        public static double[,] MultiplyMatrix(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double[,] result = new double[rA, cB];
            if (cA != rB)
            {
                Console.WriteLine("matrix cannot be multiplied !!!");
                result = null;
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        double temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        result[i, j] = temp;
                    }
                }
            }
            return result;
        }

        public static double[,] MultiplyMatrixScalar(double A, double[,] B)
        {
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double[,] result = new double[rB,cB];
            for (int i = 0; i < rB; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    result[i, j] = A * B[i, j];
                }
            }
            return result;
        }

        public static double[,] AdditionMatrix(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double[,] result = new double[rA, cA];
            if (cA != cB || rA != rB)
            {
                Console.WriteLine("matrix cannot be added !!!");
                result = null;
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cA; j++)
                    {
                        result[i, j] = A[i, j] + B[i, j];
                    }
                }
            }
            return result;
        }

		public static double[,] SubstractMatrix(double[,] A, double[,] B)
		{
			int rA = A.GetLength(0);
			int cA = A.GetLength(1);
			int rB = B.GetLength(0);
			int cB = B.GetLength(1);
			double[,] result = new double[rA, cA];
			if (cA != cB || rA != rB)
			{
				Console.WriteLine("matrix cannot be added !!!");
                result = null;
			}
			else
			{
				for (int i = 0; i < rA; i++)
				{
					for (int j = 0; j < cA; j++)
					{
						result[i, j] = A[i, j] - B[i, j];
					}
				}
			}
			return result;
		}

    }
}


