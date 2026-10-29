using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZombieParty.Utility;
using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Controllers
{
    public class ZombieController : Controller
    {
        private ZombiePartyDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IImageFileManager imageFileManager;

        public ZombieController(ZombiePartyDbContext context, IWebHostEnvironment webHostEnvironment, IImageFileManager image)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.imageFileManager = image;
        }

        public async Task<IActionResult> Index()
        {
            this.ViewBag.MaListe = await this.context.Zombie.ToListAsync();

            return View();
        }

        //GET UPSERT
        public async Task<IActionResult> Upsert(int zombieId = 0)
        {
            if (zombieId == 0)
            {
                ViewBag.ZombieTypes = new SelectList(await this.context.ZombieType.ToListAsync(), "Id", "TypeName", null);
                return View(new Zombie());

            }
            else
            {
                var zombie = await this.context.Zombie.FindAsync(zombieId);
                ZombieType selectedZombieType = await this.context.ZombieType.FirstOrDefaultAsync(x => x.Id == zombie.ZombieTypeId);

                ViewBag.ZombieTypes = new SelectList(await this.context.ZombieType.ToListAsync(), "Id", "TypeName", selectedZombieType);
                return View(zombie);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromServices] IImageFileManager imageFileManager, Zombie zombie)
        {
            if (ModelState.IsValid)
            {
                IFormCollection form = HttpContext.Request.Form;
                bool edit = zombie.Id != 0;
                string? oldImagePath = zombie.Image;

                zombie.Image = imageFileManager.UploadImage(form, edit, oldImagePath);
                //// Début du code qui doit être déplacer dans ImageFileManager
                //var files = form.Files; //nouvelle image récupérée

                //if (files.Count > 0)
                //{
                //    // Il faut utiliser la DI pour obtenir webHostEnvironment
                //    string webRootPath = webHostEnvironment.WebRootPath;
                //    string fileName = Guid.NewGuid().ToString();// Nom fichier généré, unique
                //    var uploads = webRootPath + AppConstants.ImagePath;// chemin pour les image
                //    var extension = Path.GetExtension(files[0].FileName); // extraire l'extension du fichier

                //    // Créer un cannal pour transférer le fichier 
                //    using (var filesStreams = new FileStream(uploads + fileName + extension, FileMode.Create))
                //    {
                //        files[0].CopyTo(filesStreams);
                //    }

                //    // Je supprime l'ancien fichier si c'est une édition
                //    if (edit && !string.IsNullOrWhiteSpace(oldImagePath))
                //    {
                //        try
                //        {
                //            System.IO.File.Delete(uploads + oldImagePath);
                //        }
                //        catch { }
                //    }

                //    // Il faudra retourner la valeur dans notre nouvelle fonction
                //    zombie.Image = fileName + extension;
                //}
                //// Fin du code qui doit être déplacer dans ImageFileManager

                if (zombie.Id == 0)
                    this.context.Zombie.Add(zombie);
                else
                    this.context.Update(zombie);

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            //Je vais chercher le type sélectionné. 
            ZombieType selectedZombieType = await this.context.ZombieType.FirstAsync(x => x.Id == zombie.ZombieTypeId);
            ViewBag.ZombieTypes = new SelectList(await this.context.ZombieType.ToListAsync(), "Id", "TypeName", selectedZombieType);
            return this.View(zombie);
        }
    }
}
