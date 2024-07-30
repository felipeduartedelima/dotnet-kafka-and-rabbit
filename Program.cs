using config;
using workers;
using core.interfaces;
using infrastructure.messageries;
using application.service;
using strategy;

var builder = Host.CreateApplicationBuilder(args);
// configs
IConfiguration config = builder.Configuration;
builder.Services.Configure<KafkaProducerConfig>(config.GetSection("Kafka:Producer"));
builder.Services.Configure<KafkaConsumerConfig>(config.GetSection("Kafka:Consumer"));
builder.Services.Configure<RabbitMQProducerConfig>(config.GetSection("RabbitMQ:Producer"));
builder.Services.Configure<RabbitMQConsumerConfig>(config.GetSection("RabbitMQ:Consumer"));
builder.Services.Configure<DatabaseConfig>(config.GetSection("Database"));

// Services
builder.Services.AddSingleton<KafkaProdCon>();
builder.Services.AddSingleton<RabbitMQProdCon>();
builder.Services.AddSingleton<DatabaseConnection>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

// Consumer Strategy
builder.Services.AddSingleton<ConsumerStrategy>(provider =>
{
    var defaultStrategy = provider.GetRequiredService<KafkaProdCon>();
    return new ConsumerStrategy(defaultStrategy);
});
builder.Services.AddSingleton<IDictionary<string, IConsumer>>(provider => new Dictionary<string, IConsumer>
{
    { "Kafka", provider.GetRequiredService<KafkaProdCon>() },
    { "RabbitMQ", provider.GetRequiredService<RabbitMQProdCon>() }
});
builder.Services.AddSingleton<IConsumerStrategyManager, ConsumerStrategyManager>();

// Provider Strategy
builder.Services.AddSingleton<ProducerStrategy>(provider =>
{
    var defaultStrategy = provider.GetRequiredService<KafkaProdCon>();
    return new ProducerStrategy(defaultStrategy);
});
builder.Services.AddSingleton<IDictionary<string, IProducer>>(provider => new Dictionary<string, IProducer>
{
    { "Kafka", provider.GetRequiredService<KafkaProdCon>() },
    { "RabbitMQ", provider.GetRequiredService<RabbitMQProdCon>() }
});
builder.Services.AddSingleton<IProducerStrategyManager, ProducerStrategyManager>();

// Application
builder.Services.AddSingleton<CreateFromConsumer>();

// Worker
builder.Services.AddHostedService<ConsumerWorker>();

var host = builder.Build();
host.Run();
