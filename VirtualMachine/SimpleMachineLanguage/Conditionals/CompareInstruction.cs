using System;

using SVM.VirtualMachine;

namespace SVM.SimpleMachineLanguage.Conditionals
{
	public abstract class CompareInstruction : BaseInstructionWithOperand
	{
		protected delegate bool CompareMethod(int value, int operandValue);

		protected void Run(CompareInstruction.CompareMethod compar)
		{
			if (compar is null)
			{
				throw new ArgumentNullException(nameof (compar));
			}

			if (this.Operands is null)
			{
				throw new SvmRuntimeException();
			}

			int value = this.PopInt();
			this.VirtualMachine.Stack.Push(value);
			int operandValue;

			try
			{
				operandValue = Int32.Parse(this.Operands[0]);
			}

			catch
			{
				throw new SvmRuntimeException();
			}

			if (compar(value, operandValue))
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
