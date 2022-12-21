using System;
using System.Collections.Generic;
using System.Text;
using WebViewModel.Common;

namespace WebViewModel.OtherService
{
    public class CategoryAssignRequest
    {
        public int Id { get; set; }
        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();
    }
}
