﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfcurve.curve
{
    public class CurveModel : ViewModelBase
    {
        public delegate void DelegateNotify();
        public DelegateNotify? ViewUpdate;

        protected static double maxh = 4096;

        public void RequestRedraw()
        {
            if(ViewUpdate != null)
            {
                ViewUpdate();
            }
        }
    }
}
