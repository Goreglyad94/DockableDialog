using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DockableDialog.DTO;
using DockableDialog.Model;
using DockableDialog.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DockableDialog.EventHendler
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class GetFamilyByIdEventHendler : IExternalEventHandler
    {
        public static event Action<object> ChangedUIAndAddSerializer;
        public static ObservableCollection<FamilyDto> familyDtos;
        public void Execute(UIApplication app)
        {
            try
            {
                //TODO: если нет семейства то выдать сообщение
                Transaction trans = new Transaction(app.ActiveUIDocument.Document, "Получить выбранные семейства");
                trans.Start();
                foreach(var a in familyDtos)
                {
                    ElementId elementId = new ElementId(Convert.ToInt32(a.ID));
                    Element dd = app.ActiveUIDocument.Document.GetElement(elementId);

                    FamilySymbol familySymbol = dd as FamilySymbol;
                    a.FamilySymbolDto = familySymbol;
                    MainWindowViewModel.familyDtosForSelectItem.Add(a);
                }
                trans.Commit();
                
                ChangedUIAndAddSerializer?.Invoke(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка с GetFamilyByIdEventHendler");
            }
        }

        public string GetName() => nameof(GetFamilyByIdEventHendler);
    }
}
