﻿namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}