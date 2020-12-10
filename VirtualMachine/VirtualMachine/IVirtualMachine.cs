using System;
using System.Collections;

namespace SVM.VirtualMachine
{
	/// <summary>
	/// Represents a virtual machine for compiling and executing interpreted code. This interface can be implemented for any stack-based instruction set.
	/// </summary>
	public interface IVirtualMachine
	{
		/// <summary>
		/// Gets the line of the execution currently being executed.
		/// </summary>
		/// <value>The line of the execution currently being executed.</value>
		/// <exception cref="InvalidOperationException">The code has not been compiled.</exception>
		int ProgramCounter { get; }

		/// <summary>
		/// Gets the non-generic stack used by the virtual machine instance.
		/// </summary>
		/// <value>The non-generic stack used by the virtual machine instance.</value>
		Stack Stack { get; }

		/// <summary>
		/// Instruct the <see cref="IVirtualMachine"/> to jump to the specified <c><paramref name="label"/></c>.
		/// </summary>
		/// <param name="label">The label to jump to.</param>
		/// <exception cref="SvmRuntimeException">The specified label could not be found.</exception>
		void Jump(String label);

		/// <summary>
		/// Pops an integer from the top of the <see cref="IVirtualMachine.Stack"/>.
		/// </summary>
		/// <returns>The integer popped from the top of the <see cref="IVirtualMachine.Stack"/>.</returns>
		/// <exception cref="InvalidCastException">The object at the top of the stack could not be represented as a 32-bit signed integer.</exception>
		/// <exception cref="InvalidOperationException">A stack underflow has occurred.</exception>
		int PopInt();
	}
}
