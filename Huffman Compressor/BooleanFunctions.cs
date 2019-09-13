using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Huffman.Compressor
{
    public class BooleanFunctions<TArgsOne, TArgsTwo, TBoolean>
    {
        ///// <summary>
        ///// Allows you to perform a comparison operation "less than"
        ///// </summary>
        //public Func<TArgsOne, TArgsTwo, TBoolean> LessThan { get; set; } =
        //    TreeGenerator<TArgsOne, TArgsTwo, TBoolean>.BooleanExpressionToFunc((a, b) => Expression.LessThan(a, b));

        ///// <summary>
        ///// Allows you to perform a comparison operation "less than or equal to"
        ///// </summary>
        //public Func<TArgsOne, TArgsTwo, TBoolean> LessThanOrEquals { get; set; } = 
        //    TreeGenerator<TArgsOne, TArgsTwo, TBoolean>.BooleanExpressionToFunc((a, b) => LessThanOrEqual(a, b));

        ///// <summary>
        ///// Allows the comparison operation "greater than"
        ///// </summary>
        //public Func<TArgsOne, TArgsTwo, TBoolean> GreaterThan { get; set; } = 
        //    TreeGenerator<TArgsOne, TArgsTwo, TBoolean>.BooleanExpressionToFunc((a, b) => Expression.GreaterThan(a, b));

        ///// <summary>
        ///// Allows the operation to compare "greater than or equal to"
        ///// </summary>
        //public Func<TArgsOne, TArgsTwo, TBoolean> GreaterThanOrEquals { get; set; } = 
        //    TreeGenerator<TArgsOne, TArgsTwo, TBoolean>.BooleanExpressionToFunc((a, b) => GreaterThanOrEqual(a, b));

        /// <summary>
        /// Allows an equal operation
        /// </summary>
        public new Func<TArgsOne, TArgsTwo, TBoolean> Equals { get; set; } = 
            TreeGenerator<TArgsOne, TArgsTwo, TBoolean>.BooleanExpressionToFunc((a, b) => Expression.Equal(a, b));

        /// <summary>
        /// Allows you to perform a comparison operation "not equal"
        /// </summary>
        public Func<TArgsOne, TArgsTwo, TBoolean> NotEquals { get; set; } = 
            TreeGenerator<TArgsOne, TArgsTwo, TBoolean>.BooleanExpressionToFunc((a, b) => Expression.NotEqual(a, b));
    }

    internal static class TreeGenerator<TArgsOne, TArgsTwo, TBoolean>
    {
        internal static Func<TArgsOne, TArgsTwo, TBoolean> BooleanExpressionToFunc(
            Func<ParameterExpression, ParameterExpression, BinaryExpression> f)
        {
            var ap = Parameter(typeof(TArgsOne), "a");
            var bp = Parameter(typeof(TArgsTwo), "b");

            var lambda = Lambda<Func<TArgsOne, TArgsTwo, TBoolean>>(f(ap, bp), ap, bp);
            return lambda.Compile();
        }
    }
}