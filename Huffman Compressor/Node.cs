using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Huffman.Compressor
{
    internal class Node
    {
        internal Char Symbol { get; private set; }
        internal Int32 Frequency { get; private set; }
        internal Node Right { get; private set; }
        internal Node Left { get; private set; }

        public Node(Char symbol, Int32 frequency)
        {
            this.Symbol = symbol;
            this.Frequency = frequency;
        }

        public Node(Char symbol, Int32 frequency, Node left, Node right) : this(symbol, frequency)
        {
            this.Left = left;
            this.Right = right;
        }

        public List<Boolean> Traverse(Char symbol, List<Boolean> data)
        {

            if (IsNode(Right) && IsNode(Left))
            {
                return symbol.Equals(Symbol) ? data : null;
            }
            else
            {
                List<Boolean> left = null;
                List<Boolean> right = null;

                if (IsNotNode(Left))
                {
                    List<Boolean> leftPath = new List<Boolean>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(symbol, leftPath);
                }

                if (IsNotNode(Right))
                {
                    List<Boolean> rightPath = new List<Boolean>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(symbol, rightPath);
                }

                return left.NotEquals<List<Boolean>, Object>(null) ? left : right;
            }
        }

        private Boolean IsNotNode(Node node) => node.NotEquals<Node, Object>(null);
        private Boolean IsNode(Node node) => node.Equals<Node, Object>(null);
    }
}