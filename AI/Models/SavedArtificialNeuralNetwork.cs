using AI.Classes;

namespace AI.Models
{
    public class SavedArtificialNeuralNetwork
    {
        public string Name { get; set; } = "";
        public List<SavedNode> InputNodes { get; set; } = new List<SavedNode>();
        public List<List<SavedNode>> Network { get; set; } = new List<List<SavedNode>>();

        public static SavedArtificialNeuralNetwork PopulateSavedANN(ArtificialNeuralNetwork ann)
        {
            if (ann == null) return new SavedArtificialNeuralNetwork();

            SavedArtificialNeuralNetwork savedANN = new SavedArtificialNeuralNetwork()
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
