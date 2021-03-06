﻿using System;

namespace CRUDApp.Helpers
{
    public interface IWindowService
    {
        event EventHandler<CanOpenWindowEventArgs> OnCanOpenWindowChanged;
        bool CanOpenWindow(string name);
        void OpenWindow(string name);
    }
}
