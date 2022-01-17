using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictCoreBuilderExtensions
{
    public static OpenIddictCoreBuilder AddAbpOpenIddictCore(this OpenIddictBuilder builder)
    {
        var coreBuilder = builder.AddCore();

        //TODO: Performance Optimization 
        coreBuilder.DisableEntityCaching();

        coreBuilder.SetDefaultApplicationEntity<OpenIddictApplication>();
        coreBuilder.SetDefaultAuthorizationEntity<OpenIddictAuthorization>();
        coreBuilder.SetDefaultScopeEntity<OpenIddictScope>();
        coreBuilder.SetDefaultTokenEntity<OpenIddictToken>();

        return coreBuilder;
    }

    public static OpenIddictCoreBuilder AddAbpStore(this OpenIddictCoreBuilder coreBuilder)
    {
        coreBuilder
            .AddApplicationStore<AbpOpenIddictApplicationStore>(ServiceLifetime.Transient)
            .AddAuthorizationStore<AbpOpenIddictAuthorizationStore>(ServiceLifetime.Transient)
            .AddScopeStore<AbpOpenIddictScopeStore>(ServiceLifetime.Transient)
            .AddTokenStore<AbpOpenIddictTokenStore>(ServiceLifetime.Transient);
        return coreBuilder;
    }
}