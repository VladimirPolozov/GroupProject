using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using TextEditor;

namespace UnitTest
{
    [TestClass]
    public class FileHandlerTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Используем рефлексию для сброса состояния синглтона перед каждым тестом
            FieldInfo instance = typeof(FileHandler).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            instance.SetValue(null, null);
        }

        [TestMethod]
        public void GetInstance_ShouldReturnSameInstance()
        {
            var instance1 = FileHandler.GetInstance();
            var instance2 = FileHandler.GetInstance();

            Assert.AreEqual(instance1, instance2);
        }

        [TestMethod]
        public void OpenFile_ShouldReadFileCorrectly()
        {
            var fileHandler = FileHandler.GetInstance();
            var fileFormat = new TxtFileFormat();
            fileHandler.SetFileFormat(fileFormat);
            string filePath = "test.txt";
            string expectedContent = "Hello, World!";

            // Имитация записи в файл
            System.IO.File.WriteAllText(filePath, expectedContent);

            var content = fileHandler.OpenFile(filePath);

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void SaveFile_ShouldWriteFileCorrectly()
        {
            var fileHandler = FileHandler.GetInstance();
            var fileFormat = new TxtFileFormat();
            fileHandler.SetFileFormat(fileFormat);
            string filePath = "test_save.txt";
            string contentToSave = "This is a saved file content";

            fileHandler.SaveFile(filePath, contentToSave);
            var savedContent = System.IO.File.ReadAllText(filePath);

            Assert.AreEqual(contentToSave, savedContent);
        }
    }
}
