using Microsoft.AspNetCore.Authorization;

namespace WebApiAuthentication.Authorization;

public static class AuthorizationPolicies
{ 
    public const string MustHaveGoldSubscriptionAndBeOver21 = "MustHaveGoldSubscriptionAndBeOver21";
    
    public static AuthorizationPolicy MustBeGoldAndOlderThan21()
    {
        var policyBuilder = new AuthorizationPolicyBuilder();

        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireClaim("subscriptionlevel", "gold");
        policyBuilder.RequireAssertion(context =>
        {
            var ageClaim = context.User.FindFirst(c => c.Type == "age");
            if (ageClaim != null && int.TryParse(ageClaim.Value, out var age))
            {
                return age >= 21;
            }
            return false;
        });

        return policyBuilder.Build();
    }
}


