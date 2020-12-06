using System.Collections;

using SVM.VirtualMachine;

namespace SVM_Test
{
	internal class FakeVirtualMachine : IVirtualMachine
	{
		public int ProgramCounter => 0;
		public Stack Stack { get; }

		internal FakeVirtualMachine() => this.Stack = new Stack();

		public int PopInt() => (int) this.Stack.Pop();
	}
}
