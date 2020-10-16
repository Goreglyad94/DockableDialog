using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialog.DTO
{
    class ImageDto
    {
        public ImageDto(string path)
        {
            Path = path;
        }
        public string Path { get; set; }
    }
}
