using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;

namespace TextEditor
{
    public interface IFileView
    {
        string FilePath { get; set; }
        string FileContent { get; set; }
        event EventHandler OpenFile;
        event EventHandler SaveFile;
        void ShowErrors(string message);
    }

    public partial class Form1 : Form, IFileView
    {
        public string FilePath { get; set; }
        public string FileContent
        {
            get
            {
                return RichTextBox.Text;
            }
            set
            {
                RichTextBox.Text = value;
            }
        }

        public event EventHandler OpenFile;
        public event EventHandler SaveFile;
        string filePath;
        string[] filePathInArray;
        string fileName;
        string documentsFilter = "All Acceptable Documents|*.txt;*.xml|Text Documents|*.txt|XML Documents|*.xml";

        public Form1()
        {
            InitializeComponent();
            OpenButton.Click += (sender, e) => OpenFile?.Invoke(sender, e);
            SaveAsButton.Click += (sender, e) => SaveFile?.Invoke(sender, e);
        }

        public void ShowErrors(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Filter = documentsFilter;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FilePath = OpenFileDialog.FileName;
                    filePathInArray = FilePath.Split(new char[] { '\\' });
                    fileName = filePathInArray[filePathInArray.Length - 1];
                    UpdateFileNameLabel(fileName);
                    OpenFile?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    ShowErrors(ex.Message);
                }
            }
        }

        private void UpdateFileNameLabel(string newFileName)
        {
            FileNameLabel.Text = newFileName;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog.Filter = "Text Documents|*.txt";
            SaveFileDialog.DefaultExt = ".txt";
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FilePath = SaveFileDialog.FileName;
                    SaveFile?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    ShowErrors(ex.Message);
                }
            }
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {

        }

        private void FileNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void FolderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {

        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public interface IFileFormat
    {
        string Read(string filePath);
        void Write(string filePath, string content);
    }

    public class TxtFileFormat : IFileFormat
    {
        public string Read(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public void Write(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
    }

    public class XmlFileFormat : IFileFormat
    {
        public IFileView _view;

        public string Read(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(filePath);
                return ParseXmlTextNodes(xmlDoc.DocumentElement);
            }
            catch (Exception ex)
            {
                _view.ShowErrors(ex.Message);
                throw new Exception("Error reading XML file: " + ex.Message);
            }
        }

        public void Write(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        private string ParseXmlTextNodes(XmlNode node)
        {
            string result = "";

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Element:
                        result += ParseXmlTextNodes(childNode);
                        break;
                    case XmlNodeType.Text:
                        result += childNode.InnerText.Trim() + Environment.NewLine;
                        break;
                }
            }

            return result;
        }
    }

    public class FilePresenter
    {
        public IFileView _view;
        public FileHandler _fileHandler;

        public FilePresenter(IFileView view)
        {
            _view = view;
            _fileHandler = new FileHandler();
            _view.OpenFile += OnOpenFile;
            _view.SaveFile += OnSaveFile;
        }

        private void OnOpenFile(object sender, EventArgs e)
        {
            try
            {
                IFileFormat fileFormat = GetFileFormat(_view.FilePath);
                _fileHandler.SetFileFormat(fileFormat);
                _view.FileContent = _fileHandler.OpenFile(_view.FilePath);
            }
            catch (Exception ex)
            {
                _view.ShowErrors(ex.Message);
            }
        }

        private void OnSaveFile(object sender, EventArgs e)
        {
            try
            {
                IFileFormat fileFormat = GetFileFormat(_view.FilePath);
                _fileHandler.SetFileFormat(fileFormat);
                _fileHandler.SaveFile(_view.FilePath, _view.FileContent);
            }
            catch (Exception ex)
            {
                _view.ShowErrors(ex.Message);
            }
        }

        private IFileFormat GetFileFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".txt":
                    return new TxtFileFormat();
                case ".xml":
                    return new XmlFileFormat();
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }
    }

    public class FileHandler
    {
        private IFileFormat _fileFormat;

        public void SetFileFormat(IFileFormat fileFormat)
        {
            _fileFormat = fileFormat;
        }

        public string OpenFile(string filePath)
        {
            return _fileFormat.Read(filePath);
        }

        public void SaveFile(string filePath, string content)
        {
            _fileFormat.Write(filePath, content);
        }
    }
}