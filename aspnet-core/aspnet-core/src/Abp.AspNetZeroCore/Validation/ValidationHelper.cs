using Abp.Extensions;
using System.Text.RegularExpressions;

namespace Abp.AspNetZeroCore.Validation
{
  public static class ValidationHelper
  {
    public const string EmailRegex = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

    public static bool IsEmail(string value)
    {
      return !value.IsNullOrEmpty() && new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$").IsMatch(value);
    }
  }
}