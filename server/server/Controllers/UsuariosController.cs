using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using server.Data;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Usuarios")]
    [Authorize]
    public class UsuariosController : Controller
    {
        //Fornece acesso aos usuários
        private UserManager<Usuario> userManager;

        //Fornece validações
        private IUserValidator<Usuario> userValidator;
        private IPasswordValidator<Usuario> passwordValidator;

        //Faz o Hash da senha
        private IPasswordHasher<Usuario> passwordHasher;

        private readonly LeilaoContext context;

        //Recebe os parametros via Dependency Injection
        public UsuariosController(UserManager<Usuario> usrMgr, IUserValidator<Usuario> userValid,
            IPasswordValidator<Usuario> passValid, IPasswordHasher<Usuario> passwordHash, LeilaoContext ctx)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            context = ctx;
        }

        // GET: api/Usuarios
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Usuario> GetAll()
        {
            return context.Usuario;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            Usuario usuario = await userManager.FindByIdAsync(id);

            if (usuario != null)
            {
                return Ok(usuario);
            }
            else
            {
                ModelState.AddModelError("Usuario", "Usuário não encontrado");
                return NotFound(ModelState.Values.SelectMany(v => v.Errors));
            }
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] UsuarioModel model)
        {
            var usuario = await userManager.FindByIdAsync(id);
            IdentityResult validEmail = new IdentityResult();
            bool email = false;

            if (usuario != null)
            {
                //Verificar se o email não foi alterado
                if (!usuario.Email.Equals(model.Email))
                {
                    //Validar o email do usuario
                    usuario.Email = model.Email;
                    usuario.UserName = model.Email;
                    validEmail =
                        await userValidator.ValidateAsync(userManager, usuario);

                    if (!validEmail.Succeeded)
                    {
                        ModelState.AddModelError("Email", "E-mail já cadastrado");
                    }
                }
                else
                {
                    email = true;
                }

                //Validar senha(se foi passada)
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(model.Senha))
                {
                    //valida a senha
                    validPass = await passwordValidator.ValidateAsync(userManager, usuario, model.Senha);
                    if (validPass.Succeeded)
                    {
                        usuario.PasswordHash = passwordHasher.HashPassword(usuario, model.Senha);
                    }
                    else
                    {
                        ModelState.AddModelError("Senha", "Senha invalida");
                    }
                }


                if (((validEmail.Succeeded || email) && validPass == null) || ((validEmail.Succeeded || email) && validPass.Succeeded))
                {
                    usuario.Nome = model.Nome;
                    usuario.Tipo = model.Tipo;

                    //atualiza o BD
                    IdentityResult result = await userManager.UpdateAsync(usuario);
                    if (result.Succeeded)
                    {
                        return Ok(usuario);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Email já cadastrado");
                    }
                }
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }
            else
            {
                ModelState.AddModelError("Usuario", "Usuário não encontrado");
                return NotFound(ModelState.Values.SelectMany(v => v.Errors));
            }

        }



        // POST: api/Usuarios
        [HttpPost]
        //Permite que usuarios não logados interajam
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] UsuarioModel model)
        {
            Usuario usuario = await userManager.FindByEmailAsync(model.Email);

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Tipo = model.Tipo,
                    UserName = model.Email
                };

                //Tenta criar o usuário
                IdentityResult result = await userManager.CreateAsync(usuario, model.Senha);

                //Verifica se o usuário foi criado
                if (result.Succeeded)
                {
                    //Retorna codigo 201 (OK)
                    return CreatedAtAction("Create", usuario);
                }
                else
                {
                    AddErrorsFromResult(result);
                    ModelState.AddModelError("Usuario", "Usuário não pode ser criado");
                    return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email já cadastrado");
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }


        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                Usuario usuario = await userManager.FindByIdAsync(id);
                if (usuario != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(usuario);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError("Usuario", "Usuário não pode ser apagado");
                        return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
                    }
                }
                else
                {
                    ModelState.AddModelError("Usuario", "Usuário não encontrado");
                    return NotFound(ModelState.Values.SelectMany(v => v.Errors));
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        //ESSE NÃO VAI PRO DIAGRAMA
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}