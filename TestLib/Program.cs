using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDll;
namespace TestLib
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] matrix = new double[]{1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3};
            double[,] toTwoDimMatrix = MyDll.Source.ToRectangular(matrix, 3);
            double[] test = new double[0];
            int nb = MyDll.Source.ToLinear(ref test, toTwoDimMatrix);
            Console.WriteLine("");
        }
    }
}
