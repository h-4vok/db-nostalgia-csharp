using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia.Test.Integration
{
    [TestClass]
    public class Integration_UnitOfWork_NonTransactional
    {
        private UnitOfWork uow;

        [TestInitialize]
        public void Init()
        {
            this.uow = new UnitOfWork(Shared.BuildConnectionClosure);
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_NonTransactional_NonQuery_NoParams()
        {
            this.uow.NonQueryDirect("RecordTable1_insertTest_noParams");

            // TODO: Add an assert
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_NonTransactional_Scalar_NoParams()
        {
            var output = this.uow.ScalarDirect("NoTable_scalarTest_noParams");
            Assert.AreEqual("This is a scalar test", output);
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_NonTransactional_GetOne_NoParams()
        {
            var output = this.uow.GetOneDirect("NoTable_getOne_noParams", (reader) =>
            {
                return reader.GetString("Column1");
            });

            Assert.AreEqual("This is data from Column1", output);
        }

        private class NoTableRecord
        {
            public string Column1 { get; set; }
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_NonTransactional_Get_NoParams()
        {
            var output = this.uow.GetDirect("NoTable_get_noParams",
                (reader) =>
                {
                    return new NoTableRecord
                    {
                        Column1 = reader.GetString("Column1")
                    };
                });

            Assert.AreEqual(3, output.Count());

            var lastItem = output.ToArray().Last();

            Assert.AreEqual("Column1 value is 3", lastItem.Column1);
        }
    }
}
