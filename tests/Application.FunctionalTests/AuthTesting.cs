using Buynamics.Toolkit.Domain.Units;
using Buynamics.Toolkit.Localization;
using Buynamics.Toolkit.Security.Models;
using PermissionsEnum = Buynamics.Toolkit.Security.Permissions;

namespace SortedTunes.Application.FunctionalTests;

[SetUpFixture]
public static partial class AuthTesting
{
    private static AuthUser s_currentUser = CreateUser();
    private static PermissionsEnum[] s_permissions = [];

    public static void ResetState()
    {
        s_currentUser = CreateUser();
        s_permissions = Enum.GetValues<PermissionsEnum>();
    }

    public static AuthUser GetCurrentUser()
    {
        return s_currentUser;
    }

    private static AuthUser CreateUser()
    {
        return new AuthUser()
        {
            AuthId = "auth0|test-user",
            CompanyId = 1,
            IsAdmin = true,
            CultureCode = BuynamicsCultureCode.en_US,
            DefaultCurrencyCode = "EUR",
            DefaultUnit = WeightUnits.KG
        };
    }

    public static void SetLoggedInUser(AuthUser user)
    {
        s_currentUser = user;
    }

    public static IList<PermissionsEnum> AddPermission(PermissionsEnum permission)
    {
        s_permissions = [
            permission
        ];
        return s_permissions;
    }

    public static IEnumerable<PermissionsEnum> GetPermissions()
    {
        return s_permissions;
    }
}
