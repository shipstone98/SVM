using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM_Test
{
	[TestClass]
	public class IncrTest
	{
		private Incr _Instruction;
		private IVirtualMachine _VirtualMachine;

		[TestInitialize]
		public void Initialize()
		{
			this._Instruction = new Incr();
			this._Instruction.VirtualMachine = this._VirtualMachine = new FakeVirtualMachine();
		}

		[TestMethod]
		public void TestRun_EmptyStack() => Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());

		[TestMethod]
		public void TestRun_WrongTypeOfObjectOnStack()
		{
			this._VirtualMachine.Stack.Push(new Object());
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			Assert.AreEqual(0, this._VirtualMachine.Stack.Count);
		}

		[TestMethod]
		public void TestRun_CorrectTypeOfObjectOnStack_InRange()
		{
			const int X = 0;
			this._VirtualMachine.Stack.Push(X);
			this._Instruction.Run();
			Assert.AreEqual(1, this._VirtualMachine.Stack.Count);
			int x = this._VirtualMachine.PopInt();
			Assert.AreEqual(X + 1, x);
			this._VirtualMachine.Stack.Push(Int32.MaxValue - 1);
			this._Instruction.Run();
			x = this._VirtualMachine.PopInt();
			Assert.AreEqual(Int32.MaxValue, x);
			Assert.AreEqual(0, this._VirtualMachine.Stack.Count);
		}

		[TestMethod]
		public void TestRun_CorrectTypeOfObjectOnStack_OutOfRange()
		{
			this._VirtualMachine.Stack.Push(Int32.MaxValue);
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			Assert.AreEqual(0, this._VirtualMachine.Stack.Count);
		}
	}
}
