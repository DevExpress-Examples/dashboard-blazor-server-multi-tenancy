using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuth {
    public static class AuthHelper {
        internal static string? GetUserName(AuthenticationStateProvider provider) {
            var stateTask = provider.GetAuthenticationStateAsync();

            if (!stateTask.IsCompletedSuccessfully)
                throw new InvalidOperationException("Authentication state is not ready.");

            var identity = stateTask.GetAwaiter().GetResult().User.Identity;

            return identity?.IsAuthenticated == true
                ? identity.Name?.Split('@')[0]
                : null;
        }
    }
}