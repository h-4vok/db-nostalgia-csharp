using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia.Test.Integration
{
    [TestClass]
    public class Integration_UnitOfWork_Transactional
    {
        private UnitOfWork uow;

        [TestInitialize]
        public void Init()
        {
            this.uow = new UnitOfWork(Shared.BuildConnectionClosure);
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_Transactional_NonQuery_NoParams()
        {
            this.uow.Run(() =>
            {
                this.uow.NonQuery("RecordTable1_insertTest_noParams");
            });
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_Transactional_Scalar_NoParams()
        {
            var output = this.uow.Run(() =>
            {
                return this.uow.Scalar("NoTable_scalarTest_noParams");
            });

            Assert.AreEqual("This is a scalar test", output);
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_Transactional_GetOne_NoParams()
        {
            var output = this.uow.Run(() =>
            {
                return this.uow.GetOne("NoTable_getOne_noParams", (reader) =>
                {
                    return reader.GetString("Column1");
                });
            });

            Assert.AreEqual("This is data from Column1", output);
        }

        private class NoTableRecord
        {
            public string Column1 { get; set; }
        }

        [TestMethod, TestCategory(Category.Integration)]
        public void UnitOfWork_Transactional_Get_NoParams()
        {
            var output = this.uow.Run(() =>
            {
                return this.uow.Get("NoTable_get_noParams",
                    (reader) =>
                    {
                        return new NoTableRecord
                        {
                            Column1 = reader.GetString("Column1")
                        };
                    });
            });

            Assert.AreEqual(3, output.Count());

            var lastItem = output.ToArray().Last();

            Assert.AreEqual("Column1 value is 3", lastItem.Column1);
        }
    }
}
