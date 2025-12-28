using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class Layer
    {
        public Neuron[] Neurons { get; set; }
        public double[] Outputs { get; set; }

        public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer)
        {
            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(activation, previousLayer.Neurons);
            }
            Outputs = new double[neuronCount];
        }

        public Layer(ActivationFunction activation, int neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(activation, new Neuron[0]);
            }
            Outputs = new double[neuronCount];
        }

        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Randomize(random, min, max);
            }
        }

        public double[] Compute()
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Outputs[i] = Neurons[i].Compute();
            }
            return Outputs;
        }

    }
}
