using System;

namespace LibraProgramming.Ecs.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDependencyProvider : IServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemType"></param>
        /// <returns></returns>
        ISystem CreateSystem(Type systemType);
    }
}