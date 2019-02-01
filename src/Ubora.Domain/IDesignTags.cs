namespace Ubora.Domain
{
    public interface ITagsAndKeywords
    {
        string ClinicalNeedTag { get; }
        string AreaOfUsageTag { get; }
        string PotentialTechnologyTag { get; }
        string Keywords { get; }
    }
}