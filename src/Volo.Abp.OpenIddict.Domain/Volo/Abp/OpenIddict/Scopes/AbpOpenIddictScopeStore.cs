using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using SR = OpenIddict.Abstractions.OpenIddictResources;

namespace Volo.Abp.OpenIddict.Scopes;

/// <inheritdoc/>
public class AbpOpenIddictScopeStore : OpenIddictScopeStoreBase
{
    protected IOpenIddictScopeRepository ScopeRepository { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AbpOpenIddictScopeStore(
        IGuidGenerator guidGenerator,
        IOpenIddictScopeRepository scopeRepository,
        IUnitOfWorkManager unitOfWorkManager) : base(guidGenerator)
    {
        ScopeRepository = scopeRepository;
        UnitOfWorkManager = unitOfWorkManager;
    }

    /// <inheritdoc/>
    public override async ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        return await ScopeRepository.LongCountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async ValueTask<long> CountAsync<TResult>(
        Func<IQueryable<OpenIddictScope>, IQueryable<TResult>> query,
        CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        var queryable = await ScopeRepository.GetQueryableAsync();
        return await ScopeRepository.AsyncExecuter
            .LongCountAsync(query(queryable), cancellationToken);
    }

    /// <inheritdoc/>
    //[UnitOfWork]
    public override async ValueTask CreateAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        using var unitOfWork = UnitOfWorkManager.Begin();

        await ScopeRepository.InsertAsync(scope, true, cancellationToken);

        await unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    //[UnitOfWork]
    public override async ValueTask UpdateAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        using var unitOfWork = UnitOfWorkManager.Begin();

        await ScopeRepository.UpdateAsync(scope, true, cancellationToken);

        await unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    //[UnitOfWork]
    public override async ValueTask DeleteAsync(OpenIddictScope scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        using var unitOfWork = UnitOfWorkManager.Begin();

        await ScopeRepository.DeleteAsync(scope, true, cancellationToken);

        await unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public override async ValueTask<OpenIddictScope> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        if (identifier.IsNullOrWhiteSpace())
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
        }

        var key = ConvertIdentifierFromString(identifier);

        return await ScopeRepository.FindAsync(key, false, cancellationToken);
    }

    /// <inheritdoc/>
    public override async ValueTask<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        if (name.IsNullOrWhiteSpace())
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0202), nameof(name));
        }

        return await ScopeRepository.FindByNameAsync(name, cancellationToken);
    }

    /// <inheritdoc/>
    public override IAsyncEnumerable<OpenIddictScope> FindByNamesAsync(
        ImmutableArray<string> names,
        CancellationToken cancellationToken)
    {
        if (names.Any(name => name.IsNullOrWhiteSpace()))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0203), nameof(names));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<OpenIddictScope> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = (await ScopeRepository.FindByNamesAsync(names.ToList(), cancellationToken))
                .AsAsyncEnumerable();

            await foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }
    }

    /// <inheritdoc/>
    public override IAsyncEnumerable<OpenIddictScope> FindByResourceAsync(
        string resource,
        CancellationToken cancellationToken)
    {
        if (resource.IsNullOrWhiteSpace())
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0062), nameof(resource));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<OpenIddictScope> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var scopes = (await ScopeRepository.FindByResourceAsync(resource, cancellationToken))
                .AsAsyncEnumerable();

            await foreach (var scope in scopes)
            {
                var resources = await GetResourcesAsync(scope, cancellationToken);
                if (resources.Contains(resource, StringComparer.Ordinal))
                {
                    yield return scope;
                }
            }
        }
    }

    /// <inheritdoc/>
    public override async ValueTask<TResult> GetAsync<TState, TResult>(
        Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query,
        TState state,
        CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        var queryable = await ScopeRepository.GetQueryableAsync();
        return await ScopeRepository.AsyncExecuter
            .FirstOrDefaultAsync(query(queryable, state), cancellationToken);
    }

    /// <inheritdoc/>
    public override IAsyncEnumerable<OpenIddictScope> ListAsync(
        int? count,
        int? offset,
        CancellationToken cancellationToken)
    {
        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<OpenIddictScope> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = (await ScopeRepository
                .GetListAsync(count, offset, cancellationToken))
                .AsAsyncEnumerable();

            await foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }
    }

    /// <inheritdoc/>
    public override IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
        Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query,
        TState state, CancellationToken cancellationToken)
    {
        Check.NotNull(query, nameof(query));

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TResult> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var queryable = await ScopeRepository.GetQueryableAsync();

            var results = (await ScopeRepository.AsyncExecuter
                .ToListAsync(query(queryable, state), cancellationToken))
                .AsAsyncEnumerable();

            await foreach (var result in results)
            {
                yield return result;
            }
        }
    }
}