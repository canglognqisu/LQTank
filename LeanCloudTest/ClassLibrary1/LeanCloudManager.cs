using AVOSCloud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanCloudCommond
{
    public class LeanCloudManager
    {
        public static void LeanCloudInitialize()
        {
            AVClient.Initialize("gauwwfvxglzir7eh5fypz16vqinzh4ome93w7cythgj316zp", "wqm7b0w032mwjepsuw9kjtfguy2cjsanmtc29c4bjt0q9jsb", AVRegion.CN);
        }
    }
}
