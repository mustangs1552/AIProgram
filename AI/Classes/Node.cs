using AI.Enums;

namespace AI.Classes
{
    /// <summary>
    /// A node of a neural network.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name { get; set; } = "Node";
        /// <summary>
        /// The input links to this node's input nodes.
        /// </summary>
        public List<NodeLink> InputLinks { get; protected set; } = new List<NodeLink>();
        /// <summary>
        /// This node's current algorithm used.
        /// </summary>
        public Algorithms Algorithm { get; set; } = Algorithms.None;
        /// <summary>
        /// The output links to this node's output nodes.
        /// </summary>
        public List<NodeLink> OutputLinks { get; protected set; } = new List<NodeLink>();
        /// <summary>
        /// The latest calculated value of this node.
        /// </summary>
        public float Value { get; protected set; } = 0;

        public Node() { }
        /// <summary>
        /// Setup this node with a name and its algorithm.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="algorithmType">The algorithm this node should use.</param>
        public Node(string name, Algorithms algorithmType = Algorithms.None)
        {
            Name = name;
            Algorithm = algorithmType;
        }

        /// <summary>
        /// Add the given node to this node's input nodes.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddInputLink(Node? node)
        {
            if (node == null) return;

            NodeLink link = new NodeLink(node, this);
            InputLinks.Add(link);
            node.OutputLinks.Add(link);
        }
        /// <summary>
        /// Add the given node to this node's output nodes.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddOutputLink(Node? node)
        {
            if (node == null) return;

            NodeLink link = new NodeLink(this, node);
            OutputLinks.Add(link);
            node.InputLinks.Add(link);
        }

        /// <summary>
        /// Sum up all of this node's input nodes' values with their link's weights.
        /// </summary>
        /// <returns>The sum.</returns>
        public float CalcSumInputNodesWithWeights()
        {
            float weightedSum = 0;
            foreach (NodeLink link in InputLinks) weightedSum += link.StartNode != null ? link.StartNode.Value * link.Weight : 0;
            return weightedSum;
        }

        /// <summary>
        /// Pull the values of this node's input nodes with each of thier weights, sum them up and calculate this node's value via its current algorithm.
        /// </summary>
        public void CalculateValue()
        {
            float weightedSum = CalcSumInputNodesWithWeights();

            switch (Algorithm)
            {
                case Algorithms.None:
                    Value = weightedSum;
                    break;
                case Algorithms.Threshold:
                    Value = weightedSum < 0 ? 0 : 1;
                    break;
                case Algorithms.Sigmoid:
                    Value = (float)(1 / (1 + Math.Pow(Math.E, -weightedSum)));
                    break;
                case Algorithms.Rectifier:
                    Value = MathF.Max(weightedSum, 0);
                    break;
                case Algorithms.HyperbolicTangent:
                    Value = MathF.Tanh(weightedSum);
                    break;
            }
        }

        /// <summary>
        /// Calculate this node's cost from the given correct value.
        /// </summary>
        /// <param name="correctValue">The correct value that was expected.</param>
        /// <returns>This node's cost.</returns>
        public float CalculateCost(float correctValue)
        {
            return .5f * MathF.Pow(Value - correctValue, 2);
        }

        /// <summary>
        /// Format: "{Input nodes} ({Input nodes' weights}) -> {Name} = {Current Value} ({Algorithm}) -> {Output nodes} ({Output nodes' weights})"
        /// </summary>
        /// <returns>The formated string.</returns>
        public override string ToString()
        {
            string returnStr = "";

            foreach (NodeLink link in InputLinks)
            {
                if (returnStr.Length > 0) returnStr += ", ";
                returnStr += $"{(link != null && link.StartNode != null ? $"{link.StartNode.Name} ({link.Weight})" : "")}";
            }
            if (returnStr.Length > 0) returnStr += " -> ";

            returnStr += $"[{Name} = {Value} ({Algorithm})]";

            string outputNodesStr = "";
            foreach (NodeLink link in OutputLinks)
            {
                if (outputNodesStr.Length > 0) outputNodesStr += ", ";
                outputNodesStr += $"{(link != null && link.EndNode != null ? $"{link.EndNode.Name} ({link.Weight})" : "")}";
            }
            
            return $"{returnStr}{(string.IsNullOrWhiteSpace(outputNodesStr) ? "" : $" -> {outputNodesStr}")}";
        }
    }
}
