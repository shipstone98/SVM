using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SVM.SimpleMachineLanguage.Conditionals;
using SVM.VirtualMachine;

namespace SVM_Test
{
	[TestClass]
	public class NotequTest
	{
		private const String Label = "labelOne";
		private const int Value = 30;

		private static readonly IDictionary<String, int> _Labels;

		private Notequ _Instruction;
		private IVirtualMachine _VirtualMachine;

		static NotequTest()
		{
			NotequTest._Labels = new Dictionary<String, int>();
			NotequTest._Labels.Add(NotequTest.Label, -1);
		}

		[TestInitialize]
		public void Initialize()
		{
			this._Instruction = new Notequ();
			this._Instruction.Operands = new String[1] { NotequTest.Label };
			this._VirtualMachine = this._Instruction.VirtualMachine = new FakeVirtualMachine(NotequTest._Labels);
		}

		[TestMethod]
		public void TestRun_TwoEqualIntegersOnStack()
		{
			const int N = 93;
			this._VirtualMachine.Stack.Push(N);
			this._VirtualMachine.Stack.Push(N);
			this._Instruction.Run();
			Assert.AreEqual(0, this._VirtualMachine.ProgramCounter);
		}

		[TestMethod]
		public void TestRun_TwoUnequalIntegersOnStack()
		{
			const int N = 93;
			this._VirtualMachine.Stack.Push(N);
			this._VirtualMachine.Stack.Push(N + 1);
			this._Instruction.Run();
			Assert.AreEqual(NotequTest.Value, this._VirtualMachine.ProgramCounter); // As IVM.Run increments program counter every instruction, must subtract one from label in Notequ.Run
		}

		[TestMethod]
		public void TestRun_TwoEqualObjectsOnStack()
		{
			const int N = 173;
			this._VirtualMachine.Stack.Push(N);
			this._VirtualMachine.Stack.Push(N.ToString());
			this._Instruction.Run();
			Assert.AreEqual(0, this._VirtualMachine.ProgramCounter);
		}

		[TestMethod]
		public void TestRun_TwoUnequalObjectsOnStack()
		{
			const int N = 173;
			this._VirtualMachine.Stack.Push(N);
			this._VirtualMachine.Stack.Push((N + 1).ToString());
			this._Instruction.Run();
			Assert.AreEqual(NotequTest.Value, this._VirtualMachine.ProgramCounter); // As IVM.Run increments program counter every instruction, must subtract one from label in Notequ.Run
		}

		[TestMethod]
		public void TestRun_StackUnderflow() => Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());

		[TestMethod]
		public void TestRun_IncorrectOperands()
		{
			Assert.ThrowsException<SvmCompilationException>(() => this._Instruction.Operands = null);
			this._Instruction.Operands = new String[0];
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[1] { null };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[1] { NotequTest.Label };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
		}
	}
}
