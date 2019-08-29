using System;
using System.Threading.Tasks;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public interface ILED
    {
        void SetColor(Color color);
        Task SetColorAsync(Color color);

        event EventHandler OnColorChanged;
    }
}