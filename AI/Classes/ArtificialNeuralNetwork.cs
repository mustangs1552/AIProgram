﻿using AI.Enums;
using AI.Models;

namespace AI.Classes
{
    /// <summary>
    /// An artificial nueral network that manages the state of the network.
    /// Will only run one pass at a time and not be able to run multiple passes in one call.
    /// https://www.superdatascience.com/blogs/the-ultimate-guide-to-artificial-nueral-networks-ann
    /// https://www.youtube.com/watch?v=Ilg3gGewQ5U&t=1s
    /// https://www.youtube.com/watch?v=ILsA4nyG7I0
    /// </summary>
    public class ArtificialNueralNetwork
    {
        /// <summary>
        /// The name of the ANN.
        /// </summary>
        public string Name
        {
            get => name;
            set => name = string.IsNullOrWhiteSpace(value) ? "" : value;
        }

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

        protected string name = "ANN";
        protected int numOfInputNodes = 1;
        protected List<int> numOfHiddenLayerNodes = new List<int>();
        protected int numOfOutputNodes = 1;

        protected List<float> preCostsBPON = new List<float>();
        protected List<float> postCostsBPON = new List<float>();
        protected List<float> preCostsBPN = new List<float>();
        protected List<float> postCostsBPN = new List<float>();
        protected float firstAdjustmentBPON = 0;
        protected float secondAdjustmentBPON = 0;
        protected float firstAdjustmentBPN = 0;
        protected float secondAdjustmentBPN = 0;
        protected List<float> costs = new List<float>();

