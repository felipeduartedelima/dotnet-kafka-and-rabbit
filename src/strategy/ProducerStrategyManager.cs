using core.interfaces;
using static Confluent.Kafka.ConfigPropertyNames;

namespace strategy;

public interface IProducerStrategyManager
{
    void ChangeStrategy(string strategyName);
    void ExecuteCurrentStrategy(string topic, string message);
}

public class ProducerStrategyManager : IProducerStrategyManager
{
    private readonly ProducerStrategy _context;
    private readonly IDictionary<string, IProducer> _strategies;

    public ProducerStrategyManager(ProducerStrategy context, IDictionary<string, IProducer> strategies)
    {
        _context = context;
        _strategies = strategies;
    }

    public void ChangeStrategy(string strategyName)
    {
        if (_strategies.TryGetValue(strategyName, out var strategy))
        {
            _context.SetStrategy(strategy);
        }
        else
        {
            throw new ArgumentException($"Strategy '{strategyName}' not found", nameof(strategyName));
        }
    }

    public void ExecuteCurrentStrategy(string topic, string message)
    {
        _context.ExecuteStrategy(topic, message);
    }

}
