using System;
using System.Collections.Generic;
using System.Text.Json;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.OpenIddict.Scopes;

public class OpenIddictScope : AggregateRoot<Guid>
{
    public string Description { get; protected set; }

    [NotNull]
    public Dictionary<string, string> Descriptions { get; protected set; }

    public string DisplayName { get; protected set; }

    [NotNull]
    public Dictionary<string, string> DisplayNames { get; protected set; }

    public string Name { get; protected set; }

    [NotNull]
    public Dictionary<string, JsonElement> Properties { get; protected set; }

    [NotNull]
    public HashSet<string> Resources { get; protected set; }

    protected OpenIddictScope() { }

    public OpenIddictScope(Guid id) : base(id)
    {
        Descriptions = new();
        DisplayNames = new();
        Properties = new();
    }

    public void SetDescription(string description)
    {
        Description = Check.Length(description, nameof(description), OpenIddictScopeConst.DescriptionMaxLength);
    }

    public void SetDisplayName(string displayName)
    {
        DisplayName = Check.Length(displayName, nameof(displayName), OpenIddictScopeConst.DisplayNameMaxLength);
    }

    public void SetName(string name)
    {
        Name = Check.Length(name, nameof(name), OpenIddictScopeConst.NameMaxLength);
    }

    public void SetDisplayNames([NotNull] Dictionary<string, string> names)
    {
        Check.NotNull(names, nameof(names));

        DisplayNames = names;
    }

    public void SetDescriptions([NotNull] Dictionary<string, string> descriptions)
    {
        Check.NotNull(descriptions, nameof(descriptions));

        Descriptions = descriptions;
    }

    public void SetResources([NotNull] HashSet<string> resources)
    {
        Check.NotNull(resources, nameof(resources));

        Resources = resources;
    }

    public void SetProperties([NotNull] Dictionary<string, JsonElement> properties)
    {
        Check.NotNull(properties, nameof(properties));

        Properties = properties;
    }
}