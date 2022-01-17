using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Scopes;

public class EfCoreOpenIddictScopeRepository
    : EfCoreRepository<IOpenIddictDbContext, OpenIddictScope, Guid>, IOpenIddictScopeRepository
{
    public EfCoreOpenIddictScopeRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<OpenIddictScope> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query.FirstOrDefaultAsync(a => a.Name == name, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> FindByNamesAsync(
        List<string> names,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        return await query.Where(x => names.Contains(x.Name)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> FindByResourceAsync(
        string resource,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        //TODO: EF CORE 6 https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0/whatsnew#more-flexible-sql-server-free-text-search
        //https://entityframeworkcore.com/knowledge-base/60969027/how-to-convert-string-to-datetime-in-csharp-ef-core-query
        query = query
            .Where(x => ((string)(object)x.Resources).Contains(resource));

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> GetListAsync(
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
}