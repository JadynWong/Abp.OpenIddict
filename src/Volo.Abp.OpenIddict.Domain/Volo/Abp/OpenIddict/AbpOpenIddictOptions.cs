using System.Collections.Generic;
using OpenIddict.Abstractions;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictOptions
{
    public List<OpenIddictApplicationDescriptor> Applications { get; set; } = new();

    public List<OpenIddictScopeDescriptor> Scopes { get; set; } = new();
}