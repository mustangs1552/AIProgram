﻿using AI.Enums;
using AI.Models;
using System.Xml.XPath;

namespace AI.Classes
{
    /// <summary>
    /// An artificial neural network that manages the state of the network.
    /// Will only run one pass at a time and not be able to run multiple passes in one call.
    /// https://www.superdatascience.com/blogs/the-ultimate-guide-to-artificial-neural-networks-ann
    /// https://www.youtube.com/watch?v=Ilg3gGewQ5U&t=1s
    /// https://www.youtube.com/watch?v=ILsA4nyG7I0
    /// </summary>
    public class ArtificialNeuralNetwork
    {
        /// <summary>
        /// The number of input nodes used to build current network.
        /// </summary>
        public int NumOfInputNodes
        {
            get => numOfInputNodes;
            protected set
            {
                if (InputNodes.Any()) return;
                numOfInputNodes = value < 1 ? 1 : value;
            }
        }
        /// <summary>
        /// The number of hidden layer nodes used to build current network.
        /// </summary>
        public List<int> NumOfHiddenLayerNodes
        {
            get => numOfHiddenLayerNodes;
            protected set
            {
                if (Network.Any()) return;

                numOfHiddenLayerNodes.Clear();

                if (value == null) numOfHiddenLayerNodes = new List<int>();
                else
                {
                    foreach (int nodes in value)
                    {
                        if (nodes > 0) numOfHiddenLayerNodes.Add(nodes);
                    }
                }
            }
        }
        /// <summary>
        /// The number of output nodes used to build current network.
        /// </summary>
        public int NumOfOutputNodes
        {
            get => numOfOutputNodes;
            protected set
            {
                if (Network.Any()) return;
                numOfOutputNodes = value < 1 ? 1 : value;
            }
        }

        /// <summary>
        /// The latest values of the output nodes.
        /// </summary>
        public List<float> OutputValues
        {
            get
            {
                List<float> outputs = new List<float>();
                foreach (Node node in Network[Network.Count - 1]) outputs.Add(node.Value);
                return outputs;
            }
        }

        /// <summary>
        /// The input nodes in the network.
        /// </summary>
        public List<InputNode> InputNodes { get; protected set; } = new List<InputNode>();
        /// <summary>
        /// The hidden layers and output nodes of the network.
        /// </summary>
        public List<List<Node>> Network { get; protected set; } = new List<List<Node>>();

        protected int numOfInputNodes = 1;
        protected List<int> numOfHiddenLayerNodes = new List<int>();
        protected int numOfOutputNodes = 1;

        /// <summary>
        /// Setup the network using the given template.
        /// </summary>
        /// <param name="template">The template to use.</param>
        public ArtificialNeuralNetwork(ANNTemplate template)
        {
            BuildNewNetwork(template.NumOfInputNodes, template.NumOfHiddentLayerNodes, template.OutputNodes.Count);

            for (int layerNodesI = 0; layerNodesI < Network.Count - 1; layerNodesI++)
            {
                if (layerNodesI >= template.HiddenLayerNodes.Count) break;

                for (int nodeI = 0; nodeI < Network[layerNodesI].Count; nodeI++)
                {
                    if (layerNodesI >= template.HiddenLayerNodes[layerNodesI].Count) break;
                    Network[layerNodesI][nodeI].Algorithm = template.HiddenLayerNodes[layerNodesI][nodeI];
                }
            }

            for (int nodeI = 0; nodeI < Network[Network.Count - 1].Count; nodeI++)
            {
                if (nodeI >= template.OutputNodes.Count) break;
                Network[Network.Count - 1][nodeI].Algorithm = template.OutputNodes[nodeI];
            }
        }
        /// <summary>
        /// Setup the network with the given settings.
        /// </summary>
        /// <param name="numOfInputNodes">The number of input nodes to build.</param>
        /// <param name="numOfHiddenLayerNodes">The number of hidden layer nodes to build.</param>
        /// <param name="numOfOutputNodes">The number of output nodes to build.</param>
        /// <param name="hiddenLayerAlgorithmType">The algorithm to use for all the hidden layer nodes.</param>
        /// <param name="outputNodesAlgorithmType">The algorithm to use for all the output nodes.</param>
        public ArtificialNeuralNetwork(int numOfInputNodes, List<int> numOfHiddenLayerNodes, int numOfOutputNodes, Algorithms hiddenLayerAlgorithmType = Algorithms.None, Algorithms outputNodesAlgorithmType = Algorithms.None)
        {
            BuildNewNetwork(numOfInputNodes, numOfHiddenLayerNodes, numOfOutputNodes, hiddenLayerAlgorithmType, outputNodesAlgorithmType);
        }

