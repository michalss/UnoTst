using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoTST.Models
{
    public class MRepository
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public string? url { get; set; }
        public string? logo { get; set; }
    }

    public class MPluginRepository
    {
        public string? version { get; set; }
        public string? creator { get; set; }
        public string? name { get; set; }

        [NotMapped]
        public string? image { get; set; } = "";
        public string? logo { get; set; } = "";
        public string? plugin_type { get; set; }
        public List<String>? links { get; set; }
    }
}
