using UnityEditor.Experimental.GraphView;

namespace DecisionNS.Utilities
{
    public static class NodeExtension
    {
        private static Port CreatePort(this Node node,
            string portName,
            Orientation orientation=Orientation.Horizontal,
            Direction direction=Direction.Input,
            Port.Capacity capacity=Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }
        
        public static Port CreateInput(this Node node, Port.Capacity capacity, string name="Input")
        {
            return node.CreatePort(name, direction:Direction.Input, capacity:capacity);
        }

        public static Port CreateOutput(this Node node, Port.Capacity capacity, string name="Output")
        {
            return node.CreatePort(name, direction:Direction.Output, capacity:capacity);
        }
    }
}