namespace config;
using Confluent.Kafka;

public class KafkaConsumerConfig
{
    public string BootstrapServers { get; set; }

    public SaslMechanism SaslMechanism = SaslMechanism.ScramSha256;

    public SecurityProtocol SecurityProtocol = SecurityProtocol.SaslSsl;
    public string SaslUsername { get; set; }
    public string SaslPassword { get; set; }
    public string GroupId { get; set; }

    public AutoOffsetReset AutoOffsetReset = AutoOffsetReset.Earliest;
}