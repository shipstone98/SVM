using System;

using SVM.VirtualMachine;

namespace SVM.SimpleMachineLanguage.Conditionals
{
	public class Equint : BaseInstructionWithOperand
	{
		public override void Run()
		{
			if (this.Operands is null)
			{
				throw new SvmRuntimeException();
			}

			int value = this.PopInt();
			this.VirtualMachine.Stack.Push(value);
			int compar;

			try
			{
				compar = Int32.Parse(this.Operands[0]);
			}

			catch
			{
				throw new SvmRuntimeException();
			}

			if (value == compar)
			{
				int line;

				try
				{
					line = this.VirtualMachine.Labels[this.Operands[1]];
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
