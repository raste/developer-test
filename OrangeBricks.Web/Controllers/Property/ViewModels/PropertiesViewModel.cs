using System.Collections.Generic;

namespace OrangeBricks.Web.Controllers.Property.ViewModels
{
    public class PropertiesViewModel
    {
        public bool IsSeller { get; set; }
        public List<PropertyViewModel> Properties { get; set; }
        public string Search { get; set; }
    }
}