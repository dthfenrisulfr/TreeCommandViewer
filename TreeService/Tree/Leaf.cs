using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeService.Tree
{
    class Leaf : AbstractNode
    {
        public bool IsVisited = false;
        public Leaf(AbstractNode parent, string nodeName, int level = 0)
        {
            NodeName = nodeName;
            Level = level;
            NodeElement = $"<span>{NodeName}</span>";
            Parent = parent;
        }
        public override string GetTreeAsUnorderedLists()
        {
            return $"<li>{NodeElement}</li>";
        }
    }
}
