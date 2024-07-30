namespace config;
using Confluent.Kafka;

public class KafkaProducerConfig
{
    public string BootstrapServers { get; set; }

    public SaslMechanism SaslMechanism = SaslMechanism.ScramSha256;

    public SecurityProtocol SecurityProtocol = SecurityProtocol.SaslSsl;
    public string SaslUsername { get; set; }
    public string SaslPassword { get; set; }
}