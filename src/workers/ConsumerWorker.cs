namespace workers;
using core.interfaces;
using application.service;
using strategy;

public class ConsumerWorker : BackgroundService
{
    private readonly IConsumerStrategyManager _consumer;
    private readonly CreateFromConsumer _service;

    public ConsumerWorker(IConsumerStrategyManager consumer, CreateFromConsumer service)
    {
        _consumer = consumer;
        _service = service;
    }

    private void ProcessData(string message)
    {
        _service.Execute(message);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.ExecuteCurrentStrategy("test-topic", ProcessData);
    }
}