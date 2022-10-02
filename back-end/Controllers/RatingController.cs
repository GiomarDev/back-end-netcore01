using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public RatingController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO) 
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var userID = usuario.Id;
        
            var ratingActual = await context.Ratings.FirstOrDefaultAsync(x => x.peliculaID == ratingDTO.peliculaID && 
                                                                         x.userID == userID);
            if (ratingActual == null)
            {
                var rating = new Rating();
                rating.peliculaID = ratingDTO.peliculaID;
                rating.puntuacion = ratingDTO.puntuacion;
                rating.userID = userID;
                context.Add(rating);
            }
            else {
                ratingActual.puntuacion = ratingDTO.puntuacion;
            }

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
