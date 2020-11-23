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
        public int Level { get; set; }
        public string NodeElement { get; set; }
        public string NodeName { get; set; }

        public abstract string GetTreeAsUnorderedLists();
        public abstract void SelectNode(AbstractNode currentComponent, AbstractNode prevComponent);
    }
}
