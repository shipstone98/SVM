using System;
using System.Windows.Forms;

using SVM.VirtualMachine.Debug;
using SVM.VirtualMachine;

namespace Debugger
{
	public partial class MainForm : Form
	{
		private int _CodeSelection;
		private readonly Debugger _Debugger;

		public MainForm()
		{
			this.InitializeComponent();
			this.ContinueButton.Focus();
		}

		public MainForm(Debugger debugger) : this() => this._Debugger = debugger;

		internal void Break(IDebugFrame debugFrame)
		{
			this.SuspendLayout();

			for (int i = 0; i < debugFrame.CodeFrame.Count; i ++)
			{
				IInstruction instruction = debugFrame.CodeFrame[i];
				this.CodeListBox.Items.Add(instruction.ToString());

				if (Object.ReferenceEquals(instruction, debugFrame.CurrentInstruction))
				{
					this.CodeListBox.SelectedIndex = this._CodeSelection = i;
				}
			}

			if (this._Debugger._VirtualMachine.Stack.Count == 0)
			{
				this.StackListBox.Items.Add("The stack is currently empty.");
			}

			else
			{
				foreach (Object obj in this._Debugger._VirtualMachine.Stack)
				{
					this.StackListBox.Items.Add(obj.ToString());
				}
			}

			this.ContinueButton.Enabled = true;
			this.ResumeLayout();
		}

		private void Disable()
		{
			this.SuspendLayout();
			this.ContinueButton.Enabled = false;
			this.CodeListBox.Items.Clear();
			this.StackListBox.Items.Clear();
			this.ResumeLayout();
			this._CodeSelection = 0;
			this._Debugger._AwaitingForm = false;
		}

		private void ContinueButton_Click(Object sender, EventArgs e) => this.Disable();
		private void MainForm_FormClosed(Object sender, FormClosedEventArgs e) => this.Disable();
		private void CodeListBox_SelectedIndexChanged(Object sender, EventArgs e) => this.CodeListBox.SelectedIndex = this._CodeSelection;
		private void StackListBox_SelectedIndexChanged(Object sender, EventArgs e) => this.StackListBox.SelectedIndex = -1;
	}
}
