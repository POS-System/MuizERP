using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.SampleEntity
{
    public class SampleEntityFilter : IFilter
    {
        int _valueMin;
        int _valueMax;
        string _userName;

        public int ValueMin
        {
            get
            {
                return _valueMin;
            }

            set
            {
                _valueMin = value;
            }
        }

        public int ValueMax
        {
            get
            {
                return _valueMax;
            }

            set
            {
                _valueMax = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }
    }
}
