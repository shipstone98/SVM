using System;
using System.Collections;
using System.Collections.Generic;

using SVM.VirtualMachine;

namespace SVM_Test
{
	internal class FakeVirtualMachine : IVirtualMachine
	{
		private readonly Dictionary<String, int> _Labels;

		public int ProgramCounter { get; private set; }
		public Stack Stack { get; }

		internal FakeVirtualMachine()
		{
			this._Labels = new Dictionary<String, int>();
			this.Stack = new Stack();
		}

		internal FakeVirtualMachine(IEnumerable<KeyValuePair<String, int>> labels) : this()
		{
			foreach (KeyValuePair<String, int> label in labels)
			{
				this._Labels.Add(label.Key, label.Value);
			}
		}

		public void Jump(String label) => this.ProgramCounter = this._Labels[label];
		public int PopInt() => (int) this.Stack.Pop();
	}
}
