using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AForge.Neuro;
using AForge.Neuro.Learning;
using CsvHelper;
using SearchRelevance.DataModel;

namespace SearchRelevance
{
    class Program
    {
        static void Main(string[] args)
        {
            //Training();
            Testing();
        }

        private static void Testing()
        {
            Network network = Network.Load("Network.bin");
            var csv = new CsvReader(new StreamReader("C:\\Users\\jantk_000\\Documents\\SearchResultRelevance\\test_extracted.csv"));
            List<UnlabeledData> data = csv.GetRecords<UnlabeledData>().ToList();
            FileStream result = File.OpenWrite("result.csv");
            StreamWriter writer = new StreamWriter(result);
            writer.WriteLine("\"id\",\"prediction\"");

            foreach (UnlabeledData row in data)
            {
                double[] output = network.Compute(row.GetInputVector());
                writer.WriteLine("{0},{1}", row.Id, row.GetRelevance(output[0]));
            }
            writer.Close();
        }

        private static void Training()
        {
            var csv = new CsvReader(new StreamReader("C:\\Users\\jantk_000\\Documents\\SearchResultRelevance\\trainextracted.csv"));
            List<LabeledData> data = csv.GetRecords<LabeledData>().ToList();

            List<LabeledData> train = new List<LabeledData>();
            List<LabeledData> validation = new List<LabeledData>();

            for (int i = 0; i < data.Count; i++)
            {
                if (i%5 == 0)
                {
                    validation.Add(data[i]);
                }
                else
                {
                    train.Add(data[i]);
                }
            }

            double[][] trainInput = train.Select(labeledData => labeledData.GetInputVector()).ToArray();
            double[][] trainOutput = train.Select(labeledData => new[] {labeledData.GetOutput()}).ToArray();

            double[][] validationInput = validation.Select(labeledData => labeledData.GetInputVector()).ToArray();
            double[][] validationOutput = validation.Select(labeledData => new[] {labeledData.GetOutput()}).ToArray();

            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), 3, 4, 1);
            BackPropagationLearning rprop = new BackPropagationLearning(network) {LearningRate = 0.01, Momentum = 0.01};

            double lastError = double.MaxValue;
            for (int i = 0;; i++)
            {
                rprop.RunEpoch(trainInput, trainOutput);
                double err = 0;
                for (int j = 0; j < validationInput.Length; j++)
                {
                    double[] input = validationInput[j];
                    double[] output = network.Compute(input);
                    err += Math.Sqrt(Math.Pow(validationOutput[j][0] - output[0], 2));
                }
                err = err/validationInput.Length;
                var improvement = lastError - err;
                lastError = err;
                Console.WriteLine("Iteration {0} RMSE {1} Improvement {2}", i, err, improvement);
                if (improvement < 0.000001)
                {
                    break;
                }
            }

            network.Save("Network.bin");
        }
    }
}
