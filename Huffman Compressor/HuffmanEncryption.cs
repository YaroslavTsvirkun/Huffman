using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman.Compressor
{
    /// <summary>
    /// Encodes and decodes strings by using a Huffman tree built from a source text.
    /// </summary>
    public class HuffmanCodec
    {
        private readonly Dictionary<char, int> frequencies = new();
        private readonly Dictionary<char, bool[]> bitCodeBook = new();
        private readonly Dictionary<char, string> codeTable = new();
        private Node? root;

        /// <summary>
        /// Gets the symbol frequency table from the most recent build operation.
        /// </summary>
        public IReadOnlyDictionary<char, int> Frequencies => frequencies;

        /// <summary>
        /// Gets the generated Huffman code table for each symbol.
        /// </summary>
        public IReadOnlyDictionary<char, string> CodeTable => codeTable;

        /// <summary>
        /// Gets a value indicating whether the codec has an active Huffman tree.
        /// </summary>
        public bool IsBuilt => root is not null;

        /// <summary>
        /// Builds a Huffman tree and code table from the supplied source text.
        /// </summary>
        /// <param name="source">The source text used to build the tree.</param>
        public void Build(string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (source.Length == 0)
            {
                throw new ArgumentException("The source text cannot be empty.", nameof(source));
            }

            frequencies.Clear();
            bitCodeBook.Clear();
            codeTable.Clear();
            root = null;

            foreach (var symbol in source)
            {
                frequencies.TryGetValue(symbol, out var count);
                frequencies[symbol] = count + 1;
            }

            var sequence = 0;
            var queue = new PriorityQueue<Node, (int Frequency, char MinSymbol, int Sequence)>();

            foreach (var entry in frequencies.OrderBy(entry => entry.Key))
            {
                var node = new Node(entry.Key, entry.Value);
                queue.Enqueue(node, (entry.Value, entry.Key, sequence++));
            }

            while (queue.Count > 1)
            {
                var left = queue.Dequeue();
                var right = queue.Dequeue();
                var parent = new Node(left, right);

                queue.Enqueue(parent, (parent.Frequency, parent.MinSymbol, sequence++));
            }

            root = queue.Dequeue();
            BuildCodeBook(root, new List<bool>());
        }

        /// <summary>
        /// Encodes text by using the currently built Huffman tree.
        /// </summary>
        /// <param name="source">The text to encode.</param>
        /// <returns>A bit array that represents the encoded text.</returns>
        public BitArray Encode(string source)
        {
            EnsureBuilt();
            ArgumentNullException.ThrowIfNull(source);

            if (source.Length == 0)
            {
                return new BitArray(0);
            }

            var encodedBits = new List<bool>();

            foreach (var symbol in source)
            {
                if (!bitCodeBook.TryGetValue(symbol, out var code))
                {
                    throw new ArgumentException(
                        "The source contains symbols that are not present in the current Huffman tree.",
                        nameof(source));
                }

                encodedBits.AddRange(code);
            }

            return new BitArray(encodedBits.ToArray());
        }

        /// <summary>
        /// Decodes a Huffman bit sequence by using the currently built tree.
        /// </summary>
        /// <param name="bits">The encoded bit sequence.</param>
        /// <returns>The decoded text.</returns>
        public string Decode(BitArray bits)
        {
            EnsureBuilt();
            ArgumentNullException.ThrowIfNull(bits);

            if (bits.Length == 0)
            {
                return string.Empty;
            }

            if (root!.IsLeaf)
            {
                return new string(root.Symbol, bits.Length);
            }

            var current = root;
            var decoded = new StringBuilder();

            foreach (bool bit in bits)
            {
                current = bit ? current.Right : current.Left;

                if (current is null)
                {
                    throw new InvalidOperationException("The bit sequence is not valid for the current Huffman tree.");
                }

                if (!current.IsLeaf)
                {
                    continue;
                }

                decoded.Append(current.Symbol);
                current = root;
            }

            if (!ReferenceEquals(current, root))
            {
                throw new InvalidOperationException("The bit sequence ended before a complete symbol was decoded.");
            }

            return decoded.ToString();
        }

        private void BuildCodeBook(Node node, List<bool> path)
        {
            if (node.IsLeaf)
            {
                var code = path.Count == 0 ? new[] { false } : path.ToArray();
                bitCodeBook[node.Symbol] = code;
                codeTable[node.Symbol] = string.Concat(code.Select(bit => bit ? '1' : '0'));
                return;
            }

            if (node.Left is not null)
            {
                path.Add(false);
                BuildCodeBook(node.Left, path);
                path.RemoveAt(path.Count - 1);
            }

            if (node.Right is not null)
            {
                path.Add(true);
                BuildCodeBook(node.Right, path);
                path.RemoveAt(path.Count - 1);
            }
        }

        private void EnsureBuilt()
        {
            if (root is null)
            {
                throw new InvalidOperationException("Build must be called before encoding or decoding.");
            }
        }
    }

    /// <summary>
    /// Legacy compatibility wrapper for older code that still uses the previous type name.
    /// </summary>
    [Obsolete("HuffmanEncryption is a legacy name. Use HuffmanCodec instead.")]
    public class HuffmanEncryption : HuffmanCodec
    {
    }
}
