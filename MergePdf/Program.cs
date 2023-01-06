using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MergePdf
{
    static class Program
    {
        static readonly PdfDocument m_OutputDoc = new PdfDocument();

        [STAThread]
        static void Main()
        {
            var inputFilePaths = SelectInputFiles();
            if (inputFilePaths == null)
            {
                return;
            }
            foreach (var filePath in inputFilePaths.OrderBy(filePath => filePath))
            {
                Console.WriteLine($"Merging {filePath}...");
                MergeFile(filePath);
            }

            var outputFilePath = SelectOutputFile();
            if (outputFilePath == null)
            {
                return;
            }
            m_OutputDoc.Save(outputFilePath);
            Process.Start(outputFilePath);
        }

        private static void MergeFile(string filePath)
        {
            using (var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
            {
                foreach (var page in doc.Pages)
                {
                    m_OutputDoc.AddPage(page);
                }
            }
        }

        const string m_Filter = "PDF Files (*.pdf)|*.pdf";

        private static string[] SelectInputFiles()
        {
            var dialog = new OpenFileDialog
            {
                Filter = m_Filter,
                Multiselect = true
            };
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileNames : null;
        }

        private static string SelectOutputFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = m_Filter,
            };
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }
    }
}
