using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SVM.SimpleMachineLanguage.Conditionals;
using SVM.VirtualMachine;

namespace SVM_Test
{
	[TestClass]
	public class BgrintTest
	{
		private const String Label = "labelOne";
		private const int Value = 30;

		private static readonly IDictionary<String, int> _Labels;

		private Bgrint _Instruction;
		private IVirtualMachine _VirtualMachine;

		static BgrintTest()
		{
			BgrintTest._Labels = new Dictionary<String, int>();
			BgrintTest._Labels.Add(BgrintTest.Label, BgrintTest.Value);
		}

		[TestInitialize]
		public void Initialize()
		{
			this._Instruction = new Bgrint();
			this._Instruction.Operands = new String[2] { BgrintTest.Value.ToString(), BgrintTest.Label };
			this._VirtualMachine = this._Instruction.VirtualMachine = new FakeVirtualMachine(BgrintTest._Labels);
		}

		[TestMethod]
		public void TestRun_LesserIntegerOnStack()
		{
			this._VirtualMachine.Stack.Push(BgrintTest.Value - 1);
			this._Instruction.Run();
			Assert.AreEqual(BgrintTest.Value, this._VirtualMachine.ProgramCounter);
		}

		[TestMethod]
		public void TestRun_EqualIntegerOnStack()
		{
			this._VirtualMachine.Stack.Push(BgrintTest.Value);
			this._Instruction.Run();
			Assert.AreEqual(0, this._VirtualMachine.ProgramCounter); // As IVM.Run increments program counter every instruction, must subtract one from label in Equint.Run
		}

		[TestMethod]
		public void TestRun_GreaterIntegerOnStack()
		{
			this._VirtualMachine.Stack.Push(BgrintTest.Value + 1);
			this._Instruction.Run();
			Assert.AreEqual(0, this._VirtualMachine.ProgramCounter);
		}

		[TestMethod]
		public void TestRun_StackUnderflow() => Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());

		[TestMethod]
		public void TestRun_IncorrectTypeOnStack()
		{
			this._VirtualMachine.Stack.Push(BgrintTest.Label);
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
			this._Instruction.Operands = new String[1] { BgrintTest.Label };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[2] { BgrintTest.Label, null };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
			this._Instruction.Operands = new String[2] { BgrintTest.Label, BgrintTest.Label };
			Assert.ThrowsException<SvmRuntimeException>(() => this._Instruction.Run());
		}
	}
}
