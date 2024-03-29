﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebViewModel.Common
{
    public class PageResultBase<T>
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageCount 
        { 
            get {
                return (int)Math.Ceiling((double)(Total / Size));
            } 
        }

        public List<T> Items { get; set; }

    }
}
