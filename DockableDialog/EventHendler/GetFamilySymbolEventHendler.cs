using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
using DockableDialog.DTO;
using DockableDialog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DockableDialog.EventHendler
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class GetFamilySymbolEventHendler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            Transaction trans = new Transaction(app.ActiveUIDocument.Document, "Получить выбранные семейства");
            trans.Start();

            UIDocument uidoc = app.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;
            Selection sel = app.ActiveUIDocument.Selection;
            ICollection<Autodesk.Revit.DB.ElementId> selectedIds = uidoc.Selection.GetElementIds();

            

            //Reference annotation = sel.PickObject(ObjectType.Element, "Select item");
            FamilyInstance elem = doc.GetElement(selectedIds.FirstOrDefault()) as FamilyInstance;
            FamilySymbol familySymbol = elem.Symbol;
            FamilyDto famDto = new FamilyDto();
            famDto.Name = familySymbol.Name;
            famDto.FamilySymbolDto = familySymbol;
            MainWindowViewModel.FamilySymbolList.Add(famDto);
            MessageBox.Show(familySymbol.Name.ToString());
            trans.Commit();
        }
        public string GetName() => nameof(GetFamilySymbolEventHendler);
    }
}
