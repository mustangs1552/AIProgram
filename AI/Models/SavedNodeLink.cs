using AI.Classes;

namespace AI.Models
{
    public class SavedNodeLink
    {
        public string StartNodeName { get; set; } = "";
        public string EndNodeName { get; set; } = "";
        public float Weight { get; set; } = 1;

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
