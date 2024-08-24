namespace TRINET_CORE.Modules
{
    public enum ERequestStatus
    {
        UNDEFINED,
        FAILED,
        PENDING,
        SENT
    }

    public struct TrinetModuleRequestResult
    {
        public ERequestStatus Status { get; set; }

        public string? Message { get; set; }
    }

    public abstract class ModuleBase
    {
    }
}
