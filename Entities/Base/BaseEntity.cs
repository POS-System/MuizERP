using Entities.Base.Attributes;
using System;
using System.ComponentModel;
using System.Data;

namespace Entities.Base
{
    public class BaseEntity : IBaseEntity, INotifyPropertyChanged
    {
        #region Fields

        private Int32 _ID;
        private Int32 _companyID;
        private DateTime _createDate;
        private DateTime _modifyDate;
        private Int32 _createByUserID;
        private Int32 _modifyByUserID;

        #endregion

        #region Properties

        [LoadParameter]
        [SaveParameter(Direction = ParameterDirection.InputOutput)]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        [LoadParameter(Required = false)]
        [SaveParameter]
        public int CompanyID
        {
            get { return _companyID; }
            set
            {
                _companyID = value;
                OnPropertyChanged("CompanyID");
            }
        }

        [LoadParameter]        
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                _createDate = value;
                OnPropertyChanged("CreateDate");
            }
        }

        [LoadParameter]
        public int CreateByUserID
        {
            get { return _createByUserID; }
            set
            {
                _createByUserID = value;
                OnPropertyChanged("CreateByUserID");
            }
        }

        [LoadParameter]       
        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set
            {
                _modifyDate = value;
                OnPropertyChanged("ModifyDate");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public int ModifyByUserID
        {
            get { return _modifyByUserID; }
            set
            {
                _modifyByUserID = value;
                OnPropertyChanged("ModifyByUserID");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
