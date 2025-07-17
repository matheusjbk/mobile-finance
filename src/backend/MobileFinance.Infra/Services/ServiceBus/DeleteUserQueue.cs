using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Services.ServiceBus;
using RabbitMQ.Client;
using System.Text;

namespace MobileFinance.Infra.Services.ServiceBus;
public class DeleteUserQueue : IDeleteUserQueue
{
    private readonly string _hostName;
    private readonly string _queueName;

    public DeleteUserQueue(string hostName, string queueName)
    {
        _hostName = hostName;
        _queueName = queueName;
    }

    public async Task SendMessage(User user)
    {
        var factory = new ConnectionFactory { HostName = _hostName };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var message = user.UserIdentifier.ToString();
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            body: body,
            mandatory: true,
            basicProperties: new BasicProperties());
    }
}
