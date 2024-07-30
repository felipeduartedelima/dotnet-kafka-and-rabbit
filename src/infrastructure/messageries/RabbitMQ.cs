namespace infrastructure.messageries;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using config;
using core.interfaces;

public class RabbitMQProdCon : IProducer, IConsumer
{

    private readonly ILogger<RabbitMQProdCon> _logger;
    private readonly IConnection _producerConnection;
    private readonly IConnection _consumerConnection;
    public RabbitMQProdCon(
        IOptions<RabbitMQProducerConfig> producerConfig,
        IOptions<RabbitMQConsumerConfig> consumerConfig,
        ILogger<RabbitMQProdCon> logger
    )
    {
        _logger = logger;
        ConnectionFactory producerFactory = new ConnectionFactory();
        producerFactory.UserName = producerConfig.Value.UserName;
        producerFactory.Password = producerConfig.Value.Password;
        producerFactory.VirtualHost = producerConfig.Value.VirtualHost;
        producerFactory.HostName = producerConfig.Value.HostName;
        producerFactory.Port = producerConfig.Value.Port;
        _producerConnection = producerFactory.CreateConnection();
        ConnectionFactory consumerFactory = new ConnectionFactory();
        consumerFactory.UserName = consumerConfig.Value.UserName;
        consumerFactory.Password = consumerConfig.Value.Password;
        consumerFactory.VirtualHost = consumerConfig.Value.VirtualHost;
        consumerFactory.HostName = consumerConfig.Value.HostName;
        consumerFactory.Port = consumerConfig.Value.Port;
        _consumerConnection = consumerFactory.CreateConnection();
    }

    public async void Produce(string topic, string message)
    {
        using (var channel = _producerConnection.CreateModel())
        {
            try
            {
                channel.QueueDeclare(
                    queue: topic,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                var body = Encoding.UTF8.GetBytes(message);
                _logger.LogInformation($"Message [RabbitMQ] topic - {topic} at: {DateTimeOffset.Now}");
                channel.BasicPublish(
                    exchange: "",
                    routingKey: topic,
                    basicProperties: null,
                    body: body
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error [RabbitMQ] topic - {topic} at: {DateTimeOffset.Now}", ex);
                throw ex;
            }
        }

    }

    public async void Consume(string topic, ICallbackDelegate callback)
    {
        try
        {
            using (var channel = _consumerConnection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: topic,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    callback(message);
                    _logger.LogInformation($"Message read from RabbitMQ topic - {topic} at: {DateTimeOffset.Now}");
                };
                channel.BasicConsume(
                    queue: topic,
                    autoAck: true,
                    consumer: consumer
                );
                while (true)
                {
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during read message from RabbitMQ topic - {topic} at: {DateTimeOffset.Now}", ex);
            throw ex;
        }

    }
}