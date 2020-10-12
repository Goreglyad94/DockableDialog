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
        public static ObservableCollection<FamilyDto> FamilySymbolList = new ObservableCollection<FamilyDto>();
        public ICommand AddFamily { get; set; }
        public ICommand UseFamily { get; set; }

        public ExternalEvent ApplyEventGetFamily;
        public ExternalEvent ApplyPasteGetFamily;
        public MainWindowViewModel()
        {
            AddFamily = new RelayCommand(o => AddFamilyMethod("MainButton"));
            UseFamily = new RelayCommand(o => UseFamilyMethod(o));
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
    }
}
