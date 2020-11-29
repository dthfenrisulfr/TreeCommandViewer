using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TreeService.Tree;

namespace TreeService.Builder
{
    class TreeBuilder
    {
        private static Node tree;
        public bool RequsionIsStoped { get; set; } = false;
        public AbstractNode GetBinaryTree()
        {
            tree = new Node("", true);
            BuildBinaryTree(tree);
            return tree;
        }
        public AbstractNode GetTree()
        {
            GetBinaryTree();
            return tree;
        }
        private int BuildBinaryTree(AbstractNode node)
        {
            var depth = 4;

            if (node.Level < depth - 1)
            {
                node.Left = new Node("", false, node.Level + 1);
                node.Right = new Node("", false, node.Level + 1);
                BuildBinaryTree(node.Left);
                BuildBinaryTree(node.Right);
            }
            if (node.Level == depth - 1)
            {
                node.Left = new Leaf("", node.Level + 1);
                node.Right = new Leaf("", node.Level + 1);
            }

            return 0;
        }
        public async Task<bool> NLR(AbstractNode node)
        {
            if (RequsionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    VisitNode(node);
                    await NLR(node.Left);

                    if (!(node is Leaf)) VisitNode(node);
                    await NLR(node.Right);
                    if (!(node is Leaf)) VisitNode(node);
                }
            });
            return false;
        }

        private void VisitNode(AbstractNode node)
        {
            node.SelectNode();
            TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
            Thread.Sleep(500);

            node.NodeIsVisited = !node.NodeIsVisited;
        }

        public async Task<bool> LNR(AbstractNode node)
        {
            if (RequsionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    await LNR(node.Left);
                    VisitNode(node);

                    await LNR(node.Right);
                    if (!(node is Leaf)) VisitNode(node);
                }
            });
            return false;
        }

        public async Task<bool> LRN(AbstractNode node)
        {
            if (RequsionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    await LRN(node.Left);
                    //if (!(node is Leaf)) VisitNode(node);
                    await LRN(node.Right);
                    VisitNode(node);
                }
            });
            return false;
        }

        public async Task<bool> BFS(AbstractNode node)
        {
            Queue<AbstractNode> nodes = new Queue<AbstractNode>();
            nodes.Enqueue(node);
            await Task.Run(() =>
            {
                while (nodes.Count > 0)
                {
                    if (RequsionIsStoped) break;
                    node = nodes.Dequeue();
                    VisitNode(node);

                    if (node.Left != null)
                    {
                        nodes.Enqueue(node.Left);
                    }
                    if (node.Right != null)
                    {
                        nodes.Enqueue(node.Right);
                    }
                }
            });
            return false;
        }

        public void Custom(AbstractNode node,string command)
        {
            node.SelectNode();
            TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
            Thread.Sleep(500);

            var result = Regex.Split(command, @"\W|_").ToArray();

            for(var i = result.Count() - 1; i != -1; i--)
            {
                if(result[i] == "car")
                {
                    node = node?.Left;
                }
                if (result[i] == "cdr")
                {
                    node = node?.Right;
                }
                VisitNode(node);
            }
        }
        public EventHandler<string> TreeChannge { get; set; }
    }
}

