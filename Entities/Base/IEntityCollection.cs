﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Base
{
    interface IEntityCollection
    {
        bool IsChanged { get; }
        void FixValues();
        void ResetState();
    }
}
