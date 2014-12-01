using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientHelpers
{
    /*
     * Missing implementations for:
     * Any()
     */
    public class ODataExpressionVisitor : ExpressionVisitor
    {
        private bool _useParameter = false;

        private readonly StringBuilder _queryBuilder;
        private readonly IComparer<ExpressionType> _comparer;

        public ODataExpressionVisitor()
        {
            _queryBuilder = new StringBuilder();
            _comparer = new OperatorPrecedenceComparer();
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (_useParameter)
                _queryBuilder.Append(node.Expression.ToString() + "/");

            _queryBuilder.Append(node.Member.Name);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _queryBuilder.Append(GetValue(node.Value));
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.NodeType == ExpressionType.Call)            
            {
                Visit(node.Arguments[0]);
                _queryBuilder.Append("/" + node.Method.Name.ToLower() + "(");
                _useParameter = true;
                Visit(node.Arguments[1]);
                _useParameter = false;
                _queryBuilder.Append(")");
                return node;
            }

            return base.VisitMethodCall(node);;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                _queryBuilder.Append("not(");
                var exp = base.VisitUnary(node);
                _queryBuilder.Append(")");
                return exp;
            }

            return base.VisitUnary(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node, node.Left);
            _queryBuilder.Append(GetOperator(node.NodeType));
            var memberLeft = node.Left as MemberExpression;
            
            if (memberLeft != null && memberLeft.Expression is ParameterExpression)
            {
                var f = Expression.Lambda(node.Right).Compile();
                var value = f.DynamicInvoke();
                _queryBuilder.Append(GetValue(value));
            }
            else
                Visit(node, node.Right);
            
            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (_useParameter)
                _queryBuilder.Append(node.Parameters[0] + ": ");

            Visit(node.Body);
            return node;
        }

        private void Visit(Expression parent, Expression child, object value = null)
        {
            if (_comparer.Compare(child.NodeType, parent.NodeType) < 0)
            {
                _queryBuilder.Append("(");
                Visit(child);
                _queryBuilder.Append(")");
            }
            else
                Visit(child);
        }

        public override string ToString()
        {
            return _queryBuilder.ToString();
        }

        private string GetOperator(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Equal:
                    return " eq ";
                case ExpressionType.NotEqual:
                    return " ne ";
                case ExpressionType.AndAlso:
                    return " and ";
                case ExpressionType.OrElse:
                    return " or ";
                case ExpressionType.GreaterThan:
                    return " gt ";
                case ExpressionType.LessThan:
                    return " lt ";
                case ExpressionType.GreaterThanOrEqual:
                    return " ge ";
                case ExpressionType.LessThanOrEqual:
                    return " le ";
                default:
                    throw new NotImplementedException("Only filter operators are implemented");
            }
        }

        private string GetValue(object value)
        {
            if (value == null) return "null";
            else if (value is Guid || value is Guid?)
                return string.Format("guid'{0}'", value);
            else if (value is string)
                return string.Format("'{0}'", value);
            else if (value is DateTime || value is DateTime?)
                return string.Format("datetime'{0:yyyy-MM-ddTHH:mm:ss}'", value);
            else if (value is bool)
                return value.ToString().ToLower();

            return value.ToString();
        }
    }
}
