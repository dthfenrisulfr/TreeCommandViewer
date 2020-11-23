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
        private static AbstractNode prevNode;
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
                    node.SelectNode(node, prevNode);
                    TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                    prevNode = node;
                    Thread.Sleep(500);

                    await NLR(node.Left);
                    await NLR(node.Right);
                }
            });
            return false;
        }

        public async Task<bool> LNR(AbstractNode node)
        {
            if (RequsionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    await LNR(node.Left);
                    node.SelectNode(node, prevNode);
                    TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                    prevNode = node;
                    Thread.Sleep(500);

                    await LNR(node.Right);
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
                    await LRN(node.Right);
                    node.SelectNode(node, prevNode);
                    TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                    prevNode = node;
                    Thread.Sleep(500);
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
                    node.SelectNode(node, prevNode);
                    TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                    prevNode = node;
                    Thread.Sleep(500);

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
            node.SelectNode(node, prevNode);
            TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
            prevNode = node;
            Thread.Sleep(500);

            var result = Regex.Split(command, @"\W|_");

            foreach(var op in result)
            {
                if(op == "car")
                {
                    node = node?.Left;
                }
                if (op == "cdr")
                {
                    node = node?.Right;
                }
                node?.SelectNode(node, prevNode);
                TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                prevNode = node;
                Thread.Sleep(500);
            }
        }
        public EventHandler<string> TreeChannge { get; set; }
    }
}

