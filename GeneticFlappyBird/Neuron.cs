using NeuralNetworkGate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class Neuron
    {
        public double bias;
        public Dendrite[] dendrites;
        public double Output { get; set; }
        public double Input { get; set; }
        public ActivationFunction activation{get; set;}

        public Neuron(ActivationFunction activationFunc, Neuron[] previousNeurons)
        {
            activation = activationFunc;
            dendrites = new Dendrite[previousNeurons.Length];
            for (int i = 0; i < previousNeurons.Length; i++)
            {
                dendrites[i] = new Dendrite(previousNeurons[i], this, bias);
            }

        }

        public void Randomize(Random random, double min, double max)
        {
            for (int i = 0; i < dendrites.Length; i++)
            {
                dendrites[i].Weight = random.NextDouble() * (max-min) - min;
            }
            bias = random.NextDouble() * (max-min) - min;
        }

        public double Compute()
        {
            Input = 0;
            for(int i = 0; i < dendrites.Length; i++)
            {
                Input += dendrites[i].Compute();
            }
            Input += bias;

            return activation.Function(Input);
        }

    }
}
