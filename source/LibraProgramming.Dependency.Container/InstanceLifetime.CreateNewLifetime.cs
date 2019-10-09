﻿using System;
using System.Collections.Generic;

namespace LibraProgramming.Dependency.Container
{
    partial class InstanceLifetime
    {
        /// <summary>
        /// 
        /// </summary>
        public static Func<Factory, InstanceLifetime> CreateNew
        {
            get
            {
                return factory => new CreateNewLifetime(factory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class CreateNewLifetime : InstanceLifetime
        {
            public CreateNewLifetime(Factory factory)
                : base(factory)
            {
            }

            public override object ResolveInstance(Queue<ServiceTypeReference> queue)
            {
                return Factory.Create(queue);
            }
        }
    }
}