using Entities.Base.Attributes;
using MuizEnums;
using System;
using System.ComponentModel;
using System.Data;

namespace Entities.Base
{
    public class BaseEntity : IBaseEntity, INotifyPropertyChanged
    {
        #region Fields

        protected Int32 _ID;
        private EState state;
        protected Int32 _companyID;
        protected DateTime _createDate;
        protected DateTime _modifyDate;
        protected Int32 _createByUserID;
        protected Int32 _modifyByUserID;
        protected byte[] _timeStamp;

        #endregion

        #region Properties

        [LoadParameter]
        [SaveParameter(Direction = ParameterDirection.InputOutput)]
        public virtual int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }
        protected EState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        [LoadParameter()]
        [SaveParameter]
        public virtual int CompanyID
        {
            get { return _companyID; }
            set
            {
                _companyID = value;
                OnPropertyChanged("CompanyID");
            }
        }

        [LoadParameter]        
        public DateTime CreatedDate
        {
            get { return _createDate; }
            set
            {
                _createDate = value;
                OnPropertyChanged("CreateDate");
            }
        }

        [LoadParameter]
        public int CreatedByUserID
        {
            get { return _createByUserID; }
            set
            {
                _createByUserID = value;
                OnPropertyChanged("CreateByUserID");
            }
        }

        [LoadParameter]       
        public virtual DateTime ModifyDate
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
        public virtual int ModifyByUserID
        {
            get { return _modifyByUserID; }
            set
            {
                _modifyByUserID = value;
                OnPropertyChanged("ModifyByUserID");
            }
        }

        [LoadParameter]
        [SaveParameter(Nullable = true)]
        public virtual byte[] TimeStamp
        {
            get
            {
                return _timeStamp;
            }

            set
            {
                _timeStamp = value;
                OnPropertyChanged("TimeStamp");
            }
        }        

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            State = EState.Update;
        }
    }
}
