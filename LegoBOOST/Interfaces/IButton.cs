using System;

namespace LegoBOOSTNet.Interfaces
{
    public interface IButton
    {
        event EventHandler OnButtonPressed;
        event EventHandler OnButtonReleased;
    }
}