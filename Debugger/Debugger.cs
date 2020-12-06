using System;

using SVM.VirtualMachine;
using SVM.VirtualMachine.Debug;

namespace Debugger
{
    public class Debugger : IDebugger
    {
        #region TASK 5 - TO BE IMPLEMENTED BY THE STUDENT
        public IVirtualMachine VirtualMachine { private get; set; }

        public void Break(IDebugFrame debugFrame) => throw new NotImplementedException();
        #endregion
    }
}