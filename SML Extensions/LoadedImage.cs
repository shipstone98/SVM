using System;
using System.Collections.Generic;
using System.IO;

namespace SML_Extensions
{
	public class LoadedImage : IDisposable
	{
		private bool _IsDisposed;
		private readonly String _ObjectName;
		private readonly byte[] _RawData;
		private readonly MemoryStream _Stream;

		public String FileName { get; }
		public Stream Stream => this._IsDisposed ? throw new ObjectDisposedException(this._ObjectName) : this._Stream;
		public IReadOnlyCollection<byte> RawData => this._RawData;

		internal LoadedImage(String fileName, byte[] rawData)
		{
			this._ObjectName = this.GetType().FullName;
			this._Stream = new MemoryStream();
			this._RawData = rawData;
			this.FileName = fileName;
			this._Stream.Write(rawData, 0, rawData.Length);
			this._Stream.Seek(0, SeekOrigin.Begin);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this._IsDisposed)
			{
				return;
			}

			if (disposing)
			{
				this._Stream.Dispose();
			}

			this._IsDisposed = true;
		}

		public long Reset() => this._IsDisposed ? throw new ObjectDisposedException(this._ObjectName) : this.Stream.Seek(0, SeekOrigin.Begin);

		public byte[] ToArray()
		{
			byte[] arr = new byte[this._RawData.Length];
			Array.Copy(this._RawData, arr, arr.Length);
			return arr;
		}
	}
}
