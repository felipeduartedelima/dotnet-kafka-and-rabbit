namespace infrastructure.messageries;

using Confluent.Kafka;
using Microsoft.Extensions.Options;
using config;
using core.interfaces;

public class KafkaProdCon : IProducer, IConsumer
{
    private readonly ProducerConfig _producerConfig;
    private readonly ConsumerConfig _consumerConfig;
    private readonly ILogger<KafkaProdCon> _logger;

    public KafkaProdCon(
        IOptions<KafkaProducerConfig> producerConfig,
        IOptions<KafkaConsumerConfig> consumerConfig,
        ILogger<KafkaProdCon> logger
    )
    {
        _logger = logger;
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = producerConfig.Value.BootstrapServers,
            SaslMechanism = producerConfig.Value.SaslMechanism,
            SecurityProtocol = producerConfig.Value.SecurityProtocol,
            SaslUsername = producerConfig.Value.SaslUsername,
            SaslPassword = producerConfig.Value.SaslPassword,
        };
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = consumerConfig.Value.BootstrapServers,
            SaslMechanism = producerConfig.Value.SaslMechanism,
            SecurityProtocol = producerConfig.Value.SecurityProtocol,
            SaslUsername = producerConfig.Value.SaslUsername,
            SaslPassword = producerConfig.Value.SaslPassword,
            GroupId = consumerConfig.Value.GroupId,
            AutoOffsetReset = consumerConfig.Value.AutoOffsetReset
        };
    }
    public async void Produce(string topic, string message)
    {
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            var dr = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            _logger.LogInformation($"Message [Kafka] topic - {topic} at: {DateTimeOffset.Now}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error [Kafka] topic - {topic} at: {DateTimeOffset.Now}", ex);
            throw ex;
        }
    }

    public async void Consume(string topic, ICallbackDelegate callback)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(topic);
        while (true)
        {
            var cr = consumer.Consume(TimeSpan.FromSeconds(1));
            if (cr != null)
            {
                try
                {
                    callback(cr.Message.Value);
                    _logger.LogInformation($"Message read from Kafka topic - {topic} at: {DateTimeOffset.Now}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during read message from kafka topic - {topic} at: {DateTimeOffset.Now}", ex);
                    throw ex;
                }
            }
        }

    }

}
