using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public void Start(List<int> topology)
        {
            Network network = new Network(topology);

            //Todo:
            //Implement correct loop.
            //while ()
            {
                List<double> inputValues = new List<double>();
                List<double> targetValues = new List<double>();

                network.FeedForward(inputValues);
                network.BackPropagate(targetValues);

                List<double> resultValues = network.GetResults();
            }
        }
    }
}
