using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DockableDialog.ViewModel
{
    class ImageSelectViewModel : INotifyPropertyChanged
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

        public ImageSelectViewModel()
        {
            GetAllResourceFile();
        }

        private ICollectionView imageResourcesList;

        public ICollectionView ImageResourcesList
        {
            get => imageResourcesList;
            set => SetProperty(ref imageResourcesList, value);
        }

        public void GetAllResourceFile()
        {
            ResourceSet rsrcSet = DockableDialog.Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, false, true);
            foreach (DictionaryEntry entry in rsrcSet)
            {
                MessageBox.Show(entry.Key.ToString());
            }
        }
    }
}
