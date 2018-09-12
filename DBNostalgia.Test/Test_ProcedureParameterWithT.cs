using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia.Test
{
    [TestClass]
    public class Test_ProcedureParameterWithT
    {
        [TestMethod]
        public void ProcedureParameterWithT_Constructor_SetsValueAndName()
        {
            var expectedName = "aName";
            var expectedValue = 1002;

            var subject = new ProcedureParameter<int>(expectedName, expectedValue);

            Assert.AreEqual(expectedValue, subject.Value);
            Assert.AreEqual(expectedName, subject.Name);
        }

        [TestMethod]
        public void ProcedureParameterWithT_GetValue()
        {
            var expectedValue = 1003;

            var subject = new ProcedureParameter<int>("aName", expectedValue);

            Assert.AreEqual(expectedValue, subject.GetValue());
        }

        [TestMethod]
        public void ProcedureParameterWithT_SetupDbCommand_SetsCommandAsExpected()
        {
            var parameterName = "parameter_name";
            var parameterValue = "a-string-value";

            var subject = new ProcedureParameter<string>(parameterName, parameterValue);

            var dbCommandMock = new Mock<IDbCommand>();
            var dbParameterMock = new Mock<IDbDataParameter>();
            var dbParameterCollectionMock = new Mock<IDataParameterCollection>();
            var myParameterList = new List<IDbDataParameter>();
            var actualParameterName = default(string);
            var actualParameterValue = default(object);

            dbCommandMock
                .Setup(m => m.CreateParameter())
                .Returns(() => dbParameterMock.Object);

            dbCommandMock
                .SetupGet(m => m.Parameters)
                .Returns(() => dbParameterCollectionMock.Object);

            dbParameterMock
                .SetupSet(m => m.ParameterName = It.IsAny<string>())
                .Callback<string>(value => actualParameterName = value);

            dbParameterMock
                .SetupSet(m => m.Value = It.IsAny<string>())
                .Callback<object>(value => actualParameterValue = value);

            dbParameterCollectionMock
                .Setup(m => m.Add(It.IsAny<IDbDataParameter>()))
                .Callback<object>(parameter => myParameterList.Add(parameter as IDbDataParameter));

            subject.SetupDbCommand(dbCommandMock.Object);

            Assert.AreEqual(1, myParameterList.Count);
            var actualParameter = myParameterList.First();

            Assert.AreEqual("@parameter_name", actualParameterName);
            Assert.AreEqual("a-string-value", actualParameterValue);
        }
    }
}
