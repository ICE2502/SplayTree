using System.Collections;
using System.Collections.Generic;

namespace SplayTree.SplayTree
{
    public class InOrderEnumerator<T> : IEnumerator<T>
    {
        public SplayTreeNode<T> CurrentNode { get; set; }
        private Stack<SplayTreeNode<T>> _stack;
        private SplayTreeNode<T> _ouput;

        public InOrderEnumerator(SplayTreeNode<T> paRoot)
        {
            _stack = new Stack<SplayTreeNode<T>>();
            CurrentNode = paRoot;
        }

        public void Dispose()
        {
            _stack = null;
        }

        public bool MoveNext()
        {
            while (_stack.Count > 0 || CurrentNode != null)
            {
                if (CurrentNode != null)
                {
                    _stack.Push(CurrentNode);
                    CurrentNode = CurrentNode.LeftChild;
                }
                else
                {
                    CurrentNode = _stack.Pop();
                    _ouput = CurrentNode;
                    CurrentNode = CurrentNode.RightChild;
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            _stack = new Stack<SplayTreeNode<T>>();
        }

        public T Current => _ouput.Data;

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
