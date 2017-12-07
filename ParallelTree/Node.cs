using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BSTParallel
{
    class Node
    {
        public int key;
        public int value;

        public Node parent = null;
        public Node leftChild = null;
        public Node rightChild = null;

        public Node(int k, int v)
        {
            key = k;
            value = v;
        }

        public Node(int k, int v, Node p)
        {
            key = k;
            value = v;
            parent = p;
        }
    }
}
