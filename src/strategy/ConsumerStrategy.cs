
using core.interfaces;

namespace strategy;

public class ConsumerStrategy
{
    private IConsumer _strategy;

    public ConsumerStrategy(IConsumer consumer)
    {
        _strategy = consumer;
    }

    public void SetStrategy(IConsumer consumer)
    {
        _strategy = consumer;
    }

    public void ExecuteStrategy(string topic, ICallbackDelegate callback)
    {
        _strategy.Consume(topic, callback);
    }
}
