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
    public class Test_ParametersBuilder
    {
        [TestMethod]
        [TestCategory(Category.Unit)]
        public void ParametersBuilder_GetProcedureParameters_EmptyList()
        {
            var subject = new ParametersBuilder();
            var output = subject.GetProcedureParameters();

            Assert.AreEqual(0, output.Count());
        }

        [TestMethod]
        [TestCategory(Category.Unit)]
        public void ParametersBuilder_Add_AddsOne()
        {
            const string expectedName = "param1";
            const int expectedValue = 100;

            var subject = new ParametersBuilder();
            subject.And(expectedName, expectedValue);

            var output = subject.GetProcedureParameters();

            Assert.AreEqual(1, output.Count());

            var onlyParameter = output.First();

            Assert.AreEqual(expectedName, onlyParameter.Name);
            Assert.AreEqual(expectedValue, onlyParameter.GetValue());
        }

        [TestMethod]
        [TestCategory(Category.Unit)]
        public void ParametersBuilder_Add_AddManyWithChaining()
        {
            var subject = new ParametersBuilder();
            subject
                .And("Param1", 200)
                .And("Param2", "string-value")
                .And("Param3", true);

            var output = subject.GetProcedureParameters().ToArray();

            Assert.AreEqual(3, output.Count());

            Assert.AreEqual("Param1", output[0].Name);
            Assert.AreEqual("Param2", output[1].Name);
            Assert.AreEqual("Param3", output[2].Name);

            Assert.AreEqual(200, output[0].GetValue());
            Assert.AreEqual("string-value", output[1].GetValue());
            Assert.AreEqual(true, output[2].GetValue());
        }

        [TestMethod]
        [TestCategory(Category.Unit)]
        public void ParametersBuilder_Add_NullValue()
        {
            var subject = new ParametersBuilder();

            // For this test we are assuming that the developer user will have a 
            // variable that contains a null value, and that variable is not of just type object.
            string nullString = null;
            subject.And("Param4", nullString);

            var output = subject.GetProcedureParameters().ToArray();

            Assert.AreEqual(1, output.Count());

            Assert.AreEqual("Param4", output[0].Name);
            Assert.IsNull(output[0].GetValue());
        }

        [TestMethod]
        [TestCategory(Category.Unit)]
        public void ParametersBuilder_With_WorksLikeAnd()
        {
            var subject = ParametersBuilder.With("Param1", 40).And("Param2", "string-value");

            var output = subject.GetProcedureParameters().ToArray();

            Assert.AreEqual(2, output.Count());

            Assert.AreEqual("Param1", output[0].Name);
            Assert.AreEqual("Param2", output[1].Name);

            Assert.AreEqual(40, output[0].GetValue());
            Assert.AreEqual("string-value", output[1].GetValue());
        }

        [TestMethod]
        [TestCategory(Category.Unit), TestCategory(Category.Slow)]
        public void ParametersBuilder_SetupDbCommand_SetsParametersProperly()
        {
            var dbCommandMock = new Mock<IDbCommand>();
            var dbParameter1Mock = new Mock<IDbDataParameter>();
            var dbParameter2Mock = new Mock<IDbDataParameter>();
            var dbParameterCollectionMock = new Mock<IDataParameterCollection>();
            var myParameterList = new List<IDbDataParameter>();
            var actualNames = new HashSet<string>();
            var actualValues = new HashSet<object>();
            var parametersQueue = new Queue<Mock<IDbDataParameter>>();

            void SetupIDbDataParameter(Mock<IDbDataParameter> mock)
            {
                mock
                    .SetupSet(m => m.ParameterName = It.IsAny<string>())
                    .Callback<string>(value => actualNames.Add(value));

                mock
                    .SetupSet(m => m.Value = It.IsAny<object>())
                    .Callback<object>(value => actualValues.Add(value));

                parametersQueue.Enqueue(mock);
            }

            dbCommandMock
                .Setup(m => m.CreateParameter())
                .Returns(() => parametersQueue.Dequeue().Object);

            dbCommandMock
                .SetupGet(m => m.Parameters)
                .Returns(() => dbParameterCollectionMock.Object);

            SetupIDbDataParameter(dbParameter1Mock);
            SetupIDbDataParameter(dbParameter2Mock);

            dbParameterCollectionMock
                .Setup(m => m.Add(It.IsAny<IDbDataParameter>()))
                .Callback<object>(parameter => myParameterList.Add(parameter as IDbDataParameter));

            var subject = ParametersBuilder.With("Param1", "string-value").And("Param2", 900);
            subject.SetupDbCommand(dbCommandMock.Object);

            Assert.AreEqual(2, myParameterList.Count);

            Assert.IsTrue(actualNames.Contains("@Param1"));
            Assert.IsTrue(actualNames.Contains("@Param2"));

            Assert.IsTrue(actualValues.Contains("string-value"));
            Assert.IsTrue(actualValues.Contains(900));
        }
    }
}
