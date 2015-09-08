using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanCloudCommond.Model;

namespace LeanCloudModel
{
    [Serializable]
    public class RegUsers : ModelBase
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Nation { get; set; }
        //public Sex Sex { get; set; }
        
        [EntityAttribute("Entity")]
        public UserProfile UserProfile { get; set; }
    }
    public enum Sex
    { 
        Male=1,
        Female=2
    }
}
