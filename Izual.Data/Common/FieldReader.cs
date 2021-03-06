﻿#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.FieldReader.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Izual.Linq;

namespace Izual.Data.Common {
    public abstract class FieldReader {
        private const TypeCode tcGuid = (TypeCode)20;
        private const TypeCode tcByteArray = (TypeCode)21;
        private const TypeCode tcCharArray = (TypeCode)22;
        private static Dictionary<Type, MethodInfo> _readerMethods;
        private static MethodInfo _miReadValue;
        private static MethodInfo _miReadNullableValue;
        private TypeCode[] typeCodes;

        protected abstract int FieldCount{ get; }

        protected void Init() {
            typeCodes = new TypeCode[FieldCount];
        }

        protected abstract Type GetFieldType(int ordinal);
        protected abstract bool IsDBNull(int ordinal);
        protected abstract T GetValue<T>(int ordinal);
        protected abstract Byte GetByte(int ordinal);
        protected abstract Char GetChar(int ordinal);
        protected abstract DateTime GetDateTime(int ordinal);
        protected abstract Decimal GetDecimal(int ordinal);
        protected abstract Double GetDouble(int ordinal);
        protected abstract Single GetSingle(int ordinal);
        protected abstract Guid GetGuid(int ordinal);
        protected abstract Int16 GetInt16(int ordinal);
        protected abstract Int32 GetInt32(int ordinal);
        protected abstract Int64 GetInt64(int ordinal);
        protected abstract String GetString(int ordinal);

        public T ReadValue<T>(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(T);
            }
            return GetValue<T>(ordinal);
        }

        public T? ReadNullableValue<T>(int ordinal) where T : struct {
            if(IsDBNull(ordinal)) {
                return default(T?);
            }
            return GetValue<T>(ordinal);
        }

