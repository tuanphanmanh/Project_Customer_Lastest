using System;
using Abp.Dependency;

namespace Abp.AspNetZeroCore.Timing
{
  public class AppTimes : ISingletonDependency
  {
    public DateTime StartupTime { get; set; }
  }
}