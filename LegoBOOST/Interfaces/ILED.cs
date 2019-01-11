using System;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public interface ILED
    {
        void SetColor(Color color);
        void SetColorAsync(Color color);

        event EventHandler OnColorChanged;
    }
}