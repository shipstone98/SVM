using System.Collections.Generic;

namespace SVM.VirtualMachine.Debug
{
	internal class DebugFrame : IDebugFrame
	{
		public List<IInstruction> CodeFrame { get; }
		public IInstruction CurrentInstruction { get; }

		/// <summary>
		/// WARNING!!! Makes shallow copy of list.
		/// </summary>
		/// <param name="currentInstruction"></param>
		/// <param name="codeFrame"></param>
		internal DebugFrame(IInstruction currentInstruction, List<IInstruction> codeFrame)
		{
			this.CodeFrame = codeFrame;
			this.CurrentInstruction = currentInstruction;
		}

		internal DebugFrame(IInstruction currentInstruction, IEnumerable<IInstruction> codeFrame)
		{
			this.CodeFrame = new List<IInstruction>(codeFrame);
			this.CurrentInstruction = currentInstruction;
		}
	}
}
