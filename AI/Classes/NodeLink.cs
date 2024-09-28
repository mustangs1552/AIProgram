namespace AI.Classes
{
    /// <summary>
    /// The connection between two nodes with the weight for this connection..
    /// </summary>
    public class NodeLink
    {
        /// <summary>
        /// The starting point of the link.
        /// </summary>
        public Node? StartNode { get; protected set; } = null;
        /// <summary>
        /// The end popint of the link.
        /// </summary>
        public Node? EndNode { get; protected set; } = null;
        /// <summary>
        /// The link's current weight.
        /// </summary>
        public float Weight { get; protected set; } = 1;
        /// <summary>
        /// The previous weight before the latest adjustment.
        /// </summary>
        public float LastWeight { get; protected set;} = 0;

        /// <summary>
        /// Setup the link with the its two nodes and a random weight between -1 and 1.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        public NodeLink(Node? startNode, Node? endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            Weight = new Random().NextSingle() * 2 + -1;
        }

        /// <summary>
        /// Adjust this link's weight by the given adjustment.
        /// </summary>
        /// <param name="adjustment">The adjustment amount.</param>
        /// <returns>The new weight.</returns>
        public float AdjustWeight(float adjustment)
        {
            LastWeight = Weight;
            Weight += adjustment;

            return Weight;
        }

        /// <summary>
        /// Reset the current weight to the previous weight.
        /// </summary>
        /// <returns>The new weight.</returns>
        public float ResetToLastWeight()
        {
            Weight = LastWeight;
            return Weight;
        }
    }
}
