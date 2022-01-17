using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictBuilderOptions
{
    /// <summary>
    /// Updates <see cref="JwtSecurityTokenHandler.DefaultInboundClaimTypeMap"/> to be compatible with openiddict server claims.
    /// Default: true.
    /// </summary>
    public bool UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap { get; set; } = true;

    /// <summary>
    /// Updates <see cref="AbpClaimTypes"/> to be compatible with openiddict server claims.
    /// Default: true.
    /// </summary>
    public bool UpdateAbpClaimTypes { get; set; } = true;

    /// <summary>
    /// For development purposes, a certificate can be generated and stored by OpenIddict in the certificates store of the user account running the OpenIddict server feature. Unlike ephemeral keys, development certificates are persisted - but not shared across instances - and will be reused when the application host is restarted.
    /// Set false to suppress AddDeveloperSigningCredential() and AddDevelopmentSigningCertificate() call on the OpenIddictServerBuilder.
    /// Default: false.
    /// </summary>
    public bool AddDeveloperSigningCredential { get; set; } = false;

    /// <summary>
    /// For development purposes, an ephemeral key - that is not persisted or shared across instances - can be used to sign or encrypt tokens:
    /// Set false to suppress AddEphemeralEncryptionKey() and AddEphemeralSigningKey() call on the OpenIddictServerBuilder.
    /// Default: true.
    /// </summary>
    public bool AddEphemeralEncryptionKey { get; set; } = true;

    /// <summary>
    /// Set false to suppress RequireProofKeyForCodeExchange() call on the OpenIddictServerBuilder.
    /// Default: true.
    /// </summary>
    public bool RequireProofKeyForCodeExchange { get; set; } = true;

    /// <summary>
    /// Set true to support code_challenge_method=plain.
    /// Default: false.
    /// </summary>
    public bool SupportPlainCodeChallengeMethod { get; set; } = false;
}