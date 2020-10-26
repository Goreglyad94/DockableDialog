using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialog.DTO
{
    [Serializable]
    public class FamilySetDto
    {
        public FamilySetDto()
        {

        }
        public string Name { get; set; }
        public ObservableCollection<FamilyDto> FamiliesDto { get; set; } = new ObservableCollection<FamilyDto>();
        public bool IsVisable { get; set; }
    }
}
