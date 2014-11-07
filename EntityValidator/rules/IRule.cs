﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public interface IRule
    {
        Boolean validate();
        void throwException();
    }
}