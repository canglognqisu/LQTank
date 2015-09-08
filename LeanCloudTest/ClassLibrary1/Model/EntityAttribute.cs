using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanCloudCommond.Model
{
    public class EntityAttribute:Attribute
    {
        public string Entity { get; set; }  
  
        public EntityAttribute() {    }
        public EntityAttribute(string Entity)
        {
            this.Entity = Entity;  
        }  
    }
}
