using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;

namespace TestFamilyPaste
{
    [Transaction(TransactionMode.Manual)]
    public class TestCmd : IExternalCommand
    {
        FamilySymbol dfd;
        List<ElementId> _added_element_ids = new List<ElementId>();
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;

            Selection sel = uiapp.ActiveUIDocument.Selection;
            ICollection<Autodesk.Revit.DB.ElementId> selectedIds = uidoc.Selection.GetElementIds();


            //Reference annotation = sel.PickObject(ObjectType.Element, "Select item");
            FamilyInstance elem = doc.GetElement(selectedIds.FirstOrDefault()) as FamilyInstance;


            //Reference annotation = sel.PickObject(ObjectType.Element, "Select item");
            //FamilyInstance elem = doc.GetElement(annotation) as FamilyInstance;
            FamilySymbol familySymbol = elem.Symbol;


            PromptForFamilyInstancePlacementOptions promptForFamilyInstancePlacementOptions = new PromptForFamilyInstancePlacementOptions();

            try
            {
                uidoc.PromptForFamilyInstancePlacement(familySymbol);
            }
            catch
            {
            }
            


            return Result.Succeeded;
        }
        void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            _added_element_ids.AddRange(e.GetAddedElementIds());
        }
    }
}
