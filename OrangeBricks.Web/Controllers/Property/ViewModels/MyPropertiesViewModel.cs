using System.Collections.Generic;

namespace OrangeBricks.Web.Controllers.Property.ViewModels
{
    public class MyPropertiesViewModel
    {
        public bool HasProperties { get { return Properties != null
                                                 && Properties.Count > 0; } }
        public List<PropertyViewModel> Properties { get; set; }
    }
}