﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imprest.Data
{
    interface IDataConnection
    {
         void  openConnection();
        void closeConnection();
    }
}
