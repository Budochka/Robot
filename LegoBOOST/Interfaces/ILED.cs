using System;
using LegoBOOST.Constants;

namespace LegoBOOST.Interfaces
{
    public interface ILED
    {
        OperationResult SetColor(Color color);

        event EventHandler OnColorChange;
    }
}