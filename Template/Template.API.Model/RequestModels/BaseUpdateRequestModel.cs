using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.API.Model.RequestModels
{
    public class BaseUpdateRequestModel
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
    }
}
