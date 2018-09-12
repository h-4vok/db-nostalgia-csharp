using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DBNostalgia.Test
{
    [TestClass]
    public class Test_ProcedureParameter
    {
        [TestMethod]
        [TestCategory(Category.Unit), TestCategory(Category.Slow)]
        public void ProcedureParameter_Ctor_SetsName()
        {
            var expectedParamName = "ParameterName1";
            var mock = new Mock<ProcedureParameter>(MockBehavior.Strict, new object[] { expectedParamName });
            var subject = mock.Object;

            Assert.AreEqual(expectedParamName, subject.Name);
        }
    }
}
