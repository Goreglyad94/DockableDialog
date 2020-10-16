using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using DockableDialog.EventHendler;
using DockableDialog.Properties;
using DockableDialog.ViewModel;

namespace DockableDialog
{
    public class MainClass : IExternalApplication
    {
        Viewer dockableWindow = null;
        ExternalCommandData edata = null;
        public void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            dockableWindow.CustomInitiator(edata);
        }
        public Result OnStartup(UIControlledApplication application)
        {
            #region Регистрация нового окна, новый ViewModel, регистрация нового внешнего события
            Viewer dock = new Viewer();
            dockableWindow = dock;

            // create a new dockable pane id
            DockablePaneId id = new DockablePaneId(new Guid("{68D44FAC-CF09-46B2-9544-D5A3F809373C}"));
            try
            {
                // register dockable pane
                application.RegisterDockablePane(id, "TwentyTwo Dockable Sample", dockableWindow as IDockablePaneProvider);
                //TaskDialog.Show("Info Message", "Dockable window have registered!");
                // subscribe document opened event
                //application.Application.DocumentOpened += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(Application_DocumentOpened);
                // subscribe view activated event
                application.ViewActivated += new EventHandler<ViewActivatedEventArgs>(Application_ViewActivated);
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }
            #endregion



            RibbonPanel ribbonPanel = application.CreateRibbonPanel(Tab.AddIns, "TwentyTwo Sample");
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyPath = assembly.Location;


            //PushButton registerButton = ribbonPanel.AddItem(new PushButtonData("Register Window", "Register", assemblyPath, "DockableDialog.Register")) as PushButton;

            //registerButton.AvailabilityClassName = "DockableDialog.CommandAvailability";
            // btn tooltip 
            //registerButton.ToolTip = "Register dockable window at the zero document state.";
            
            // register button icon images
            //registerButton.LargeImage = GetResourceImage(assembly, "RevitAddInsWPFSample.Resources.register32.png");
            //registerButton.Image = GetResourceImage(assembly, "RevitAddInsWPFSample.Resources.register16.png");

            // Create Show Button
            PushButton showButton = ribbonPanel.AddItem(new PushButtonData("Show Window", "Show", assemblyPath, "DockableDialog.Show")) as PushButton;
            // btn tooltip
            showButton.ToolTip = "Show the registered dockable window.";
            // show button icon images
            //showButton.LargeImage = GetResourceImage(assembly, "Resources.green.png");
            //showButton.Image = GetResourceImage(assembly, "Resources.green.png");

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(application);
            //ImageSelectViewModel imageSelectViewModel = new ImageSelectViewModel();

            GetFamilySymbolEventHendler registerEventHendler = new GetFamilySymbolEventHendler();
            PasteFamilyEventHendler pasteFamilyEventHendler = new PasteFamilyEventHendler();


            ExternalEvent ExEventGetFamily = ExternalEvent.Create(registerEventHendler);
            ExternalEvent ExEventPasteFamily = ExternalEvent.Create(pasteFamilyEventHendler);



            mainWindowViewModel.ApplyEventGetFamily = ExEventGetFamily;
            mainWindowViewModel.ApplyPasteGetFamily = ExEventPasteFamily;


            dockableWindow.DataContext = mainWindowViewModel;
            //imageSelect.DataContext = imageSelectViewModel;
            // return status
            return Result.Succeeded;
        }


        // execute when application close
        public Result OnShutdown(UIControlledApplication application)
        {
            // return status
            return Result.Succeeded;

        }

        // get embedded images from assembly resources
        public ImageSource GetResourceImage(Assembly assembly, string imageName)
        {
            try
            {
                // bitmap stream to construct bitmap frame
                Stream resource = assembly.GetManifestResourceStream(imageName);
                // return image data
                return BitmapFrame.Create(resource);
            }
            catch
            {
                return null;
            }
        }

    }

    [Transaction(TransactionMode.Manual)]
    public class Show : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // dockable window id
                DockablePaneId id = new DockablePaneId(new Guid("{68D44FAC-CF09-46B2-9544-D5A3F809373C}"));
                DockablePane dockableWindow = commandData.Application.GetDockablePane(id);
                dockableWindow.Show();
                
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }
            // return result
            return Result.Succeeded;
        }
    }
}
