using System.Collections.Generic;
using DecisionNS.Elements;

namespace DecisionNS.Data.Error
{
    public class DNSNodeErrorData
    {
        public List<DNSNode> Nodes { get; set; }

        public DNSNodeErrorData()
        {
            Nodes = new List<DNSNode>();
        }
    }
}