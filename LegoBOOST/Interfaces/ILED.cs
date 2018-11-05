using System;
using LegoBOOST.Constants;

namespace LegoBOOST.Interfaces
{
    public interface ILED
    {
        bool SetColor(Color color);

        event EventHandler OnColorChange;
    }
}