﻿#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.RedundantJoinRemover.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Linq.Expressions;
using Izual.Linq;

namespace Izual.Data.Common {
    /// <summary>
    /// Removes joins expressions that are identical to joins that already exist
    /// </summary>
    public class RedundantJoinRemover : DbExpressionVisitor {
        private readonly Dictionary<TableAlias, TableAlias> map;

        private RedundantJoinRemover() {
            map = new Dictionary<TableAlias, TableAlias>();
        }

        public static Expression Remove(Expression expression) {
            return new RedundantJoinRemover().Visit(expression);
        }

        protected override Expression VisitJoin(JoinExpression join) {
            Expression result = base.VisitJoin(join);
            join = result as JoinExpression;
            if(join != null) {
                var right = join.Right as AliasedExpression;
                if(right != null) {
                    var similarRight = (AliasedExpression)FindSimilarRight(join.Left as JoinExpression, join);
                    if(similarRight != null) {
                        map.Add(right.Alias, similarRight.Alias);
                        return join.Left;
                    }
                }
            }
            return result;
        }

        private Expression FindSimilarRight(JoinExpression join, JoinExpression compareTo) {
            if(join == null)
                return null;
            if(join.Join == compareTo.Join) {
                if(join.Right.NodeType == compareTo.Right.NodeType && DbExpressionComparer.AreEqual(join.Right, compareTo.Right)) {
                    if(join.Condition == compareTo.Condition)
                        return join.Right;
                    var scope = new ScopedDictionary<TableAlias, TableAlias>(null);
                    scope.Add(((AliasedExpression)join.Right).Alias, ((AliasedExpression)compareTo.Right).Alias);
                    if(DbExpressionComparer.AreEqual(null, scope, join.Condition, compareTo.Condition))
                        return join.Right;
                }
            }
            Expression result = FindSimilarRight(join.Left as JoinExpression, compareTo);
            if(result == null) {
                result = FindSimilarRight(join.Right as JoinExpression, compareTo);
            }
            return result;
        }

        protected override Expression VisitColumn(ColumnExpression column) {
            TableAlias mapped;
            if(map.TryGetValue(column.Alias, out mapped)) {
                return new ColumnExpression(column.Type, column.QueryType, mapped, column.Name);
            }
            return column;
        }
    }
}