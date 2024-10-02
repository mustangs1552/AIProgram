using AI.Classes;
using AI.Enums;

namespace AI.Models
{
    /// <summary>
    /// The saved file of a node.
    /// </summary>
    public class SavedNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// The saved node links that represents the input links.
        /// </summary>
        public List<SavedNodeLink> InputLinks { get; set; } = new List<SavedNodeLink>();
        /// <summary>
        /// The node's algorithm.
        /// </summary>
        public Algorithms Algorithm { get; set; } = Algorithms.None;
        /// <summary>
        /// The saved node links that represents the ouput links.
        /// </summary>
        public List<SavedNodeLink> OutputLinks { get; set; } = new List<SavedNodeLink>();

        /// <summary>
        /// Create and populate a new saved node from a node.
        /// </summary>
        /// <param name="node">The node to use.</param>
        /// <returns>The created and populated saved node.</returns>
        public static SavedNode PopulateSavedNode(Node node)
        {
            if (node == null) return new SavedNode();

            SavedNode savedNode = new SavedNode()
            {
                Name = node.Name,
                Algorithm = node.Algorithm
            };

            SavedNodeLink currLink = new SavedNodeLink();
            node.InputLinks.ForEach(link => savedNode.InputLinks.Add(SavedNodeLink.PopulateSavedNodeLink(link)));
            node.OutputLinks.ForEach(link => savedNode.OutputLinks.Add(SavedNodeLink.PopulateSavedNodeLink(link)));

            return savedNode;
        }
    }
}
