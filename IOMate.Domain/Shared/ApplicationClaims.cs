namespace IOMate.Domain.Shared
{
    public static class ApplicationClaims
    {
        public static readonly Dictionary<string, List<string>> ResourcesAndActions = new()
        {
            { "users", new List<string> { "read", "write", "delete" } },
            { "claims", new List<string> { "admin" } },
            { "events", new List<string> { "read" } }
        };

        public static bool IsValidClaim(string resource, string action)
        {
            return ResourcesAndActions.TryGetValue(resource, out var actions) &&
                   actions.Contains(action);
        }

        public static List<ResourceActionDto> GetAllResourceActions()
        {
            var result = new List<ResourceActionDto>();

            foreach (var resource in ResourcesAndActions)
            {
                foreach (var action in resource.Value)
                {
                    result.Add(new ResourceActionDto(resource.Key, action));
                }
            }

            return result;
        }
    }

    public record ResourceActionDto(string Resource, string Action)
    {
        public string PolicyName => $"{Resource}:{Action}";
    }
}
