using System;

namespace ConsoleApp2.Core
{
    public interface IGameTimer : IObservable<TimeSpan>
    {
    }
}