using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
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
    class PasteFamilyEventHendler : IExternalEventHandler
    {
        public static FamilyDto familyDto;
        public void Execute(UIApplication app)
        {
            Transaction trans = new Transaction(app.ActiveUIDocument.Document, "Получить выбранные семейства");
            trans.Start();
            UIDocument uidoc = app.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;
            TaskDialog.Show("а это ревитовская херная", "");
            XYZ xYZ = new XYZ(0, 0, 0);
            //try
            //{
            //    doc.Create.NewFamilyInstance(xYZ, familyDto.FamilySymbolDto, uidoc.ActiveView);
            //}
            //catch (Exception ex)
            //{
            //    TaskDialog.Show(ex.Message.ToString(), ex.Message.ToString());
            //}
            trans.Commit();
            try
            {
                uidoc.PromptForFamilyInstancePlacement(familyDto.FamilySymbolDto as FamilySymbol);
            }
            catch { }
            
        }

        public string GetName() => nameof(PasteFamilyEventHendler);
    }
}
