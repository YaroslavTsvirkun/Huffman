using System;
using System.Collections.Generic;
using System.Text;
using Huffman.Compressor;

namespace Huffman.Compressor
{
    public static  class Extentions
    {
        public static Boolean Equals<TOne, TTwo>(
            this TOne whatСompares, 
            TTwo comparable) => 
            new BooleanFunctions<TOne, TTwo, Boolean>()
            .Equals(whatСompares, comparable);

        public static Boolean NotEquals<TOne, TTwo>(
            this TOne whatСompares, 
            TTwo comparable) =>
            new BooleanFunctions<TOne, TTwo, Boolean>()
            .NotEquals(whatСompares, comparable);
    }
}