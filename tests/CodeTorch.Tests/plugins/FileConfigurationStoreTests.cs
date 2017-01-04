using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeTorch.Configuration.FileStore;
using CodeTorch.Core;
using System.Threading.Tasks;
using log4net;
using CodeTorch.Logger.Log4Net;

namespace CodeTorch.Tests
{
    

    [TestClass]
    public class FileConfigurationStoreTests
    {
        const string TestPath = @"D:\Sandbox\dfsp\src\DFSP.Web\App_Data\CodeTorch\Web";
        readonly CodeTorch.Abstractions.ILogManager log;
        public FileConfigurationStoreTests()
        {

            log4net.Config.XmlConfigurator.Configure();

            log = new CodeTorch.Logger.Log4Net.Log4NetLogManager();
            
            
        }

        [TestMethod]
        public void FileStore_Add_ShouldAddNewFile()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Add_AddExistingFile_ShouldThrowError()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Delete_ExistingFile_ShouldDeleteFile()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Delete_NoFile_ShouldThrowError()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Exists_NoFile_ShouldReturnFalse()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Exists_FileExists_ShouldReturnTrue()
        {
            Assert.Fail();
        }

        [TestMethod]
        public async Task FileStore_Get_FileExists_ShouldReturnObject()
        {
            //arrange
            var store = new FileConfigurationStore(log);
            store.Path = TestPath;
            var key = "Audit_Insert";

            //act
            var item = await store.GetItem<DataCommand>(key);

            //asert
            Assert.IsNotNull(item);
            Assert.AreEqual(key, item.Name);

        }

        [TestMethod]
        public async Task FileStore_Get_FileNotExists_ShouldReturnNull()
        {
            //arrange
            var store = new FileConfigurationStore(log);
            store.Path = TestPath;
            var key = Guid.NewGuid().ToString();

            //act
            var item = await store.GetItem<DataCommand>(key);

            //asert
            Assert.IsNull(item);

        }

        [TestMethod]
        public async Task FileStore_Get_FileInvalidFile_ShouldReturnNull()
        {
            //arrange
            var store = new FileConfigurationStore(log);
            store.Path = TestPath;
            var key = "bad";

            //act
            var item = await store.GetItem<DataCommand>(key);

            //asert
            Assert.IsNull(item);
        }

        [TestMethod]
        public async Task FileStore_GetItems_FilesExists_ShouldReturnCorrectItemsBack()
        {
            //arrange
            var testItemsCount = 417;
            var store = new FileConfigurationStore(log);
            store.Path = TestPath;


            //act
            var items = await store.GetItems<DataCommand>();

            //asert
            Assert.IsNotNull(items);
            Assert.AreEqual(testItemsCount, items.Count);
        }

        [TestMethod]
        public async Task FileStore_GetScreens_FilesExists_ShouldReturnCorrectItemsBack()
        {
            //arrange
            var testItemsCount = 212;
            var store = new FileConfigurationStore(log);
            store.Path = TestPath;


            //act
            var sectionTypes = await store.GetItems<SectionType>();
            var controlTypes = await store.GetItems<ControlType>();

            foreach (var sectionType in sectionTypes)
            {
                CodeTorch.Core.Configuration.GetInstance().SectionTypes.Add(sectionType);
            }

            foreach (var controlType in controlTypes)
            {
                CodeTorch.Core.Configuration.GetInstance().ControlTypes.Add(controlType);
            }

            var items = await store.GetItems<Screen>();

            //asert
            Assert.IsNotNull(items);
            Assert.AreEqual(testItemsCount, items.Count);
        }

        [TestMethod]
        public async Task FileStore_GetItems_NonExists_ShouldReturnEmptyList()
        {
            //arrange
            var testItemsCount = 0;
            var store = new FileConfigurationStore(log);
            store.Path = TestPath;


            //act
            var items = await store.GetItems<Sequence>();

            //asert
            Assert.IsNotNull(items);
            Assert.AreEqual(testItemsCount, items.Count);
        }

        [TestMethod]
        public void FileStore_Save_NonExists_ShouldCreateNewFile()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Save_AlreadyExists_ShouldOverwriteFile()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Update_NonExists_ShouldThrowError()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FileStore_Update_AlreadyExists_ShouldOverwriteFile()
        {
            Assert.Fail();
        }
    }
}
