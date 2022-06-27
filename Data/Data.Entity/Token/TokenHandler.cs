using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Data.Entity.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Data.Entity.Token;

public class TokenHandler
{
    public IConfiguration Configuration { get; set; }

    public TokenHandler(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public Token CreateAccessToken(KU_KULLANICI user)
    {
        Token tokenInstance = new Token();

        //Security  Key'in simetriğini alıyoruz.
        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minimumSixteenCharacters"));

        //Şifrelenmiş kimliği oluşturuyoruz.
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //Oluşturulacak token ayarlarını veriyoruz.
        tokenInstance.Expiration = DateTime.Now.AddDays(365);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: "mertcanduldul",
            audience: "mertcanduldul",
            expires: tokenInstance.Expiration, 
            notBefore: DateTime.Now, //Token üretildikten ne kadar süre sonra devreye girsin ayarlıyouz.
            signingCredentials: signingCredentials
        );

        //Token oluşturucu sınıfında bir örnek alıyoruz.
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        //Token üretiyoruz.
        tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);

        //Refresh Token üretiyoruz.
        tokenInstance.RefreshToken = CreateRefreshToken();
        return tokenInstance;
    }

    //Refresh Token üretecek metot.
    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}