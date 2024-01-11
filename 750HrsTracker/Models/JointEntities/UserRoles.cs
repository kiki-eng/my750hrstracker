using Microsoft.AspNetCore.Identity;

namespace _750HrsTracker.Models.JointEntities
{
    public class UserRoles : IdentityUserRole<Guid>
    {
        public Guid? TeamId { get; set; }
    }

    public class UserClaims : IdentityUserClaim<Guid> { }
    public class RoleClaims : IdentityRoleClaim<Guid> { }
    public class UserLogins : IdentityUserLogin<Guid> { }
    public class UserTokens : IdentityUserToken<Guid> { }
}
