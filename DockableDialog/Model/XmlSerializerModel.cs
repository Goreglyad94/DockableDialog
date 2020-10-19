using DockableDialog.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace DockableDialog.Model
{
    class XmlSerializerModel
    {
        public void ParamsXmlSerializer(ObservableCollection<FamilyDto> ParamsSetDto)
        {
            try
            {
                ParamsSetDto.ToList();
                var xml = new XmlSerializer(typeof(List<FamilyDto>));

                using (var fs = new FileStream(@"C:\ProgramData\Autodesk\Revit\Addins\2019\FamilyPalette.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, ParamsSetDto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        public ObservableCollection<FamilyDto> ParamsXmlDeserializer()
        {
            try
            {
                var xml = new XmlSerializer(typeof(List<FamilyDto>));

                using (var fs = new FileStream(@"C:\ProgramData\Autodesk\Revit\Addins\2019\FamilyPalette.xml", FileMode.OpenOrCreate))
                {
                    ObservableCollection<FamilyDto> prs = (ObservableCollection<FamilyDto>)xml.Deserialize(fs);
                    return prs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }

        }
    }
}
