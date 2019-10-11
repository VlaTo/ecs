﻿using System;
using LibraProgramming.Ecs;

namespace ConsoleApp2.Components
{
    [Component(Alias = nameof(UpdateComponent))]
    public class UpdateComponent : Component
    {
        public TimeSpan Elapsed
        {
            get;
            set;
        }

        public UpdateComponent()
        {
            Elapsed = TimeSpan.Zero;
        }

        private UpdateComponent(UpdateComponent instance)
        {
            Elapsed = instance.Elapsed;
        }

        public override IComponent Clone()
        {
            return new UpdateComponent(this);
        }

        protected override void DoAttach()
        {
            ;
        }

        protected override void DoRelease()
        {
            ;
        }
    }
}