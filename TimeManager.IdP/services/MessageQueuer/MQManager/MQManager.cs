using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace TimeManager.IdP.services.MessageQueuer
{
    public class MQManager : IMQManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly BlockingCollection<bool> resp = new BlockingCollection<bool>();
        private IModel channel { get; }

        public MQManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
            channel = _objectPool.Get();
        }

        public bool Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            if (message == null) return false;

            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ReplyTo = exchangeName+"-reply";

                channel.BasicPublish(
                    exchangeName,
                    routeKey,
                    properties,
                    sendBytes
                    );
                var consumer = new EventingBasicConsumer(channel);


                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    string response = Encoding.UTF8.GetString(body);

                    bool result = bool.Parse(response);
                    resp.Add(result);
                };
                channel.BasicConsume(exchangeName+"-reply", false, consumer);

                return resp.Take();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
