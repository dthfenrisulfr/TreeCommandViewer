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
        }

        static TreeBuilder treeBuilder = new TreeBuilder();
        public static EventHandler<string> TreeChannge { get; set; }
        public static string GetBinaryTree()
        {
            return $"{treeBuilder.GetBinaryTree().GetTreeAsUnorderedLists()}";
        }
        public async static void NLR()
        {
            treeBuilder.RequsionIsStoped = await treeBuilder.NLR(treeBuilder.GetTree());
        }
        public async static void LNR()
        {
            treeBuilder.RequsionIsStoped = await treeBuilder.LNR(treeBuilder.GetTree());
        }

        public async static void LRN()
        {
            treeBuilder.RequsionIsStoped = await treeBuilder.LRN(treeBuilder.GetTree());
        }

        public async static void BFS()
        {
            treeBuilder.RequsionIsStoped = await treeBuilder.BFS(treeBuilder.GetTree());
        }

        private static void OnTreeChange(object sender, string arg)
        {
            TreeChannge.Invoke(sender, arg);
        }

        public static void Custom(string command)
        {
            treeBuilder.Custom(treeBuilder.GetBinaryTree(), command);
        }

        public static void Stop()
        {
            treeBuilder.RequsionIsStoped = true;
        }
    }
}

