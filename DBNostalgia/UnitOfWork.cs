using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(Func<IDbConnection> buildConnectionClosure)
        {
            this.buildConnectionClosure = buildConnectionClosure;
        }

        private readonly Func<IDbConnection> buildConnectionClosure;

        private IDbTransaction transaction;
        private IDbConnection connection;

        public T Run<T>(Func<T> closure)
        {
            T output = default(T);

            this.RunInDatabaseContext(() => { output = closure(); });

            return output;
        }

        public void Run(Action closure)
        {
            this.RunInDatabaseContext(closure);
        }

        protected void RunInDatabaseContext(Action action)
        {
            var isTransactionRunning = false;

            using (this.connection = this.buildConnectionClosure())
            using (this.transaction = this.connection.BeginTransaction())
            {
                isTransactionRunning = true;

                try
                {
                    action();
                }
                catch
                {
                    this.transaction.Rollback();
                    isTransactionRunning = false;

                    throw;
                }
                finally
                {
                    if (isTransactionRunning) this.transaction.Commit();
                }
            }
        }

        public IEnumerable<T> Get<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null)
        {
            var output = new List<T>();

            void iterateAndFetch(IDbCommand command)
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = fetchClosure(reader);
                        output.Add(item);
                    }
                }
            }

            this.RunCommand(
                procedure,
                iterateAndFetch,
                parametersBuilder
            );

            return output;
        }

        public void Execute(string procedure, ParametersBuilder parametersBuilder = null)
        {
            this.RunCommand(
                procedure,
                command => command.ExecuteNonQuery(),
                parametersBuilder
            );
        }

        public object Scalar(string procedure, ParametersBuilder parametersBuilder = null)
        {
            object output = null;

            this.RunCommand(
                procedure,
                command => output = command.ExecuteScalar(),
                parametersBuilder
            );

            return output;
        }

        protected void RunCommand(string procedure, Action<IDbCommand> closure, ParametersBuilder parametersBuilder = null)
        {
            parametersBuilder = parametersBuilder ?? ParametersBuilder.DefaultInstance;

            using (var command = this.connection.CreateCommand())
            {
                command.Transaction = this.transaction;
                command.CommandText = procedure;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;

                parametersBuilder.SetupDbCommand(command);

                closure(command);
            }
        }

        public void ExecuteDirect(string procedure, ParametersBuilder parametersBuilder = null)
        {
            this.Run(() =>this.Execute(procedure, parametersBuilder));
        }

        public object ScalarDirect(string procedure, ParametersBuilder parametersBuilder = null)
        {
            var output = this.Run<object>(() => this.Scalar(procedure, parametersBuilder));

            return output;
        }

        public IEnumerable<T> GetDirect<T>(string procedure, Func<IDataReader, T> fetchClosure, ParametersBuilder parametersBuilder = null)
        {
            var output = this.Run<IEnumerable<T>>(() => this.Get(procedure, fetchClosure, parametersBuilder));

            return output;
        }
    }
}
