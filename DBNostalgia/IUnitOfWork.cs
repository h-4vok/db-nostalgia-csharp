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
        /// Use this method when you need to define exactly what happens between calls and there is an output you will return.
        /// Recommended when there are more than one statement that should re-use the same connection or even the same transaction.
        /// </summary>
        /// <typeparam name="T">The output type of the entire closure.</typeparam>
        /// <param name="closure">The closure Func that will output the value you need within the database connection context.</param>
        /// <returns>A result that comes from your closure.</returns>
        T Run<T>(Func<T> closure);

        /// <summary>
        /// Runs a closure within the context of a database connection.
        /// Use this method when you need to define exactly what happens between calls and there is no output.
        /// Recommended when there are more than one statement that should re-se the same connection or even the same transaction.
        /// </summary>
        /// <param name="closure">The closure Action that will perform the code you need within the database connection context.</param>
        void Run(Action closure);

        /// <summary>
        /// Runs a stored procedure and reads the output as a ExecuteReader.
        /// Use this method inside a Run() statement in order to utilize the database connection context. Otherwise it will fail.
        /// </summary>
        /// <typeparam name="T">The type you want the data to be returned as.</typeparam>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="fetchClosure">The closure that will fetch the data for a single row.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        /// <returns>A collection of objects of type T.</returns>
        IEnumerable<T> Get<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure and reads the output as ExecuteReader, but only fetches one row.
        /// Use this method inside a Run() statement in order to utilize the database connection context. Otherwise it will fail.
        /// </summary>
        /// <typeparam name="T">The type you want the data to be returned as.</typeparam>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="fetchClosure">The closure that will fetch the data for a single row.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        /// <returns>A single T object.</returns>
        T GetOne<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure and reads the output as a ExecuteReader.
        /// Use this method outside a Run() statement as it utilizes its own database connection context. Otherwise it will fail.
        /// </summary>
        /// <typeparam name="T">The type you want the data to be returned as.</typeparam>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="fetchClosure">The closure that will fetch the data for a single row.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        /// <returns>A collection of T obejcts.</returns>
        IEnumerable<T> GetDirect<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure and reads the output as a ExecuteReader, but only fetches one row.
        /// Use this method outside a Run() statement as it utilizes its own database connection context. Otherwise it will fail.
        /// </summary>
        /// <typeparam name="T">The type you want the data to be returned as.</typeparam>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="fetchClosure">The closure that will fetch the data for a single row.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        /// <returns>A single T object.</returns>
        T GetOneDirect<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure as a ExecuteNonQuery.
        /// Use this method inside a Run() statement in order to utilize the database connection context. Otherwise it will fail.
        /// </summary>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        void Execute(string procedure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure as a ExecuteNonQuery.
        /// Use this method outside a Run() statement as it utilizes its own database connection context. Otherwise it will fail.
        /// </summary>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        void ExecuteDirect(string procedure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure as a ExecuteScalar
        /// Use this method inside a Run() statement in order to utilize the database connection context. Otherwise it will fail.
        /// </summary>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        /// <returns>An object with the value returned from the first row and first cell of your execution.</returns>
        object Scalar(string procedure, ParametersBuilder parametersBuilder = null);

        /// <summary>
        /// Runs a stored procedure as a ExecuteScalar
        /// Use this method outside a Run() statement as it utilizes its own database connection context. Otherwise it will fail.
        /// </summary>
        /// <param name="procedure">Stored procedure name from your database.</param>
        /// <param name="parametersBuilder">An optional ParametersBuilder object with stored procedure parameter values.</param>
        /// <returns>An object with the value returned from the first row and first cell of your execution.</returns>
        object ScalarDirect(string procedure, ParametersBuilder parametersBuilder = null);
    }
}
