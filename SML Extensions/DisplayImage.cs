using System;
using System.Windows.Forms;

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

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ImageForm form;

			try
			{
				form = new ImageForm(image);
			}

			catch (ArgumentException)
			{
				String err = String.Format(LoadImage.InvalidImageMessage, this.ToString(), this.VirtualMachine.ProgramCounter);
				throw new SvmRuntimeException(err);
			}

			Application.Run(form);
			image.Dispose();
			image = null;
			//Console.WriteLine($"Image file {image.FileName} contains raw data of length {image.RawData.Count}: {System.Text.Encoding.Default.GetString(image.ToArray())}");
		}
	}
}
