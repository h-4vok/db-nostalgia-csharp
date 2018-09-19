using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    public class UseTransactionBehavior : ITransactionBehavior
    {
        public IDbTransaction TryBeginTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        public void TryCommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
        }

        public void TryRollbackTransaction(IDbTransaction transaction)
        {
            transaction.Rollback();
        }
    }
}
