using AVOSCloud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeanCloudCommond;
using LeanCloudCommond.Repository;
using LeanCloudModel;

namespace LeanCloudTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LeanCloudManager.LeanCloudInitialize();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Class1 c=new Class1();
            //c.aa();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegUsers user = new RegUsers()
            {
                Name = "王振鑫",
                Nation = "汉族",
                Password = "222222",
                UserName = "haha6",
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                UserProfile = new UserProfile() { Address="七里河", Age=32, Phone="13919089657" }
            };
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.Add(user);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.FindEvent += rep_FindEvent;
            rep.Find("55eafc98ddb263b55d2bad39");
            
        }

        void rep_FindEvent(RepositoryEventData eventData)
        {
            var lean = eventData.EventSource as LeanCloudRepository<RegUsers>;
            lean.FindEvent-=rep_FindEvent;
            RegUsers user = eventData.Data as RegUsers;
            MessageBox.Show(user.UserName);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            RegUsers user = new RegUsers()
            {
                Name = "王振鑫",
                Nation = "汉族",
                Password = "111111",
                UserName = "haha",
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                objectId = "55eafc98ddb263b55d2bad39",
                UserProfile = new UserProfile() { Address="七里河", Age=33, Phone="13919873455" }
            };
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.Update(user);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RegUsers user = new RegUsers()
            {
                Name = "王振鑫",
                Nation = "汉族",
                Password = "222222",
                UserName = "haha",
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                objectId = "55e9409f60b2fb7557baa51b"
            };
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.Delete(user);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.FindAllEvent += rep_FindAllEvent;
            rep.FindAll();

        }


        private void button7_Click(object sender, EventArgs e)
        {
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.FindEvent+=rep_FindEvent;
            Query query=new Query();
            query.AddParam(new QueryParam("UserName", "haha1", QuerySign.EQ, ConnectSign.AND, true))
                .AddParam(new QueryParam("Password", "222222", QuerySign.EQ, ConnectSign.AND, true));
            rep.FindByCondition(query);
        }

        void rep_FindAllEvent(RepositoryEventData eventData)
        {
            var lean = eventData.EventSource as LeanCloudRepository<RegUsers>;
            lean.FindAllEvent -= rep_FindAllEvent;
            List<RegUsers> user = eventData.Data as List<RegUsers>;
            MessageBox.Show(user.Count.ToString());
        }
        private void button8_Click(object sender, EventArgs e)
        {
            LeanCloudRepository<RegUsers> rep = new LeanCloudRepository<RegUsers>();
            rep.FindAllEvent+=rep_FindAllEvent;
            Query query = new Query();
            query.AddParam(new QueryParam("UserName", "6", QuerySign.LIKE, ConnectSign.AND, true));
            rep.FindAllByCondition(query);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }

    }
}
