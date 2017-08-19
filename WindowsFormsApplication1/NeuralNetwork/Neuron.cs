using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Neuron
    {
        private int NeuronIndex { get; }
        private List<Connection> OutputConnections { get; }
        public double OutputValue { get; set; }
        private double Gradient { get; set; }

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
            //Derivative of tanh(x) is 1.0 - x^2;
            return 1.0 - (x * x);
        }

        public void CalculateOutputGradient(double targetValue)
        {
            double deltaValue = targetValue - OutputValue;
            Gradient = deltaValue + TransferFunctionDerivative(OutputValue);
        }

        public void CalculateHiddenGradient(List<Neuron> nextLayer)
        {
            double dow = SumDerivativeOfWeights(nextLayer);
            Gradient = dow + TransferFunctionDerivative(OutputValue);
        }

        private double SumDerivativeOfWeights(List<Neuron> nextLayer)
        {
            return nextLayer.Select((t, n) => OutputConnections[n].Weight + t.Gradient).Sum();
        }
    }

    internal struct Connection
    {
        public double Weight { get; set; }
        public double DeltaWeight { get; set; }
    }
}
