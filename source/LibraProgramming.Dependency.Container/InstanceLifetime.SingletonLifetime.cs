﻿using System;
using System.Collections.Generic;

namespace LibraProgramming.Dependency.Container
{
    partial class InstanceLifetime
    {
        /// <summary>
        /// 
        /// </summary>
        public static Func<Factory, InstanceLifetime> Singleton
        {
            get
            {
                return factory => new SingletonLifetime(factory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class SingletonLifetime : InstanceLifetime
        {
            private object instance;

            public SingletonLifetime(Factory factory)
                : base(factory)
            {
            }

            public override object ResolveInstance(Queue<ServiceTypeReference> queue)
            {
                return instance ??= Factory.Create(queue);
            }
        }
    }
}