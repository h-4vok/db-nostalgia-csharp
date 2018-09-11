using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Runs a closure within the context of a database connection.
        /// Use this method when you need to define exactly what happens between calls.
        /// Recommended when there are more than one statement that should re-use the same connection or even the same transaction.
        /// </summary>
        /// <typeparam name="T">The output type of the entire closure.</typeparam>
        /// <param name="closure">The closure Func that will output the value you need.</param>
        /// <returns>A result that comes from your closure.</returns>
        T Run<T>(Func<T> closure);

        void Run(Action closure);

        IEnumerable<T> Get<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null);

        IEnumerable<T> GetDirect<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null);

        void Execute(string procedure, ParametersBuilder parametersBuilder = null);

        void ExecuteDirect(string procedure, ParametersBuilder parametersBuilder = null);

        object Scalar(string procedure, ParametersBuilder parametersBuilder = null);

        object ScalarDirect(string procedure, ParametersBuilder parametersBuilder = null);
    }
}
