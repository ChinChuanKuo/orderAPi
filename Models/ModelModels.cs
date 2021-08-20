using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace orderAPi.Models
{
    public class userModels
    {
        public Dictionary<string, object> client { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
    }

    /*public class dataModels
    {
        public Dictionary<string, object> item { get; set; }
        public List<Dictionary<string, object>> items { get; set; }
    }*/

    /*public class sitemModels
    {
        public Dictionary<string, object> item { get; set; }
    }*/
}