namespace api.Models
{
    public enum DocumentStatus
    {
        ReceivedBySecretary,
        SentToDirector,
        SentToExecutor,
        InProgress,
        ReturnedToDirector,
        Approved,
        ReturnedForRevision
    }

    public enum DocumentRouteAction
    {
        Forward,
        ReturnForRevision,
        Approve
    }
}
