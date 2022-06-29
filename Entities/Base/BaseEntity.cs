using Entities.Base.Attributes;
using System;
using System.ComponentModel;

namespace Entities.Base
{
    public class BaseEntity : INotifyPropertyChanged
    {
        private Int32 _ID;
        private Int32 _companyID;
        private Int32 _modifyUserID;        
        
        private DateTime _createdDate;
        private DateTime _lastModifiedDate;
        private Int32 _createdByUserID;
        private Int32 _lastModifiedByUserID;

        [LoadParameter]
        [SaveParameter]
        public int ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public int CompanyID
        {
            get
            {
                return _companyID;
            }

            set
            {
                _companyID = value;
                OnPropertyChanged("CompanyID");
            }
        }
        
        [SaveParameter]
        public int ModifyUserID
        {
            get
            {
                return _modifyUserID;
            }

            set
            {
                _modifyUserID = value;
                OnPropertyChanged("ModifyUserID");
            }
        }

        [LoadParameter]        
        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }

            set
            {
                _createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }

        [LoadParameter]       
        public DateTime LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }

            set
            {
                _lastModifiedDate = value;
                OnPropertyChanged("LastModifiedDate");
            }
        }

        [LoadParameter]
        public int CreatedByUserID
        {
            get
            {
                return _createdByUserID;
            }

            set
            {
                _createdByUserID = value;
                OnPropertyChanged("CreatedByUserID");
            }
        }

        [LoadParameter]
        public int LastModifiedByUserID
        {
            get
            {
                return _lastModifiedByUserID;
            }

            set
            {
                _lastModifiedByUserID = value;
                OnPropertyChanged("LastModifiedByUserID");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseEntity()
        {

        }
    }
}
