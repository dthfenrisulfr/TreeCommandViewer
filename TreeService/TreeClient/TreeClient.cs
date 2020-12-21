using System;
using System.Collections.Generic;
using System.Text;
using TreeService.Builder;
using TreeService.Tree;

namespace TreeService.TreeClient
{
    public static class TreeClient
    {
        static TreeClient()
        {
            treeBuilder.TreeChannge += OnTreeChange;
            treeBuilder.IsComlete += OnComplete;
            treeBuilder.Step += OnStep;
        }

        static TreeBuilder treeBuilder = new TreeBuilder();
        public static EventHandler<string> TreeChannge { get; set; }
        public static EventHandler<string> Step { get; set; }
        public static EventHandler<bool> IsComplete { get; set; }
        public static string GetCurrentTree()
        {
            return $"{treeBuilder.GetSomeTree().GetTreeAsUnorderedLists()}";
        }
        public async static void NLR()
        {
            treeBuilder.ReCreateList();
            treeBuilder.ReqursionIsStoped = await treeBuilder.NLR(treeBuilder.GetTree());
            treeBuilder.GoToTree();
        }
        public async static void LNR()
        {
            treeBuilder.ReCreateList();
            treeBuilder.ReqursionIsStoped = await treeBuilder.LNR(treeBuilder.GetTree());
            treeBuilder.GoToTree();
        }

        public async static void LRN()
        {
            treeBuilder.ReCreateList();
            treeBuilder.ReqursionIsStoped = await treeBuilder.LRN(treeBuilder.GetTree());
            treeBuilder.GoToTree();
        }

        public async static void BFS()
        {
            treeBuilder.ReCreateList();
            treeBuilder.ReqursionIsStoped = await treeBuilder.BFS(treeBuilder.GetTree());
            treeBuilder.GoToTree();
        }

        private static void OnTreeChange(object sender, string arg)
        {
            TreeChannge.Invoke(sender, arg);
        }

        private static void OnStep(object sender, string arg)
        {
            Step?.Invoke(sender, arg);
        }

        private static void OnComplete(object sender, bool arg)
        {
            IsComplete.Invoke(sender, arg);
        }

        public static void Custom(string command)
        {
            treeBuilder.ReCreateList();
            treeBuilder.Custom(treeBuilder.GetSomeTree(), command);
        }

        public static void Stop()
        {
            treeBuilder.ReqursionIsStoped = true;
        }
        public static string GetTree(bool isBinary)
        {
            return $"{treeBuilder.Build(isBinary).GetTreeAsUnorderedLists()}";
        }
    }
}

