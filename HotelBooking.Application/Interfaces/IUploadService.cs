using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadImage(string image);
        Task<string> FromBase64ToFile(string base64File, string filename);
    }
}
