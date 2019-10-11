using System;
using System.Collections.Generic;

namespace LibraProgramming.Dependency.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceLocator : IServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetInstance(Type serviceType, string key = null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TService GetInstance<TService>(string key = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IEnumerable<object> GetInstances(Type serviceType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        object CreateInstance(Type serviceType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService CreateInstance<TService>() where TService : class;
    }
}