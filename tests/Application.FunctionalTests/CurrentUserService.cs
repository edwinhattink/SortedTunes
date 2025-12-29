using Buynamics.Toolkit.Security;
using Buynamics.Toolkit.Security.Interfaces;
using Buynamics.Toolkit.Security.Models;

namespace SortedTunes.Application.FunctionalTests;

public class CurrentUserService : ICurrentUserService
{
    public bool IsLoggedIn
    {
        get
        {
            return !string.IsNullOrEmpty(GetUser().AuthId);
        }
    }

    public AuthUser GetUser() => AuthTesting.GetCurrentUser();

    public bool HasPermission(Permissions permission)
    {
        return AuthTesting.GetPermissions().Contains(permission);
    }
}
