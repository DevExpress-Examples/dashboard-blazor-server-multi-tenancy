using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuth {
    public interface IDashboardUserProvider {
        Task<string?> GetUserNameAsync();
        string? GetUserName();
    }

    public class DashboardUserProvider : IDashboardUserProvider {
        readonly AuthenticationStateProvider authProvider;

        public DashboardUserProvider(AuthenticationStateProvider provider) {
            this.authProvider = provider;
        }

        public string? GetUserName() {
            var stateTask = authProvider.GetAuthenticationStateAsync();
            if(!stateTask.IsCompletedSuccessfully)
                throw new InvalidOperationException("Authentication state is not ready.");

            return ReadName(stateTask.Result);
        }

        public async Task<string?> GetUserNameAsync() {
            var state = await authProvider.GetAuthenticationStateAsync();

            return ReadName(state);
        }

        static string? ReadName(AuthenticationState state) {
            var identity = state.User.Identity;
            return identity?.IsAuthenticated == true ? identity.Name?.Split('@')[0] : null;
        }
    }
}