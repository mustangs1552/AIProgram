using AI.Classes;
using AI.Enums;

namespace AI.Models
{
    public class SavedNode
    {
        public string Name { get; set; } = "";
        public List<SavedNodeLink> InputLinks { get; set; } = new List<SavedNodeLink>();
        public Algorithms Algorithm { get; set; } = Algorithms.None;
        public List<SavedNodeLink> OutputLinks { get; set; } = new List<SavedNodeLink>();

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
