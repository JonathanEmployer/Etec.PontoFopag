using System;

namespace RabbitMq.Interface
{
    public interface IRabbitMq
    {
        void SendMessage(string queue, string msg);
        void ReceiveMessage(Func<string, bool> result);
    }
}
