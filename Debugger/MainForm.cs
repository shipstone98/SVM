using System;
using System.Text;
using System.Windows.Forms;

using SVM.VirtualMachine.Debug;
using SVM.VirtualMachine;

namespace Debugger
{
	public partial class MainForm : Form
	{
		private readonly Debugger Debugger;

		public MainForm() => this.InitializeComponent();
		public MainForm(Debugger debugger) : this() => this.Debugger = debugger;

		internal void Break(IDebugFrame debugFrame)
		{
			this.SuspendLayout();
			StringBuilder code = new StringBuilder(), stack = new StringBuilder();

			foreach (IInstruction instruction in debugFrame.CodeFrame)
			{
				code.AppendLine(instruction.ToString());
			}

			foreach (Object obj in this.Debugger.VirtualMachine.Stack)
			{
				stack.AppendLine(obj.ToString());
			}

			this.CodeTextBox.Text = code.ToString();
			this.StackTextBox.Text = stack.ToString();
			this.ContinueButton.Enabled = true;
			this.ResumeLayout();
		}

		private void ContinueButton_Click(Object sender, EventArgs e)
		{
			this.ContinueButton.Enabled = false;
			this.Debugger._AwaitingForm = false;
		}
	}
}
