using System;
using System.Data;

namespace PSnappy
{
    public static class SafeDataReaderExtensions
    {
        public static SafeDataReader ToSafeDataReader(this IDataReader dataReader)
        {
            return new SafeDataReader(dataReader);
        }
    }

    public class SafeDataReader : IDataReader
    {
        protected IDataReader DataReader { get; }

        public SafeDataReader(IDataReader dataReader)
        {
            DataReader = dataReader;
        }

        #region data

        public int RecordsAffected
        {
            get
            {
                return DataReader.RecordsAffected;
            }
        }

        public int GetValues(object[] values)
        {
            return DataReader.GetValues(values);
        }

        public IDataReader GetData(string name)
        {
            return GetData(DataReader.GetOrdinal(name));
        }

        public virtual IDataReader GetData(int i)
        {
            return DataReader.GetData(i);
        }

        public DataTable GetSchemaTable()
        {
            return DataReader.GetSchemaTable();
        }

        public bool IsClosed
        {
            get
            {
                return DataReader.IsClosed;
            }
        }

        #endregion data

        #region navigation

        public virtual bool Read()
        {
            return DataReader.Read();
        }

        public virtual bool NextResult()
        {
            return DataReader.NextResult();
        }

        public virtual void Close()
        {
            DataReader.Close();
        }

        public int Depth
        {
            get
            {
                return DataReader.Depth;
            }
        }

        #endregion navigation

        #region columns

        public int FieldCount
        {
            get { return DataReader.FieldCount; }
        }

        public virtual string GetName(int i)
        {
            if (i == -1)
            {
                return null;
            }
            else
            {
                return DataReader.GetName(i);
            }
        }

        public int GetOrdinal(string name)
        {
            try
            {
                return DataReader.GetOrdinal(name);
            }
            catch
            {
                return -1;
            }
        }

        public Type GetFieldType(string name)
        {
            return GetFieldType(DataReader.GetOrdinal(name));
        }

        public virtual Type GetFieldType(int i)
        {
            if (i == -1)
            {
                return null;
            }
            else
            {
                return DataReader.GetFieldType(i);
            }
        }

        public string GetDataTypeName(string name)
        {
            return GetDataTypeName(DataReader.GetOrdinal(name));
        }

        public virtual string GetDataTypeName(int i)
        {
            if (i == -1)
            {
                return null;
            }
            else
            {
                return DataReader.GetDataTypeName(i);
            }
        }

        #endregion columns

        //Fields
        public virtual bool IsDBNull(int i)
        {
            if (i == -1)
            {
                return true;
            }
            else
            {
                return DataReader.IsDBNull(i);
            }
        }

        public object this[string name]
        {
            get
            {
                object val = DataReader[name];
                if (DBNull.Value.Equals(val))
                {
                    return null;
                }
                else
                {
                    return val;
                }
            }
        }

        public virtual object this[int i]
        {
            get
            {
                if (IsDBNull(i))
                {
                    return null;
                }
                else
                {
                    return DataReader[i];
                }
            }
        }

        public object GetValue(string name)
        {
            return GetValue(DataReader.GetOrdinal(name));
        }

        public virtual object GetValue(int i)
        {
            var value = DataReader.GetValue(i);

            //previously this was calling IsDBNull, which for structs, was re-allocating the struct.
            return value is DBNull ? null : value;
        }

        public virtual DateTime GetDateTime(string name)
        {
            return GetDateTime(DataReader.GetOrdinal(name));
        }

        public virtual DateTime GetDateTime(int i)
        {
            if (IsDBNull(i))
            {
                return DateTime.MinValue;
            }
            else if (DataReader.GetFieldType(i) == typeof(DateTime))
            {
                return DataReader.GetDateTime(i);
            }
            else
            {
                return DateTime.Parse(DataReader.GetString(i));
            }
        }

        public string GetString(string name)
        {
            return GetString(DataReader.GetOrdinal(name));
        }

