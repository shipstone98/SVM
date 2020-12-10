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

			if (!Notequ.Equals(a, b))
			{
				try
				{
					this.VirtualMachine.Jump(this.Operands[0]);
				}

				catch
				{
					throw new SvmRuntimeException();
				}
			}
		}

		private new static bool Equals(Object a, Object b)
		{
			if (Object.Equals(a, b))
			{
				return true;
			}

			if (a is null || b is null)
			{
				return false;
			}

			Type aType = a.GetType(), bType = b.GetType(), intType = typeof (int), stringType = typeof (String);

			if (aType.Equals(intType))
			{
				return !Int32.TryParse(b.ToString(), out int bValue) ? false : (int) a == bValue;
			}

			else
			{
				return !Int32.TryParse(a.ToString(), out int aValue) ? false : (int) b == aValue;
			}
		}
	}
}
