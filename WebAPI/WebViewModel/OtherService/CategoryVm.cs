using System;
using System.Collections.Generic;
using System.Text;

namespace WebViewModel.OtherService
{
    public class CategoryVm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
