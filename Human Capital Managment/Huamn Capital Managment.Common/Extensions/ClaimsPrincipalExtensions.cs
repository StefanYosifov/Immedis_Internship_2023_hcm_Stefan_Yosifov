namespace Huamn_Capital_Managment.Common.Extensions
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal user)
        { 
              return user
                    .FindFirst(ClaimTypes.NameIdentifier)!
                    .Value;
        }

        //public static bool IsManager(this ClaimsPrincipal user)
        //{
        //    return user.IsInRole("");
        //}
    }
}
