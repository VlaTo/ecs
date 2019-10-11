using System;
using LibraProgramming.Ecs.Core.Reactive;

namespace ConsoleApp2.Core
{
    public sealed class GameRenderer : ObservableBase<IRenderContext>, IRenderer
    {
        public GameRenderer()
        {
        }
    }
}