using System;
using System.Collections.Generic;

namespace SML_Extensions
{
	public class LoadedImage
	{
		private readonly byte[] _RawData;

		public String FileName { get; }
		public IReadOnlyCollection<byte> RawData => this._RawData;

		internal LoadedImage(String fileName, byte[] rawData)
		{
			this._RawData = rawData;
			this.FileName = fileName;
		}

		public byte[] ToArray()
		{
			byte[] arr = new byte[this._RawData.Length];
			Array.Copy(this._RawData, arr, arr.Length);
			return arr;
		}
	}
}
