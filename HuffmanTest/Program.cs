using Huffman.Compressor;
using System;
using System.Collections;

namespace HuffmanTest
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("Please enter the string:");
            var input = Console.ReadLine();
            var huffmanEncryption = new HuffmanEncryption();

            // Build the Huffman tree
            huffmanEncryption.Build(input);

            // Encode
            var encoded = huffmanEncryption.Encode(input);

            Console.Write("Encoded: ");
            foreach (Boolean bit in encoded)
            {
                Console.Write((bit ? 1 : 0) + "");
            }
            Console.WriteLine();

            // Decode
            var decoded = huffmanEncryption.Decode(encoded);

            Console.WriteLine("Decoded: " + decoded);
            Console.ReadLine();
        }
    }
}