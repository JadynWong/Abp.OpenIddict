using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace Volo.Abp.Account.Web.AbpGrantTypes;

public interface IGrantTypeProvider
{
    string GrantType { get; }

    Task<GrantTypeResult> HandleAsync(OpenIddictRequest request);
}