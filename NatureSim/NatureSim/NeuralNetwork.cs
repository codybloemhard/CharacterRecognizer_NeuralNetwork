using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NatureSim
{
    public class NeuralNetwork
    {
        public double learnRate;
        public double momentum;
        public List<Neuron> inputLayer;
        public List<Neuron> hiddenLayer;
        public List<Neuron> outputLayer;
        static Random random = new Random();

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize, string file = "")
        {
            learnRate = .9d;
            momentum = .04d;
            inputLayer = new List<Neuron>();
            hiddenLayer = new List<Neuron>();
            outputLayer = new List<Neuron>();

            for (int i = 0; i < inputSize; i++)
                inputLayer.Add(new Neuron());

            for (int i = 0; i < hiddenSize; i++)
                hiddenLayer.Add(new Neuron(inputLayer));

            for (int i = 0; i < outputSize; i++)
                outputLayer.Add(new Neuron(hiddenLayer));

            if (file != "")
                loadNetwork(file);
        }
        
        public void saveNetwork(string name)
        {
            StreamWriter writer = new StreamWriter(name);
            //bias. biasDelta, gradient
            string[,] neuronsIn = new string[inputLayer.Count, 3];
            string[,] neuronsHidden = new string[inputLayer.Count, 3];
            string[,] neuronsOut = new string[inputLayer.Count, 3];

            for(int i = 0; i < inputLayer.Count; i++)
            {
                neuronsIn[i, 0] = "" + inputLayer[i].bias;
                neuronsIn[i, 1] = "" + inputLayer[i].biasDelta;
                neuronsIn[i, 2] = "" + inputLayer[i].gradient;
            }
            for (int i = 0; i < hiddenLayer.Count; i++)
            {
                neuronsHidden[i, 0] = "" + hiddenLayer[i].bias;
                neuronsHidden[i, 1] = "" + hiddenLayer[i].biasDelta;
                neuronsHidden[i, 2] = "" + hiddenLayer[i].gradient;
            }
            for (int i = 0; i < outputLayer.Count; i++)
            {
                neuronsOut[i, 0] = "" + outputLayer[i].bias;
                neuronsOut[i, 1] = "" + outputLayer[i].biasDelta;
                neuronsOut[i, 2] = "" + outputLayer[i].gradient;
            }

            for(int j = 0; j < inputLayer.Count; j++)
            {
                writer.WriteLine(neuronsIn[j, 0] + "," + neuronsIn[j, 1] + "," + neuronsIn[j, 2] + ",");
            }
            writer.WriteLine("#");
            for (int j = 0; j < hiddenLayer.Count; j++)
            {
                writer.WriteLine(neuronsHidden[j, 0] + "," + neuronsHidden[j, 1] + "," + neuronsHidden[j, 2] + ",");
            }
            writer.WriteLine("#");
            for (int j = 0; j < outputLayer.Count; j++)
            {
                writer.WriteLine(neuronsOut[j, 0] + "," + neuronsOut[j, 1] + "," + neuronsOut[j, 2] + ",");
            }
            writer.WriteLine("#");
            for(int i = 0; i < inputLayer.Count; i++)
            {
                for(int j = 0; j < inputLayer[i].outputSynapses.Count; j++)
                {
                    writer.WriteLine("" + inputLayer[i].outputSynapses[j].weight + "," + inputLayer[i].outputSynapses[j].weightDelta + ",");
                }
            }
            writer.WriteLine("#");
            for (int i = 0; i < hiddenLayer.Count; i++)
            {
                for (int j = 0; j < hiddenLayer[i].outputSynapses.Count; j++)
                {
                    writer.WriteLine("" + hiddenLayer[i].outputSynapses[j].weight + "," + hiddenLayer[i].outputSynapses[j].weightDelta + ",");
                }
            }
            writer.WriteLine("#");
            writer.Close();
        }
        public void loadNetwork(string name)
        {
            StreamReader reader = new StreamReader(name);
            //bias. biasDelta, gradient
            //input, hidden, out, in->hidden, hidden->out
            //weight, weightdelta

            string content = reader.ReadToEnd();
            int section = 0, neuron = 0, synapse = 0, segment = 0;
            string current = "";

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '#')
                {
                    section++;
                    neuron = 0;
                    segment = 0;
                    synapse = 0;
                    current = "";
                }
                else if (content[i] == ',')
                {
                    if (section == 0)
                    {
                        if (segment == 0)
                            inputLayer[neuron].bias = double.Parse(current);
                        else if (segment == 1)
                            inputLayer[neuron].biasDelta = double.Parse(current);
                        else if (segment == 2)
                            inputLayer[neuron].gradient = double.Parse(current);
                        segment++;
                        if (segment > 2)
                        {
                            segment = 0;
                            neuron++;
                        }
                    }
                    else if(section == 1)
                    {
                        if (segment == 0)
                            hiddenLayer[neuron].bias = double.Parse(current);
                        else if (segment == 1)
                            hiddenLayer[neuron].biasDelta = double.Parse(current);
                        else if (segment == 2)
                            hiddenLayer[neuron].gradient = double.Parse(current);
                        segment++;
                        if (segment > 2)
                        {
                            segment = 0;
                            neuron++;
                        }
                    }
                    else if (section == 2)
                    {
                        if (segment == 0)
                            outputLayer[neuron].bias = double.Parse(current);
                        else if (segment == 1)
                            outputLayer[neuron].biasDelta = double.Parse(current);
                        else if (segment == 2)
                            outputLayer[neuron].gradient = double.Parse(current);
                        segment++;
                        if (segment > 2)
                        {
                            segment = 0;
                            neuron++;
                        }
                    }
                    else if (section == 3)
                    {
                        if (segment == 0)
                        {
                            inputLayer[neuron].outputSynapses[synapse].weight = double.Parse(current);
                            segment = 1;
                        }
                        else if(segment == 1)
                        {
                            inputLayer[neuron].outputSynapses[synapse].weightDelta = double.Parse(current);
                            segment = 0;
                            synapse++;
                            if(synapse > inputLayer[neuron].outputSynapses.Count - 1)
                            {
                                synapse = 0;
                                neuron++;
                            }
                        }
                    }
                    else if (section == 4)
                    {
                        if (segment == 0)
                        {
                            hiddenLayer[neuron].outputSynapses[synapse].weight = double.Parse(current);
                            segment = 1;
                        }
                        else if (segment == 1)
                        {
                            hiddenLayer[neuron].outputSynapses[synapse].weightDelta = double.Parse(current);
                            segment = 0;
                            synapse++;
                            if (synapse > hiddenLayer[neuron].outputSynapses.Count - 1)
                            {
                                synapse = 0;
                                neuron++;
                            }
                        }
                    }
                    current = "";
                }
                else if(content[i] == '0' || content[i] == '1' || content[i] == '2' || content[i] == '3' || content[i] == '4' || content[i] == '5' || content[i] == '6' ||
                    content[i] == '7' || content[i] == '8' || content[i] == '9' || content[i] == '.' || content[i] == 'E' || content[i] == '-')
                    current += content[i];
            }
            reader.Close();
            Console.WriteLine("Loaded ANN");
        }
        
        public void train(params double[] inputs)
        {
            for(int i = 0; i < inputLayer.Count; i++)
            {
                inputLayer[i].value = inputs[i];
            }
            for(int i = 0; i < hiddenLayer.Count; i++)
            {
                hiddenLayer[i].calculateValue();
            }
            for (int i = 0; i < outputLayer.Count; i++)
            {
                outputLayer[i].calculateValue();
            }
        }

        public double[] compute(params double[] inputs)
        {
            train(inputs);
            return outputLayer.Select(a => a.value).ToArray();
        }

        public double calculateError(params double[] targets)
        {
            int i = 0;
            return outputLayer.Sum(a => Math.Abs(a.calculateError(targets[i++])));
        }

        public void backPropagate(params double[] targets)
        {
            for(int i = 0; i < outputLayer.Count; i++)
            {
                outputLayer[i].calculateGradient(targets[i]);
            }
            for (int i = 0; i < hiddenLayer.Count; i++)
            {
                hiddenLayer[i].calculateGradient();
                hiddenLayer[i].updateWeights(learnRate, momentum);
            }
            for (int i = 0; i < outputLayer.Count; i++)
            {
                outputLayer[i].updateWeights(learnRate, momentum);
            }
        }

        public static double getRandom()
        {
            return random.NextDouble() * 2 - 1;
        }
    }

    public class Neuron
    {
        public List<Synapse> inputSynapses;
        public List<Synapse> outputSynapses;
        public double bias;
        public double biasDelta;
        public double gradient;
        public double value;

        public Neuron()
        {
            inputSynapses = new List<Synapse>();
            outputSynapses = new List<Synapse>();
            bias = NeuralNetwork.getRandom();
        }

        public Neuron(List<Neuron> inputNeurons)
        {
            inputSynapses = new List<Synapse>();
            outputSynapses = new List<Synapse>();
            bias = NeuralNetwork.getRandom();

            for (int i = 0; i < inputNeurons.Count; i++)
            {
                Synapse synapse = new Synapse(inputNeurons[i], this);
                inputNeurons[i].outputSynapses.Add(synapse);
                inputSynapses.Add(synapse);
            }
        }

        public virtual double calculateValue()
        {
            double sum = bias;
            for(int i = 0; i < inputSynapses.Count; i++)
            {
                sum += inputSynapses[i].weight * inputSynapses[i].inputNeuron.value;
            }
            return value = sigmoidFunction(sum);
        }

        private double sigmoidFunction(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public virtual double calculateDerivative()
        {
            return value * (1 - value);
        }

        public double calculateError(double target)
        {
            return target - value;
        }

        public double calculateGradient(double target)
        {
            return gradient = calculateError(target) * calculateDerivative();
        }

        public double calculateGradient()
        {
            gradient = 0;
            for(int i = 0; i < outputSynapses.Count; i++)
            {
                gradient += outputSynapses[i].outputNeuron.gradient * outputSynapses[i].weight;
            }
            gradient *= calculateDerivative();

            return gradient;
        }

        public void updateWeights(double learnRate, double momentum)
        {
            double prevDelta = biasDelta;
            biasDelta = learnRate * gradient;
            bias += biasDelta + (momentum * prevDelta);

            for (int i = 0; i < inputSynapses.Count; i++)
            {
                prevDelta = inputSynapses[i].weightDelta;
                inputSynapses[i].weightDelta = learnRate * gradient * inputSynapses[i].inputNeuron.value;
                inputSynapses[i].weight += inputSynapses[i].weightDelta + momentum * prevDelta;
            }
        }
    }

    public class Synapse
    {
        public Neuron inputNeuron;
        public Neuron outputNeuron;
        public double weight;
        public double weightDelta;

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            inputNeuron = inNeuron;
            outputNeuron = outNeuron;
            weight = NeuralNetwork.getRandom();
        }
    }
}