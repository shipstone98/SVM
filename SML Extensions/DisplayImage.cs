using System;

using SVM.VirtualMachine;

namespace SML_Extensions
{
	public class DisplayImage : BaseInstruction
	{
		public override void Run()
		{
			LoadedImage image;

			try
			{
				image = (LoadedImage) this.VirtualMachine.Stack.Pop();
			}

			catch (InvalidCastException)
			{
				String err = String.Format(BaseInstruction.OperandOfWrongTypeMessage, this.ToString(), this.VirtualMachine.ProgramCounter);
				throw new SvmRuntimeException(err);
			}

			catch (InvalidOperationException)
			{
				String err = String.Format(BaseInstruction.OperandOfWrongTypeMessage, this.ToString(), this.VirtualMachine.ProgramCounter);
				throw new SvmRuntimeException(err);
			}

			throw new NotImplementedException();
			//Console.WriteLine($"Image file {image.FileName} contains raw data of length {image.RawData.Count}: {System.Text.Encoding.Default.GetString(image.ToArray())}");
		}
	}
}
