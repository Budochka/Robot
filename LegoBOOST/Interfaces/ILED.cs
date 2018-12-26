using System;
using LegoBOOST.Constants;

namespace LegoBOOST.Interfaces
{
    public interface ILED
    {
        void SetColor(Color color);

        event EventHandler OnColorChange;
    }
}