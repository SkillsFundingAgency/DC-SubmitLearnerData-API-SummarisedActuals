namespace ESFA.DC.PublicApi.FCS.Interfaces
{
    public interface IPagingHeader
    {
        int TotalItems { get; }

        int PageNumber { get; }

        int PageSize { get; }

        int TotalPages { get; }

        string ToJson();
    }
}
