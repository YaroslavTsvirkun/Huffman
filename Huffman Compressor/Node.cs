namespace Huffman.Compressor
{
    internal sealed class Node
    {
        internal Node(char symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
            MinSymbol = symbol;
        }

        internal Node(Node left, Node right)
        {
            Left = left;
            Right = right;
            Frequency = checked(left.Frequency + right.Frequency);
            MinSymbol = left.MinSymbol < right.MinSymbol ? left.MinSymbol : right.MinSymbol;
        }

        internal char Symbol { get; }
        internal int Frequency { get; }
        internal char MinSymbol { get; }
        internal Node? Right { get; }
        internal Node? Left { get; }
        internal bool IsLeaf => Left is null && Right is null;
    }
}
