using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SVM.SimpleMachineLanguage.Conditionals;
using SVM.VirtualMachine;

namespace SVM_Test
{
	[TestClass]
	public class EquintTest
	{
		private const String Label = "labelOne";
		private const int Value = 30;

		private static readonly IDictionary<String, int> _Labels;

		private Equint _Instruction;
		private IVirtualMachine _VirtualMachine;

		static EquintTest()
		{
			EquintTest._Labels = new Dictionary<String, int>();
			EquintTest._Labels.Add(EquintTest.Label, -1);
		}

		[TestInitialize]
		public void Initialize()
		{
			this._Instruction = new Equint();
			this._Instruction.Operands = new String[2] { EquintTest.Value.ToString(), EquintTest.Label };
			this._VirtualMachine = this._Instruction.VirtualMachine = new FakeVirtualMachine(EquintTest._Labels);
		}

		[TestMethod]
		public void TestRun_TwoEqualIntegersOnStack()
		{
			this._VirtualMachine.Stack.Push(EquintTest.Value);
			this._Instruction.Run();
			Assert.AreEqual(this._VirtualMachine.ProgramCounter, this._VirtualMachine.Labels[EquintTest.Label] - 1); // As IVM.Run increments program counter every instruction, must subtract one from label in Equint.Run
		}

		[TestMethod]
		public void TestRun_TwoUnequalIntegersOnStack()
		{
			this._VirtualMachine.Stack.Push(EquintTest.Value + 1);
			this._Instruction.Run();
			Assert.AreNotEqual(this._VirtualMachine.ProgramCounter, this._VirtualMachine.Labels[EquintTest.Label] - 1);
		}

		[TestMethod]
		public void TestRun_StackUnderflow() => Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());

		[TestMethod]
		public void TestRun_IncorrectTypeOnStack()
		{
			this._VirtualMachine.Stack.Push(EquintTest.Label);
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
		}

		[TestMethod]
		public void TestRun_IncorrectOperands()
		{
			Assert.ThrowsException<SvmCompilationException>(() => this._Instruction.Operands = null);
			this._Instruction.Operands = new String[0];
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[1] { null };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[1] { EquintTest.Label };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[2] { EquintTest.Label, null };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[2] { EquintTest.Label, EquintTest.Label };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
		}
	}
}
