namespace CTripOSS.Baiji.Editor
{
    partial class GenerateOCForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_OutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.m_AutoReleaseCheckBox = new System.Windows.Forms.CheckBox();
            this.m_GenIncludesCheckBox = new System.Windows.Forms.CheckBox();
            this.m_GenCommentsCheckBox = new System.Windows.Forms.CheckBox();
            this.m_OutputFolderLabel = new System.Windows.Forms.Label();
            this.m_CancelButton = new System.Windows.Forms.Button();
            this.m_IdlFileTextBox = new System.Windows.Forms.TextBox();
            this.m_GenerateButton = new System.Windows.Forms.Button();
            this.m_FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_BrowseButton = new System.Windows.Forms.Button();
            this.m_IdlFileLabel = new System.Windows.Forms.Label();
            this.m_OptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_OutputFolderTextBox
            // 
            this.m_OutputFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_OutputFolderTextBox.Location = new System.Drawing.Point(96, 36);
            this.m_OutputFolderTextBox.Name = "m_OutputFolderTextBox";
            this.m_OutputFolderTextBox.Size = new System.Drawing.Size(316, 21);
            this.m_OutputFolderTextBox.TabIndex = 17;
            // 
            // m_OptionsGroupBox
            // 
            this.m_OptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_OptionsGroupBox.Controls.Add(this.m_AutoReleaseCheckBox);
            this.m_OptionsGroupBox.Controls.Add(this.m_GenIncludesCheckBox);
            this.m_OptionsGroupBox.Controls.Add(this.m_GenCommentsCheckBox);
            this.m_OptionsGroupBox.Location = new System.Drawing.Point(9, 80);
            this.m_OptionsGroupBox.Name = "m_OptionsGroupBox";
            this.m_OptionsGroupBox.Size = new System.Drawing.Size(443, 53);
            this.m_OptionsGroupBox.TabIndex = 11;
            this.m_OptionsGroupBox.TabStop = false;
            this.m_OptionsGroupBox.Text = "Options";
            // 
            // m_AutoReleaseCheckBox
            // 
            this.m_AutoReleaseCheckBox.AutoSize = true;
            this.m_AutoReleaseCheckBox.Location = new System.Drawing.Point(287, 25);
            this.m_AutoReleaseCheckBox.Name = "m_AutoReleaseCheckBox";
            this.m_AutoReleaseCheckBox.Size = new System.Drawing.Size(96, 16);
            this.m_AutoReleaseCheckBox.TabIndex = 2;
            this.m_AutoReleaseCheckBox.Text = "Auto Release";
            this.m_AutoReleaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_GenIncludesCheckBox
            // 
            this.m_GenIncludesCheckBox.AutoSize = true;
            this.m_GenIncludesCheckBox.Location = new System.Drawing.Point(149, 25);
            this.m_GenIncludesCheckBox.Name = "m_GenIncludesCheckBox";
            this.m_GenIncludesCheckBox.Size = new System.Drawing.Size(126, 16);
            this.m_GenIncludesCheckBox.TabIndex = 1;
            this.m_GenIncludesCheckBox.Text = "Generate Includes";
            this.m_GenIncludesCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_GenCommentsCheckBox
            // 
            this.m_GenCommentsCheckBox.AutoSize = true;
            this.m_GenCommentsCheckBox.Location = new System.Drawing.Point(17, 25);
            this.m_GenCommentsCheckBox.Name = "m_GenCommentsCheckBox";
            this.m_GenCommentsCheckBox.Size = new System.Drawing.Size(126, 16);
            this.m_GenCommentsCheckBox.TabIndex = 0;
            this.m_GenCommentsCheckBox.Text = "Generate Comments";
            this.m_GenCommentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_OutputFolderLabel
            // 
            this.m_OutputFolderLabel.AutoSize = true;
            this.m_OutputFolderLabel.Location = new System.Drawing.Point(7, 39);
            this.m_OutputFolderLabel.Name = "m_OutputFolderLabel";
            this.m_OutputFolderLabel.Size = new System.Drawing.Size(83, 12);
            this.m_OutputFolderLabel.TabIndex = 16;
            this.m_OutputFolderLabel.Text = "Output Folder";
            // 
            // m_CancelButton
            // 
            this.m_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_CancelButton.Location = new System.Drawing.Point(377, 180);
            this.m_CancelButton.Name = "m_CancelButton";
            this.m_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_CancelButton.TabIndex = 13;
            this.m_CancelButton.Text = "&Cancel";
            this.m_CancelButton.UseVisualStyleBackColor = true;
            this.m_CancelButton.Click += new System.EventHandler(this.m_CancelButton_Click);
            // 
            // m_IdlFileTextBox
            // 
            this.m_IdlFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_IdlFileTextBox.Location = new System.Drawing.Point(96, 9);
            this.m_IdlFileTextBox.Name = "m_IdlFileTextBox";
            this.m_IdlFileTextBox.ReadOnly = true;
            this.m_IdlFileTextBox.Size = new System.Drawing.Size(356, 21);
            this.m_IdlFileTextBox.TabIndex = 15;
            // 
            // m_GenerateButton
            // 
            this.m_GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GenerateButton.Location = new System.Drawing.Point(296, 180);
            this.m_GenerateButton.Name = "m_GenerateButton";
            this.m_GenerateButton.Size = new System.Drawing.Size(75, 23);
            this.m_GenerateButton.TabIndex = 12;
            this.m_GenerateButton.Text = "&Generate";
            this.m_GenerateButton.UseVisualStyleBackColor = true;
            this.m_GenerateButton.Click += new System.EventHandler(this.m_GenerateButton_Click);
            // 
            // m_FolderBrowserDialog
            // 
            this.m_FolderBrowserDialog.SelectedPath = "Please select the output folder of code generation:";
            // 
            // m_BrowseButton
            // 
            this.m_BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_BrowseButton.Location = new System.Drawing.Point(417, 34);
            this.m_BrowseButton.Name = "m_BrowseButton";
            this.m_BrowseButton.Size = new System.Drawing.Size(34, 23);
            this.m_BrowseButton.TabIndex = 9;
            this.m_BrowseButton.Text = "...";
            this.m_BrowseButton.UseVisualStyleBackColor = true;
            this.m_BrowseButton.Click += new System.EventHandler(this.m_BrowseButton_Click);
            // 
            // m_IdlFileLabel
            // 
            this.m_IdlFileLabel.AutoSize = true;
            this.m_IdlFileLabel.Location = new System.Drawing.Point(7, 12);
            this.m_IdlFileLabel.Name = "m_IdlFileLabel";
            this.m_IdlFileLabel.Size = new System.Drawing.Size(53, 12);
            this.m_IdlFileLabel.TabIndex = 14;
            this.m_IdlFileLabel.Text = "IDL File";
            // 
            // GenerateOCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 212);
            this.Controls.Add(this.m_OutputFolderTextBox);
            this.Controls.Add(this.m_OptionsGroupBox);
            this.Controls.Add(this.m_OutputFolderLabel);
            this.Controls.Add(this.m_CancelButton);
            this.Controls.Add(this.m_IdlFileTextBox);
            this.Controls.Add(this.m_GenerateButton);
            this.Controls.Add(this.m_BrowseButton);
            this.Controls.Add(this.m_IdlFileLabel);
            this.Name = "GenerateOCForm";
            this.Text = "GenerateOCForm";
            this.m_OptionsGroupBox.ResumeLayout(false);
            this.m_OptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_OutputFolderTextBox;
        private System.Windows.Forms.GroupBox m_OptionsGroupBox;
        private System.Windows.Forms.CheckBox m_GenIncludesCheckBox;
        private System.Windows.Forms.CheckBox m_GenCommentsCheckBox;
        private System.Windows.Forms.Label m_OutputFolderLabel;
        private System.Windows.Forms.Button m_CancelButton;
        private System.Windows.Forms.TextBox m_IdlFileTextBox;
        private System.Windows.Forms.Button m_GenerateButton;
        private System.Windows.Forms.FolderBrowserDialog m_FolderBrowserDialog;
        private System.Windows.Forms.Button m_BrowseButton;
        private System.Windows.Forms.Label m_IdlFileLabel;
        private System.Windows.Forms.CheckBox m_AutoReleaseCheckBox;
    }
}