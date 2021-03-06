﻿#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryExecutor.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;

namespace Izual.Data.Common {
    public interface ICreateExecutor {
        QueryExecutor CreateExecutor();
    }

    public abstract class QueryExecutor {
        // called from compiled execution plan
        public abstract int RowsAffected{ get; }
        public abstract object Convert(object value, Type type);
        public abstract IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, EntryMapping entity, object[] paramValues);
        public abstract IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream);
        public abstract IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, EntryMapping entity, int batchSize, bool stream);
        public abstract IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, EntryMapping entity, object[] paramValues);
        public abstract int ExecuteCommand(QueryCommand query, object[] paramValues);
    }
}