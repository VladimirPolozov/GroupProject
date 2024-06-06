using System;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        string filePath;
        string[] filePathInArray;
        string fileName;
        string documentsFilter = "All Acceptable Documents|*.txt;*.doc;*.docx;*.xml|Text Documents|*.txt|Word Documents|*.doc;*.docx|XML Documents|*.xml";

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Filter = documentsFilter;
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = OpenFileDialog.FileName;
                filePathInArray = filePath.Split(new char[] { '\\' });
                fileName = filePathInArray[filePathInArray.Length - 1];
                UpdateFileNameLabel(fileName);

                try
                {
                    var reader = new System.IO.StreamReader(OpenFileDialog.FileName, Encoding.GetEncoding(1251));
                    RichTextBox.Text = reader.ReadToEnd();
                    reader.Close();
                }
                catch (Exception Ситуация)
                {
                    MessageBox.Show(Ситуация.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    var writer = new System.IO.StreamWriter(SaveFileDialog.FileName, false, System.Text.Encoding.GetEncoding(1251));
                    writer.Write(RichTextBox.Text);
                    writer.Close();
                }
                catch (Exception Ситуация)
                {
                    MessageBox.Show(Ситуация.Message,
                        "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
}