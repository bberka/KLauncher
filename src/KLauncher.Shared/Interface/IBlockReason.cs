namespace KLauncher.Shared.Interface;

public interface IBlockReason
{
    public DateTime BlockEndDate { get; set; }
    public string Reason { get; set; }
}