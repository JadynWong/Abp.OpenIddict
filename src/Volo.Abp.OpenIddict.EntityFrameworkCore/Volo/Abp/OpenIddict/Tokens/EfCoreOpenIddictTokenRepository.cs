using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict.Tokens;

public class EfCoreOpenIddictTokenRepository
    : EfCoreRepository<IOpenIddictDbContext, OpenIddictToken, Guid>, IOpenIddictTokenRepository
{
    public EfCoreOpenIddictTokenRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(
        string subject,
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.Subject == subject && token.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(
        string subject,
        Guid applicationId,
        string status,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.Subject == subject &&
                    token.ApplicationId == applicationId &&
                    token.Status == status)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(
        string subject,
        Guid applicationId,
        string status,
        string type,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.Subject == subject &&
                    token.ApplicationId == applicationId &&
                    token.Status == status &&
                    token.Type == type)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(
        Guid authorizationId,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.AuthorizationId == authorizationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(

        string referenceId, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.ReferenceId == referenceId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(
        string subject,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query
            .Where(token => token.Subject == subject)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> GetListAsync(
        int? maxResultCount,
        int? skipCount,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        if (skipCount.HasValue)
        {
            query = query.Skip(skipCount.Value);
        }
        if (maxResultCount.HasValue)
        {
            query = query.Take(maxResultCount.Value);
        }
        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> GetPruneListAsync(
        DateTime date,
        int maxResultCount = 1_000,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();

        return
            await (from token in query
                   where token.CreationDate < date
                   where (token.Status != Statuses.Inactive && token.Status != Statuses.Valid) ||
                          token.ExpirationDate < DateTime.UtcNow ||
                          dbContext.Authorizations.Any(authorization => authorization.Id == token.ApplicationId && authorization.Status != Statuses.Valid)
                   select token)
                   .OrderBy(token => token.Id)
                   .Take(maxResultCount)
                   .ToListAsync(GetCancellationToken(cancellationToken));
    }
}