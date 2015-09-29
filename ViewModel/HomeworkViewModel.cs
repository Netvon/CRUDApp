using CRUDApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using CRUDApp.Helpers;

namespace CRUDApp.ViewModel
{
    public class HomeworkViewModel : ViewModelBase
    {
        #region Fields
        const string BrowseViewName = "BrowseView";
        IHomeworkRepository homeworkRepo;
        IWindowService windowService;
        int currentIndex;

        #region Backing Fields
        ObservableCollection<Homework> _homeworkList;
        Homework _selectedHomework;
        Homework _tempHomework;
        Visibility _isEditOpen;        
        #endregion

        #endregion

        #region Properties
        public ObservableCollection<Homework> HomeworkList
        {
            get { return _homeworkList; }
            set
            {
                _homeworkList = value;
                RaisePropertyChanged();
            }
        }

        public Homework SelectedHomework
        {
            get { return _selectedHomework; }
            set
            {
                _selectedHomework = value;
                currentIndex = HomeworkList.IndexOf(_selectedHomework);
                RaisePropertyChanged();
                GotoNextHomeworkCommand.RaiseCanExecuteChanged();
                GotoPreviousHomeworkCommand.RaiseCanExecuteChanged();
            }
        }

        public Homework TempHomework
        {
            get { return _tempHomework; }
            set
            {
                _tempHomework = value;
                RaisePropertyChanged(nameof(TempHomework));
                SaveHomeworkCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility IsEditOpen
        {
            get { return _isEditOpen; }
            set
            {
                _isEditOpen = value;
                RaisePropertyChanged();
                if (SaveHomeworkCommand != null)
                    SaveHomeworkCommand.RaiseCanExecuteChanged();
            }
        }

        #region Commands

        public RelayCommand SaveHomeworkCommand { get; set; }
        public RelayCommand CreateHomeworkCommand { get; set; }
        public RelayCommand<Guid> EditHomeworkCommand { get; set; }
        public RelayCommand ShowBrowseCommand { get; set; }
        public RelayCommand GotoNextHomeworkCommand { get; set; }
        public RelayCommand GotoPreviousHomeworkCommand { get; set; }
        public RelayCommand<Guid> DeleteHomeworkCommand { get; set; }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        public HomeworkViewModel(IHomeworkRepository homeworkRepo, IWindowService windowService)
        {
            IsEditOpen = Visibility.Hidden;
            currentIndex = 0;

            this.homeworkRepo = homeworkRepo;
            this.windowService = windowService;
            this.windowService.OnCanOpenWindowChanged += WindowService_OnCanOpenWindowChanged;
            HomeworkList = new ObservableCollection<Homework>(homeworkRepo.GetHomework());

            EditHomeworkCommand = new RelayCommand<Guid>(EditHomeWorkHandler);
            DeleteHomeworkCommand = new RelayCommand<Guid>(DeleteHomeWorkHandler);
            SaveHomeworkCommand = new RelayCommand(SaveHomeworkHandler, CanSaveHomework);
            CreateHomeworkCommand = new RelayCommand(CreateHomeworkHandler);
            ShowBrowseCommand = new RelayCommand(ShowBrowseHandler, CanShowBrowse);

            GotoNextHomeworkCommand = new RelayCommand(GotoNextHomeworkHandler, CanGotoNextHomework);
            GotoPreviousHomeworkCommand = new RelayCommand(GotoPreviousHomeworkHandler, CanGotoPreviousHomework);
        }

        #endregion

        #region Event Handlers

        void TempHomework_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsEditOpen == Visibility.Visible)
                SaveHomeworkCommand.RaiseCanExecuteChanged();
        }

        void WindowService_OnCanOpenWindowChanged(object sender, CanOpenWindowEventArgs e)
        {
            if (e.WindowName.Contains(BrowseViewName))
                ShowBrowseCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Command Handlers

        void ShowBrowseHandler()
        {
            windowService.OpenWindow(BrowseViewName);
            ShowBrowseCommand.RaiseCanExecuteChanged();
        }

        void EditHomeWorkHandler(Guid obj)
        {
            TempHomework = new Homework(HomeworkList.First(hw => hw.Guid == obj));
            TempHomework.PropertyChanged += TempHomework_PropertyChanged;
            IsEditOpen = Visibility.Visible;
        }

        void DeleteHomeWorkHandler(Guid obj)
        {
            var homework = HomeworkList.First(hw => hw.Guid == obj);
            HomeworkList.Remove(homework);

            RaisePropertyChanged(nameof(SelectedHomework));
            GotoNextHomeworkCommand.RaiseCanExecuteChanged();
            GotoPreviousHomeworkCommand.RaiseCanExecuteChanged();
        }


        void CreateHomeworkHandler()
        {
            IsEditOpen = Visibility.Visible;
            TempHomework = new Homework() { DueDate = DateTime.Today };
            TempHomework.PropertyChanged += TempHomework_PropertyChanged;            
        }

        void SaveHomeworkHandler()
        {
            var target = HomeworkList.FirstOrDefault(hw => hw.Guid == TempHomework.Guid);

            if (target == null)
            {
                HomeworkList.Add(TempHomework);
            }
            else
            {
                target.Course = TempHomework.Course;
                target.Summary = TempHomework.Summary;
                target.DueDate = TempHomework.DueDate;
            }

            IsEditOpen = Visibility.Hidden;
            TempHomework.PropertyChanged -= TempHomework_PropertyChanged;
        }

        void GotoNextHomeworkHandler()
        {
            currentIndex++;
            SelectedHomework = HomeworkList[currentIndex];
        }

        void GotoPreviousHomeworkHandler()
        {
            currentIndex--;
            SelectedHomework = HomeworkList[currentIndex];
        }

        #endregion

        #region Command Predicates

        bool CanSaveHomework()
        {
            if (TempHomework == null)
                return false;

            if (string.IsNullOrWhiteSpace(TempHomework.Course) ||
                string.IsNullOrWhiteSpace(TempHomework.Summary))
                return false;

            return true;
        }

        bool CanGotoNextHomework()
        {
            if (currentIndex + 1 < HomeworkList.Count)
                return true;

            return false;
        }

        bool CanGotoPreviousHomework()
        {
            if (currentIndex > 0)
                return true;

            return false;
        }

        bool CanShowBrowse()
        {
            return windowService.CanOpenWindow(BrowseViewName);
        }

        #endregion
    }
}