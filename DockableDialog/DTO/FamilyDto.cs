using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialog.DTO
{
    class FamilyDto
    {
        public string Name { get; set; }
        public FamilySymbol FamilySymbolDto { get; set; }
    }
}
