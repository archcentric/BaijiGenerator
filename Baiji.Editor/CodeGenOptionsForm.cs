using System;
using System.Windows.Forms;
using CTripOSS.Baiji.Editor.Properties;

namespace CTripOSS.Baiji.Editor
{
    public partial class CodeGenOptionsForm : Form
    {
        public CodeGenOptionsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void m_SaveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoadSettings()
        {
            m_CSharpGenCommentsCheckBox.Checked = Settings.Default.GenComment_CSharp;
            m_JavaGenCommentsCheckBox.Checked = Settings.Default.GenComment_Java;
            m_JavaGenPublicFieldsCheckBox.Checked = Settings.Default.GenPublicFields_Java;
        }

        private void SaveSettings()
        {
            Settings.Default.GenComment_CSharp = m_CSharpGenCommentsCheckBox.Checked;
            Settings.Default.GenComment_Java = m_JavaGenCommentsCheckBox.Checked;
            Settings.Default.GenPublicFields_Java = m_JavaGenPublicFieldsCheckBox.Checked;
            Settings.Default.Save();
        }
    }
}