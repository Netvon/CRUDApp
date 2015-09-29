using System;

namespace CRUDApp.Helpers
{
    public class CanOpenWindowEventArgs : EventArgs
    {
        public string WindowName { get; }
        public bool CanOpen { get; }

        public CanOpenWindowEventArgs(string windowName, bool canOpen)
        {
            WindowName = windowName;
            CanOpen = canOpen;
        }
    }
}
