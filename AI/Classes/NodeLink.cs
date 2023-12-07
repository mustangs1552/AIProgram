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
        public float LastWeightAdjustment { get; protected set;} = 0;

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

        public float AdjustWeight(float adjustment)
        {
            Weight -= adjustment;
            LastWeightAdjustment = adjustment;

            return Weight;
        }
    }
}
