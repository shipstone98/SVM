using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using SVM.VirtualMachine;
using SVM.VirtualMachine.Debug;

namespace Debugger
{
    public class Debugger : IDebugger
    {
        #region TASK 5 - TO BE IMPLEMENTED BY THE STUDENT
        internal bool _AwaitingForm;
        private MainForm _Form;
        private Thread _FormThread;
        private readonly Object _LockObject;
        internal IVirtualMachine _VirtualMachine;

        public IVirtualMachine VirtualMachine
        {
            set
			{
                lock (this._LockObject)
				{
                    this._VirtualMachine = value ?? throw new ArgumentNullException(nameof (value));
                }
			}
		}

        public Debugger() => this._LockObject = new Object();

        public void Break(IDebugFrame debugFrame)
		{
            if (debugFrame is null || !(this._FormThread is null || this._FormThread.IsAlive))
			{
                return;
			}

            lock (this._LockObject)
			{
                if (this._VirtualMachine is null)
				{
                    throw new InvalidOperationException();
				}

                if (this._FormThread is null)
				{
                    this._FormThread = new Thread(() =>
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        this._Form = new MainForm(this);
                        Application.Run(this._Form);
                    });

                    this._FormThread.IsBackground = true;
                    this._FormThread.Start();
				}

                while (this._Form is null || !this._Form.Visible)
				{
                    continue;
				}

                MethodInvoker invoker = delegate ()
                {
                    this._Form.Break(debugFrame);
                };

                try
                {
                    this._Form.Invoke(invoker);
                }

                catch (InvalidAsynchronousStateException)
				{
                    return;
				}

                this._AwaitingForm = true;

                while (this._AwaitingForm)
                {
                    continue;
                }
			}
		}
        #endregion
    }
}