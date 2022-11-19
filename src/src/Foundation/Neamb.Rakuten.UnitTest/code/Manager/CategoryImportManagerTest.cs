using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Neambc.Neamb.Foundation.Rakuten.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;


namespace Neambc.Neamb.Foundation.Rakuten.UnitTest.Manager {
	[TestFixture]
	public class CategoryImportManagerTest
    {
        #region Fields
        private ICategoryImportManager _sut;
        private Mock<IRakutenImportOperation> _rakutenImportOperationMock;
        #endregion

        [SetUp]
        public void SetUp()
        {
            _rakutenImportOperationMock = new Mock<IRakutenImportOperation>();
            _sut = new CategoryImportManager(_rakutenImportOperationMock.Object);
        }

        [Test]
        public void ProcessProcessCsvFileWhenNoInputParameters()
        {
            Assert.Throws<ArgumentException>(() => _sut.ProcessCsvFile(null, null));
        }

        [Test]
        public void ProcessProcessCsvFileWhenNoHeaderCsv() {
            var resultProcessing = new CategoryImportResult();
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("")))
            {
                _sut.ProcessCsvFile(test_Stream, resultProcessing);
                Assert.AreEqual(resultProcessing.Errors.Count, 1);
            }
        }

        [Test]
        public void ProcessProcessCsvFileWhenNoData() {
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("\"id\",\"parent_id\", \"name\""))) {
                var itemCsvItems= _sut.ProcessCsvFile(test_Stream, new CategoryImportResult());
                Assert.AreEqual(itemCsvItems.Count,0);
            }
        }

        [Test]
        public void ProcessProcessCsvFileWithData()
        {
            var csv = new StringBuilder();
            var newLine = "\"id\",\"parent_id\", \"name\"";
            csv.AppendLine(newLine);
            var newLine2 = "0,-1,\"Parent Category\"";
            csv.AppendLine(newLine2);

            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString())))
            {
                var itemCsvItems = _sut.ProcessCsvFile(test_Stream, new CategoryImportResult());
                Assert.AreEqual(itemCsvItems.Count, 1);
            }
        }
        
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void SetCsvItemLevelWithInputListItems() {
            List<CategoryExcelItem> listCategoryItems= new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem{Id="0",Name= "Parent Category",ParentId = "-1"});
            listCategoryItems.Add(new CategoryExcelItem { Id = "1", Name = "Baby, Kids & Toys", ParentId = "0" });
            
            using (var db = new Sitecore.FakeDb.Db()) {
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{1EA33232-AC25-42E5-A550-6C9232F319ED}"));
                root.Add(categories);
                db.Add(root);
                _sut.SetCsvItemLevel(listCategoryItems, new CategoryImportResult());
                var parentItem = listCategoryItems.FirstOrDefault(item => item.Id == "0");
                var childItem = listCategoryItems.FirstOrDefault(item => item.Id == "1");
                Assert.AreEqual(parentItem.Level,0);
                Assert.AreEqual(childItem.Level, 1);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]

        public void SetCsvItemLevelWithInputListItemsUnOrdered()
        {
            List<CategoryExcelItem> listCategoryItems = new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem { Id = "0", Name = "Parent Category", ParentId = "-1" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "1", Name = "Baby, Kids & Toys", ParentId = "0" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "456", Name = "Jewelry", ParentId = "662" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "662", Name = "Accessories", ParentId = "0" });

            using (var db = new Sitecore.FakeDb.Db())
            {
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{1EA33232-AC25-42E5-A550-6C9232F319ED}"));
                root.Add(categories);
                db.Add(root);
                _sut.SetCsvItemLevel(listCategoryItems, new CategoryImportResult());
                var parentItem = listCategoryItems.FirstOrDefault(item => item.Id == "0");
                var childItem = listCategoryItems.FirstOrDefault(item => item.Id == "1");
                var childItem2 = listCategoryItems.FirstOrDefault(item => item.Id == "456");
                var childItem3 = listCategoryItems.FirstOrDefault(item => item.Id == "662");
                Assert.AreEqual(parentItem.Level, 0);
                Assert.AreEqual(childItem.Level, 1);
                Assert.AreEqual(childItem2.Level, 2);
                Assert.AreEqual(childItem3.Level, 1);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void SetCsvItemLevelWithInputListItemsUnOrderedWithParentNotExist()
        {
            List<CategoryExcelItem> listCategoryItems = new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem { Id = "0", Name = "Parent Category", ParentId = "-1" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "1", Name = "Baby, Kids & Toys", ParentId = "0" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "456", Name = "Jewelry", ParentId = "662" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "662", Name = "Accessories", ParentId = "0" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "663", Name = "Accessories", ParentId = "10" });

            using (var db = new Sitecore.FakeDb.Db()) {
                var result = new CategoryImportResult();
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{1EA33232-AC25-42E5-A550-6C9232F319ED}"));
                root.Add(categories);
                db.Add(root);
                _sut.SetCsvItemLevel(listCategoryItems,result );
                var parentItem = listCategoryItems.FirstOrDefault(item => item.Id == "0");
                var childItem = listCategoryItems.FirstOrDefault(item => item.Id == "1");
                var childItem2 = listCategoryItems.FirstOrDefault(item => item.Id == "456");
                var childItem3 = listCategoryItems.FirstOrDefault(item => item.Id == "662");
                Assert.AreEqual(parentItem.Level, 0);
                Assert.AreEqual(childItem.Level, 1);
                Assert.AreEqual(childItem2.Level, 2);
                Assert.AreEqual(childItem3.Level, 1);
                Assert.AreEqual(result.Errors.Count,1);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ProcessSitecoreItems() {
            List<CategoryExcelItem> listCategoryItems = new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem { Id = "0", Name = "Parent Category", ParentId = "-1" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "1", Name = "Baby, Kids & Toys", ParentId = "0" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "456", Name = "Jewelry", ParentId = "662" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "662", Name = "Accessories", ParentId = "0" });
            listCategoryItems.Add(new CategoryExcelItem { Id = "663", Name = "Accessories", ParentId = "10" });

            using (var db = new Sitecore.FakeDb.Db())
            {
                var result = new CategoryImportResult();
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{1EA33232-AC25-42E5-A550-6C9232F319ED}"));
                root.Add(categories);
                db.Add(root);
                _sut.ProcessSitecoreItems(listCategoryItems, result);
                Assert.AreEqual(result.Errors.Count,0);
                Assert.AreEqual(result.DeletedItems.Count, 0);
                Assert.AreEqual(result.NewItems.Count, 0);
                Assert.AreEqual(result.UpdatedItems.Count, 0);
                
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ProcessSitecoreItemsToExecuteInsertProcess()
        {
            List<CategoryExcelItem> listCategoryItems = new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem { Id = "0", Name = "Parent Category", ParentId = "-1", Level = 1});
            listCategoryItems.Add(new CategoryExcelItem { Id = "1", Name = "Baby, Kids & Toys", ParentId = "0",Level = 2});
            listCategoryItems.Add(new CategoryExcelItem { Id = "456", Name = "Jewelry", ParentId = "662",Level =3 });
            listCategoryItems.Add(new CategoryExcelItem { Id = "662", Name = "Accessories", ParentId = "0", Level = 2 });
            listCategoryItems.Add(new CategoryExcelItem { Id = "663", Name = "Accessories", ParentId = "10" });

            using (var db = new Sitecore.FakeDb.Db())
            {
                var result = new CategoryImportResult();
                var templateId = new ID("40E9D39E-1F56-48A8-BB70-CA76353C5E21");
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{837C7EC5-596B-470B-83E6-186B11248919}"));
                var parentCategory = new DbItem("Parent Category", new ID("{837C7EC5-596B-470B-83E6-186B11248915}"), templateId)
                {
                    Fields = {
                        new DbField("Id", new ID("{D2C33213-B7F8-49CA-B70E-0D64BB099A8F}")) {
                            Value = "0"
                        },
                        new DbField("Name", new ID("{0620BA80-EEF0-4489-A4D9-969841A782BA}")) {
                            Value = "Parent Category"
                        },
                        
                    }
                };
                root.Add(categories);
                categories.Add(parentCategory);
                db.Add(root);
                _sut.ProcessSitecoreItems(listCategoryItems, result);
                //There is a error in publish because fakedb doesn't work to publish items
                Assert.AreEqual(result.Errors.Count, 0);
                Assert.AreEqual(result.DeletedItems.Count, 0);
                Assert.AreEqual(result.NewItems.Count, 4);
                Assert.AreEqual(result.UpdatedItems.Count, 0);

            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ProcessSitecoreItemsToExecuteUpdateProcess()
        {
            List<CategoryExcelItem> listCategoryItems = new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem { Id = "0", Name = "Parent Category", ParentId = "-1", Level = 1 });
            listCategoryItems.Add(new CategoryExcelItem { Id = "1", Name = "Baby, Kids & Toys", ParentId = "0", Level = 2 });
            listCategoryItems.Add(new CategoryExcelItem { Id = "456", Name = "Jewelry", ParentId = "662", Level = 3 });
            listCategoryItems.Add(new CategoryExcelItem { Id = "662", Name = "Accessories", ParentId = "0", Level = 2 });
            listCategoryItems.Add(new CategoryExcelItem { Id = "663", Name = "Accessories", ParentId = "10" });

            using (var db = new Sitecore.FakeDb.Db())
            {
                var result = new CategoryImportResult();
                var templateId = new ID("40E9D39E-1F56-48A8-BB70-CA76353C5E21");
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{837C7EC5-596B-470B-83E6-186B11248919}"));
                var parentCategory = new DbItem("Parent Category", new ID("{837C7EC5-596B-470B-83E6-186B11248915}"), templateId)
                {
                    Fields = {
                        new DbField("Id", new ID("{D2C33213-B7F8-49CA-B70E-0D64BB099A8F}")) {
                            Value = "0"
                        },
                        new DbField("Name", new ID("{0620BA80-EEF0-4489-A4D9-969841A782BA}")) {
                            Value = "Parent Category"
                        },

                    }
                };
                categories.Add(parentCategory);
                root.Add(categories);
                db.Add(root);
                _sut.SetCsvItemLevel(listCategoryItems,result);
                _sut.ProcessSitecoreItems(listCategoryItems, result);
                //There is a error in publish because fakedb doesn't work to publish items
                Assert.AreEqual(result.Errors.Count, 1);
                Assert.AreEqual(result.DeletedItems.Count, 0);
                Assert.AreEqual(result.NewItems.Count, 3);
                Assert.AreEqual(result.UpdatedItems.Count, 1);

            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ProcessSitecoreItemsToExecuteDeleteProcess()
        {
            List<CategoryExcelItem> listCategoryItems = new List<CategoryExcelItem>();
            listCategoryItems.Add(new CategoryExcelItem { Id = "663", Name = "Accessories", ParentId = "10" });

            using (var db = new Sitecore.FakeDb.Db())
            {
                var result = new CategoryImportResult();
                var templateId = new ID("40E9D39E-1F56-48A8-BB70-CA76353C5E21");
                var root = new DbItem("MBCShared", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var categories = new DbItem("Rakuten Categories", new ID("{837C7EC5-596B-470B-83E6-186B11248919}"));
                var parentCategory = new DbItem("Parent Category", new ID("{837C7EC5-596B-470B-83E6-186B11248915}"), templateId)
                {
                    Fields = {
                        new DbField("Id", new ID("{D2C33213-B7F8-49CA-B70E-0D64BB099A8F}")) {
                            Value = "0"
                        },
                        new DbField("Name", new ID("{0620BA80-EEF0-4489-A4D9-969841A782BA}")) {
                            Value = "Parent Category"
                        },

                    }
                };
                categories.Add(parentCategory);
                root.Add(categories);
                db.Add(root);
                _sut.SetCsvItemLevel(listCategoryItems, result);
                _sut.ProcessSitecoreItems(listCategoryItems, result);
                //There is a error in publish because fakedb doesn't work to publish items
                Assert.AreEqual(result.Errors.Count, 1);
                Assert.AreEqual(result.DeletedItems.Count, 1);
            }
        }

    }
}
