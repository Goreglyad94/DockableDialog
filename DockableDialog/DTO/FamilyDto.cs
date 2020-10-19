using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialog.DTO
{
    [Serializable]
    public class FamilyDto
    {
        public FamilyDto()
        {

        }
        public string Name { get; set; }
        public FamilySymbol FamilySymbolDto { get; set; }
        public string ImagePath { get; set; }
    }
}
