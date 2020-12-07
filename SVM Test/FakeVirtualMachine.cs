using System;
using System.Collections;
using System.Collections.Generic;

using SVM.VirtualMachine;

namespace SVM_Test
{
	internal class FakeVirtualMachine : IVirtualMachine
	{
		public IReadOnlyDictionary<String, int> Labels => throw new NotImplementedException();
		public int ProgramCounter { get; set; }
		public Stack Stack { get; }

		internal FakeVirtualMachine() => this.Stack = new Stack();

		public int PopInt() => (int) this.Stack.Pop();
	}
}
