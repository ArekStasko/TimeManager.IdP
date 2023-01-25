using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TimeManager.IdP.services.MessageQueuer
{
    public class MQModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection _connection;

        public MQModelPooledObjectPolicy() => _connection = GetConnection();

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();   
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen) return true;

            obj.Dispose();
            return false;
        }
    }
}
