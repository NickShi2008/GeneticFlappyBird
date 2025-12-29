using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class Trainer
    {
        public void Mutation(NeuralNetwork network, Random random, double mutationRate)
        {
            foreach (Layer layer in network.layers.Skip(1))
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int i = 0; i < neuron.dendrites.Length; i++)
                    {
                        if (random.NextDouble() < mutationRate)
                        {
                            if (random.Next(2) == 0)
                            {
                                neuron.dendrites[i].Weight *= random.NextDouble() + 0.5;
                            }
                            else
                            {
                                neuron.dendrites[i].Weight *= -1;
                            }
                        }
                    }

                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.bias *= random.NextDouble() + 0.5;
                        }
                        else
                        {
                            neuron.bias *= -1;
                        }
                    }
                }
            }
        }

        public void CrossOver(NeuralNetwork winner, NeuralNetwork loser, Random random)
        {
            for(int i = 0; i < winner.layers.Length; i++)
            {
                Layer winnerLayer = winner.layers[i];
                Layer loserLayer = loser.layers[i];

                int cutPoint = random.Next(winnerLayer.Neurons.Length);
                bool flip = random.Next(2) == 0;

                for(int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winnerLayer.Neurons.Length); j++)
                {

                    Neuron winnerNeuron = winnerLayer.Neurons[j];
                    Neuron childNeuron = loserLayer.Neurons[j];

                    for (int k = 0; k < winnerNeuron.dendrites.Length; k++)
                    {
                        childNeuron.dendrites[k].Weight = winnerNeuron.dendrites[k].Weight;
                    }
                    childNeuron.bias = winnerNeuron.bias;
                }
            }
        }

        public void Train((NeuralNetwork network, double fitness)[] population, Random random, double mutationRate)
        {
            Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            for (int i = start; i < end; i++)
            {
                CrossOver(population[random.Next(start)].network, population[i].network, random);
                Mutation(population[i].network, random, mutationRate);
            }

            for (int i = end; i < population.Length; i++)
            {
                population[i].network.Randomize(random, -1, 1);
            }
        }
    }
}
