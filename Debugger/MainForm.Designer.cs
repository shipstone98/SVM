
namespace Debugger
{
	partial class MainForm
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
			this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.CodeLabel = new System.Windows.Forms.Label();
			this.StackLabel = new System.Windows.Forms.Label();
			this.ContinueButton = new System.Windows.Forms.Button();
			this.StackListBox = new System.Windows.Forms.ListBox();
			this.CodeListBox = new System.Windows.Forms.ListBox();
			this.MainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainTableLayoutPanel
			// 
			this.MainTableLayoutPanel.ColumnCount = 2;
			this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.MainTableLayoutPanel.Controls.Add(this.CodeLabel, 0, 0);
			this.MainTableLayoutPanel.Controls.Add(this.StackLabel, 1, 0);
			this.MainTableLayoutPanel.Controls.Add(this.ContinueButton, 0, 2);
			this.MainTableLayoutPanel.Controls.Add(this.StackListBox, 1, 1);
			this.MainTableLayoutPanel.Controls.Add(this.CodeListBox, 0, 1);
			this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
			this.MainTableLayoutPanel.RowCount = 3;
			this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.MainTableLayoutPanel.Size = new System.Drawing.Size(601, 531);
			this.MainTableLayoutPanel.TabIndex = 0;
			// 
			// CodeLabel
			// 
			this.CodeLabel.AutoSize = true;
			this.CodeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CodeLabel.Location = new System.Drawing.Point(3, 0);
			this.CodeLabel.Name = "CodeLabel";
			this.CodeLabel.Size = new System.Drawing.Size(294, 50);
			this.CodeLabel.TabIndex = 0;
			this.CodeLabel.Text = "Code";
			this.CodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// StackLabel
			// 
			this.StackLabel.AutoSize = true;
			this.StackLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StackLabel.Location = new System.Drawing.Point(303, 0);
			this.StackLabel.Name = "StackLabel";
			this.StackLabel.Size = new System.Drawing.Size(295, 50);
			this.StackLabel.TabIndex = 0;
			this.StackLabel.Text = "Stack";
			this.StackLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ContinueButton
			// 
			this.MainTableLayoutPanel.SetColumnSpan(this.ContinueButton, 2);
			this.ContinueButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ContinueButton.Location = new System.Drawing.Point(3, 484);
			this.ContinueButton.Name = "ContinueButton";
			this.ContinueButton.Size = new System.Drawing.Size(595, 44);
			this.ContinueButton.TabIndex = 3;
			this.ContinueButton.Text = "Continue";
			this.ContinueButton.UseVisualStyleBackColor = true;
			this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
			// 
			// StackListBox
			// 
			this.StackListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StackListBox.FormattingEnabled = true;
			this.StackListBox.ItemHeight = 20;
			this.StackListBox.Location = new System.Drawing.Point(303, 53);
			this.StackListBox.Name = "StackListBox";
			this.StackListBox.Size = new System.Drawing.Size(295, 425);
			this.StackListBox.TabIndex = 2;
			this.StackListBox.SelectedIndexChanged += new System.EventHandler(this.StackListBox_SelectedIndexChanged);
			// 
			// CodeListBox
			// 
			this.CodeListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CodeListBox.FormattingEnabled = true;
			this.CodeListBox.ItemHeight = 20;
			this.CodeListBox.Location = new System.Drawing.Point(3, 53);
			this.CodeListBox.Name = "CodeListBox";
			this.CodeListBox.Size = new System.Drawing.Size(294, 425);
			this.CodeListBox.TabIndex = 1;
			this.CodeListBox.SelectedIndexChanged += new System.EventHandler(this.CodeListBox_SelectedIndexChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(601, 531);
			this.Controls.Add(this.MainTableLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Debugger - SVM";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.MainTableLayoutPanel.ResumeLayout(false);
			this.MainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
		private System.Windows.Forms.Label CodeLabel;
		private System.Windows.Forms.Label StackLabel;
		private System.Windows.Forms.Button ContinueButton;
		private System.Windows.Forms.ListBox StackListBox;
		private System.Windows.Forms.ListBox CodeListBox;
	}
}