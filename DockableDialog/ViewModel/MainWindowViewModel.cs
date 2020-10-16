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
                TextboxWidth = ValueWidthWindow - 125;
                ListboxWidth = ValueWidthWindow;
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
        

        UIControlledApplication UIApp;
        public MainWindowViewModel(UIControlledApplication uIApp)
        {
            UIApp = uIApp;
            AddFamily = new RelayCommand(o => AddFamilyMethod("MainButton"));
            UseFamily = new RelayCommand(o => UseFamilyMethod(o));
            ValueWidthWindow = 250;

            List<ImageDto> imageDtos = new List<ImageDto>();

            imageDtos.Add(new ImageDto("Resources/green.png"));
            imageDtos.Add(new ImageDto("Resources/orange.png"));
            imageDtos.Add(new ImageDto("Resources/red.png"));
            ImageDtoList = CollectionViewSource.GetDefaultView(imageDtos);
            ImageDtoList.Refresh();
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

            FamDtoList = CollectionViewSource.GetDefaultView(FamilySymbolList);
            FamDtoList.Refresh();
        }
        private void UseFamilyMethod(object o)
        {
            PasteFamilyEventHendler.familyDto = o as FamilyDto;
            ApplyPasteGetFamily.Raise();
        }

        private ICollectionView imageDtoList;

        public ICollectionView ImageDtoList
        {
            get => imageDtoList;
            set => SetProperty(ref imageDtoList, value);
        }

        private ImageDto p_SelectedItem;
        public ImageDto SelectedItem
        {
            get { return p_SelectedItem; }

            set
            {
                p_SelectedItem = value;
                GetFamilySymbolEventHendler.Path = SelectedItem.Path;
                RaisePropertyChanged("SelectedItem");
            }
        }
        private int textboxWidth;

        public int TextboxWidth
        {
            get { return textboxWidth; }
            set 
            { 
                textboxWidth = value;
                RaisePropertyChanged("TextboxWidth");
            }
        }


        private int listboxWidth;

        public int ListboxWidth
        {
            get { return listboxWidth; }
            set 
            { 
                listboxWidth = value;
                RaisePropertyChanged("ListboxWidth");

            }
        }

        private string familyName;
        public string FamilyName
        {
            get { return familyName; }
            set 
            { 
                familyName = value;
                GetFamilySymbolEventHendler.FamilyName = FamilyName;
            }
        }
    }
}
