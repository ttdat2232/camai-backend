using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;
using RabbitMQ.Client;

namespace Host.CamAI.API.Consumers;

[Consumer(
    ConsumerConstant.ConfirmedActivation,
    ConsumerConstant.ConfirmedActivation,
    exchangeType: ExchangeType.Fanout
)]
public class ConfirmedEdgeBoxActivationConsumer(
    IAppLogging<ConfirmedEdgeBoxActivationConsumer> logger,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    INotificationService notificationService,
    IJwtService jwtService
) : IConsumer<ConfirmedEdgeBoxActivationMessage>
{
    public async Task Consume(ConsumeContext<ConfirmedEdgeBoxActivationMessage> context)
    {
        logger.Info($"Confirmed activation message from edge box ID: {context.Message.EdgeBoxId}.");
        var edgeBoxInstall = (await edgeBoxInstallService.GetLatestInstallingByEdgeBox(context.Message.EdgeBoxId))!;
        if (edgeBoxInstall.ActivationStatus == EdgeBoxActivationStatus.Activated)
            return;
        edgeBoxInstall.ActivationStatus = EdgeBoxActivationStatus.Activated;
        unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
        try
        {
            if (await unitOfWork.CompleteAsync() > 0)
            {
                var brandManagerAccountIds = (
                    await unitOfWork.Accounts.GetAsync(
                        a =>
                            a.Role == Role.BrandManager
                            && a.ManagingShop != null
                            && a.ManagingShop.Id == edgeBoxInstall.ShopId,
                        takeAll: true
                    )
                ).Values.Select(a => a.Id);

                await SendNotification(
                    context.Message.IsActivatedSuccessfully,
                    context.Message.EdgeBoxId,
                    brandManagerAccountIds
                );
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    private Task SendNotification(bool isActivatedSuccessfully, Guid edgeBoxId, IEnumerable<Guid> sendToAccountIds)
    {
        var content = $"Edge box {edgeBoxId} is activated";
        var title = "Edge box is activated";
        var notificationType = NotificationType.EdgeBoxInstallActivation;
        var priority = NotificationPriority.Normal;
        if (!isActivatedSuccessfully)
        {
            content = $"Edge box {edgeBoxId} cannot be activated";
            title = "Edge box cannot be activated";
            priority = NotificationPriority.Urgent;
        }

        notificationService.CreateNotification(
            new CreateNotificationDto
            {
                Content = content,
                Title = title,
                Type = notificationType,
                Priority = priority,
                SentToId = sendToAccountIds
            },
            true
        );
        return Task.CompletedTask;
    }
}
