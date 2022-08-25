// ReSharper disable once CheckNamespace

namespace JobLister;

public class JobModel
{
    public string? JobId { get; set; }
    public int? CreatedAt { get; set; }
    public string? UploadLink { get; set; }
    public string? DownloadLink { get; set; }
    public string? Status { get; set; }
}