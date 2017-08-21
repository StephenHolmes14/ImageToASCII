using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    public class Neuron
    {
        private int NeuronIndex { get; }
        private List<Connection> OutputConnections { get; }
        public double OutputValue { get; set; }
        private double Gradient { get; set; }
        private static double ETA = 0.15; //Overall learning rate [0.0 .. 1.0]
        private static double Alpha = 0.5; //Momentum, multiplier of deltaWeight

        public Neuron(int numberOfOutputs, int neuronIndex)
        {
            NeuronIndex = neuronIndex;
            OutputConnections = new List<Connection>();

            for (int output = 0; output < numberOfOutputs; output++)
            {
                double randomWeight = new Random().NextDouble(); //Random weight between 0 and 1
                OutputConnections.Add(new Connection {Weight = randomWeight });
            }
        }

        public void FeedForward(List<Neuron> previousLayer)
        {
            double sum = previousLayer.Sum(neuron => neuron.OutputValue * neuron.OutputConnections[NeuronIndex].Weight);

            OutputValue = TransferFunction(sum);
        }

        private static double TransferFunction(double sum)
        {
            //Tanh has output range of [-1,0 .. 1.0]
            return Math.Tanh(sum);
        }

        private double TransferFunctionDerivative(double x)
        {
            //Good approximation of derivative of tanh x
            return 1.0 - (x * x);
        }

        public void CalculateOutputGradient(double targetValue)
        {
            double deltaValue = targetValue - OutputValue;
            Gradient = deltaValue * TransferFunctionDerivative(OutputValue);
        }

        public void CalculateHiddenGradient(List<Neuron> nextLayer)
        {
            double dow = SumDerivativeOfWeights(nextLayer);
            Gradient = dow * TransferFunctionDerivative(OutputValue);
        }

        private double SumDerivativeOfWeights(List<Neuron> nextLayer)
        {
            var sum = 0.0;

            for (int n = 0; n < nextLayer.Count - 1; n++)
            {
                sum += OutputConnections[n].Weight * nextLayer[n].Gradient;
            }

            return sum;
        }

        public void UpdateInputWeights(List<Neuron> previousLayer)
        {
            foreach (Neuron neuron in previousLayer)
            {
                double oldDeltaWeight = neuron.OutputConnections[NeuronIndex].DeltaWeight;

                double newDeltaWeight = ETA * neuron.OutputValue * Gradient
                                        + Alpha * oldDeltaWeight;

                neuron.OutputConnections[NeuronIndex].DeltaWeight = newDeltaWeight;
                neuron.OutputConnections[NeuronIndex].Weight += newDeltaWeight;
            }
        }
    }

    internal class Connection
    {
        public double Weight { get; set; }
        public double DeltaWeight { get; set; }
    }
}
