using System;
using System.Diagnostics;
using Abp.Zero.Configuration;

namespace Abp.AspNetZeroCore.Licensing
{
  public abstract class AspNetZeroBaseLicenseChecker
  {
    private readonly IAbpZeroConfig _abpZeroConfig;

    private string LicenseCode { get; }

    protected abstract string GetSalt();

    protected abstract string GetHashedValueWithoutUniqueComputerId(string str);

    protected AspNetZeroBaseLicenseChecker(
      AspNetZeroConfiguration configuration,
      IAbpZeroConfig abpZeroConfig,
      string configFilePath = "")
    {
      this._abpZeroConfig = abpZeroConfig;
      this.LicenseCode = configuration.LicenseCode;
    }

    protected string GetLicenseCode()
    {
      return this.LicenseCode;
    }

    protected bool CompareProjectName(string hashedProjectName)
    {
      string[] strArray = this.GetAssemblyName().Split('.', StringSplitOptions.None);
      for (int index1 = 0; index1 < strArray.Length; ++index1)
      {
        for (int index2 = 0; index2 <= index1; ++index2)
        {
          string str = strArray[index1];
          for (int index3 = index1 - 1; index3 > index1 - 1 - index2; --index3)
            str = strArray[index3] + "." + str;
          if (hashedProjectName == this.GetHashedValueWithoutUniqueComputerId(str))
            return true;
        }
      }
      return false;
    }

    protected string GetAssemblyName()
    {
      return this._abpZeroConfig.EntityTypes.User.Assembly.GetName().Name;
    }

    protected string GetLicenseController()
    {
      return "WebProject";
    }

    protected bool IsThereAReasonToNotCheck()
    {
      return !Debugger.IsAttached;
    }
  }
}