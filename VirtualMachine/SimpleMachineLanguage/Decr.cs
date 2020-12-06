namespace SVM.SimpleMachineLanguage
{
    #region Using directives
    using System;
    using SVM.VirtualMachine;
    #endregion
    /// <summary>
    /// Implements the SML Decr  instruction
    /// Decrements the integer value stored on top of the stack, 
    /// leaving the result on the stack
    /// </summary>
    public class Decr : BaseInstruction
    {
        #region TASK 3 - TO BE IMPLEMENTED BY THE STUDENT
		public override void Run()
		{
            int value = this.VirtualMachine.PopInt();

            if (value == Int32.MinValue)
			{
                throw new SvmRuntimeException(String.Format(BaseInstruction.BufferOverflowMessage,
                                                    this.ToString(), this.VirtualMachine.ProgramCounter));
			}

            -- value;
            this.VirtualMachine.Stack.Push(value);
		}
        #endregion
    }
}
