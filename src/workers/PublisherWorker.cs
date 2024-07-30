namespace workers;
using core.interfaces;
using strategy;

public class ProducerWorker : BackgroundService
{
    private readonly ProducerStrategyManager _producer;

    public ProducerWorker(ProducerStrategyManager producer)
    {
        _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _producer.ExecuteCurrentStrategy("test-topic", "My Example");
    }
}