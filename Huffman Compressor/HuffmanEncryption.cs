using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman.Compressor
{
    public class HuffmanEncryption
    {
        private List<Node> nodes;

        internal Node Root { get; set; }

        public Dictionary<Char, Int32> Frequencies { get; set; }

        public HuffmanEncryption()
        {
            nodes = new List<Node>();
            Frequencies = new Dictionary<Char, Int32>();
        }

        public void Build(String source)
        {
            source.ToList().ForEach(
                action => {
                    if(!Frequencies.ContainsKey(action)) Frequencies.Add(action, 0);
                    Frequencies[action]++;
                });
        
            Frequencies.ToList().ForEach(
                action => nodes.Add(new Node(action.Key, action.Value)));
        
            while (nodes.Count > 1)
            {
                var orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();
        
                if (orderedNodes.Count >= 2)
                {
                    var taken = orderedNodes.Take(2).ToList();
                    var frequency = taken[0].Frequency + taken[1].Frequency;
                    var parent = new Node('*', frequency, taken[0], taken[1]);
        
                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }
                this.Root = nodes.FirstOrDefault();
            }
        }

        public BitArray Encode(String source)
        {
            var encodedSource = new List<Boolean>();
            source.ToList().ForEach(
                action => encodedSource.AddRange(
                    Root.Traverse(action, new List<Boolean>())));
            return new BitArray(encodedSource.ToArray());
        }

        public String Decode(BitArray bits)
        {
            Node current = this.Root;
            String decoded = String.Empty;
        
            foreach(Boolean bit in bits)
            {
                if(bit) current = IsNotNode(current.Left) ? current.Right : null;
                else current = IsNotNode(current.Right) ? current.Left : null;
        
                if(IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }
            return decoded;
        }

        private Boolean IsLeaf(Node node) => IsNode(node.Left) && IsNode(node.Right);
        private Boolean IsNode(Node node) => node.Equals<Node, Object>(null);
        private Boolean IsNotNode(Node node) => node.NotEquals<Node, Object>(null);
    }
}