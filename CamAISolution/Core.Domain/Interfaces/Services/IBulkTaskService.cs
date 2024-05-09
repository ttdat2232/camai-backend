using Core.Domain.DTO;

namespace Core.Domain.Interfaces.Services;

public interface IBulkTaskService
{
    void AddUpsertTask(Guid actorId, Task<BulkUpsertTaskResultResponse> task, string taskId);
    void GetTaskByActorId(Guid actorId, out List<string> taskIds);
    void GetTaskById(Guid actorId, string taskId, out Task<BulkUpsertTaskResultResponse> result);
    void RemoveTaskById(Guid actorId, string taskId);

    Task<BulkUpsertTaskResultResponse?> GetBulkUpsertTaskResultResponse(
        Guid actorId,
        string taskId,
        CancellationToken cancellationToken,
        TimeSpan timeout
    );
}
