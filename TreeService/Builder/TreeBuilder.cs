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
        private int _nameCounter { get; set; }
        public bool IsBinary { get; set; }
        public bool ReqursionIsStoped { get; set; } = false;
        public void ReCreateList()
        {
            abstractNodes = new List<AbstractNode>();
        }
        public AbstractNode GetSomeTree()
        {
            _nameCounter = 0;
            tree = new Node($"{_nameCounter}", true);
            if (IsBinary) BuildBinaryTree(tree);
            else BuildTree(tree);
            return tree;
        }
        public AbstractNode GetTree()
        {
            GetSomeTree();
            return tree;
        }
        public AbstractNode Build(bool isBinary)
        {
            IsBinary = isBinary;
            return GetTree();
        }
        private void BuildBinaryTree(AbstractNode node)
        {
            var depth = 4;

            if (node.Level < depth - 1)
            {
                _nameCounter++;
                node.Left = new Node($"{$"{_nameCounter}"}", false, node.Level + 1);
                _nameCounter++;
                node.Right = new Node($"{_nameCounter}", false, node.Level + 1);
                BuildBinaryTree(node.Left);
                BuildBinaryTree(node.Right);
            }
            if (node.Level == depth - 1)
            {
                _nameCounter++;
                node.Left = new Leaf($"{_nameCounter}", node.Level + 1);
                _nameCounter++;
                node.Right = new Leaf($"{_nameCounter}", node.Level + 1);
            }
        }
        private void BuildTree(AbstractNode node) 
        {
            node.Left = new Node("1", false, 1)
            {
                Left = new Node("3", false, 2),
                Right = new Leaf("4", 2)
            };
            node.Left.Left.Left = new Node("7", false, 3)
            {
                Left = new Leaf("12", 3),
                Right = new Leaf("13", 3)
            };

            node.Right = new Node("2", false, 1)
            {
                Left = new Node("5", false, 1),
                Right = new Node("6", false, 1)
            };

            node.Right.Left.Left = new Node("8", false, 1);
            node.Right.Left.Right = new Node("9", false, 1);

            node.Right.Left.Left.Left = new Leaf("14", 1);
            node.Right.Left.Left.Right = new Leaf("15", 1);

            node.Right.Left.Right.Left = new Leaf("16", 1);
            node.Right.Left.Right.Right = new Leaf("17", 1);

            node.Right.Right.Right = new Node("11", false, 1)
            {
                Right = new Node("20", false, 1)
            };
            node.Right.Right.Right.Right.Left = new Leaf("21", 1);
            node.Right.Right.Left = new Node("10", false, 1);
            node.Right.Right.Left.Right = new Leaf("19", 1);
            node.Right.Right.Left.Left = new Leaf("18", 1);
        }
        public async Task<bool> NLR(AbstractNode node)
        {
            if (ReqursionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    VisitNode(node);
                    await NLR(node.Left);

                    if (!(node is Leaf) && node.Left != null) VisitNode(node);
                    await NLR(node.Right);
                    if (!(node is Leaf) && node.Right != null) VisitNode(node);
                }
            });
            return false;
        }

        public async Task<bool> LNR(AbstractNode node)
        {
            if (ReqursionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    await LNR(node.Left);
                    VisitNode(node);

                    await LNR(node.Right);
                    if (!(node is Leaf) && node.Right != null) VisitNode(node);
                }
            });
            return false;
        }

        public async Task<bool> LRN(AbstractNode node)
        {
            if (ReqursionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    await LRN(node.Left);
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
                    if (ReqursionIsStoped) break;
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

        private void VisitNode(AbstractNode node)
        {
            abstractNodes.Add(node);
        }

        public async void GoToTree()
        {
            IsComlete.Invoke(this, false);
            await Task.Run(() => {
                foreach (var node in abstractNodes)
                {
                    if (ReqursionIsStoped) break;
                    node.SelectNode(abstractNodes.Where(x => x == node).Count(), IsBinary);
                    TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                    Thread.Sleep(500);
                    abstractNodes = abstractNodes.Skip(1).ToList();
                }
            });
            ReqursionIsStoped = false;
            IsComlete.Invoke(this, true);
        }

        public async void Custom(AbstractNode node,string command)
        {
            var result = Regex.Split(command, @"\W|_").Where(x=>x.Length > 0).ToArray();

            var targetNode = await node.GetByName(node, result.Last());

            for (var i = result.Count() - 1; i != -1; i--)
            {
                if(result[i] == "car")
                {
                    if(targetNode?.Left != null) targetNode = targetNode?.Left;
                    else targetNode = targetNode?.Right;
                }
                if (result[i] == "cdr")
                {
                    if (targetNode?.Right != null) targetNode = targetNode?.Right;
                    else targetNode = targetNode?.Left;

                }
                targetNode?.SelectNode(1, IsBinary);
                TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                Thread.Sleep(500);
            }
        }
        public EventHandler<string> TreeChannge { get; set; }
        public EventHandler<bool> IsComlete { get; set; }
        private List<AbstractNode> abstractNodes = new List<AbstractNode>();
    }
}

