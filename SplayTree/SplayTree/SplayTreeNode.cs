namespace SplayTree.SplayTree
{
    public class SplayTreeNode<T>
    {
        public T Data { get; set; }
        public SplayTreeNode<T> RightChild { get; set; }
        public SplayTreeNode<T> LeftChild { get; set; }
        public SplayTreeNode<T> Parent { get; set; }

        public SplayTreeNode(T paData)
        {
            Parent = null;
            Data = paData;
        }
    }
}