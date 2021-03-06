﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileScanner.SearchSummary
{
    public struct ReportOptions
    {
        public bool headerHasSearchQuery;
        public bool headerHasGenerationDate;
        public bool headerHasInputPaths;

        public bool resultHasFullFilePath;
        public bool resultHasFileSize;
        public bool resultHasCreationTime;
        public bool resultHasLastAccessTime;
        public bool resultHasLastModificationTime;
        public bool resultHasContext;

        public string outputFilePath;

        public int contextSizeChars;
        public int maxEntries;
    }

    public partial class SummaryOptionsForm : Form
    {
        public ReportOptions Options
        {
            get
            {
                ReportOptions options = new ReportOptions();

                options.headerHasSearchQuery = HeaderSearchQueryCheckbox.Checked;
                options.headerHasGenerationDate = HeaderGenerationDateCheckbox.Checked;
                options.headerHasInputPaths = HeaderInpuPathsCheckbox.Checked;

                options.resultHasFullFilePath = ResultFullFilePathCheckbox.Checked;
                options.resultHasFileSize = ResultFileSizeCheckbox.Checked;
                options.resultHasLastAccessTime = ResultAccessTimeCheckbox.Checked;
                options.resultHasLastModificationTime = ResultLastModificationTime.Checked;
                options.resultHasContext = ResultContext.Checked;

                options.outputFilePath = OutputFilePath.Text;
                options.contextSizeChars = (int)ContextSizeCharacters.Value;
                options.maxEntries = (int)MaxEntries.Value;

                return options;
            }
        }

        public SummaryOptionsForm()
        {
            Application.EnableVisualStyles();
            InitializeComponent();

            OutputFilePath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                               "report" + DocumentBuilderFactory.GetDefaultFormat());
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelReportButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.CreatePrompt = false;
            dialog.Title = "Save report as";

            IEnumerable<string> supportedFormats = DocumentBuilderFactory.GetSupportedFormats();
            dialog.InitialDirectory = Path.GetDirectoryName(OutputFilePath.Text);
            dialog.FileName = Path.GetFileName(OutputFilePath.Text);
            dialog.DefaultExt = Path.GetExtension(OutputFilePath.Text);
            dialog.Filter = String.Join("|", supportedFormats.Select(format => String.Format("{0} file|*{1}", format.Substring(1).ToUpper(), format)));

            if (dialog.ShowDialog() == DialogResult.OK)
                OutputFilePath.Text = dialog.FileName;
        }

        private void ResultContext_CheckedChanged(object sender, EventArgs e)
        {
            ContextSizeCharacters.Enabled = ResultContext.Checked;
        }
    }
}
