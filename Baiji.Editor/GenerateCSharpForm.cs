using CTripOSS.Baiji.Editor.Properties;
using CTripOSS.Baiji.Generator;
using CTripOSS.Baiji.Generator.CSharp;
using CTripOSS.Baiji.Generator.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CTripOSS.Baiji.Editor
{
    public partial class GenerateCSharpForm : Form
    {
        private Generator.Generator _codeGenerator;

        public GenerateCSharpForm()
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
            m_OutputFolderTextBox.Text = Settings.Default.LastOutputFolder_CSharp;
            m_GenCommentsCheckBox.Checked = Settings.Default.GenComment_CSharp;
            m_GenIncludesCheckBox.Checked = Settings.Default.GenIncludes_CSharp;
            m_GenerateAllRadioButton.Checked = Settings.Default.GenAll_Default;
        }

        private void ListMethods()
        {
            var inputBaseFolder = new Uri(Path.GetDirectoryName(IdlFilename) + "\\", UriKind.Absolute);
            var configBuilder = CreateConfigBuilder(inputBaseFolder, Path.GetTempPath());
            var inputs = new List<Uri> { new Uri(IdlFilename, UriKind.Absolute) };
            try
            {
                _codeGenerator = new CSharpGenerator(configBuilder.Build());
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
            if (m_PrunerPanel.SelectedMethods.Count == 0)
            {
                MessageBox.Show(this, "No operation is selected.",
                    Resources.ProductName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                return;
            }
            var outputFolder = GetOutputFolder();
            if (outputFolder == null)
            {
                m_BrowseButton.Focus();
                return;
            }
            var inputBaseFolder = new Uri(Path.GetDirectoryName(IdlFilename) + "\\", UriKind.Absolute);
            var configBuilder = CreateConfigBuilder(inputBaseFolder, outputFolder);
            var inputs = new List<Uri> { new Uri(IdlFilename, UriKind.Absolute) };
            _codeGenerator.UpdateConfig(configBuilder.Build());

            try
            {
                if (m_GenerateAllRadioButton.Checked)
                {
                    _codeGenerator.Parse(inputs);
                }
                else
                {
                    var service = m_PrunerPanel.Service;
                    var selectedMethods = m_PrunerPanel.SelectedMethods;
                    _codeGenerator.Parse(inputs, service, selectedMethods);
                }
                var result = MessageBox.Show(this, "Code generation succeeded. Open the output folder?",
                                             Resources.ProductName,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Process.Start(outputFolder);
                }

                Settings.Default.LastOutputFolder_CSharp = outputFolder;
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
                .CodeFlavor("csharp-ctor");
            configBuilder.AddTweak(CSharpGeneratorTweak.ADD_DISPOSABLE_INTERFACE);
            configBuilder.AddTweak(CSharpGeneratorTweak.USE_PLAIN_CSHARP_NAMESPACE);
            if (m_ClientRadioButton.Checked)
            {
                configBuilder.AddTweak(CSharpGeneratorTweak.GEN_CLIENT_PROXY);
            }
            if (m_ServiceRadioButton.Checked)
            {
                configBuilder.AddTweak(CSharpGeneratorTweak.GEN_SERVICE_STUB);
            }
            if (m_GenCommentsCheckBox.Checked)
            {
                configBuilder.AddTweak(CSharpGeneratorTweak.GEN_COMMENTS);
            }
            return configBuilder;
        }

        private void m_GenerateAllRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (m_GenerateAllRadioButton.Checked)
            {
                m_PrunerPanel.SelectAll();
                m_PrunerPanel.Enabled = false;
            }
        }

        private void m_GenerateSelectedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (m_GenerateSelectedRadioButton.Checked)
            {
                m_PrunerPanel.Enabled = true;
            }
        }
    }
}