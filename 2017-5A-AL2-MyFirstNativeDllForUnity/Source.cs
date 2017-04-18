using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static alglib;

namespace MyDll
{
    public static class Source
    {
        public static IntPtr linear_create_model(int inputDimension)
        {
            double[] model = new double[inputDimension];
            for (int i = 0; i < inputDimension; i++)
            {
                Random random = new Random();
                model[i] = random.NextDouble();
            }

            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(model));
            Marshal.StructureToPtr(model, intPtr, false);

            return intPtr;
        }

        public static void linear_remove_model(IntPtr model)
        {
            Marshal.FreeHGlobal(model);
        }


        public static int linear_fit_regression(IntPtr model, double[] inputs, int inputSize, double[] outputs)
        {
            int i;
            alglib.matinvreport mr;

            double[,] Y = ToRectangular(outputs, 1);
            double[,] X = ToRectangular(inputs, inputSize);
            double[,] Xt = Transpose(X);

            rmatrixinverse(ref X , out i, out mr);

            return 0;
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

        public static double[,] MatricialProduct(double[,] matrix1, double[,] matrix2)
        {
            double[,] result = new double[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    //result[i, j] = ma
                }
            }

            return result;
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
    }
}


