using Entities.Base;
using Entities.Base.Attributes;
using System;

namespace Entities.SampleEntity
{   
    [SaveCommand("xp_SaveSampleEntityDetails")]
    public class SampleEntityDetails : BaseEntity
    {
        #region Поля        
        private int _sampleEntityID;
        private int _value;
        private string _description;
        #endregion

        #region Свойства

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

        #endregion
    }
}
