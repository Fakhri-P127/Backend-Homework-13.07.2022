using Backend_Homework_Pronia.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Extensions
{
    public static class FileValidator
    {
        public static async Task<string> FileCreate(this IFormFile file,string root,string folder)
        {
            //string filename = "";
            //bool isSame = data.Sliders.Any(s => s.Photo.FileName == file.FileName);
            //if (isSame)
            //{
             string   filename = string.Concat(Guid.NewGuid(), file.FileName);
            //}
            //else
            //{
                filename = file.FileName;
            //}
            string path = Path.Combine(root, folder);
            string filePath = Path.Combine(path, filename);

            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                throw new FileLoadException();
            }

            return filename;
        } 

        public static void FileDelete(string root, string folder, string image)
        {
            string filePath = Path.Combine(root, folder, image);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static bool IsImageOkay(this IFormFile file, int mb)
        {

            return file.Length / 1024 / 1024 < mb && file.ContentType.Contains("image/");

        }

       
    }

}
