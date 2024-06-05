using System.Collections.Generic;

namespace esign.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyDefinitionAppService
    {
        List<string> GetAllAllowedInputTypeNames();

        List<string> GetAllEntities();
    }
}
