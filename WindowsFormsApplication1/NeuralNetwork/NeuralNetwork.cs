using System;
using System.Collections.Generic;
using System.IO;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        /// <summary>
        /// Starts the neural network
        /// </summary>
        /// <param name="topology">List of ints which represent neurons per layer</param>
        /// <param name="filePath"></param>
        public static IEnumerable<string> Start(List<int> topology, string filePath)
        {
            Logger.Logger.Initialize("NeuralNetworkLogger.txt");

            if (!File.Exists(filePath))
            {
                Logger.Logger.WriteLine("The data file could not be found.");
                throw new FileNotFoundException("The data file could not be found!");
            }

            Network network = new Network(topology);

            int pass = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                pass++;
                var values = line.Split(' ');
                
                var inputValues = new List<double> {Convert.ToDouble(values[0]), Convert.ToDouble(values[1])};
                var targetValues = new List<double> {Convert.ToDouble(values[2])};

                network.FeedForward(inputValues);
                network.BackPropagate(targetValues);

                List<double> resultValues = network.GetResults();

                string formattedString = string.Format($"Training Pass: {pass} Input: {inputValues[0]} {inputValues[1]} Output: {resultValues[0]}");

                Logger.Logger.WriteLine(formattedString);

                yield return formattedString;
            }

            Logger.Logger.Dispose();
        }
    }
}
