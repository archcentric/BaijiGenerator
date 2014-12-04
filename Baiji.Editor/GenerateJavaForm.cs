﻿using System;
using CTripOSS.Baiji.Generator.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CTripOSS.Baiji.Editor.Properties;
using CTripOSS.Baiji.Generator;
using CTripOSS.Baiji.Generator.Java;

namespace CTripOSS.Baiji.Editor
{
    public partial class GenerateJavaForm : Form
    {
        private Generator.Generator _codeGenerator;

        public GenerateJavaForm()
        {
            InitializeComponent();
            LoadDefaults();
        }

        public string IdlFilename
        {
            get
            {
                return m_IdlFileTextBox.Text;
            }
            set
            {
                m_IdlFileTextBox.Text = value;
                ListMethods();
            }
        }

        private void LoadDefaults()
        {
            m_OutputFolderTextBox.Text = Settings.Default.LastOutputFolder_Java;
            m_GenCommentsCheckBox.Checked = Settings.Default.GenComment_Java;
            m_GenPublicFieldsCheckBox.Checked = Settings.Default.GenPublicFields_Java;
            m_GenIncludesCheckBox.Checked = Settings.Default.GenIncludes_Java;
            m_GenerateAllRadioButton.Checked = Settings.Default.GenAll_Default;
        }

        private void ListMethods()
        {
            var outputFolder = GetOutputFolder();
            if (outputFolder == null)
            {
                m_BrowseButton.Focus();
                return;
            }

            var inputBaseFolder = new Uri(Path.GetDirectoryName(IdlFilename) + "\\", UriKind.Absolute);
            var configBuilder = CreateConfigBuilder(inputBaseFolder, outputFolder);
            var inputs = new List<Uri> { new Uri(IdlFilename, UriKind.Absolute) };
            try
            {
                _codeGenerator = new JavaGenerator(configBuilder.Build());
                var service = ContextUtils.ExtractService(_codeGenerator.GetContexts(inputs).Values.ToList());
                m_PrunerPanel.Service = service;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Code generation failed: " + ex.Message, Resources.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_BrowseButton_Click(object sender, EventArgs e)
        {
            m_FolderBrowserDialog.SelectedPath = m_OutputFolderTextBox.Text;
            var result = m_FolderBrowserDialog.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }
            m_OutputFolderTextBox.Text = m_FolderBrowserDialog.SelectedPath;
        }

        private void m_GenerateButton_Click(object sender, EventArgs e)
        {
            var outputFolder = GetOutputFolder();
            if (outputFolder == null)
            {
                m_BrowseButton.Focus();
                return;
            }

            try
            {
                if (m_ServiceRadioButton.Checked || m_GenerateAllRadioButton.Checked)
                {
                    _codeGenerator.Parse();
                }
                else
                {
                    var selectedMethods = m_PrunerPanel.SelectedMethods;
                    if (selectedMethods.Count == 0)
                    {
                        return;
                    }
                    _codeGenerator.Parse(selectedMethods);
                }
                var result = MessageBox.Show(this, "Code generation succeeded. Open the output folder?",
                                             Resources.ProductName,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Process.Start(outputFolder);
                }

                Settings.Default.LastOutputFolder_Java = outputFolder;
                Settings.Default.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Code generation failed: " + ex.Message, Resources.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetOutputFolder()
        {
            var outputFolder = m_OutputFolderTextBox.Text;
            if (string.IsNullOrEmpty(outputFolder))
            {
                MessageBox.Show(this,
                                "Please select the output folder first.",
                                Resources.ProductName, MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Information);
                return null;
            }
            if (Directory.Exists(outputFolder))
            {
                var isTargetFolderEmpty = !Directory.EnumerateFileSystemEntries(outputFolder).Any();
                if (!isTargetFolderEmpty)
                {
                    var result = MessageBox.Show(this,
                                                 "The output folder isn't empty. Please be noted that existed files may be overwritten during the code generation. Click OK to continue.",
                                                 Resources.ProductName, MessageBoxButtons.OKCancel,
                                                 MessageBoxIcon.Warning);
                    if (result != DialogResult.OK)
                    {
                        return null;
                    }
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch
                {
                    MessageBox.Show(this,
                                    "Unable to create the output folder. Please select another one.",
                                    Resources.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return null;
                }
            }
            return outputFolder;
        }

        private GeneratorConfig.Builder CreateConfigBuilder(Uri inputBaseFolder, string outputFolder)
        {
            var configBuilder = new GeneratorConfig.Builder()
                .InputBase(inputBaseFolder)
                .OutputFolder(new DirectoryInfo(outputFolder))
                .OverrideNamespace(null)
                .DefaultNamespace(null)
                .GenerateIncludedCode(m_GenIncludesCheckBox.Checked)
                .CodeFlavor("java-ctor");
            configBuilder.AddTweak(JavaGeneratorTweak.USE_PLAIN_JAVA_NAMESPACE);
            if (m_GenCommentsCheckBox.Checked)
            {
                configBuilder.AddTweak(JavaGeneratorTweak.GEN_COMMENTS);
            }
            if (m_GenPublicFieldsCheckBox.Checked)
            {
                configBuilder.AddTweak(JavaGeneratorTweak.GEN_PUBLIC_FIELDS);
            }
            if (m_ClientRadioButton.Checked)
            {
                configBuilder.AddTweak(JavaGeneratorTweak.GEN_CLIENT_PROXY);
            }
            if (m_ServiceRadioButton.Checked)
            {
                configBuilder.AddTweak(JavaGeneratorTweak.GEN_SERVICE_STUB);
            }
            return configBuilder;
        }

        private void m_GenerateAllRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (m_GenerateAllRadioButton.Checked)
            {
                m_PrunerPanel.ForceSelectAll();
            }
        }

        private void m_GenerateSelectedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_ServiceRadioButton.Checked && m_GenerateSelectedRadioButton.Checked)
            {
                m_PrunerPanel.EnableSelect();
            }
        }
    }
}