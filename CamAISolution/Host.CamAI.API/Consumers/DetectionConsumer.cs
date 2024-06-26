using System.Collections.Concurrent;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using Infrastructure.Observer.Messages;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_Detection", ConsumerConstant.Detection)]
public class DetectionConsumer(
    IAppLogging<DetectionConsumer> logger,
    IIncidentService incidentService,
    IBaseMapping mapper
) : IConsumer<ReceivedIncidentMessage>
{
    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks = new();

    public async Task Consume(ConsumeContext<ReceivedIncidentMessage> context)
    {
        var receivedIncident = context.Message;
        logger.Info(
            $"Receive new detection type {receivedIncident.IncidentType} from edge box {receivedIncident.EdgeBoxId}"
        );
        var @lock = Locks.GetOrAdd(receivedIncident.Id, _ => new SemaphoreSlim(1, 1));
        await @lock.WaitAsync();
        try
        {
            await incidentService.UpsertIncident(Map(receivedIncident));
        }
        catch (Exception) { }
        finally
        {
            @lock.Release();
        }
    }

    private CreateIncidentDto Map(ReceivedIncidentMessage receivedIncidentMessage)
    {
        return new CreateIncidentDto
        {
            EdgeBoxId = receivedIncidentMessage.EdgeBoxId,
            Id = receivedIncidentMessage.Id,
            AiId = receivedIncidentMessage.AiId,
            IncidentType = receivedIncidentMessage.IncidentType,
            StartTime = receivedIncidentMessage.StartTime,
            EndTime = receivedIncidentMessage.EndTime,
            Evidences = receivedIncidentMessage.Evidences.Select(Map).ToList()
        };
    }

    private CreateEvidenceDto Map(ReceivedEvidence receivedEvidence)
    {
        return new CreateEvidenceDto
        {
            Content = receivedEvidence.Content,
            EvidenceType = receivedEvidence.EvidenceType,
            Camera = mapper.Map<EdgeBoxCameraDto, Camera>(receivedEvidence.Camera)
        };
    }
}
