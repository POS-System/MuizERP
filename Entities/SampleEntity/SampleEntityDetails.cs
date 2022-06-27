using Entities.Base.Attributes;
using System;

namespace Entities.SampleEntity
{
    [LoadCommand("ID")]
    [SaveCommand("xp_SaveSampleEntityDetails")]
    public class SampleEntityDetails
    {
        #region Поля

        private int _id;
        private int _sampleEntityID;
        private int _value;
        private string _description;
        private DateTime _createdDate;
        private DateTime _lastModifiedDate;
        private int _createdByUserID;
        private int _lastModifiedByUserID;

        #endregion

        #region Свойства

        [LoadParameter]
        [SaveParameter]
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                    _id = value;
            }
        }

        [LoadParameter]
        [SaveParameter]
        public int SampleEntityID
        {
            get { return _sampleEntityID; }
            set
            {
                if (_sampleEntityID != value)
                    _sampleEntityID = value;
            }
        }

        [LoadParameter]
        [SaveParameter]
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                    _value = value;
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                    _description = value;
            }
        }

        [LoadParameter]
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set
            {
                if (_createdDate != value)
                    _createdDate = value;
            }
        }

        [LoadParameter]
        public DateTime LastModifiedDate
        {
            get { return _lastModifiedDate; }
            set
            {
                if (_lastModifiedDate != value)
                    _lastModifiedDate = value;
            }
        }

        [LoadParameter]
        public int CreatedByUserID
        {
            get { return _createdByUserID; }
            set
            {
                if (_createdByUserID != value)
                    _createdByUserID = value;
            }
        }

        [LoadParameter]
        [SaveParameter]
        public int LastModifiedByUserID
        {
            get { return _lastModifiedByUserID; }
            set
            {
                if (_lastModifiedByUserID != value)
                    _lastModifiedByUserID = value;
            }
        }

        #endregion
    }
}
