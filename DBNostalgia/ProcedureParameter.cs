using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    internal abstract class ProcedureParameter
    {
        public ProcedureParameter(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public abstract void SetupDbCommand(IDbCommand command);

        public abstract object GetValue();
    }

    internal class ProcedureParameter<T> : ProcedureParameter
    {
        public ProcedureParameter(string name, T value) : base(name)
        {
            this.Value = value;
        }

        public T Value { get; set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetupDbCommand(IDbCommand command)
        {
            var dbParameter = command.CreateParameter();

            dbParameter.ParameterName = "@" + this.Name;
            dbParameter.Value = this.Value;

            command.Parameters.Add(dbParameter);
        }
    }
}
