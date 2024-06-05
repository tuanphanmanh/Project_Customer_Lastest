using System.Collections.Generic;
using Abp.Authorization;
using Abp.DynamicEntityProperties;
using esign.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace esign.DynamicEntityProperties.Ver1
{
    [AbpAuthorize(AppPermissions.Pages_DynamicEntityPropertyDefinition)]
    public class DynamicEntityPropertyDefinitionAppService : esignVersion1AppServiceBase, IDynamicEntityPropertyDefinitionAppService
    {
        private readonly IDynamicEntityPropertyDefinitionManager _dynamicEntityPropertyDefinitionManager;

        public DynamicEntityPropertyDefinitionAppService(IDynamicEntityPropertyDefinitionManager dynamicEntityPropertyDefinitionManager)
        {
            _dynamicEntityPropertyDefinitionManager = dynamicEntityPropertyDefinitionManager;
        }

        [HttpGet]
        public List<string> GetAllAllowedInputTypeNames()
        {
            return _dynamicEntityPropertyDefinitionManager.GetAllAllowedInputTypeNames();
        }

        [HttpGet]
        public List<string> GetAllEntities()
        {
            return _dynamicEntityPropertyDefinitionManager.GetAllEntities();
        }
    }
}
