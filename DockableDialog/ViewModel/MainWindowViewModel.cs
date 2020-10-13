using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DockableDialog.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using DockableDialog.EventHendler;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DockableDialog.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
    
        #region INotifyPropertyChanged realise
        public event PropertyChangedEventHandler PropertyChanged;
        public int GetPropertyChangedSubscribledLenght()
        {
            return PropertyChanged?.GetInvocationList()?.Length ?? 0;
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
        #endregion

        /// <summary>DoWork is a method in the TestClass class.
        /// <para>Here's how you could make a second paragraph in a description. <see cref="System.Console.WriteLine(System.String)"/> for information about output statements.</para>
        /// <seealso cref="TestClass.Main"/>
        /// </summary>
        private int valueWidthWindow;
        public int ValueWidthWindow
        {
            get { return valueWidthWindow; }
            set
            {
                valueWidthWindow = value;
                if (valueWidthWindow < 150)
                {
                    IsVisable = false;
                }
                else
                {
                    IsVisable = true;
                }
                RaisePropertyChanged("ValueWidthWindow");
            }
        }
        private bool isVisable;
        public bool IsVisable
        {
            get { return isVisable; }
            set
            {
                isVisable = value;
                RaisePropertyChanged("IsVisable");
            }
        }

        public static ObservableCollection<FamilyDto> FamilySymbolList = new ObservableCollection<FamilyDto>();
        public ICommand AddFamily { get; set; }
        public ICommand UseFamily { get; set; }

        public ExternalEvent ApplyEventGetFamily;
        public ExternalEvent ApplyPasteGetFamily;
        public ExternalEvent ApplyEventShowDialog;

        UIControlledApplication UIApp;
        public MainWindowViewModel(UIControlledApplication uIApp)
        {
            UIApp = uIApp;
            AddFamily = new RelayCommand(o => AddFamilyMethod("MainButton"));
            UseFamily = new RelayCommand(o => UseFamilyMethod(o));
            ValueWidthWindow = 250;
        }


        private ICollectionView famDtoList;

        public ICollectionView FamDtoList
        {
            get => famDtoList;
            set => SetProperty(ref famDtoList, value);
        }
        private void AddFamilyMethod(object o)
        {
            ApplyEventGetFamily.Raise();

            ActivateWindow();
            ImageSelect MyWindow = new ImageSelect();
            HwndSource hwndSource = HwndSource.FromHwnd(UIApp.MainWindowHandle);
            Window wnd = hwndSource.RootVisual as Window;
            if (wnd != null)
            {
                MyWindow.Owner = wnd;
                //MyWindow.ShowInTaskbar = false;
                MyWindow.Show();
            }

            

            FamDtoList = CollectionViewSource.GetDefaultView(FamilySymbolList);
            FamDtoList.Refresh();
        }
        private void UseFamilyMethod(object o)
        {
            PasteFamilyEventHendler.familyDto = o as FamilyDto;
            ApplyPasteGetFamily.Raise();
        }
        public static bool ActivateWindow()
        {
            Process p = Process.GetProcessesByName("Revit").FirstOrDefault();
            IntPtr ptr = p.MainWindowHandle;

            if (ptr != IntPtr.Zero)
            {
                return SetForegroundWindow(ptr);
            }

            return false;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
