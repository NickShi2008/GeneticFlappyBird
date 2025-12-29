using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class NeuralNetwork
    {
        public Layer[] layers;
        public ErrorFunction errorFunction;

        public NeuralNetwork(ErrorFunction errorFunction, ActivationFunction activationFunc, params int[] layerAmounts)
        {
            this.errorFunction = errorFunction;
            layers = new Layer[layerAmounts.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                if(i != 0)
                {
                    layers[i] = new Layer(activationFunc, layerAmounts[i], layers[i - 1]);
                }
                else
                {
                    layers[i] = new Layer(activationFunc, layerAmounts[i]);
                }
                   
                
            }

        }

        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < layers.Length; i++)
            {
                layers[i].Randomize(random, min, max);
            }
        }

        public double[] Compute(double[] inputs)
        {
            layers[0].Outputs = inputs;
             for (int i = 0; i < inputs.Length; i++)
            {
                layers[0].Neurons[i].Output = inputs[i];
            }
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].Compute();
            }

            return layers[layers.Length - 1].Outputs;
        }

        public double GetError(double[] inputs, double[] desiredOutputs)
        {
            double error = 0.0;
            double[] outputs = Compute(inputs);

            for (int i = 0; i < outputs.Length; i++)
            {
                error += errorFunction.Function(outputs[i], desiredOutputs[i]);
            }

            return error/desiredOutputs.Length;
        }

    }
}
