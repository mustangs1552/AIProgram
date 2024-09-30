using AI.Models;

namespace AI.Classes
{
    /// <summary>
    /// Nodes that are used for supplying inputs to the neural network.
    /// </summary>
    public class InputNode : Node
    {
        public InputNode() : base() { }
        public InputNode(string name) : base(name) { }
        public InputNode(SavedNode savedNode) : base(savedNode) { }

        /// <summary>
        /// Set the node's value as the current input value.
        /// </summary>
        /// <param name="value">The input value.</param>
        public void SetInputValue(float value)
        {
            Value = value;
        }
    }
}
