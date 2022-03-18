using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.OpenIddict.OpenIddictApplications;

namespace Volo.Abp.OpenIddict.Applications;

public class OpenIddictApplication : FullAuditedAggregateRoot<Guid>
{
    public string ClientId { get; protected set; }

    public string ClientSecret { get; protected set; }

    public string ConsentType { get; protected set; }

    public string DisplayName { get; protected set; }

    [NotNull]
    public Dictionary<string, string> DisplayNames { get; protected set; }

    [NotNull]
    public HashSet<string> Permissions { get; protected set; }

    [NotNull]
    public HashSet<string> PostLogoutRedirectUris { get; protected set; }

    [NotNull]
    public Dictionary<string, JsonElement> Properties { get; protected set; }

    [NotNull]
    public HashSet<string> RedirectUris { get; protected set; }

    [NotNull]
    public HashSet<string> Requirements { get; protected set; }

    public string Type { get; protected set; }

    protected OpenIddictApplication() { }

    public OpenIddictApplication(Guid id, string clientId)
    : base(id)
    {
        SetClientId(clientId);
        DisplayNames = new();
        Permissions = new();
        PostLogoutRedirectUris = new();
        Properties = new();
        RedirectUris = new();
        Requirements = new();
    }

    public void SetClientId([NotNull] string clientId)
    {
        ClientId = Check.NotNullOrWhiteSpace(clientId, nameof(clientId), OpenIddictApplicationConst.ClientIdMaxLength);
    }

    public void SetClientSecret(string secret)
    {
        ClientSecret = Check.Length(secret, nameof(secret), OpenIddictApplicationConst.ClientSecretMaxLength);
    }

    public void SetClientType(string type)
    {
        Type = Check.Length(type, nameof(type), OpenIddictApplicationConst.TypeMaxLength);
    }

    public void SetConsentType(string consentType)
    {
        ConsentType = Check.Length(consentType, nameof(consentType), OpenIddictApplicationConst.ConsentTypeMaxLength);
    }

    public void SetDisplayName(string displayName)
    {
        DisplayName = Check.Length(displayName, nameof(displayName), OpenIddictApplicationConst.DisplayNameMaxLength);
    }

    public void SetDisplayNames(Dictionary<string, string> displayNames)
    {
        Check.NotNull(displayNames, nameof(displayNames));

        DisplayNames = displayNames;
    }

    public void SetPermissions([NotNull] HashSet<string> permissions)
    {
        Check.NotNull(permissions, nameof(permissions));

        Permissions = permissions;
    }

    public void SetPostLogoutRedirectUris([NotNull] HashSet<string> addresses)
    {
        Check.NotNull(addresses, nameof(addresses));

        PostLogoutRedirectUris = addresses;
    }

    public void SetProperties([NotNull] Dictionary<string, JsonElement> properties)
    {
        Check.NotNull(properties, nameof(properties));

        Properties = properties;
    }

    public void SetRedirectUris([NotNull] HashSet<string> addresses)
    {
        Check.NotNull(addresses, nameof(addresses));

        RedirectUris = addresses;
    }

    public void SetRequirements([NotNull] HashSet<string> requirements)
    {
        Check.NotNull(requirements, nameof(requirements));

        Requirements = requirements;
    }
}