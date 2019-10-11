using System;
using System.Collections.Generic;
using LibraProgramming.Ecs.Core.Reactive;

namespace ConsoleApp2.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageQueue : IDisposable
    {
        private readonly IDictionary<Type, object> messages;

        /// <summary>
        /// 
        /// </summary>
        public MessageQueue()
        {
            messages = new Dictionary<Type, object>();
        }

        public ISubject<TMessage> For<TMessage>()
            where TMessage : IMessage
        {
            var messageType = typeof(TMessage);

            if (false == messages.TryGetValue(messageType, out var subject))
            {
                subject = new Subject<TMessage>();
                messages.Add(messageType, subject);
            }

            return (ISubject<TMessage>) subject;
        }

        public void Dispose()
        {
            foreach (var subject in messages.Values)
            {
                ((IDisposable) subject).Dispose();
            }
        }
    }
}