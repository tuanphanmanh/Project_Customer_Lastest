namespace Abp.AspNetZeroCore.Licensing
{
  public class LicenseValidationResult
  {
    public bool Success { get; set; }
    public bool LastRequest { get; set; }
    public string ControlCode { get; set; }
  }
}