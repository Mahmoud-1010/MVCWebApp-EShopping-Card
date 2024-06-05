namespace PresentationLayer.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder Path
            //string folderPath = @"D:\Route\Cycle 37\Project Solution\Project.PL\wwwroot\files\Imgs\";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName);

            // 2. Get File Name and Make its Name UNIQUE
            var fileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";

            // 3. Get File Path
            var filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Streams
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
