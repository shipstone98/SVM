using System;

namespace SVM.VirtualMachine
{
	public class InvalidHashException : Exception
	{
		public String FileName { get; }

		internal InvalidHashException(String fileName) => this.FileName = fileName;
	}
}
