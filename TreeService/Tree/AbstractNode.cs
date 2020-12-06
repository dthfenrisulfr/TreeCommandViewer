using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeService.Tree
{
    abstract class AbstractNode
    {
        public AbstractNode Left { get; set; }
        public AbstractNode Right { get; set; }
        public bool IsRoot { get; set; }
        public bool NodeIsVisited { get; set; } = false;
        public int Level { get; set; }
        public string NodeElement { get; set; }
        public string NodeName { get; set; }

        public abstract string GetTreeAsUnorderedLists();
        public virtual void SelectNode(int count)
        {
            if(this is Leaf) NodeElement = $"<finish>{NodeName}</finish>";
            else
            {
                if (count == 3) NodeElement = $"<code>{NodeName}</code>";
                if (count == 2) NodeElement = $"<three>{NodeName}</three>";
                if (count == 1) NodeElement = $"<finish>{NodeName}</finish>";
            }
        }
        public async Task<AbstractNode> GetByName(AbstractNode head, string name)
        {
            Queue<AbstractNode> nodes = new Queue<AbstractNode>();
            nodes.Enqueue(head);
            await Task.Run(() =>
            {
                while (nodes.Count > 0)
                {
                    head = nodes.Dequeue();
                    if (head.NodeName == name) break;

                    if (head.Left != null)
                    {
                        nodes.Enqueue(head.Left);
                    }
                    if (head.Right != null)
                    {
                        nodes.Enqueue(head.Right);
                    }
                }
            });
            if (head.NodeName != name) return null;
            return head;
        }
    }
}
