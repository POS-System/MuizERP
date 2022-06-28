using Entities.Base;
using Entities.Base.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entities.SampleEntityDetailsN;

namespace Entities.SampleEntityN
{
    [SaveCommand]
    [LoadCommand]
    public class SampleEntity : BaseEntity
    {
        #region Поля
        
        private int _value;
        private string _description;        
        private ObservableCollection<SampleEntityDetails> _sampleEntityDetailsList;

        #endregion

        #region Свойства       

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

        [SaveParameter]
        public ObservableCollection<SampleEntityDetails> SampleEntityDetailsList
        {
            get { return _sampleEntityDetailsList; }
            set
            {
                if (_sampleEntityDetailsList != value)
                    _sampleEntityDetailsList = value;
            }
        }

        #endregion

        #region Constructor

        public SampleEntity()
        {
            _sampleEntityDetailsList = new ObservableCollection<SampleEntityDetails>();
        }

        #endregion
    }
}
