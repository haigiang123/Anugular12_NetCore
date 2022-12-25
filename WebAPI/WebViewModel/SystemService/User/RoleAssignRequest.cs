using System;
using System.Collections.Generic;
using System.Text;
using WebViewModel.Common;

namespace WebViewModel.SystemService.User
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
