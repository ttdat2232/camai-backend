using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Publishers;
using Core.Domain.Services;
using Host.CamAI.API.BackgroundServices;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_HealthCheckResponse", ConsumerConstant.HealthCheckResponse)]
public class HealthCheckResponseConsumer(
    IAppLogging<HealthCheckResponseConsumer> logger,
    ICacheService cache,
    IEdgeBoxInstallService edgeBoxInstallService,
    INotificationService notificationService,
    IMessageQueueService messageQueueService
) : IConsumer<HealthCheckResponseMessage>
{
    public async Task Consume(ConsumeContext<HealthCheckResponseMessage> context)
    {
        var message = context.Message;
        logger.Info($"Receive health check response from edge box {message.EdgeBoxId}");
        EdgeBoxHealthCheckService.ReceivedEdgeBoxHealthResponse(message.EdgeBoxId);
        var ebInstall = await edgeBoxInstallService.GetLatestInstallingByEdgeBox(message.EdgeBoxId);
        if (ebInstall == null)
        {
            logger.Info($"Edge box install not found for {message.EdgeBoxId}");
            return;
        }

        if (
            ebInstall.ActivationStatus == EdgeBoxActivationStatus.Failed
            && message.Status == EdgeBoxInstallStatus.Working
        )
        {
            await messageQueueService.Publish(
                new ActivatedEdgeBoxMessage
                {
                    Message = "Activate edge box",
                    RoutingKey = ebInstall.EdgeBoxId.ToString("N")
                }
            );
        }

        if (ebInstall.EdgeBoxInstallStatus == message.Status)
        {
            logger.Info($"Edge box install status not change {message.Status}");
            return;
        }

        await edgeBoxInstallService.UpdateStatus(ebInstall, message.Status, message.Reason);

        CreateNotificationDto dto;
        if (message.Status == EdgeBoxInstallStatus.Unhealthy)
        {
            dto = new CreateNotificationDto
            {
                Title = "Edge box is unhealthy failed",
                Content = $"Edge box does not response. Status changed to {EdgeBoxInstallStatus.Unhealthy}",
                Priority = NotificationPriority.Urgent,
                Type = NotificationType.EdgeBoxUnhealthy,
                RelatedEntityId = ebInstall.Id,
            };
        }
        else
        {
            // for healthy case
            dto = new CreateNotificationDto
            {
                // TODO: add retrying message
                Title = "Edge box is now connected to server",
                Content = "Edge box is now connected to server",
                Priority = NotificationPriority.Urgent,
                Type = NotificationType.EdgeBoxHealthy,
                RelatedEntityId = ebInstall.Id,
            };
            // TODO: try to reactivate edge box
        }

        var sentTo = new List<Guid> { await cache.GetAdminAccount() };
        if (ebInstall.Shop.ShopManagerId.HasValue)
            sentTo.Add(ebInstall.Shop.ShopManagerId.Value);
        dto.SentToId = sentTo;

        await notificationService.CreateNotification(dto);
    }
}
