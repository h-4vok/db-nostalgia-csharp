using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    public interface ITransactionBehavior
    {
        IDbTransaction TryBeginTransaction(IDbConnection connection);
        void TryRollbackTransaction(IDbTransaction transaction);
        void TryCommitTransaction(IDbTransaction transaction);
    }
}
