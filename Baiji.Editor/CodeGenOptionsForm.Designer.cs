﻿namespace CTripOSS.Baiji.Editor
{
    partial class CodeGenOptionsForm
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
            this.m_CSharpOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.m_CSharpGenCommentsCheckBox = new System.Windows.Forms.CheckBox();
            this.m_JavaOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.m_JavaGenPublicFieldsCheckBox = new System.Windows.Forms.CheckBox();
            this.m_JavaGenCommentsCheckBox = new System.Windows.Forms.CheckBox();
            this.m_CancelButton = new System.Windows.Forms.Button();
            this.m_SaveButton = new System.Windows.Forms.Button();
            this.m_CSharpGenIncludesCheckBox = new System.Windows.Forms.CheckBox();
            this.m_JavaGenIncludesCheckBox = new System.Windows.Forms.CheckBox();
            this.m_CSharpOptionsGroupBox.SuspendLayout();
            this.m_JavaOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_CSharpOptionsGroupBox
            // 
            this.m_CSharpOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_CSharpOptionsGroupBox.Controls.Add(this.m_CSharpGenIncludesCheckBox);
            this.m_CSharpOptionsGroupBox.Controls.Add(this.m_CSharpGenCommentsCheckBox);
            this.m_CSharpOptionsGroupBox.Location = new System.Drawing.Point(14, 12);
            this.m_CSharpOptionsGroupBox.Name = "m_CSharpOptionsGroupBox";
            this.m_CSharpOptionsGroupBox.Size = new System.Drawing.Size(443, 53);
            this.m_CSharpOptionsGroupBox.TabIndex = 3;
            this.m_CSharpOptionsGroupBox.TabStop = false;
            this.m_CSharpOptionsGroupBox.Text = "C# Options";
            // 
            // m_CSharpGenCommentsCheckBox
            // 
            this.m_CSharpGenCommentsCheckBox.AutoSize = true;
            this.m_CSharpGenCommentsCheckBox.Location = new System.Drawing.Point(17, 25);
            this.m_CSharpGenCommentsCheckBox.Name = "m_CSharpGenCommentsCheckBox";
            this.m_CSharpGenCommentsCheckBox.Size = new System.Drawing.Size(126, 16);
            this.m_CSharpGenCommentsCheckBox.TabIndex = 0;
            this.m_CSharpGenCommentsCheckBox.Text = "Generate Comments";
            this.m_CSharpGenCommentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_JavaOptionsGroupBox
            // 
            this.m_JavaOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_JavaOptionsGroupBox.Controls.Add(this.m_JavaGenIncludesCheckBox);
            this.m_JavaOptionsGroupBox.Controls.Add(this.m_JavaGenPublicFieldsCheckBox);
            this.m_JavaOptionsGroupBox.Controls.Add(this.m_JavaGenCommentsCheckBox);
            this.m_JavaOptionsGroupBox.Location = new System.Drawing.Point(14, 71);
            this.m_JavaOptionsGroupBox.Name = "m_JavaOptionsGroupBox";
            this.m_JavaOptionsGroupBox.Size = new System.Drawing.Size(443, 53);
            this.m_JavaOptionsGroupBox.TabIndex = 4;
            this.m_JavaOptionsGroupBox.TabStop = false;
            this.m_JavaOptionsGroupBox.Text = "Java Options";
            // 
            // m_JavaGenPublicFieldsCheckBox
            // 
            this.m_JavaGenPublicFieldsCheckBox.AutoSize = true;
            this.m_JavaGenPublicFieldsCheckBox.Location = new System.Drawing.Point(149, 25);
            this.m_JavaGenPublicFieldsCheckBox.Name = "m_JavaGenPublicFieldsCheckBox";
            this.m_JavaGenPublicFieldsCheckBox.Size = new System.Drawing.Size(156, 16);
            this.m_JavaGenPublicFieldsCheckBox.TabIndex = 2;
            this.m_JavaGenPublicFieldsCheckBox.Text = "Generate Public Fields";
            this.m_JavaGenPublicFieldsCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_JavaGenCommentsCheckBox
            // 
            this.m_JavaGenCommentsCheckBox.AutoSize = true;
            this.m_JavaGenCommentsCheckBox.Location = new System.Drawing.Point(17, 25);
            this.m_JavaGenCommentsCheckBox.Name = "m_JavaGenCommentsCheckBox";
            this.m_JavaGenCommentsCheckBox.Size = new System.Drawing.Size(126, 16);
            this.m_JavaGenCommentsCheckBox.TabIndex = 0;
            this.m_JavaGenCommentsCheckBox.Text = "Generate Comments";
            this.m_JavaGenCommentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_CancelButton
            // 
            this.m_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_CancelButton.Location = new System.Drawing.Point(382, 135);
            this.m_CancelButton.Name = "m_CancelButton";
            this.m_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_CancelButton.TabIndex = 6;
            this.m_CancelButton.Text = "&Cancel";
            this.m_CancelButton.UseVisualStyleBackColor = true;
            // 
            // m_SaveButton
            // 
            this.m_SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_SaveButton.Location = new System.Drawing.Point(301, 135);
            this.m_SaveButton.Name = "m_SaveButton";
            this.m_SaveButton.Size = new System.Drawing.Size(75, 23);
            this.m_SaveButton.TabIndex = 5;
            this.m_SaveButton.Text = "&Save";
            this.m_SaveButton.UseVisualStyleBackColor = true;
            this.m_SaveButton.Click += new System.EventHandler(this.m_SaveButton_Click);
            // 
            // m_CSharpGenIncludesCheckBox
            // 
            this.m_CSharpGenIncludesCheckBox.AutoSize = true;
            this.m_CSharpGenIncludesCheckBox.Location = new System.Drawing.Point(149, 25);
            this.m_CSharpGenIncludesCheckBox.Name = "m_CSharpGenIncludesCheckBox";
            this.m_CSharpGenIncludesCheckBox.Size = new System.Drawing.Size(126, 16);
            this.m_CSharpGenIncludesCheckBox.TabIndex = 0;
            this.m_CSharpGenIncludesCheckBox.Text = "Generate Includes";
            this.m_CSharpGenIncludesCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_JavaGenIncludesCheckBox
            // 
            this.m_JavaGenIncludesCheckBox.AutoSize = true;
            this.m_JavaGenIncludesCheckBox.Location = new System.Drawing.Point(311, 25);
            this.m_JavaGenIncludesCheckBox.Name = "m_JavaGenIncludesCheckBox";
            this.m_JavaGenIncludesCheckBox.Size = new System.Drawing.Size(126, 16);
            this.m_JavaGenIncludesCheckBox.TabIndex = 0;
            this.m_JavaGenIncludesCheckBox.Text = "Generate Includes";
            this.m_JavaGenIncludesCheckBox.UseVisualStyleBackColor = true;
            // 
            // CodeGenOptionsForm
            // 
            this.AcceptButton = this.m_SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_CancelButton;
            this.ClientSize = new System.Drawing.Size(469, 170);
            this.Controls.Add(this.m_CancelButton);
            this.Controls.Add(this.m_SaveButton);
            this.Controls.Add(this.m_JavaOptionsGroupBox);
            this.Controls.Add(this.m_CSharpOptionsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CodeGenOptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Code Generation Options";
            this.m_CSharpOptionsGroupBox.ResumeLayout(false);
            this.m_CSharpOptionsGroupBox.PerformLayout();
            this.m_JavaOptionsGroupBox.ResumeLayout(false);
            this.m_JavaOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_CSharpOptionsGroupBox;
        private System.Windows.Forms.CheckBox m_CSharpGenCommentsCheckBox;
        private System.Windows.Forms.GroupBox m_JavaOptionsGroupBox;
        private System.Windows.Forms.CheckBox m_JavaGenCommentsCheckBox;
        private System.Windows.Forms.Button m_CancelButton;
        private System.Windows.Forms.Button m_SaveButton;
        private System.Windows.Forms.CheckBox m_JavaGenPublicFieldsCheckBox;
        private System.Windows.Forms.CheckBox m_CSharpGenIncludesCheckBox;
        private System.Windows.Forms.CheckBox m_JavaGenIncludesCheckBox;
    }
}