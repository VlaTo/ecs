using System;
using System.IO;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IFile : IDisposable
    {
        Stream OpenRead();
    }
}