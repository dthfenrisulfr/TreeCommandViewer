using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TreeService.Tree
{
    class Node : AbstractNode
    {
        public Node(AbstractNode parent, string nodeName, bool isHead = false, int level = 0)
        {
            NodeName = nodeName;
            IsRoot = isHead;
            Level = level;
            NodeElement = $"<span>{NodeName}</span>";
            Parent = parent;
        }
        public override string GetTreeAsUnorderedLists()
        {
            string result;
            if (!IsRoot) result = $"<li>{NodeElement}<ul>";
            else result = $"<ul class=\"tree\" id=\"tree\"><li>{NodeElement}<ul>";

            result += Left?.GetTreeAsUnorderedLists();
            result += Right?.GetTreeAsUnorderedLists();

            return result + "</li></ul>";
        }
    }
}
