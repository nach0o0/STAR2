namespace Shared.Application.Interfaces.Clients
{
    public interface IPermissionRegistrationClient
    {
        Task RegisterPermissionsAsync(
            IEnumerable<(string Id, string Description, List<string> PermittedScopeTypes)> permissions,
            CancellationToken cancellationToken = default);
    }
}
