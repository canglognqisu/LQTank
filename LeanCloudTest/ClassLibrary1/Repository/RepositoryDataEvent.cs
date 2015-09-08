using LeanCloudCommond.Model;
using LeanCloudCommond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanCloudCommond.Repository
{
    public delegate void AddDelegate(RepositoryEventData eventData);
    public delegate void UpdateDelegate(RepositoryEventData eventData);
    public delegate void DeleteDelegate(RepositoryEventData eventData);
    public delegate void FindDelegate(RepositoryEventData eventData);
    public delegate void FindAllDelegate(RepositoryEventData eventData);
   
    public class RepositoryEventData
    {
        public object Data { get; set; }
        public object EventSource { get; set; }
        public ExcuteResult Result { get; set; }
    }
    
}
