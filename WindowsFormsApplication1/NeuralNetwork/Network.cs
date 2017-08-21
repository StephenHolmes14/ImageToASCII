using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Network
    {
        private List<List<Neuron>> Layers { get; }
        private double Error { get; set; }
        private double RecentAverageError { get; set; }
        private double RecentAverageErrorSmoothingFactor { get; set; }

        public Network(List<int> topology)
        {
            int numberOfLayers = topology.Count;

            Layers = new List<List<Neuron>>(numberOfLayers);

            for (int layerNumber = 0; layerNumber < numberOfLayers; layerNumber++)
            {
                int numberOfOutput = layerNumber == numberOfLayers - 1 ? 0 : topology[layerNumber + 1];

                //Create the current layers neurons
                var neurons = new List<Neuron>();
                for (int neuronCount = 0; neuronCount < topology[layerNumber]; neuronCount++)
                {
                    neurons.Add(new Neuron(numberOfOutput, neuronCount));
                }

                //Set the output value of the bias node
                neurons.Last().OutputValue = 1.0;

                Layers.Add(neurons);
            }
        }

        public void FeedForward(List<double> inputValues)
        {
            if (Layers.First().Count - 1 != inputValues.Count)
            {
                throw new ArgumentException("The number of input values does not equal the number of input neurons.");
            }

            //Set input values to the assigned neurons
            for (int i = 0; i < inputValues.Count; i++)
            {
                Layers.First()[i].OutputValue = inputValues[i];
            }

            //Forward propagate
            for (int layerNumber = 1; layerNumber < Layers.Count; layerNumber++)
            {
                List<Neuron> previousLayer = Layers[layerNumber - 1];

                foreach (Neuron neuron in Layers[layerNumber])
                {
                    neuron.FeedForward(previousLayer);
                }
            }
        }

        public void BackPropagate(List<double> targetValues)
        {
            //Calculate overall net error (here using root mean square error)
            List<Neuron> outputLayer = Layers.Last();
            Error = 0.0;

            for (int n = 0; n < outputLayer.Count - 1; n++)
            {
                double deltaValue = targetValues[n] - outputLayer[n].OutputValue;
                Error += deltaValue * deltaValue; //Sum the square of the differences
            }

            Error /= outputLayer.Count; //Get the average squared difference
            Error = Math.Sqrt(Error); //Root to get RMS (root square mean)

            //Implement a recent average measurement
            //Not used yet
            RecentAverageError = (RecentAverageError * RecentAverageErrorSmoothingFactor + Error) / (RecentAverageError + 1.0);

            System.Console.WriteLine($"Error: {Error}, Recent Average Error: {RecentAverageError}");

            //Calculate output layer gradients
            for(int n = 0; n < outputLayer.Count - 1; n++)
            {
                outputLayer[n].CalculateOutputGradient(targetValues[n]);
            }

            //Calculate hidden layer gradients
            for(int layerNumber = Layers.Count - 2; layerNumber > 0; layerNumber--)
            {
                List<Neuron> layer = Layers[layerNumber];
                List<Neuron> nextLayer = Layers[layerNumber + 1];

                for (int n = 0; n < layer.Count - 1; n++)
                {
                    layer[n].CalculateHiddenGradient(nextLayer);
                }
            }

            //For all layers from output to first hidden layers
            //Update connection weights
            for (int layerNumber = Layers.Count - 1; layerNumber > 0; layerNumber--)
            {
                List<Neuron> layer = Layers[layerNumber];
                List<Neuron> previousLayer = Layers[layerNumber - 1];

                for (int n = 0; n < layer.Count - 1; n++)
                {
                    layer[n].UpdateInputWeights(previousLayer);
                }
            }
        }

        public List<double> GetResults()
        {
            return Layers.Last().Select(neuron => neuron.OutputValue).ToList();
        }
    }
}
