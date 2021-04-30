using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSnappy
{
    public static class EnumerableDataReaderExtensions
    {
        public static IDataReader ToUnsafeDataReader<T>(this IEnumerable<T> source, IList<Func<T, object>> valuegetters)
        {
            return new EnumerableDataReader<T>(source.GetEnumerator(), valuegetters);
        }
    }

    internal class EnumerableDataReader<T> : IDataReader
    {
        IEnumerator<T> _source;
        IList<Func<T, object>> _valuegetters;
        int _fieldcount;
        private readonly DataTable _table = new DataTable();

        internal EnumerableDataReader(IEnumerator<T> source, IList<Func<T, object>> valuegetters)
        {
            _source = source;
            _fieldcount = valuegetters.Count;
            _valuegetters = valuegetters;
        }

        internal EnumerableDataReader(IEnumerator<T> source, IDictionary<string, Func<T, object>> valuegetters)
        {
            _source = source;
            _fieldcount = valuegetters.Count;
            _valuegetters = valuegetters.Values.ToList();

            foreach (string column in valuegetters.Keys)
            {
                _table.Columns.Add(column);
            }
        }

        public object GetValue(int i)
        {
            var f = _valuegetters[i];
            return f(_source.Current);
        }

        public int FieldCount
        {
            get { return _fieldcount; }
        }

        public bool Read()
        {
            return _source.MoveNext();
        }

        #region The stuff that doesn't matter

        public void Close()
        {
            // Nothing
        }

        public void Dispose()
        {
            // Nothing.
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int Depth { get { return 0; } }

        public bool IsClosed { get { return false; } }

        public int RecordsAffected { get { return -1; } }

        public DataTable GetSchemaTable()
        {
            return _table;
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { return GetValue(i); }
        }

        public bool GetBoolean(int i)
        {
            return (bool)this[i];
        }

        public byte GetByte(int i) { return (byte)this[i]; }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return (char)this[i];
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return -1;
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)this[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)this[i];
        }

        public double GetDouble(int i)
        {
            return (double)this[i];
        }

        public Type GetFieldType(int i)
        {
            return (Type)this[i];
        }

        public float GetFloat(int i)
        {
            return (float)this[i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)this[i];
        }

        public short GetInt16(int i)
        {
            return (short)this[i];
        }

        public int GetInt32(int i)
        {
            return (int)this[i];
        }

        public long GetInt64(int i)
        {
            return (long)this[i];
        }

        public string GetName(int i)
        {
            if (_table.Columns.Count > i)
            {
                return _table.Columns[i].ColumnName;
            }
            throw new IndexOutOfRangeException($"No column for index {i}");
        }

        public int GetOrdinal(string name)
        {
            if (_table.Columns.Count == 0)
            {
                throw new Exception("Schema table is empty");
            }
            return _table.Columns.IndexOf(name);
        }

        public string GetString(int i)
        {
            return (string)this[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return this[i] is DBNull;
        }

        #endregion
    }
}
