using System;
using System.Collections;
using System.Collections.Generic;

using SVM.VirtualMachine;

namespace SVM_Test
{
	internal class FakeVirtualMachine : IVirtualMachine
	{
		private readonly Dictionary<String, int> _Labels;

		public IReadOnlyDictionary<String, int> Labels => this._Labels;
		public int ProgramCounter { get; set; }
		public Stack Stack { get; }

		internal FakeVirtualMachine()
		{
			this._Labels = new Dictionary<String, int>();
			this.Stack = new Stack();
		}

		internal FakeVirtualMachine(IDictionary<String, int> labels) : this()
		{
			foreach (KeyValuePair<String, int> label in labels)
			{
				this._Labels.Add(label.Key, label.Value);
			}
		}

		public void Jump() => throw new NotImplementedException();
		public int PopInt() => (int) this.Stack.Pop();
	}
}
