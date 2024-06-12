using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using TextEditor;
using System.IO;

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
            File.WriteAllText(filePath, expectedContent);

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
            var savedContent = File.ReadAllText(filePath);

            Assert.AreEqual(contentToSave, savedContent);
        }
    }

    [TestClass]
    public class FileMementoTests
    {
        string content = "Hello, World!";

        [TestMethod]
        public void MementoTest_ShouldWorkCorrectly()
        {
            string expectedResult = "Hello, World!";
            var contentHistory = new TextEditorHistory();

            contentHistory.History.Push(SaveState());

            content = content.Substring(0, 1);

            RestoreState(contentHistory.History.Pop());

            Assert.AreEqual(expectedResult, content);
        }

        public TextEditorMemento SaveState()
        {
            return new TextEditorMemento(content);
        }

        public void RestoreState(TextEditorMemento Memento)
        {
            this.content = Memento.Content;
        }
    }
}
