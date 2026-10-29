using ZombieParty.Models;

namespace ZombieParty.Utility
{
    public class ImageFileManager : IImageFileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageFileManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string? UploadImage(IFormCollection form, bool edit, string? oldImagePath = null)
        {
            var files = form.Files; //nouvelle image récupérée

            if (files.Count > 0)
            {
                // Il faut utiliser la DI pour obtenir webHostEnvironment
                string webRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();// Nom fichier généré, unique
                var uploads = webRootPath + AppConstants.ImagePath;// chemin pour les image
                var extension = Path.GetExtension(files[0].FileName); // extraire l'extension du fichier

                // Créer un cannal pour transférer le fichier 
                using (var filesStreams = new FileStream(uploads + fileName + extension, FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }

                // Je supprime l'ancien fichier si c'est une édition
                if (edit && !string.IsNullOrWhiteSpace(oldImagePath))
                {
                    try
                    {
                        System.IO.File.Delete(uploads + oldImagePath);
                    }
                    catch { }
                }

                // Il faudra retourner la valeur dans notre nouvelle fonction
                return fileName + extension;
            }
            return oldImagePath;
            // Fin du code qui doit être déplacer dans ImageFileManager
        }
    }
}
