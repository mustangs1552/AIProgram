using AI.Classes;

namespace AI.Models
{
    /// <summary>
    /// The saved file of an Artificial Nueral Network.
    /// </summary>
    public class SavedArtificialNueralNetwork
    {
        /// <summary>
        /// The name of the ANN.
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// The saved nodes that represent the input nodes.
        /// </summary>
        public List<SavedNode> InputNodes { get; set; } = new List<SavedNode>();
        /// <summary>
        /// The saved nodes that represent the layers of hidden nodes and output nodes.
        /// </summary>
        public List<List<SavedNode>> Network { get; set; } = new List<List<SavedNode>>();

        /// <summary>
        /// Create and populate a new saved ANN from an ANN.
        /// </summary>
        /// <param name="ann">The ANN to use.</param>
        /// <returns>The created and populated saved ANN.</returns>
        public static SavedArtificialNueralNetwork PopulateSavedANN(ArtificialNueralNetwork ann)
        {
            if (ann == null) return new SavedArtificialNueralNetwork();

            SavedArtificialNueralNetwork savedANN = new SavedArtificialNueralNetwork()
            {
                Name = ann.Name,
            };

            foreach (Node node in ann.InputNodes) savedANN.InputNodes.Add(SavedNode.PopulateSavedNode(node));
            List<SavedNode> layer = new List<SavedNode>();
            foreach (List<Node> node in ann.Network)
            {
                layer = new List<SavedNode>();
                node.ForEach(node => layer.Add(SavedNode.PopulateSavedNode(node)));
                savedANN.Network.Add(layer);
            }

            return savedANN;
        }
    }
}
