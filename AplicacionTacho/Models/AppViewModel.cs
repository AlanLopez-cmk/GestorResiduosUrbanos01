using System.Collections.Generic;

namespace AplicacionTacho.Models
{
    public class AppViewModel
    {
        public AppInfo Info { get; set; }
        public List<AppComment> Comments { get; set; }
        public double AverageRating { get; set; }
    }
}
