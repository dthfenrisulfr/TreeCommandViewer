using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeService.Tree
{
    class Leaf : AbstractNode
    {
        public bool IsVisited = false;
        public Leaf(string nodeName, int level = 0)
        {
            NodeName = nodeName;
            Level = level;
            NodeElement = $"<span>{NodeName}</span>";
        }
        public override string GetTreeAsUnorderedLists()
        {
            return $"<li>{NodeElement}</li>";
        }
        public override void SelectNode(AbstractNode currentComponent, AbstractNode prevComponent)
        {
            currentComponent.NodeElement = $"<code>{NodeName}</code>";
            if (!(prevComponent is null)) prevComponent.NodeElement = $"<span>{NodeName}</span>";
        }
    }
}