        /// <summary>
        /// Setup the network using the given template.
        /// </summary>
        /// <param name="template">The template to use.</param>
        public ArtificialNueralNetwork(string name, ANNTemplate template)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            
            Name = name;
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
        public ArtificialNueralNetwork(string name, int numOfInputNodes, List<int> numOfHiddenLayerNodes, int numOfOutputNodes, Algorithms hiddenLayerAlgorithmType = Algorithms.None, Algorithms outputNodesAlgorithmType = Algorithms.None)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            Name = name;
            BuildNewNetwork(numOfInputNodes, numOfHiddenLayerNodes, numOfOutputNodes, hiddenLayerAlgorithmType, outputNodesAlgorithmType);
        }
        /// <summary>
        /// Setup the network with the given settings from the saved ANN.
        /// </summary>
        /// <param name="savedANN">The saved ANN to use.</param>
        public ArtificialNueralNetwork(SavedArtificialNueralNetwork savedANN)
        {
            if (savedANN == null) return;

            Name = savedANN.Name;

            savedANN.InputNodes.ForEach(savedNode => InputNodes.Add(new InputNode(savedNode)));
            numOfInputNodes = InputNodes.Count;

            List<Node> currLayer = new List<Node>();
            foreach (List<SavedNode> savedLayer in savedANN.Network)
            {
                currLayer = new List<Node>();
                savedLayer.ForEach(savedNode => currLayer.Add(new Node(savedNode)));
                Network.Add(currLayer);
                numOfHiddenLayerNodes.Add(currLayer.Count);
            }
            numOfOutputNodes = NumOfHiddenLayerNodes.Last();
            numOfHiddenLayerNodes.RemoveAt(NumOfHiddenLayerNodes.Count - 1);

            Node? currStartNode = null;
            Node? currEndNode = null;
            NodeLink? currLink = null;
            foreach (SavedNode savedNode in savedANN.InputNodes)
            {
                currStartNode = InputNodes.Where(node => node.Name == savedNode.Name).FirstOrDefault();

                foreach (SavedNodeLink savedNodeLink in savedNode.OutputLinks)
                {
                    currEndNode = Network[0].Where(node => node.Name == savedNodeLink.EndNodeName).FirstOrDefault();
                    if (currStartNode == null || currEndNode == null) continue;
                    currLink = new NodeLink(currStartNode, currEndNode, savedNodeLink);

                    currStartNode.AddOutputLink(currEndNode, currLink);
                }
            }
            for (int layerI = 0; layerI < savedANN.Network.Count - 1; layerI++)
            {
                for (int nodeI = 0; nodeI < savedANN.Network[layerI].Count; nodeI++)
                {
                    currStartNode = Network[layerI].Where(node => node.Name == savedANN.Network[layerI][nodeI].Name).FirstOrDefault();

                    foreach (SavedNodeLink savedNodeLink in savedANN.Network[layerI][nodeI].OutputLinks)
                    {
                        currEndNode = Network[layerI + 1].Where(node => node.Name == savedNodeLink.EndNodeName).FirstOrDefault();
                        if (currStartNode == null || currEndNode == null) continue;
                        currLink = new NodeLink(currStartNode, currEndNode, savedNodeLink);

                        currStartNode.AddOutputLink(currEndNode, currLink);
                    }
                }
            }
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

        /// <summary>
        /// Back propagate the network.
        /// </summary>
        /// <param name="inputs">The input values.</param>
        /// <param name="correctValues">The correct values that were expected.</param>
        public void BackPropagateNetwork(List<float> inputs, List<float> correctValues)
        {
            if (inputs == null || inputs.Count == 0 || correctValues == null || correctValues.Count == 0 || Network[Network.Count - 1].Count != correctValues.Count) return;

            for (int nodeI = 0; nodeI < Network[Network.Count - 1].Count; nodeI++) BackPropagateOutputNode(nodeI, inputs, correctValues);
        }

        /// <summary>
        /// Back propagate from the given output node.
        /// </summary>
        /// <param name="nodeI">The ouput node's index.</param>
        /// <param name="inputs">The input values.</param>
        /// <param name="correctValues">The correct values that were expected.</param>
        protected void BackPropagateOutputNode(int nodeI, List<float> inputs, List<float> correctValues)
        {
            if (nodeI < 0 || inputs == null || inputs.Count == 0) return;

            preCostsBPON = new List<float>();
            postCostsBPON = new List<float>();
            firstAdjustmentBPON = 0;
            secondAdjustmentBPON = 0;
            foreach (NodeLink link in Network[Network.Count - 1][nodeI].InputLinks)
            {
                if (link.StartNode == null) continue;

                preCostsBPON = CalculateOutputCosts(correctValues);
                link.AdjustWeight(preCostsBPON[nodeI]);
                PropagateNetwork(inputs);
                postCostsBPON = CalculateOutputCosts(correctValues);
                firstAdjustmentBPON = MathF.Abs(preCostsBPON[nodeI] - postCostsBPON[nodeI]);

                link.ResetToLastWeight();
                link.AdjustWeight(-preCostsBPON[nodeI]);
                PropagateNetwork(inputs);
                postCostsBPON = CalculateOutputCosts(correctValues);
                secondAdjustmentBPON = MathF.Abs(preCostsBPON[nodeI] - postCostsBPON[nodeI]);

                if (firstAdjustmentBPON == 0 && secondAdjustmentBPON == 0)
                {
                    link.ResetToLastWeight();
                    PropagateNetwork(inputs);
                }
                else if (firstAdjustmentBPON > secondAdjustmentBPON)
                {
                    link.ResetToLastWeight();
                    link.AdjustWeight(preCostsBPON[nodeI]);
                    PropagateNetwork(inputs);
                }

                if (Network.Count > 1) BackPropagateNode(Network.Count - 2, Network[Network.Count - 2].IndexOf(link.StartNode), nodeI, inputs, correctValues);
            }
        }

        /// <summary>
        /// Back propagate from the given node in the given layer.
        /// </summary>
        /// <param name="layerI">The layer index the node is in.</param>
        /// <param name="nodeI">The node index of the node within the layer.</param>
        /// <param name="outputNodeI">The output node's index that is being back propagated.</param>
        /// <param name="inputs">The input values.</param>
        /// <param name="correctValues">The correct values that were expected.</param>
        protected void BackPropagateNode(int layerI, int nodeI, int outputNodeI, List<float> inputs, List<float> correctValues)
        {
            if (layerI < 0 || nodeI < 0 || outputNodeI < 0 || inputs == null || inputs.Count == 0) return;

            preCostsBPN = new List<float>();
            postCostsBPN = new List<float>();
            firstAdjustmentBPN = 0;
            secondAdjustmentBPN = 0;
            foreach (NodeLink link in Network[layerI][nodeI].InputLinks)
            {
                if (link.StartNode == null) continue;

                preCostsBPN = CalculateOutputCosts(correctValues);
                link.AdjustWeight(preCostsBPN[outputNodeI]);
                PropagateNetwork(inputs);
                postCostsBPN = CalculateOutputCosts(correctValues);
                firstAdjustmentBPN = MathF.Abs(preCostsBPN[outputNodeI] - postCostsBPN[outputNodeI]);

                link.ResetToLastWeight();
                link.AdjustWeight(-preCostsBPN[outputNodeI]);
                PropagateNetwork(inputs);
                postCostsBPN = CalculateOutputCosts(correctValues);
                secondAdjustmentBPN = MathF.Abs(preCostsBPN[outputNodeI] - postCostsBPN[outputNodeI]);

                if (firstAdjustmentBPN == 0 && secondAdjustmentBPN == 0)
                {
                    link.ResetToLastWeight();
                    PropagateNetwork(inputs);
                }
                else if (firstAdjustmentBPN > secondAdjustmentBPN)
                {
                    link.ResetToLastWeight();
                    link.AdjustWeight(preCostsBPN[outputNodeI]);
                    PropagateNetwork(inputs);
                }

                if (layerI > 0) BackPropagateNode(layerI - 1, Network[layerI - 1].IndexOf(link.StartNode), outputNodeI, inputs, correctValues);
            }
        }

        /// <summary>
        /// Calculate all the output nodes' costs.
        /// </summary>
        /// <param name="correctValues">The correct values that was expected.</param>
        /// <returns>The output nodes' costs.</returns>
        public List<float> CalculateOutputCosts(List<float> correctValues)
        {
            if (Network[Network.Count - 1].Count != correctValues.Count) return new List<float>();

            costs = new List<float>();
            for (int outputI = 0; outputI < Network[Network.Count - 1].Count; outputI++) costs.Add(Network[Network.Count - 1][outputI].CalculateCost(correctValues[outputI]));

            return costs;
        }

        /// <summary>
        /// Lists out all the layers of nodes' `ToString()` values.
        /// </summary>
        /// <returns>The formated stirng.</returns>
        public override string ToString()
        {
            string returnStr = $"Name: {Name}\n";
            returnStr += "Node Format: '{Input nodes' Name} ({Input nodes' weights}) -> [{Node Name} = {Current Value} ({Algorithm})] -> {Output nodes' Name} ({Output nodes' weights})'\n";
            returnStr += $"Input Nodes ({InputNodes.Count}):\n";
            foreach (InputNode node in InputNodes) returnStr += $"\t{node}\n";

            for (int layerI = 0; layerI < Network.Count - 1; layerI++)
            {
                returnStr += $"Hidden Layer {layerI + 1} ({Network[layerI].Count}):\n";
                foreach (Node node in Network[layerI]) returnStr += $"\t{node}\n";
            }

            returnStr += $"Output Layers ({Network[Network.Count - 1].Count}):\n";
            foreach (Node node in Network[Network.Count - 1]) returnStr += $"\t{node}\n";

            return returnStr;
        }
    }
}