        public Byte ReadByte(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Byte);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return (Byte)GetInt16(ordinal);
                    case TypeCode.Int32:
                        return (Byte)GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Byte)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Byte)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Byte)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Byte)GetDecimal(ordinal);
                    default:
                        return GetValue<Byte>(ordinal);
                }
            }
        }

        public Byte? ReadNullableByte(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Byte?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return (Byte)GetInt16(ordinal);
                    case TypeCode.Int32:
                        return (Byte)GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Byte)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Byte)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Byte)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Byte)GetDecimal(ordinal);
                    default:
                        return GetValue<Byte>(ordinal);
                }
            }
        }

        public Char ReadChar(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Char);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return (Char)GetByte(ordinal);
                    case TypeCode.Int16:
                        return (Char)GetInt16(ordinal);
                    case TypeCode.Int32:
                        return (Char)GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Char)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Char)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Char)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Char)GetDecimal(ordinal);
                    default:
                        return GetValue<Char>(ordinal);
                }
            }
        }

        public Char? ReadNullableChar(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Char?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return (Char)GetByte(ordinal);
                    case TypeCode.Int16:
                        return (Char)GetInt16(ordinal);
                    case TypeCode.Int32:
                        return (Char)GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Char)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Char)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Char)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Char)GetDecimal(ordinal);
                    default:
                        return GetValue<Char>(ordinal);
                }
            }
        }

        public DateTime ReadDateTime(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(DateTime);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.DateTime:
                        return GetDateTime(ordinal);
                    default:
                        return GetValue<DateTime>(ordinal);
                }
            }
        }

        public DateTime? ReadNullableDateTime(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(DateTime?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.DateTime:
                        return GetDateTime(ordinal);
                    default:
                        return GetValue<DateTime>(ordinal);
                }
            }
        }

        public Time ReadTime(int ordinal) {
            if (IsDBNull(ordinal))
                return default(Time);

            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    default:
                        return GetValue<Time>(ordinal);
                }
            }
        }

        public Time? ReadNullableTime(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Time?);
            }
            while (true) {
                switch (typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    default:
                        return GetValue<Time>(ordinal);
                }
            }
        }

        public Decimal ReadDecimal(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Decimal);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Decimal)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Decimal)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return GetDecimal(ordinal);
                    default:
                        return GetValue<Decimal>(ordinal);
                }
            }
        }

        public Decimal? ReadNullableDecimal(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Decimal?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Decimal)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Decimal)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return GetDecimal(ordinal);
                    default:
                        return GetValue<Decimal>(ordinal);
                }
            }
        }

        public Double ReadDouble(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Double);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return GetDouble(ordinal);
                    case TypeCode.Single:
                        return GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Double)GetDecimal(ordinal);
                    default:
                        return GetValue<Double>(ordinal);
                }
            }
        }

        public Double? ReadNullableDouble(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Double?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return GetDouble(ordinal);
                    case TypeCode.Single:
                        return GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Double)GetDecimal(ordinal);
                    default:
                        return GetValue<Double>(ordinal);
                }
            }
        }

        public Single ReadSingle(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Single);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Single)GetDouble(ordinal);
                    case TypeCode.Single:
                        return GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Single)GetDecimal(ordinal);
                    default:
                        return GetValue<Single>(ordinal);
                }
            }
        }

        public Single? ReadNullableSingle(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Single?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Single)GetDouble(ordinal);
                    case TypeCode.Single:
                        return GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Single)GetDecimal(ordinal);
                    default:
                        return GetValue<Single>(ordinal);
                }
            }
        }

        public Guid ReadGuid(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Guid);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case tcGuid:
                        return GetGuid(ordinal);
                    default:
                        return GetValue<Guid>(ordinal);
                }
            }
        }

        public Guid? ReadNullableGuid(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Guid?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case tcGuid:
                        return GetGuid(ordinal);
                    default:
                        return GetValue<Guid>(ordinal);
                }
            }
        }

        public Int16 ReadInt16(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Int16);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return (Int16)GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Int16)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Int16)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Int16)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Int16)GetDecimal(ordinal);
                    default:
                        return GetValue<Int16>(ordinal);
                }
            }
        }

        public Int16? ReadNullableInt16(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Int16?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return (Int16)GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Int16)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Int16)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Int16)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Int16)GetDecimal(ordinal);
                    default:
                        return GetValue<Int16>(ordinal);
                }
            }
        }

        public Int32 ReadInt32(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Int32);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Int32)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Int32)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Int32)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Int32)GetDecimal(ordinal);
                    default:
                        return GetValue<Int32>(ordinal);
                }
            }
        }

        public Int32? ReadNullableInt32(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Int32?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return (Int32)GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Int32)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Int32)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Int32)GetDecimal(ordinal);
                    default:
                        return GetValue<Int32>(ordinal);
                }
            }
        }

        public Int64 ReadInt64(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Int64);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Int64)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Int64)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Int64)GetDecimal(ordinal);
                    default:
                        return GetValue<Int64>(ordinal);
                }
            }
        }

        public Int64? ReadNullableInt64(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Int64?);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal);
                    case TypeCode.Int16:
                        return GetInt16(ordinal);
                    case TypeCode.Int32:
                        return GetInt32(ordinal);
                    case TypeCode.Int64:
                        return GetInt64(ordinal);
                    case TypeCode.Double:
                        return (Int64)GetDouble(ordinal);
                    case TypeCode.Single:
                        return (Int64)GetSingle(ordinal);
                    case TypeCode.Decimal:
                        return (Int64)GetDecimal(ordinal);
                    default:
                        return GetValue<Int64>(ordinal);
                }
            }
        }

        public String ReadString(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(String);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = Type.GetTypeCode(GetFieldType(ordinal));
                        continue;
                    case TypeCode.Byte:
                        return GetByte(ordinal).ToString();
                    case TypeCode.Int16:
                        return GetInt16(ordinal).ToString();
                    case TypeCode.Int32:
                        return GetInt32(ordinal).ToString();
                    case TypeCode.Int64:
                        return GetInt64(ordinal).ToString();
                    case TypeCode.Double:
                        return GetDouble(ordinal).ToString();
                    case TypeCode.Single:
                        return GetSingle(ordinal).ToString();
                    case TypeCode.Decimal:
                        return GetDecimal(ordinal).ToString();
                    case TypeCode.DateTime:
                        return GetDateTime(ordinal).ToString();
                    case tcGuid:
                        return GetGuid(ordinal).ToString();
                    case TypeCode.String:
                        return GetString(ordinal);
                    default:
                        return GetValue<String>(ordinal);
                }
            }
        }

        public Byte[] ReadByteArray(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Byte[]);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Byte:
                        return new[] {GetByte(ordinal)};
                    default:
                        return GetValue<Byte[]>(ordinal);
                }
            }
        }

        public Char[] ReadCharArray(int ordinal) {
            if(IsDBNull(ordinal)) {
                return default(Char[]);
            }
            while(true) {
                switch(typeCodes[ordinal]) {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal);
                        continue;
                    case TypeCode.Char:
                        return new[] {GetChar(ordinal)};
                    default:
                        return GetValue<Char[]>(ordinal);
                }
            }
        }

        private TypeCode GetTypeCode(int ordinal) {
            Type type = GetFieldType(ordinal);
            TypeCode tc = Type.GetTypeCode(type);
            if(tc == TypeCode.Object) {
                if(type == typeof(Guid))
                    tc = tcGuid;
                else if(type == typeof(Byte[]))
                    tc = tcByteArray;
                else if(type == typeof(Char[]))
                    tc = tcCharArray;
            }
            return tc;
        }

        public static MethodInfo GetReaderMethod(Type type) {
            if(_readerMethods == null) {
                List<MethodInfo> meths = typeof(FieldReader).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name.StartsWith("Read")).ToList();
                _readerMethods = meths.ToDictionary(m => m.ReturnType);
                _miReadValue = meths.Single(m => m.Name == "ReadValue");
                _miReadNullableValue = meths.Single(m => m.Name == "ReadNullableValue");
            }

            MethodInfo mi;
            _readerMethods.TryGetValue(type, out mi);
            if(mi == null) {
                if(TypeHelper.IsNullableType(type)) {
                    mi = _miReadNullableValue.MakeGenericMethod(TypeHelper.GetNonNullableType(type));
                }
                else {
                    mi = _miReadValue.MakeGenericMethod(type);
                }
            }
            return mi;
        }
    }
}