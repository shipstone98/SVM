using System;
using System.IO;

using SVM.VirtualMachine;

namespace SML_Extensions
{
	public class LoadImage : BaseInstructionWithOperand
	{
		internal const String InvalidImageMessage = "The specified image file did not contain a valid image. ( at [line {0}] {1})";
		private const String FileNotFoundMessage = "The specified image file did not exist or could not be found. ( at [line {0}] {1})";

		public override void Run()
		{
			String fileName = this.Operands[0];
			byte[] rawData;

			try
			{
				rawData = File.ReadAllBytes(fileName);
			}

			catch
			{
				String err = String.Format(LoadImage.FileNotFoundMessage, this.ToString(), this.VirtualMachine.ProgramCounter);
				throw new SvmRuntimeException(err);
			}

			if (rawData.Length == 0)
			{
				String err = String.Format(LoadImage.InvalidImageMessage, this.ToString(), this.VirtualMachine.ProgramCounter);
				throw new SvmRuntimeException(err);
			}

			LoadedImage image = new LoadedImage(fileName, rawData);
			this.VirtualMachine.Stack.Push(image);
		}
	}
}
