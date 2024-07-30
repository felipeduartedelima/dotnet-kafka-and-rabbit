
using core.interfaces;

namespace strategy;

public class ProducerStrategy
{
    private IProducer _strategy;

    public ProducerStrategy(IProducer producer)
    {
        _strategy = producer;
    }

    public void SetStrategy(IProducer producer)
    {
        _strategy = producer;
    }

    public void ExecuteStrategy(string topic, string message)
    {
        _strategy.Produce(topic, message);
    }
}
