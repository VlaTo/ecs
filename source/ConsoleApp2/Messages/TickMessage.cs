using System;

namespace ConsoleApp2.Messages
{
    public class TickMessage : IMessage
    {
        public TimeSpan Elapsed
        {
            get;
        }

        public TickMessage(TimeSpan elapsed)
        {
            Elapsed = elapsed;
        }
    }
}