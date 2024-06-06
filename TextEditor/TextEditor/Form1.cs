using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TextEditor
{
    public partial class Form1 : Form, IFileView
    {
        public string FilePath { get; private set; }
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
        string documentsFilter = "All Acceptable Documents|*.txt;*.doc;*.docx;*.xml|Text Documents|*.txt|Word Documents|*.doc;*.docx|XML Documents|*.xml";

        public Form1()
        {
            InitializeComponent();
            OpenButton.Click += (sender, e) => OpenFile?.Invoke(sender, e);
            SaveAsButton.Click += (sender, e) => SaveFile?.Invoke(sender, e);
        }

        public void ShowMessage(string message)
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

                    // var reader = new System.IO.StreamReader(OpenFileDialog.FileName, Encoding.GetEncoding(1251));
                    // RichTextBox.Text = reader.ReadToEnd();
                    // reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            SaveFileDialog.Filter = documentsFilter;
            SaveFileDialog.DefaultExt = ".txt";
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FilePath = SaveFileDialog.FileName;
                    SaveFile?.Invoke(this, EventArgs.Empty);
                    
                    // var writer = new System.IO.StreamWriter(SaveFileDialog.FileName, false, System.Text.Encoding.GetEncoding(1251));
                    // writer.Write(RichTextBox.Text);
                    // writer.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {

        }

        private void FileNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void OpenFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void FolderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {

        }

        private void SaveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public interface IFileView
    {
        string FilePath { get; }
        string FileContent { get; set; }
        event EventHandler OpenFile;
        event EventHandler SaveFile;
        void ShowMessage(string message);
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
            return File.ReadAllText(filePath, Encoding.GetEncoding(1251));
        }

        public void Write(string filePath, string content)
        {
            File.WriteAllText(filePath, content, Encoding.GetEncoding(1251));
        }
    }

    public class XmlFileFormat : IFileFormat
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

    public class DocFileFormat : IFileFormat
    {
        public string Read(string filePath)
        {
            // Реализация чтения .doc файлов
            return "Документ .doc прочитан";
        }

        public void Write(string filePath, string content)
        {
            // Реализация записи .doc файлов
        }
    }

    public class DocxFileFormat : IFileFormat
    {
        public string Read(string filePath)
        {
            // Реализация чтения .docx файлов
            return "Документ .docx прочитан";
        }

        public void Write(string filePath, string content)
        {
            // Реализация записи .docx файлов
        }
    }

    public class FilePresenter
    {
        private readonly IFileView _view;
        private readonly FileHandler _fileHandler;

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
                _view.ShowMessage(ex.Message);
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
                _view.ShowMessage(ex.Message);
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