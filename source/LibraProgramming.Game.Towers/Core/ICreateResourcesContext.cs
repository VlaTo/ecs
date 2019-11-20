using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Game.Towers.Core
{
    public interface ICreateResourcesContext
    {
        ICanvasResourceCreatorWithDpi Creator
        {
            get;
        }

        Size CanvasSize
        {
            get;
        }
    }
}