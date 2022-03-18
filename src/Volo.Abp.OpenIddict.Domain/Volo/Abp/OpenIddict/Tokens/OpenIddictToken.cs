using System;
using System.Collections.Generic;
using System.Text.Json;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.OpenIddict.Tokens;

public class OpenIddictToken : AggregateRoot<Guid>
{
    public Guid? AuthorizationId { get; protected set; }

    public Guid? ApplicationId { get; protected set; }

    public DateTime? CreationDate { get; protected set; }

    public DateTime? ExpirationDate { get; protected set; }

    public string Payload { get; protected set; }

    [NotNull]
    public Dictionary<string, JsonElement> Properties { get; protected set; }

    public DateTime? RedemptionDate { get; protected set; }

    public string ReferenceId { get; protected set; }

    public string Status { get; protected set; }

    public string Subject { get; protected set; }

    public string Type { get; protected set; }

    protected OpenIddictToken() { }

    public OpenIddictToken(Guid id) : base(id)
    {
        Properties = new();
    }

    public void SetAuthorizationId(Guid? authorizationId)
    {
        AuthorizationId = authorizationId;
    }

    public void SetApplicationId(Guid? applicationId)
    {
        ApplicationId = applicationId;
    }

    public void SetCreationDate(DateTime? date)
    {
        CreationDate = date;
    }

    public void SetExpirationDate(DateTime? date)
    {
        ExpirationDate = date;
    }

    public void SetPayload(string payload)
    {
        Payload = payload;
    }

    public void SetProperties([NotNull] Dictionary<string, JsonElement> properties)
    {
        Check.NotNull(properties, nameof(properties));

        Properties = properties;
    }

    public void SetRedemptionDate(DateTime? date)
    {
        RedemptionDate = date;
    }

    public void SetReferenceId(string referenceId)
    {
        ReferenceId = Check.Length(referenceId, nameof(referenceId), OpenIddictTokenConst.ReferenceIdMaxLength);
    }

    public void SetStatus(string status)
    {
        Status = Check.Length(status, nameof(status), OpenIddictTokenConst.StatusMaxLength);
    }

    public void SetSubject(string subject)
    {
        Subject = Check.Length(subject, nameof(subject), OpenIddictTokenConst.SubjectMaxLength);
    }

    public void SetType(string type)
    {
        Type = Check.Length(type, nameof(type), OpenIddictTokenConst.TypeMaxLength);
    }
}