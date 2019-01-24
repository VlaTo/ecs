using System;
using System.Collections.Generic;
using ClassLibrary1;
using ClassLibrary1.Core;

namespace ConsoleApp2.Core
{
    public class MessageQueue : IDisposable
    {
        private readonly IDictionary<Type, object> messages;

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