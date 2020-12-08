using System;

using SVM.VirtualMachine;

namespace SVM.SimpleMachineLanguage.Conditionals
{
	public class Notequ : BaseInstructionWithOperand
	{
		public override void Run()
		{
			if (this.Operands is null)
			{
				throw new SvmRuntimeException();
			}

			Object a, b;

			try
			{
				a = this.VirtualMachine.Stack.Pop();
			}

			catch (InvalidOperationException)
			{
				throw new SvmRuntimeException();
			}

			try
			{
				b = this.VirtualMachine.Stack.Pop();
			}

			catch (InvalidOperationException)
			{
				throw new SvmRuntimeException();
			}

			this.VirtualMachine.Stack.Push(b);
			this.VirtualMachine.Stack.Push(a);

			if (!Object.Equals(a, b))
			{
				int line;

				try
				{
					line = this.VirtualMachine.Labels[this.Operands[0]];
				}

				catch
				{
					throw new SvmRuntimeException();
				}

				this.VirtualMachine.ProgramCounter = line - 1;
			}
		}
	}
}
