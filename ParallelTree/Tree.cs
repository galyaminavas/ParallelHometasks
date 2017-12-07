namespace BSTParallel
{
    class Tree
    {
        public Node root = null;

        public Node Search(int k)
        {
            var curr = root;

            if (curr == null)
                return curr;

            while (curr != null)
            {
                if (curr.parent != null)
                {
                    lock (curr.parent)
                    {
                        lock (curr)
                        {
                            if (k == curr.key)
                                return curr;
                            else if (k < curr.key)
                                curr = curr.leftChild;
                            else
                                curr = curr.rightChild;
                        }
                    }
                }
                else
                {
                    lock (curr)
                    {
                        if (k == curr.key)
                            return curr;
                        else if (k < curr.key)
                            curr = curr.leftChild;
                        else
                            curr = curr.rightChild;
                    }
                } 
            }
            return null;
        }

        public void Insert(int k, int v)
        {
            Node curr = root;

            lock (this)
            {
                if (root == null)
                {
                    root = new Node(k, v);
                    return;
                }
                curr = root;
            }

            while (curr != null)
            {
                if (curr.parent != null)
                {
                    lock (curr.parent)
                    {
                        lock (curr)
                        {
                            if (k < curr.key)
                            {
                                if (curr.leftChild == null)
                                {
                                    curr.leftChild = new Node(k, v, curr);
                                    return;
                                }
                                else
                                {
                                    lock (curr.leftChild)
                                        curr = curr.leftChild;
                                }
                            }
                            else if (k > curr.key)
                            {
                                if (curr.rightChild == null)
                                {
                                    curr.rightChild = new Node(k, v, curr);
                                    return;
                                }
                                else
                                {
                                    lock (curr.rightChild)
                                        curr = curr.rightChild;
                                }
                            }
                            else
                                return;
                        }
                    }
                }
                else
                {
                    lock (curr)
                    {
                        if (k < curr.key)
                        {
                            if (curr.leftChild == null)
                            {
                                curr.leftChild = new Node(k, v, curr);
                                return;
                            }
                            else
                            {
                                lock (curr.leftChild)
                                    curr = curr.leftChild;
                            }
                        }
                        else if (k > curr.key)
                        {
                            if (curr.rightChild == null)
                            {
                                curr.rightChild = new Node(k, v, curr);
                                return;
                            }
                            else
                            {
                                lock (curr.rightChild)
                                    curr = curr.rightChild;
                            }
                        }
                        else
                            return;
                    }
                }
            }
        }

        internal Node Min(Node x)
        {
            lock (x)
            {
                if (x.leftChild == null)
                    return x;
            }
            return Min(x.leftChild);
        }

        public void Remove(int k)
        {
            var node = Search(k);

            if (node == null)
                return;

            var succ = GetSuccessor(node);

            if (node.parent != null)
            {
                lock (node.parent)
                {
                    lock (node)
                    {
                        if (succ != null)
                        {
                            lock (succ)
                            {
                                if (node.leftChild == null || node.rightChild == null)
                                {
                                    if (node.parent.leftChild == node)
                                    {
                                        node.parent.leftChild = succ;
                                        succ.parent = node.parent;
                                    }
                                    else
                                    {
                                        node.parent.rightChild = succ;
                                        succ.parent = node.parent;
                                    }
                                }
                                else
                                {
                                    if (node.rightChild == succ)
                                    {
                                        if (node.parent.leftChild == node)
                                        {
                                            node.parent.leftChild = succ;
                                            succ.parent = node.parent;
                                            succ.leftChild = node.leftChild;
                                        }
                                        else
                                        {
                                            node.parent.rightChild = succ;
                                            succ.parent = node.parent;
                                            succ.leftChild = node.leftChild;
                                        }
                                    }
                                    else if (succ.rightChild == null)
                                    {
                                        node.key = succ.key;
                                        node.value = succ.value;
                                        succ.parent.leftChild = null;
                                    }
                                    else
                                    {
                                        node.key = succ.key;
                                        node.value = succ.value;
                                        succ.parent.leftChild = succ.rightChild;
                                        succ.rightChild.parent = succ.parent;  
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (node.parent.leftChild == node)
                                node.parent.leftChild = null;
                            else
                                node.parent.rightChild = null;
                        }
                    }
                }
            }
            else
            {
                lock (node)
                {
                    if (succ != null)
                    {
                        lock (succ)
                        {
                            if (node.leftChild == null || node.rightChild == null)
                            {
                                succ.parent = null;
                                root = succ;
                            }
                            else
                            {
                                if (succ == node.rightChild)
                                {
                                    succ.parent = null;
                                    succ.leftChild = node.leftChild;
                                    node.leftChild.parent = succ;
                                    root = succ;
                                }
                                else
                                {
                                    succ.parent.leftChild = succ.rightChild;
                                    if (succ.rightChild != null)
                                        succ.rightChild.parent = succ.parent;
                                    succ.parent = null;
                                    node.leftChild.parent = succ;
                                    root = succ;
                                }
                            }
                        }
                    }
                    else
                    {
                        root = null;
                    }
                }
            }
        }

        internal Node GetSuccessor(Node curr)
        {
            Node succ = null;

            if (curr.leftChild == null && curr.rightChild == null)
                return succ;
            else if (curr.leftChild == null)
            {
                succ = curr.rightChild;
                return succ;
            }
            else if (curr.rightChild == null)
            {
                succ = curr.leftChild;
                return succ;
            }
            else
            {
                succ = Min(curr.rightChild);
                return succ;
            }
        }
    }
}
