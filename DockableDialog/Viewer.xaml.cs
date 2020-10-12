using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DockableDialog
{
    /// <summary>
    /// Логика взаимодействия для Viewer.xaml
    /// </summary>
    public partial class Viewer : Page, IDockablePaneProvider
    {
        public ExternalCommandData eData = null;
        public Document doc = null;
        public UIDocument uidoc = null;

        // IDockablePaneProvider abstrat method
        public void SetupDockablePane(DockablePaneProviderData data)
        {
            // wpf object with pane's interface
            data.FrameworkElement = this as FrameworkElement;
            // initial state position
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
            };

        }
        public Viewer()
        {
            InitializeComponent();
        }
        public void CustomInitiator(ExternalCommandData e)
        {
            // ExternalCommandData and Doc
            eData = e;
            doc = e.Application.ActiveUIDocument.Document;
            uidoc = eData.Application.ActiveUIDocument;
        }
       
    }
}
