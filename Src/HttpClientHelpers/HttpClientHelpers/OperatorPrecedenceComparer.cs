using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientHelpers
{
    public class OperatorPrecedenceComparer : Comparer<ExpressionType>
    {
        public override int Compare(ExpressionType x, ExpressionType y)
        {
            return Precedence(x).CompareTo(Precedence(y));
        }

        private int Precedence(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.MemberAccess:
                case ExpressionType.Constant: // ??
                case ExpressionType.Call:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.AndAlso:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                return 1;
                case ExpressionType.OrElse:
                return 0;
                default:
                return 2;
            }

            //switch (expressionType)
            //{
            //        //Primary
            //    case ExpressionType.MemberAccess:
            //    case ExpressionType.Constant: // ??
            //    case ExpressionType.Call:
            //    case ExpressionType.Convert:
            //        //f(x)
            //    case ExpressionType.NewArrayInit:
            //    case ExpressionType.NewArrayBounds:
            //    case ExpressionType.PostDecrementAssign:
            //    case ExpressionType.PostIncrementAssign:
            //    case ExpressionType.New:
            //    case ExpressionType.TypeIs:
            //        //case ExpressionType.checked ??
            //        //unchecked
            //    case ExpressionType.Default:
            //    case ExpressionType.Invoke:
            //        //sizeof
            //        //->
            //    return 1;
            //        //Unary
            //    case ExpressionType.UnaryPlus:
            //        //-x
            //    case ExpressionType.Not:
            //    case ExpressionType.OnesComplement:
            //    case ExpressionType.PreIncrementAssign:
            //    case ExpressionType.PreDecrementAssign:
            //        //&x
            //        //*x
            //    return 2;
            //        // Multiplicative
            //    case ExpressionType.Multiply:
            //    case ExpressionType.Divide:
            //    case ExpressionType.Modulo:
            //    return 3;
            //        //Additive
            //    case ExpressionType.Add:
            //    case ExpressionType.Subtract:
            //    return 4;
            //        //Shift:
            //    case ExpressionType.LeftShift:
            //    case ExpressionType.RightShift:
            //    return 5;
            //        // Relational and type testing
            //    case ExpressionType.LessThan:
            //    case ExpressionType.GreaterThan:
            //    case ExpressionType.LessThanOrEqual:
            //    case ExpressionType.GreaterThanOrEqual:
            //    // is and as
            //    return 6;
            //        //Equality
            //    case ExpressionType.Equal:
            //    case ExpressionType.NotEqual:
            //    return 7;
            //        //Logical And
            //    case ExpressionType.And:
            //    return 8;
            //        //Logical Xor
            //    case ExpressionType.ExclusiveOr:
            //    return 9;
            //        //Logical Or
            //    case ExpressionType.Or:
            //    return 10;
            //        //Conditional And
            //    case ExpressionType.AndAlso:
            //    return 11;
            //        //Conditional Or
            //    case ExpressionType.OrElse:
            //    return 12;
            //        //Null-coalescing
            //    case ExpressionType.Coalesce:
            //    return 13;
            //        //Conditional ?:
            //    case ExpressionType.Conditional:
            //    return 14;
            //        //Assignment and lambda expression
            //    case ExpressionType.Assign:
            //    case ExpressionType.AddAssign:
            //    case ExpressionType.SubtractAssign:
            //    case ExpressionType.MultiplyAssign:
            //    case ExpressionType.DivideAssign:
            //    case ExpressionType.ModuloAssign:
            //    case ExpressionType.AndAssign:
            //    case ExpressionType.OrAssign:
            //    case ExpressionType.ExclusiveOrAssign:
            //    case ExpressionType.LeftShiftAssign:
            //    case ExpressionType.RightShiftAssign:
            //    case ExpressionType.Lambda:
            //    return 16;
            //    default:
            //    return 16;
            //}
        }
    }
}
