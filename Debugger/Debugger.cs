using System;
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
        private readonly Object _FormLock;
        private Thread _FormThread;

        public IVirtualMachine VirtualMachine { internal get; set; }

        public Debugger() => this._FormLock = new Object();

        public void Break(IDebugFrame debugFrame)
		{
            if (debugFrame is null)
			{
                return;
			}

            lock (this._FormLock)
			{
                if (this._FormThread is null)
				{
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    this._Form = new MainForm(this);
                    Application.Run(this._Form);
				}

                MethodInvoker invoker = delegate ()
                {
                    this._Form.Break(debugFrame);
                };

                this._Form.Invoke(invoker);

                while (this._AwaitingForm)
				{
                    continue;
				}
			}
		}
        #endregion
    }
}