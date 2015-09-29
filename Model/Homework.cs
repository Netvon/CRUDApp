using GalaSoft.MvvmLight;
using System;

namespace CRUDApp.Model
{
    public class Homework : ObservableObject
    {
        #region Fields

        string _course;
        DateTime _dueDate;
        string _summary;
        bool _completed;

        #endregion

        #region Properties
        public Guid Guid { get; }

        public string Course
        {
            get { return _course; }
            set
            {
                _course = value;
                RaisePropertyChanged(nameof(Course));
            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                RaisePropertyChanged(nameof(DueDate));
            }
        }

        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged(nameof(Summary));
            }
        }

        public bool Completed
        {
            get { return _completed; }
            set
            {
                _completed = value;
                RaisePropertyChanged(nameof(Completed));
            }
        }

        #endregion

        #region Constructors
        public Homework()
        {
            Guid = Guid.NewGuid();
        }

        public Homework(Homework cloneOf)
        {
            Course = cloneOf.Course;
            Guid = cloneOf.Guid;
            DueDate = cloneOf.DueDate;
            Summary = cloneOf.Summary;
            Completed = cloneOf.Completed;
        }
        #endregion
    }
}
