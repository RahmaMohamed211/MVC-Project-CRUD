using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace company.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadImage(IFormFile file,string FolderName)
        {
            //1.Get Located folder path 
            //string FolderPath = "D:\\task5\\img\\....";

            //string FolderPath= Directory.GetCurrentDirectory() + "\\wwwroot\\files\\"+FolderName

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",FolderName);
            //step 2 Get file Name and Make it Unique 
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            //3. Get FilePath
            string FilePath=Path.Combine(FolderPath, fileName);

            var fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);

            return fileName;
        }


        public static void DeleteFile(string fileName, string FolderName)
        {
            if(fileName is not null && FolderName is not null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, fileName);
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
               
                

            }
           


        }
       

       
    }

   