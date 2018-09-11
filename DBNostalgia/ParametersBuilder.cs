using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    public class ParametersBuilder
    {
        private IList<ProcedureParameter> parameters;

        public ParametersBuilder()
        {
            this.parameters = new List<ProcedureParameter>();
        }

        internal static ParametersBuilder DefaultInstance { get; } = new ParametersBuilder();

        public static ParametersBuilder With<T>(string name, T value)
        {
            var builder = new ParametersBuilder();
            return builder.And<T>(name, value);
        }

        public ParametersBuilder And<T>(string name, T value)
        {
            this.parameters.Add(new ProcedureParameter<T>(name, value));

            return this;
        }

        internal IEnumerable<ProcedureParameter> GetProcedureParameters()
        {
            var output = new List<ProcedureParameter>(this.parameters);
            return output;
        }

        internal void SetupDbCommand(IDbCommand command)
        {
            this.parameters.ForEach(param => param.SetupDbCommand(command));
        }
    }
}
