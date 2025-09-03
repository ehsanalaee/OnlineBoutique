using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBoutiqueCoreLayer.Services.File
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
        //bool DeleteFile(string relativePath);
    }


}
