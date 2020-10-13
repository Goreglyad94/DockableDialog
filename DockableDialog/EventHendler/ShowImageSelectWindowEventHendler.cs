using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace DockableDialog.EventHendler
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class ShowImageSelectWindowEventHendler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            MessageBox.Show("");
            Transaction trans = new Transaction(app.ActiveUIDocument.Document, "Получить выбранные семейства");
            trans.Start();
            TaskDialog.Show("","");
            trans.Commit();

            ImageSelect MyWindow = new ImageSelect();
            MyWindow.ShowDialog();
            //HwndSource hwndSource = HwndSource.FromHwnd(app.MainWindowHandle);
            //Window wnd = hwndSource.RootVisual as Window;
            //if (wnd != null)
            //{
            //    MyWindow.Owner = wnd;
            //    //MyWindow.ShowInTaskbar = false;
            //    MyWindow.Show();
            //}
        }

        public string GetName() => nameof(ShowImageSelectWindowEventHendler);
    }
}
