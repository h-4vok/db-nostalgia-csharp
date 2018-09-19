using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    public class NoTransactionBehavior : ITransactionBehavior
    {
        public IDbTransaction TryBeginTransaction(IDbConnection connection)
        {
            return null;
        }

        public void TryCommitTransaction(IDbTransaction transaction)
        {
            
        }

        public void TryRollbackTransaction(IDbTransaction transaction)
        {
            
        }
    }
}
