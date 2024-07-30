namespace application.service;
using core.entity;
using core.interfaces;
using strategy;
using System.Text.Json;
using utils;

public class CreateFromConsumer
{
    private readonly ILogger<CreateFromConsumer> _logger;
    private readonly IUserRepository _repository;
    private readonly IProducerStrategyManager _producer;

    public CreateFromConsumer(
        ILogger<CreateFromConsumer> logger,
        IUserRepository repository,
        IProducerStrategyManager producer
    )
    {
        _logger = logger;
        _repository = repository;
        _producer = producer;
    }
    public async void Execute(string message)
    {
        _logger.LogInformation($"Service [CreateFromConsumer] Started");
        _logger.LogInformation($"Service [CreateFromConsumer] {message}");
        var name = RandomCaracters.GenerateRandomString();
        var age = RandomCaracters.GenerateRandomInteger();
        var u = new User
        {
            Age = age,
            Name = name,
        };
        List<User> users = await _repository.CreateAndListUsersAsync(u);
        string jsonUsers = JsonSerializer.Serialize(users);
        _producer.ExecuteCurrentStrategy("test-topic", jsonUsers);
        _producer.ChangeStrategy("RabbitMQ");
        _producer.ExecuteCurrentStrategy("test-topic", jsonUsers);
        _producer.ChangeStrategy("Kafka");
        _logger.LogInformation($"Service [CreateFromConsumer] Ended");
    }
}
