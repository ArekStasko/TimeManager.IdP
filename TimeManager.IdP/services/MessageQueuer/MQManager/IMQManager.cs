using RabbitMQ.Client;

namespace TimeManager.IdP.services.MessageQueuer
{
    public interface IMQManager
    {
        public bool Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
    }
}
