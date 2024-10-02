using AI.Classes;

namespace AI.Models
{
    /// <summary>
    /// The saved file of a node link.
    /// </summary>
    public class SavedNodeLink
    {
        /// <summary>
        /// The name of the start node.
        /// </summary>
        public string StartNodeName { get; set; } = "";
        /// <summary>
        /// The name of the end node.
        /// </summary>
        public string EndNodeName { get; set; } = "";
        /// <summary>
        /// The weight of this link.
        /// </summary>
        public float Weight { get; set; } = 1;

        /// <summary>
        /// Create and populate a new saved node link from a node link.
        /// </summary>
        /// <param name="link">The node link to use.</param>
        /// <returns>The created and populated saved node link.</returns>
        public static SavedNodeLink PopulateSavedNodeLink(NodeLink link)
        {
            if (link == null) return new SavedNodeLink();

            return new SavedNodeLink()
            {
                StartNodeName = link.StartNode == null ? "" : link.StartNode.Name,
                EndNodeName = link.EndNode == null ? "" : link.EndNode.Name,
                Weight = link.Weight
            };
        }
    }
}
