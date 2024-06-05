using System.Collections.Generic;
using MvvmHelpers;
using prod.Models.NavigationMenu;

namespace prod.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}