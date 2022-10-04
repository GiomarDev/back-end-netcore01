using AutoMapper;
using back_end.DTOs;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, ApplicationDbContext context,
                                 IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.context = context;
            this.mapper = mapper;
        }


        #region EndPoints

        [HttpGet("listadoUsuarios")]
        public async Task<ActionResult<List<UsuarioDTO>>> ListadoUsuario([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionCabecera(queryable);
            var usuarios = await queryable.OrderBy(x => x.Email).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        [HttpPost("hacerAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="EsAdmin")]
        public async Task<ActionResult> HacerAdmin([FromBody] string userID)
        {
            var usuario = await userManager.FindByIdAsync(userID);
            await userManager.AddClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("removerAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin([FromBody] string userID)
        {
            var usuario = await userManager.FindByIdAsync(userID);
            await userManager.RemoveClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("crear")]
        public async Task<ActionResult<RespuestaAutenticacion>> Crear([FromBody] CredencialesUsuario credenciales)
        {
            var usuario = new IdentityUser { UserName = credenciales.email, Email = credenciales.email };
            var resultado = await userManager.CreateAsync(usuario, credenciales.password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login([FromBody] CredencialesUsuario credenciales)
        {
            var resultado = await signInManager.PasswordSignInAsync(credenciales.email, credenciales.password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else {
                return BadRequest("Login Incorrecto");
            }
        }

        #endregion

        #region Methods

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.email)
            };

            var usuario = await userManager.FindByEmailAsync(credenciales.email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(12);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiracion = expiracion
            };

        }

        #endregion


    }
}
