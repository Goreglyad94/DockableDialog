using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DockableDialog.DTO
{
    [Serializable]
    public class FamilyDto
    {
        public FamilyDto() 
        { 

        }
        public string Name { get; set; }
        [XmlIgnore]
        public FamilySymbol FamilySymbolDto { get; set; }
        public string ImagePath { get; set; }
        public string ID { get; set; }
        [XmlIgnore]
        public string BackGroundColor { get; set; }
    }
}
