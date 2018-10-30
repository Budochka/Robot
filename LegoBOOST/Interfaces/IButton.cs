using System;

namespace LegoBOOST.Interfaces
{
    public interface IButton
    {
        event EventHandler OnButtonPressed;
        event EventHandler OnButtonReleased;
    }
}