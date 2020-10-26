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
using DockableDialog.Model;
using System.Threading;

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

        public MainWindowViewModel()
        {
            AddFamily = new RelayCommand(o => AddFamilyMethod("MainButton"));
            UseFamily = new RelayCommand(o => UseFamilyMethod(o));
            RemoveFamily = new RelayCommand(o => RemoveFamilyMethod(o));
            Refresh = new RelayCommand(o => RefreshMethod(o));
            AddFamilySet = new RelayCommand(o => AddFamilySetMethod(o));
            RemoveFamilySet = new RelayCommand(o => RemoveFamilySetMethod(o));

            ValueWidthWindow = 250;
            IsVisable = true;
            List<ImageDto> imageDtos = new List<ImageDto>();
            #region Добавление картинок в Combobox
            var files = Directory.GetFiles(@"C:\ProgramData\Autodesk\Revit\Addins\2019\FamilyPaletteImages");
            foreach (var item in files)
            {
                imageDtos.Add(new ImageDto(item));
            }
            //imageDtos.Add(new ImageDto("Resources/Поперечное сечение стержня_Б.bmp"));
            #endregion

            ImageDtoList = CollectionViewSource.GetDefaultView(imageDtos);
            ImageDtoList.Refresh();
            GetFamilySymbolEventHendler.ChangeUI += TimeStopAddFamily;
            GetFamilyByIdEventHendler.ChangedUIAndAddSerializer += RefreshMethodTimeStop;

            familySetDtos = XmlSerializerModel.ParamsXmlDeserializer();
            FamilySetDtoList = CollectionViewSource.GetDefaultView(familySetDtos);
            FamilySetDtoList.Refresh();
        }



        /// <summary>
        /// Поле ширины основного окна
        /// </summary>
        private int valueWidthWindow;
        public int ValueWidthWindow
        {
            get { return valueWidthWindow; }
            set
            {
                valueWidthWindow = value;
                //if (valueWidthWindow < 150)
                //{
                //    IsVisable = false;
                //}
                //else
                //{
                //    IsVisable = true;
                //}
                TextboxWidth = ValueWidthWindow - 135;
                ListboxWidth = ValueWidthWindow;
                RaisePropertyChanged("ValueWidthWindow");
            }
        }

        /// <summary>
        /// Поле видимости интерфейса
        /// </summary>
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

        /// <summary>
        /// Ширина ТекстБокса
        /// </summary>
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

        /// <summary>
        /// Ширина ЛистБокса
        /// </summary>
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

        /// <summary>
        /// Имя семейства
        /// </summary>
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

        #region ICommand комнады для кнопок
        /// <summary>
        /// Добавить семейство в список
        /// </summary>
        public ICommand AddFamily { get; set; }
        /// <summary>
        /// Использовать выбранное семейство для вставки в Revit
        /// </summary>
        public ICommand UseFamily { get; set; }
        /// <summary>
        /// Удалить выбранное семейство из списка
        /// </summary>
        public ICommand RemoveFamily { get; set; }
        /// <summary>
        /// Обновить список семейств из XML
        /// </summary>
        public ICommand Refresh { get; set; }
        /// <summary>
        /// Добавить новый набор семейств
        /// </summary>
        public ICommand AddFamilySet { get; set; }
        public ICommand RemoveFamilySet { get; set; }
        #endregion

        #region External revit events
        /// <summary>
        /// Получить семейства из Revit
        /// </summary>
        public ExternalEvent ApplyEventGetFamily;
        /// <summary>
        /// Вставить семейства в Revit
        /// </summary>
        public ExternalEvent ApplyPasteGetFamily;
        /// <summary>
        /// Получить семейства по ID (для распарсивание XML)
        /// </summary>
        public ExternalEvent ApplyEventGetFamilyById;
        #endregion

        /// <summary>
        /// Главный лист для Списка семейств
        /// </summary>
        public static ObservableCollection<FamilyDto> FamilySymbolList = new ObservableCollection<FamilyDto>();
        public static FamilyDto familyDto;
        /// <summary>
        /// Главный список наборов семейств
        /// </summary>
        ObservableCollection<FamilySetDto> familySetDtos = new ObservableCollection<FamilySetDto>();


        #region Все что касается отображения списка семейств        
        private ICollectionView famDtoList;
        /// <summary>
        /// Отображение списка семейств (из View)
        /// </summary>
        public ICollectionView FamDtoList
        {
            get => famDtoList;
            set => SetProperty(ref famDtoList, value);
        }
        /// <summary>
        /// Добавить семейство в список
        /// </summary>
        private void AddFamilyMethod(object o)
        {
            ApplyEventGetFamily.Raise();
        }
        /// <summary>
        /// Метод по событию, чтобы он срабатывал только после логики ревитовского кода
        /// </summary>
        public void TimeStopAddFamily(object obj)
        {
            try
            {
                SelectedFamilySet.FamiliesDto.Add(familyDto);
                FamDtoList = CollectionViewSource.GetDefaultView(SelectedFamilySet.FamiliesDto);
                FamDtoList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Использовать семейство для вставки в Revit
        /// </summary>
        private void UseFamilyMethod(object o)
        {
            PasteFamilyEventHendler.familyDto = o as FamilyDto;
            ApplyPasteGetFamily.Raise();
        }

        /// <summary>
        /// Удалить семейство из списка UI
        /// </summary>
        public void RemoveFamilyMethod(object o)
        {
            try
            {
                SelectedFamilySet.FamiliesDto.Remove(o as FamilyDto);
                FamDtoList = CollectionViewSource.GetDefaultView(SelectedFamilySet.FamiliesDto);
                FamDtoList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Обновить UI
        /// </summary>
        public void RefreshMethod(object obj)
        {
            File.Delete(@"C:\ProgramData\Autodesk\Revit\Addins\2019\FamilyPalette.xml");
            XmlSerializerModel.ParamsXmlSerializer(familySetDtos);
            MessageBox.Show("Изменения успешно внесены в базу данных", "Внимание");
        }

        #endregion


        #region Все что касается отображения списка иконок
        private ICollectionView imageDtoList;
        /// <summary>
        /// Отображение списка исконок (из View)
        /// </summary>
        public ICollectionView ImageDtoList
        {
            get => imageDtoList;
            set => SetProperty(ref imageDtoList, value);
        }

        private ImageDto p_SelectedItem;
        /// <summary>
        /// Выбранные элемент ComboBox
        /// </summary>
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
        #endregion


        #region Все что касается отображения наборов семейств
        private ICollectionView familySetDtoList;
        /// <summary>
        /// Отображение списка наборов семейства (из View)
        /// </summary>
        public ICollectionView FamilySetDtoList
        {
            get => familySetDtoList;
            set => SetProperty(ref familySetDtoList, value);
        }
       
        /// <summary>
        /// Забить имя набора семейств
        /// </summary>
        public string FamilySetName { get; set; }
        /// <summary>
        /// Создать новый набор семейств
        /// </summary>
        public void AddFamilySetMethod(object obj)
        {
            FamilySetDto familySetDto = new FamilySetDto();
            familySetDto.Name = FamilySetName;
            familySetDtos.Add(familySetDto);
            File.Delete(@"C:\ProgramData\Autodesk\Revit\Addins\2019\FamilyPalette.xml");
            XmlSerializerModel.ParamsXmlSerializer(familySetDtos);
            FamilySetDtoList = CollectionViewSource.GetDefaultView(familySetDtos);
            FamilySetDtoList.Refresh();
        }

        private FamilySetDto selectedFamilySet;
        /// <summary>
        /// Выбранные набор семейств
        /// </summary>
        public FamilySetDto SelectedFamilySet
        {
            get { return selectedFamilySet; }

            set
            {
                selectedFamilySet = value;
                RaisePropertyChanged("SelectedFamilySet");
                GetFamilyByIdEventHendler.familyDtos = SelectedFamilySet.FamiliesDto;
                ApplyEventGetFamilyById.Raise();
            }
        }
        public static ObservableCollection<FamilyDto> familyDtosForSelectItem = new ObservableCollection<FamilyDto>();
        /// <summary>
        /// Метод по событию, чтобы он срабатывал только после логики ревитовского кода
        /// </summary>
        public void RefreshMethodTimeStop(object obj)
        {
            GetFamilyByIdEventHendler.familyDtos.Clear();
            foreach (var item in familyDtosForSelectItem)
            {
                GetFamilyByIdEventHendler.familyDtos.Add(item);
            }
            FamDtoList = CollectionViewSource.GetDefaultView(SelectedFamilySet.FamiliesDto);
            FamDtoList.Refresh();
            familyDtosForSelectItem.Clear();
        }
        public void RemoveFamilySetMethod(object obj)
        {
            try
            {
                familySetDtos.Remove(obj as FamilySetDto);
                FamDtoList = CollectionViewSource.GetDefaultView(familySetDtos);
                FamDtoList.Refresh();
                File.Delete(@"C:\ProgramData\Autodesk\Revit\Addins\2019\FamilyPalette.xml");
                XmlSerializerModel.ParamsXmlSerializer(familySetDtos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        // TODO: Сделать тригер на отсутствие необходимых семейств в проекте

    }
}
