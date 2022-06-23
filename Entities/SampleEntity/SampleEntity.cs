using Entities.Base.Attributes;
using System;
using System.Collections.Generic;

namespace Entities.SampleEntity
{
    [LoadCommand("ID")]
    [SaveCommand("xp_SetSampleEntity")]
    public class SampleEntity
    {
        #region Поля

        private int _id;
        private int _value;
        private string _description;
        private DateTime _createdDate;
        private DateTime _lastModifiedDate;
        private int _createdByUserID;
        private int _lastModifiedByUserID;
        private IEnumerable<SampleEntityDetails> _sampleEntityDetailsList;

        #endregion

        #region Свойства

        [LoadParameter("ID")]
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
        [SaveParameter]
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
        [SaveParameter]
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
        [SaveParameter]
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

        public IEnumerable<SampleEntityDetails> SampleEntityDetailsList
        {
            get { return _sampleEntityDetailsList; }
            set
            {
                if (_sampleEntityDetailsList != value)
                    _sampleEntityDetailsList = value;
            }
        }

        #endregion
    }
}
