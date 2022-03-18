using System;
using System.Collections.Generic;
using System.Text.Json;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.OpenIddict.Authorizations;

public class OpenIddictAuthorization : AggregateRoot<Guid>
{
    public Guid? ApplicationId { get; protected set; }

    [NotNull]
    public Dictionary<string, JsonElement> Properties { get; protected set; }

    [NotNull]
    public HashSet<string> Scopes { get; protected set; }

    public string Status { get; protected set; }

    public string Subject { get; protected set; }

    public string Type { get; protected set; }

    public DateTime? CreationDate { get; protected set; }

    protected OpenIddictAuthorization() { }

    public OpenIddictAuthorization(Guid id)
    : base(id)
    {
        Properties = new();
        Scopes = new();
    }

    public void SetApplicationId(Guid? applicationId)
    {
        ApplicationId = applicationId;
    }

    public void SetProperties([NotNull] Dictionary<string, JsonElement> properties)
    {
        Check.NotNull(properties, nameof(properties));

        Properties = properties;
    }

    public void SetScopes([NotNull] HashSet<string> scopes)
    {
        Check.NotNull(scopes, nameof(scopes));

        Scopes = scopes;
    }

    public void SetStatus(string status)
    {
        Status = Check.Length(status, nameof(status), OpenIddictAuthorizationConst.StatusMaxLength);
    }

    public void SetSubject(string subject)
    {
        Subject = Check.Length(subject, nameof(subject), OpenIddictAuthorizationConst.SubjectMaxLength);
    }

    public void SetType(string type)
    {
        Type = Check.Length(type, nameof(type), OpenIddictAuthorizationConst.TypeMaxLength);
    }

    public void SetCreationDate(DateTime? date)
    {
        CreationDate = date;
    }
}