        /// <summary>
        /// Build/Rebuild the network with the current settings.
        /// </summary>
        /// <param name="hiddenLayerAlgorithmType">The algorithm to use for all the hidden layer nodes.</param>
        /// <param name="outputNodesAlgorithmType">The algorithm to use for all the output nodes.</param>
        public void BuildNewNetwork(Algorithms hiddenLayerAlgorithmType = Algorithms.None, Algorithms outputNodesAlgorithmType = Algorithms.None)
        {
            InputNodes.Clear();
            Network.Clear();

            // Create nodes.
            // Input nodes.
            int currNodeCount = 0;
            for (int nodeI = 0; nodeI < NumOfInputNodes; nodeI++)
            {
                InputNodes.Add(new InputNode($"Node_0-{currNodeCount}"));
                currNodeCount++;
            }

            // Hidden layer nodes.
            for (int layerI = 0; layerI < NumOfHiddenLayerNodes.Count; layerI++)
            {
                currNodeCount = 0;
                Network.Add(new List<Node>());
                for (int nodeI = 0; nodeI < NumOfHiddenLayerNodes[layerI]; nodeI++)
                {
                    Network[layerI].Add(new Node($"Node_{Network.Count}-{currNodeCount}", hiddenLayerAlgorithmType));
                    currNodeCount++;
                }
            }

            // Output nodes.
            currNodeCount = 0;
            Network.Add(new List<Node>());
            for (int nodeI = 0; nodeI < NumOfOutputNodes; nodeI++)
            {
                Network[Network.Count - 1].Add(new Node($"Node_{Network.Count}-{currNodeCount}", outputNodesAlgorithmType));
                currNodeCount++;
            }

            // Link created nodes.
            // Input nodes.
            foreach (InputNode node in InputNodes)
            {
                foreach (Node outputNode in Network[0]) node.AddOutputLink(outputNode);
            }

            // Hidden layer and output nodes.
            for (int layerI = 0; layerI < Network.Count; layerI++)
            {
                if (layerI < Network.Count - 1)
                {
                    foreach (Node node in Network[layerI])
                    {
                        foreach (Node outputNode in Network[layerI + 1]) node.AddOutputLink(outputNode);
                    }
                }
            }
        }
        /// <summary>
        /// Build/Rebuild the network with the given settings.
        /// </summary>
        /// <param name="numOfInputNodes">The number of input nodes to build.</param>
        /// <param name="numOfHiddenLayerNodes">The number of hidden layer nodes to build.</param>
        /// <param name="numOfOutputNodes">The number of output nodes to build.</param>
        /// <param name="hiddenLayerAlgorithmType">The algorithm to use for all the hidden layer nodes.</param>
        /// <param name="outputNodesAlgorithmType">The algorithm to use for all the output nodes.</param>
        public void BuildNewNetwork(int numOfInputNodes, List<int> numOfHiddenLayerNodes, int numOfOutputNodes, Algorithms hiddenLayerAlgorithmType = Algorithms.None, Algorithms outputNodesAlgorithmType = Algorithms.None)
        {
            NumOfInputNodes = numOfInputNodes;
            NumOfHiddenLayerNodes = numOfHiddenLayerNodes;
            NumOfOutputNodes = numOfOutputNodes;

            BuildNewNetwork(hiddenLayerAlgorithmType, outputNodesAlgorithmType);
        }

        /// <summary>
        /// Propagate the given inputs through the network to get outputs.
        /// </summary>
        /// <param name="inputs">The inputs to use (must match number of out put nodes).</param>
        /// <returns>The resulting output values.</returns>
        public List<float> PropagateNetwork(List<float> inputs)
        {
            if (inputs == null || inputs.Count != InputNodes.Count) return new List<float>();

            for (int nodeI = 0; nodeI < InputNodes.Count; nodeI++) InputNodes[nodeI].SetInputValue(inputs[nodeI]);

            foreach (List<Node> nodes in Network)
            {
                foreach (Node node in nodes) node.CalculateValue();
            }

            return OutputValues;
        }

        public void BackPropagateNetwork(List<float> actualValues)
        {
            if (Network[Network.Count - 1].Count != actualValues.Count) return;

            for (int nodeI = 0; nodeI < Network[Network.Count - 1].Count; nodeI++) Network[Network.Count - 1][nodeI].BackpropagateInputWeights(actualValues[nodeI]);
            
            for (int layerI = Network.Count - 2; layerI >= 0; layerI--)
            {
                foreach (Node node in Network[layerI]) node.BackpropagateWeights();
            }
        }

        public List<float> CalculateOutputCosts(List<float> actualValues)
        {
            if (Network[Network.Count - 1].Count != actualValues.Count) return new List<float>();

            List<float> costs = new List<float>();
            for (int outputI = 0; outputI < Network[Network.Count - 1].Count; outputI++) costs.Add(Network[Network.Count - 1][outputI].CalculateCost(actualValues[outputI]));

            return costs;
        }

        /// <summary>
        /// Lists out all the layers of nodes' `ToString()` values.
        /// </summary>
        /// <returns>The formated stirng.</returns>
        public override string ToString()
        {
            string returnStr = $"Input Nodes ({InputNodes.Count}):\n";
            foreach (InputNode node in InputNodes) returnStr += $"{node}\n";

            for (int layerI = 0; layerI < Network.Count - 1; layerI++)
            {
                returnStr += $"Hidden Layer {layerI + 1} ({Network[layerI].Count}):\n";
                foreach (Node node in Network[layerI]) returnStr += $"{node}\n";
            }

            returnStr += $"Output Layers ({Network[Network.Count - 1].Count}):\n";
            foreach (Node node in Network[Network.Count - 1]) returnStr += $"{node}\n";

            return returnStr;
        }
    }
}