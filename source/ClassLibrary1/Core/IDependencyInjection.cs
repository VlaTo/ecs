using System;

namespace ClassLibrary1.Core
{
    public interface IDependencyInjection : IServiceProvider
    {
        void Register(Type service);
    }
}