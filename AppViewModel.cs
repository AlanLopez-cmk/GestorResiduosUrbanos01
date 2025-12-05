using System.Collections.Generic;

namespace CleanCityWeb.Models
{
    public class AppViewModel
    {
        public AppInfo Info { get; set; }
        public List<AppComment> Comments { get; set; }
        public double AverageRating { get; set; }
    }
}
