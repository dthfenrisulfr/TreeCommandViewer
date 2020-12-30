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
        private void FillNodeCodes(AbstractNode node, bool? isLeft = null)
        {
            if (node != null)
            {
                if (isLeft == null)
                {
                    node.Code = "0";
                }
                if (node.Parent != null)
                {
                    if (isLeft == true)
                    {
                        if (node.Code.Length == 0) node.Code = $"car({node.Parent.Code})";
                    }
                    if (isLeft == false)
                    {
                        if (node.Code.Length == 0) node.Code = $"cdr({node.Parent.Code})";
                    }
                }

                FillNodeCodes(node.Left, true);

                FillNodeCodes(node.Right, false);
            }
        }
        public AbstractNode GetSomeTree()
        {
            _nameCounter = 0;
            tree = new Node(null, $"{_nameCounter}", true);
            if (IsBinary) BuildBinaryTree(tree);
            else BuildTree(tree);
            FillNodeCodes(tree);
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
                node.Left = new Node(node, $"{$"{_nameCounter}"}", false, node.Level + 1);
                _nameCounter++;
                node.Right = new Node(node, $"{_nameCounter}", false, node.Level + 1);
                BuildBinaryTree(node.Left);
                BuildBinaryTree(node.Right);
            }
            if (node.Level == depth - 1)
            {
                _nameCounter++;
                node.Left = new Leaf(node, $"{_nameCounter}", node.Level + 1);
                _nameCounter++;
                node.Right = new Leaf(node, $"{_nameCounter}", node.Level + 1);
            }
        }
        private void BuildTree(AbstractNode node) 
        {
            node.Left = new Node(node, "1", false, 1);
            node.Left.Left = new Node(node.Left, "3", false, 2);
            node.Left.Right = new Leaf(node.Left, "4", 2);

            node.Right = new Node(node, "2", false, 1);
            node.Right.Left = new Node(node.Right, "5", false, 1);
            node.Right.Right = new Node(node.Right, "6", false, 1);

            node.Left.Left.Left = new Node(node.Left.Left, "7", false, 3);
            node.Left.Left.Left.Left = new Leaf(node.Left.Left.Left, "12", 3);
            node.Left.Left.Left.Right = new Leaf(node.Left.Left.Left, "13", 3);

            node.Right.Left.Left = new Node(node.Right.Left, "8", false, 1);
            node.Right.Left.Right = new Node(node.Right.Left, "9", false, 1);
            node.Right.Right.Left = new Node(node.Right.Right, "10", false, 1);

            node.Right.Right.Right = new Node(node.Right.Right, "11", false, 1);
            node.Right.Right.Right.Right = new Node(node.Right.Right.Right, "20", false, 1);

            node.Right.Left.Left.Left = new Leaf(node.Right.Left.Left, "14", 1);
            node.Right.Left.Left.Right = new Leaf(node.Right.Left.Left, "15", 1);

            node.Right.Left.Right.Left = new Leaf(node.Right.Left.Right, "16", 1);
            node.Right.Left.Right.Right = new Leaf(node.Right.Left.Right, "17", 1);
            node.Right.Right.Left.Left = new Leaf(node.Right.Right.Left, "18", 1);
            node.Right.Right.Left.Right = new Leaf(node.Right.Right.Left, "19", 1);

            node.Right.Right.Right.Right.Left = new Leaf(node.Right.Right.Right.Right, "21", 1);
        }
        public async Task<bool> NLR(AbstractNode node, bool? isLeft = null)
        {
            if (ReqursionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {

                       VisitNode(node);

                    await NLR(node.Left, true);
                    if (!(node is Leaf) && node.Left != null)
                    {

                        VisitNode(node);
                    }

                    await NLR(node.Right, false);
                    if (!(node is Leaf) && node.Right != null)
                    {
                        VisitNode(node);
                    }
                }
            });
            return false;
        }

        public async Task<bool> LNR(AbstractNode node, bool? isLeft = null)
        {
            if (ReqursionIsStoped) return false;
            await Task.Run(async () =>
            {
                if (node != null)
                {
                    await LNR(node.Left);
                    VisitNode(node);

                    await LNR(node.Right);
                    if (!(node is Leaf) && node.Right != null)
                    {
                        VisitNode(node);
                    }
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
                    Step?.Invoke(this, node.Code);
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
                    if (targetNode?.Left != null)
                    {
                        targetNode = targetNode?.Left;
                        Step?.Invoke(this, targetNode.Code);
                    }
                    else 
                    { 
                        targetNode = targetNode?.Right;
                        Step?.Invoke(this, targetNode.Code);
                    }
                }
                if (result[i] == "cdr")
                {   
                    if (targetNode?.Right != null)
                    {
                        targetNode = targetNode?.Right;
                        Step?.Invoke(this, targetNode.Code);
                    }
                    else
                    {
                        targetNode = targetNode?.Left;
                        Step?.Invoke(this, targetNode.Code);
                    }

                }
                targetNode?.SelectNode(1, IsBinary);
                TreeChannge.Invoke(this, tree.GetTreeAsUnorderedLists());
                Thread.Sleep(500);
            }
        }
        public EventHandler<string> TreeChannge { get; set; }
        public EventHandler<string> Step { get; set; }
        public EventHandler<bool> IsComlete { get; set; }
        private List<AbstractNode> abstractNodes = new List<AbstractNode>();
    }
}

