using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    /// <summary>
    /// Use this class to create your database parameters in a seamless way.
    /// You can chain them all using the And method.
    /// </summary>
    public class ParametersBuilder
    {
        private IList<ProcedureParameter> parameters;

        /// <summary>
        /// Constructs a new ParametersBuilder object.
        /// </summary>
        public ParametersBuilder()
        {
            this.parameters = new List<ProcedureParameter>();
        }

        internal static ParametersBuilder DefaultInstance { get; } = new ParametersBuilder();

        /// <summary>
        /// Starts a new ParametersBuilder list with a first parameter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be added. It will usually map automatically just fine based on your variable type.</typeparam>
        /// <param name="name">Name of the stored procedure parameter, without a prefix @.</param>
        /// <param name="value">Value of the stored procedure parameter.</param>
        /// <returns>Returns the new ParametersBuilder list with a first parameter added.</returns>
        public static ParametersBuilder With<T>(string name, T value)
        {
            var builder = new ParametersBuilder();
            return builder.And<T>(name, value);
        }

        /// <summary>
        /// Adds another parameter.
        /// </summary>
        /// <typeparam name="T">Returns the current ParametersBuilder object to enable easy chaining.</typeparam>
        /// <param name="name">Name of the stored procedure parameter, without a prefix @.</param>
        /// <param name="value">Value of the stored procedure parameter.</param>
        /// <returns>Returns the new ParametersBuilder list with a first parameter added.</returns>
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
