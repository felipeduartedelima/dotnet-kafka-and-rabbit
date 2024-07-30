using core.interfaces;

namespace strategy;

public interface IConsumerStrategyManager
{
    void ChangeStrategy(string strategyName);
    void ExecuteCurrentStrategy(string topic, ICallbackDelegate callback);
}

public class ConsumerStrategyManager: IConsumerStrategyManager
{
    private readonly ConsumerStrategy _context;
    private readonly IDictionary<string, IConsumer> _strategies;

    public ConsumerStrategyManager(ConsumerStrategy context, IDictionary<string, IConsumer> strategies)
    {
        _context = context;
        _strategies = strategies;
    }

    public void ChangeStrategy(string strategyName)
    {
        if(_strategies.TryGetValue(strategyName, out var strategy))
        {
            _context.SetStrategy(strategy);
        }
        else
        {
            throw new ArgumentException($"Strategy '{strategyName}' not found", nameof(strategyName));
        }
    }

    public void ExecuteCurrentStrategy(string topic, ICallbackDelegate callback)
    {
        _context.ExecuteStrategy(topic, callback);
    }

}
