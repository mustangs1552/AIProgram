using AI.Enums;

namespace AI.Models
{
    /// <summary>
    /// Template for a new Artificial Nueral Network.
    /// </summary>
    public class ANNTemplate
    {
        /// <summary>
        /// Number of input nodes.
        /// </summary>
        public int NumOfInputNodes { get; set; } = 1;
        /// <summary>
        /// The algorithms to use for each of the hidden layer nodes.
        /// </summary>
        public List<List<Algorithms>> HiddenLayerNodes { get; set; } = new List<List<Algorithms>>();
        /// <summary>
        /// The number of hiddent layer nodes by counting the `HiddenLayerNodes`.
        /// </summary>
        public List<int> NumOfHiddentLayerNodes
        {
            get
            {
                List<int> layerCounts = new List<int>();
                if (HiddenLayerNodes == null) return layerCounts;
                foreach (List<Algorithms> layer in HiddenLayerNodes) layerCounts.Add(layer.Count);
                return layerCounts;
            }
        }
        /// <summary>
        /// The algorithms to use for each of the output nodes.
        /// </summary>
        public List<Algorithms> OutputNodes { get; set; } = new List<Algorithms>();
    }
}
