﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IMsgSchedule
    {
        public void Send(byte[] message);
    }
}