using System.Collections;
using System.Collections.Generic;

namespace SplayTree.SplayTree
{
    internal enum RotationType
    {
        Right,
        Left
    }

    public class SplayTree<T> : IEnumerable<T>
    {
        private SplayTreeNode<T> _root;
        public int Count { get; private set; }
        public IDataComparator<T> Comparator { get; set; }
        private const int NextData = 5;

        public SplayTree(IDataComparator<T> paComparator)
        {
            Count = 0;
            Comparator = paComparator;
        }

        public bool Add(T paItem)
        {
            var newNode = new SplayTreeNode<T>(paItem);
            if (Add(newNode) == null) return false;
            Splay(newNode);
            return true;
        }

        private SplayTreeNode<T> Add(SplayTreeNode<T> paNode)
        {
            var newNode = paNode;
            var actualNode = _root;
            int compareResult;

            if (_root == null)
            {
                _root = newNode;
                _root.Parent = null;
                Count++;
                return newNode;
            }

            while (true)
            {
                compareResult = Comparator.Compare<T>(actualNode.Data, newNode.Data);

                switch (compareResult)
                {
                    case -1:
                        if (actualNode.RightChild == null)
                        {
                            newNode.Parent = actualNode;
                            actualNode.RightChild = newNode;
                            Count++;
                            return newNode;
                        }
                        actualNode = actualNode.RightChild;
                        break;
                    case 0:
                        return null;
                    case 1:
                        if (actualNode.LeftChild == null)
                        {
                            newNode.Parent = actualNode;
                            actualNode.LeftChild = newNode;
                            Count++;
                            return newNode;
                        }
                        actualNode = actualNode.LeftChild;
                        break;
                }
            }
        }

        public bool Contains(T paData)
        {
            return Find(paData) != null;
        }

        protected SplayTreeNode<T> FindNode(T paData, bool paNeedSplay)
        {
            var actualNode = _root;
            int compareResult;

            if (_root == null) return null;
            if (paData == null) return null;

            while (true)
            {
                compareResult = Comparator.Compare<T>(actualNode.Data, paData);

                switch (compareResult)
                {
                    case -1:
                        if (actualNode.RightChild == null)
                        {
                            if (paNeedSplay) Splay(actualNode);
                            return null;
                        }
                        actualNode = actualNode.RightChild;
                        break;
                    case 0:
                        if (paNeedSplay) Splay(actualNode);
                        return actualNode;
                    case 1:
                        if (actualNode.LeftChild == null)
                        {
                            if (paNeedSplay) Splay(actualNode);
                            return null;
                        }
                        actualNode = actualNode.LeftChild;
                        break;
                }
            }
        }

        public T Find(T paData)
        {
            var result = FindNode(paData, true);
            return result != null ? result.Data : default(T);
        }

        public List<T> FindList(T paData)
        {
            var findRes = FindNode(paData, true);
            var restult = new List<T>();
            if (findRes != null)
            {
                restult.Add(findRes.Data);
            }
            else
            {
                var temp = _root.LeftChild;
                _root.LeftChild = null;
                var i = 0;
                foreach (var data in this)
                {
                    if (i != 0)
                    {
                        if (i > NextData)
                        {
                            break;
                        }
                        restult.Add(data);
                    }
                    else
                    {
                        if (Comparator.Compare<T>(paData, data) == -1)
                        {
                            restult.Add(data);
                            i++;
                        }
                    }
                    i++;
                }
                _root.LeftChild = temp;
            }
            return restult;
        }

        public void Delete(T paData)
        {
            var deleted = FindNode(paData, false);
            if (deleted == null) return;
            Count--;
            var parent = deleted.Parent;
            SplayTreeNode<T> findRes;

            if (deleted.RightChild != null)
            {
                findRes = deleted.RightChild;
                while (true)
                {
                    if (findRes.LeftChild != null)
                        findRes = findRes.LeftChild;
                    else
                        break;
                }
                deleted.Data = findRes.Data;
                if (findRes.RightChild != null)
                {
                    if (LeftPosition(findRes))
                        findRes.Parent.LeftChild = findRes.RightChild;
                    else
                        findRes.Parent.RightChild = findRes.RightChild;
                    findRes.RightChild.Parent = findRes.Parent;
                }
                else
                {
                    if (LeftPosition(findRes))
                        findRes.Parent.LeftChild = null;
                    else
                        findRes.Parent.RightChild = null;
                }

            }
            else if (deleted.LeftChild != null)
            {
                findRes = deleted.LeftChild;
                while (true)
                {
                    if (findRes.RightChild != null)
                        findRes = findRes.RightChild;
                    else
                        break;
                }
                deleted.Data = findRes.Data;
                if (findRes.LeftChild != null)
                {
                    if (LeftPosition(findRes))
                        findRes.Parent.LeftChild = findRes.LeftChild;
                    else
                        findRes.Parent.RightChild = findRes.LeftChild;
                    findRes.LeftChild.Parent = findRes.Parent;

                }
                else
                {
                    if (LeftPosition(findRes))
                        findRes.Parent.LeftChild = null;
                    else
                        findRes.Parent.RightChild = null;
                }
            }
            else
            {
                if (parent == null)
                {
                    _root = null;
                }
                else
                {
                    if (LeftPosition(deleted))
                        deleted.Parent.LeftChild = null;
                    else
                        deleted.Parent.RightChild = null;
                }
            }
            Splay(parent);
        }

        private void Splay(SplayTreeNode<T> paNode)
        {
            while (true)
            {
                if (paNode?.Parent == null) return;
                Rotation(paNode, LeftPosition(paNode) ? RotationType.Right : RotationType.Left);
            }
        }

        private bool LeftPosition(SplayTreeNode<T> paNode)
        {
            return paNode.Parent.LeftChild == paNode;
        }

        private void Rotation(SplayTreeNode<T> paRotatedNode, RotationType paRotationType)
        {
            if (paRotatedNode.Parent == null) return;

            var rotated = paRotatedNode;
            var parent = paRotatedNode.Parent;

            if (parent.Parent != null)
            {
                if (LeftPosition(parent))
                    parent.Parent.LeftChild = rotated;
                else
                    parent.Parent.RightChild = rotated;

                rotated.Parent = parent.Parent;
            }
            else
            {
                rotated.Parent = null;
                _root = rotated;
            }

            parent.Parent = rotated;

            if (paRotationType == RotationType.Right)
            {
                parent.LeftChild = rotated.RightChild;
                if (parent.LeftChild != null)
                    parent.LeftChild.Parent = parent;
                rotated.RightChild = parent;
            }
            else
            {
                parent.RightChild = rotated.LeftChild;
                if (parent.RightChild != null)
                    parent.RightChild.Parent = parent;
                rotated.LeftChild = parent;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new InOrderEnumerator<T>(_root);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}