        public virtual string GetString(int i)
        {
            if (IsDBNull(i))
            {
                return string.Empty;
            }
            else if (DataReader.GetFieldType(i) == typeof(string))
            {
                return DataReader.GetString(i).TrimEnd();
            }
            else
            {
                return DataReader.GetValue(i).ToString().TrimEnd();
            }
        }

        public int GetInt32(string name)
        {
            return GetInt32(DataReader.GetOrdinal(name));
        }

        public virtual int GetInt32(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else if (DataReader.GetFieldType(i) == typeof(Int32))
            {
                return DataReader.GetInt32(i);
            }
            else
            {
                return Convert.ToInt32(DataReader.GetValue(i));
            }
        }

        public double GetDouble(string name)
        {
            return GetDouble(DataReader.GetOrdinal(name));
        }

        public virtual double GetDouble(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else if (DataReader.GetFieldType(i) == typeof(double))
            {
                return DataReader.GetDouble(i);
            }
            else
            {
                return Convert.ToDouble(DataReader.GetValue(i));
            }
        }

        public System.Guid GetGuid(string name)
        {
            return GetGuid(DataReader.GetOrdinal(name));
        }

        public virtual System.Guid GetGuid(int i)
        {
            if (IsDBNull(i))
            {
                return Guid.Empty;
            }
            else
            {
                return DataReader.GetGuid(i);
            }
        }

        public bool GetBoolean(string name)
        {
            return GetBoolean(DataReader.GetOrdinal(name));
        }

        public virtual bool GetBoolean(int i)
        {
            if (IsDBNull(i))
            {
                return false;
            }
            else if (DataReader.GetFieldType(i) == typeof(bool))
            {
                return DataReader.GetBoolean(i);
            }
            else
            {
                return Convert.ToBoolean(DataReader.GetValue(i));
            }
        }

        public byte GetByte(string name)
        {
            return GetByte(DataReader.GetOrdinal(name));
        }

        public virtual byte GetByte(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetByte(i);
            }
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public Int64 GetBytes(string name, Int64 fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            return GetBytes(DataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public virtual Int64 GetBytes(int i, Int64 fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
            }
        }

        public char GetChar(string name)
        {
            return GetChar(DataReader.GetOrdinal(name));
        }

        public virtual char GetChar(int i)
        {
            if (IsDBNull(i))
            {
                return char.MinValue;
            }
            else
            {
                char[] myChar = new char[1];
                DataReader.GetChars(i, 0, myChar, 0, 1);
                return myChar[0];
            }
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public Int64 GetChars(string name, Int64 fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            return GetChars(DataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public virtual Int64 GetChars(int i, Int64 fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetChars(i, fieldOffset, buffer, bufferOffset, length);
            }
        }

        /// <summary>
        /// Gets a decimal value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public decimal GetDecimal(string name)
        {
            return GetDecimal(DataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a decimal value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual decimal GetDecimal(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetDecimal(i);
            }
        }

        public float GetFloat(string name)
        {
            return GetFloat(DataReader.GetOrdinal(name));
        }

        public virtual float GetFloat(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetFloat(i);
            }
        }

        public short GetInt16(string name)
        {
            return GetInt16(DataReader.GetOrdinal(name));
        }

        public virtual short GetInt16(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetInt16(i);
            }
        }

        public Int64 GetInt64(string name)
        {
            return GetInt64(DataReader.GetOrdinal(name));
        }

        public virtual Int64 GetInt64(int i)
        {
            if (IsDBNull(i))
            {
                return 0;
            }
            else
            {
                return DataReader.GetInt64(i);
            }
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">True if called by
        /// the public Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // free unmanaged resources when explicitly called
                    DataReader.Dispose();
                }

                // free shared unmanaged resources
            }
            _disposedValue = true;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Object finalizer.
        /// </summary>
        ~SafeDataReader()
        {
            Dispose(false);
        }

        #endregion IDisposable Support
    }
}
