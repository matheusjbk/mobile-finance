using MobileFinance.Application.UseCases.User.Delete.Delete;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MobileFinance.API.BackgroundServices;

public class DeleteUserService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public DeleteUserService(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = _configuration.GetValue<string>("Settings:RabbitMQ:HostName")! };
        using var connection = await factory.CreateConnectionAsync(cancellationToken: CancellationToken.None);
        using var channel = await connection.CreateChannelAsync(cancellationToken: CancellationToken.None);
        var queueName = _configuration.GetValue<string>("Settings:RabbitMQ:QueueName")!;

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: CancellationToken.None);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var userIdentifier = Guid.Parse(message);

                var scope = _serviceProvider.CreateScope();

                var useCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

                await useCase.Execute(userIdentifier);

                await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: CancellationToken.None);


        var taskCompletionSource = new TaskCompletionSource();
        stoppingToken.Register(() => taskCompletionSource.SetResult());
        await taskCompletionSource.Task;
    }

    ~DeleteUserService() => Dispose();

    public override void Dispose()
    {
        base.Dispose();

        GC.SuppressFinalize(this);
    }
}
