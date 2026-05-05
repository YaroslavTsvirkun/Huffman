using System.Collections;
using Huffman.Compressor;

namespace Huffman.Tests;

public class HuffmanCodecTests
{
    [Fact]
    public void EncodeAndDecode_RoundTrips_Text()
    {
        const string text = "hello world";
        var codec = new HuffmanCodec();

        codec.Build(text);

        var encoded = codec.Encode(text);
        var decoded = codec.Decode(encoded);

        Assert.Equal(text, decoded);
        Assert.NotEmpty(codec.CodeTable);
    }

    [Fact]
    public void EncodeAndDecode_RoundTrips_SingleRepeatedCharacter()
    {
        const string text = "aaaaaa";
        var codec = new HuffmanCodec();

        codec.Build(text);

        var encoded = codec.Encode(text);

        Assert.Equal(text.Length, encoded.Length);
        Assert.Equal("0", codec.CodeTable['a']);
        Assert.Equal(text, codec.Decode(encoded));
    }

    [Fact]
    public void Build_Rejects_EmptyText()
    {
        var codec = new HuffmanCodec();

        var exception = Assert.Throws<ArgumentException>(() => codec.Build(string.Empty));

        Assert.Equal("source", exception.ParamName);
    }

    [Fact]
    public void Encode_Rejects_CharactersOutsideTree()
    {
        var codec = new HuffmanCodec();
        codec.Build("abc");

        var exception = Assert.Throws<ArgumentException>(() => codec.Encode("abd"));

        Assert.Equal("source", exception.ParamName);
    }

    [Fact]
    public void Decode_Rejects_IncompleteBitSequence()
    {
        var codec = new HuffmanCodec();
        codec.Build("aaabbc");

        var longCode = codec.CodeTable.Values.First(code => code.Length > 1);
        var incompleteBits = new BitArray(longCode[..^1].Select(bit => bit == '1').ToArray());

        Assert.Throws<InvalidOperationException>(() => codec.Decode(incompleteBits));
    }

    [Fact]
    public void Encode_Requires_BuiltTree()
    {
        var codec = new HuffmanCodec();

        Assert.Throws<InvalidOperationException>(() => codec.Encode("test"));
    }
}
