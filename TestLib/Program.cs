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
            double[][] inputs = new double[4][];
            double[] outputs = {1, 1, -1, -1};
            inputs[0] = new double[] {-1, 1};
            inputs[1] = new double[] { 1, -1 };
            inputs[2] = new double[] { 1, 1 };
            inputs[3] = new double[] { -1, -1 };

            SourceMulti.MLP network = SourceMulti.mlp_create_model(3, new int[] {2, 3, 1});
            //SourceMulti.mlp_fit_classification_backdrop(network, inputs, outputs, 1000, 0.1);
            SourceMulti.mlp_fit_regression_backdrop(network, inputs, outputs, 1000, 0.1);
            /*Console.WriteLine(SourceMulti.mlp_classify(network, new double[] { -1, 1 })[0]);
            Console.WriteLine(SourceMulti.mlp_classify(network, new double[] { 1, -1 })[0]);
            Console.WriteLine(SourceMulti.mlp_classify(network, new double[] { 1, 1 })[0]);
            Console.WriteLine(SourceMulti.mlp_classify(network, new double[] { -1, -1 })[0]);*/

            Console.WriteLine(SourceMulti.mlp_predict(network, new double[] { -1, 1 })[0]);
            Console.WriteLine(SourceMulti.mlp_predict(network, new double[] { 1, -1 })[0]);
            Console.WriteLine(SourceMulti.mlp_predict(network, new double[] { 1, 1 })[0]);
            Console.WriteLine(SourceMulti.mlp_predict(network, new double[] { -1, -1 })[0]);

            Console.ReadLine();
        }
    }
}
