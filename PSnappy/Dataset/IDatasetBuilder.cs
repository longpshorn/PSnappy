﻿using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetBuilder
    {
        Task BuildAsync(string connectionString);
    }

    public interface IDatasetBuilder<T>
    {
        Task BuildAsync(T config);
    }
}
