using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Csb.YerindeDonusum.Application.CustomAddons;

public class JwtAddon
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly JwtOptionModel _jwtOptions;

    public JwtAddon(IHttpContextAccessor contextAccessor, JwtOptionModel jwtOptions)
    {
        _contextAccessor = contextAccessor;
        _jwtOptions = jwtOptions;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtOptions.ValidateIssuer,
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidateAudience = _jwtOptions.ValidateAudience,
                ValidAudience = _jwtOptions.ValidAudience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = _jwtOptions.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtOptions.IssuerSigningKey),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Hatalı token");

            return principal;
        }
        catch
        {
            throw new Exception("Token çözümlenemedi!");
        }
    }

    public string GenerateJwt(Kullanici kullanici)
    {
        var jwt = new JwtSecurityToken(
           issuer: _jwtOptions.ValidIssuer,
           audience: _jwtOptions.ValidAudience,
           expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireTimeMinute),
           notBefore: DateTime.Now,
           claims: SetClaims(kullanici),
           signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_jwtOptions.IssuerSigningKey), SecurityAlgorithms.HmacSha256Signature)
       );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(jwt);
    }

    private IEnumerable<Claim> SetClaims(Kullanici kullanici)
    {
        var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()),
                new Claim("KullaniciAdi", kullanici.KullaniciAdi),
                new Claim("TcKimlikNo", kullanici.TcKimlikNo?.ToString() ?? "0"),
                new Claim("KullaniciHesapTipId", kullanici.KullaniciHesapTipId.ToString()),
                new Claim("SonSifreDegisimTarihi", kullanici.SonSifreDegisimTarihi.HasValue ? kullanici.SonSifreDegisimTarihi.Value.ToString("yyyy-MM-dd") : ""),
                new Claim(ClaimTypes.Name, $"{kullanici.Ad ?? kullanici.KullaniciAdi} {kullanici.Soyad ?? kullanici.KullaniciAdi}"),
                new Claim(ClaimTypes.GivenName, kullanici.Ad ?? kullanici.KullaniciAdi),
                new Claim(ClaimTypes.Surname, kullanici.Soyad ?? kullanici.KullaniciAdi),
                new Claim(ClaimTypes.Email, kullanici.Eposta ?? "?"),
                new Claim("BirimAdi", kullanici.Birim?.Ad ?? "?"),
                new Claim("BirimId", kullanici.BirimId?.ToString() ?? "0"),
                new Claim("BirimIlId", kullanici.BirimId != null ? (kullanici.Birim?.IlId?.ToString() ?? "0") : "1"),
                new Claim("IpAddr", _contextAccessor.HttpContext?.GetIpAddress())
            };

        foreach (var roleAd in kullanici.KullaniciRols.Where(x => x.AktifMi == true && !x.SilindiMi).Select(c => c.Rol.Ad).Distinct().ToArray())
            claims.Add(new Claim(ClaimTypes.Role, roleAd));

        return claims;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}