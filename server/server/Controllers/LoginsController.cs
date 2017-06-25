using Microsoft.AspNetCore.Mvc;
using server.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using server.Data;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Logins")]
    public class LoginsController : Controller
    {
        private readonly JwtIssuerOptions jwtOptions;
        private readonly IAntiforgery antiforgery;
        private UserManager<Usuario> userManager;
        private IPasswordHasher<Usuario> passwordHasher;
        private readonly LeilaoContext context;

        public LoginsController(UserManager<Usuario> userMgr,
            IAntiforgery antiforg, IOptions<JwtIssuerOptions> jwtOpt, IPasswordHasher<Usuario> passwordHash, LeilaoContext ctx)
        {
            antiforgery = antiforg;
            userManager = userMgr;
            jwtOptions = jwtOpt.Value;
            passwordHasher = passwordHash;
            context = ctx;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginModel login)
        {

            //busca o usuário pelo email
            Usuario usuario = await userManager.FindByEmailAsync(login.Email);
            if (usuario != null)
            {
                //verifica se a senha passada é a mesma do BD
                if (passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, login.Senha)
                    == PasswordVerificationResult.Success)
                {
                    //se estiver tudo ok, retorna o token de autenticacao e o forgery token
                    return Ok(new
                    {
                        TokenUsuario = new
                        {
                            tokenUsuario = await Token(usuario),
                            expira_em = (int)jwtOptions.ValidFor.TotalSeconds,
                            tokenNome = "Authorization"
                        },
                        usuario
                    });
                }
                else
                {
                    ModelState.AddModelError("Senha", "Login e/ou Senha Inválidos");
                    return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            else
            {
                ModelState.AddModelError("Login", "Login e/ou Senha Inválidos");
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }
        }

        [HttpPost("recuperar")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarSenha([FromBody] string email)
        {
            try
            {
                //busca o usuário pelo email
                Usuario usuario = await userManager.FindByEmailAsync(email);
                if (usuario != null)
                {
                    string novaSenha = Guid.NewGuid().ToString();
                    usuario.PasswordHash = passwordHasher.HashPassword(usuario, novaSenha);

                    string Remetente = "ileilaoscs@gmail.com";
                    string TituloRemetente = "iLeilao";

                    string Destinatario = email;
                    string TituloDestinatario = usuario.Nome;

                    string Assunto = "Recuperação de senha iLeilão";
                    string Conteudo =
                        $"Olá {usuario.Nome}.\n\n" +
                        "Se não foi você que solicitou recuperação de senha no app iLeilão, por favor, ignore este e - mail.\n" +
                        $"Se foi você, use esta senha temporária: {novaSenha}\n\n" +
                        "Obrigado!";

                    string SmtpServer = "smtp.gmail.com";
                    int SmtpPorta = 465;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(TituloRemetente, Remetente));
                    mimeMessage.To.Add(new MailboxAddress(TituloDestinatario, Destinatario));
                    mimeMessage.Subject = Assunto;
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = Conteudo
                    };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(SmtpServer, SmtpPorta, true);

                        await client.AuthenticateAsync(Remetente, "s0000000000");

                        await client.SendAsync(mimeMessage);

                        await client.DisconnectAsync(true);

                        await context.SaveChangesAsync();

                        return Ok();
                    }
                }
                else
                {
                    ModelState.AddModelError("Usuário", "Email Inválido");
                    return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
                }
            }catch(Exception e)
            {
                return Ok(e.Message);
            }
        }

        private async Task<string> Token(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    ToUnixEpochDate(jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
            };

            //Cria token e codifica
            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: jwtOptions.NotBefore,
                expires: jwtOptions.Expiration,
                signingCredentials: jwtOptions.SigningCredentials);

            string jwtCodificado = new JwtSecurityTokenHandler().WriteToken(jwt);

            return jwtCodificado;
        }

        //Para verificar exceção de qualquer operação com o Token
        private static void ThrowIfInvalid(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("TimeSpan deve ser maior que 0", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        //Converte a data para segundos desde a epoca do Unix (1 Jan 1970 00:00 UTC)
        private static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() -
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                    .TotalSeconds);
        }
    }
}