using System;

namespace Abp.AspNetZeroCore.Licensing
{
  public class DecryptedCheckCodeInfo
  {
    public bool Succeed { get; set; }
    public DateTime CheckTime { get; set; }
  }
}