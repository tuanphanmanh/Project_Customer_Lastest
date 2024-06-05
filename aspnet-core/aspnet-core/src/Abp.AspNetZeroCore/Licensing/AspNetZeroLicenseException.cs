using System;

namespace Abp.AspNetZeroCore.Licensing
{
  public class AspNetZeroLicenseException : Exception
  {
    public AspNetZeroLicenseException()
      : base("AspNet Zero License Check Failed. Please contact to administrator if you are using a licensed version!")
    {
    }

    public AspNetZeroLicenseException(string message)
      : base(message)
    {
    }
  }
}