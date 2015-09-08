using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanCloudCommond.Model;
using System.Reflection;
using AVOSCloud;
using System.Threading;
using Newtonsoft;

namespace LeanCloudCommond.Repository
{
    public class LeanCloudRepository<T> where T:ModelBase
    {
        public event AddDelegate AddEvent;
        public event UpdateDelegate UpdateEvent;
        public event DeleteDelegate DeleteEvent;
        public event FindDelegate FindEvent;
        public event FindAllDelegate FindAllEvent;
        public async void Add(T t)
        {
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = t.GetType();
                AVObject obj = new AVObject(type.Name);
                PropertyInfo[] proList = type.GetProperties();
                for (int i = 0; i < proList.Length; i++)
                {
                    PropertyInfo info = proList[i];
                    object attr = info.GetCustomAttribute(typeof(EntityAttribute), true);
                    if (attr != null)
                    {
                        EntityAttribute entityAttr = attr as EntityAttribute;
                        if (entityAttr.Entity == "Entity")
                        {
                            var entity = info.GetValue(t);
                            obj[info.Name] = JsonHelper.SerializeObject(entity);
                        }
                    }
                    else
                    {
                        obj[info.Name] = info.GetValue(t);
                    }
                }
                Task addTask = obj.SaveAsync();
                await addTask;
                ex.IsSuccess = true;
            }
            catch (Exception e)
            {
                ex.IsSuccess = false;
                ex.ErroeMessage = e.Message;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.EventSource = this;
            red.Result = ex;
            if(AddEvent!=null)
                AddEvent(red);
            
        }
        public async void Delete(T t)
        {
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = t.GetType();
                AVObject obj = new AVObject(type.Name);
                obj.ObjectId = type.GetProperty("objectId").GetValue(t).ToString();
                await obj.DeleteAsync();
                ex.IsSuccess = true;
            }
            catch(Exception e)
            {
                ex.IsSuccess = false;
                ex.ErroeMessage = e.Message;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.EventSource = this;
            red.Result = ex;
            if(DeleteEvent!=null)
                DeleteEvent(red);

        }
        public async void Delete(string objectId,Type type)
        {
            ExcuteResult ex = new ExcuteResult();
            try
            {
                AVObject obj = new AVObject(type.Name);
                obj.ObjectId = objectId;
                await obj.DeleteAsync();
            }
            catch (Exception e)
            {
                ex.IsSuccess = false;
                ex.ErroeMessage = e.Message;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.EventSource = this;
            red.Result = ex;
            if (DeleteEvent != null)
                DeleteEvent(red); ;
        }
        public async void Update(T t)
        {
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = t.GetType();
                AVObject obj = new AVObject(type.Name);
                obj.ObjectId = type.GetProperty("objectId").GetValue(t).ToString();
                PropertyInfo[] proList = type.GetProperties();
                for (int i = 0; i < proList.Length; i++)
                {
                    PropertyInfo info = proList[i];
                    object attr = info.GetCustomAttribute(typeof(EntityAttribute), true);
                    if (attr != null)
                    {
                        EntityAttribute entityAttr = attr as EntityAttribute;
                        if (entityAttr.Entity == "Entity")
                        {
                            var entity = info.GetValue(t);
                            obj[info.Name] = JsonHelper.SerializeObject(entity);
                        }
                    }
                    else
                    {
                        obj[info.Name] = info.GetValue(t);
                    }
                }
                await obj.SaveAsync().ContinueWith(o =>
                {

                    obj.SaveAsync();
                });
                ex.IsSuccess = true;
            }
            catch(Exception e)
            {
                ex.IsSuccess = false;
                ex.ErroeMessage = e.Message;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.EventSource = this;
            red.Result = ex;
            if(UpdateEvent!=null)
                UpdateEvent(red);
        }

        public async void Find(string objectId)
        {
            T t = null;
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = typeof(T);
                AVQuery<AVObject> query = AVObject.GetQuery(type.Name);
                AVObject obj = await query.GetAsync(objectId);
                t = ToEntity(obj);
                ex.IsSuccess = true;
            }
            catch (Exception e)
            {
                ex.ErroeMessage = e.Message;
                ex.IsSuccess = false;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.Data = t;
            red.Result = ex;
            red.EventSource = this;
            if(FindEvent!=null)
                FindEvent(red);
        }
        
        public async void FindAll()
        {
            List<T> list = null;
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = typeof(T);
                AVQuery<AVObject> query = new AVQuery<AVObject>(type.Name);
                await query.FindAsync().ContinueWith(t =>
                {
                    IEnumerable<AVObject> persons = t.Result;
                    list = ToEntityList(persons);
                });
                ex.IsSuccess = true;
            }
            catch (Exception e)
            {
                ex.ErroeMessage = e.Message;
                ex.IsSuccess = false;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.Data = list;
            red.Result = ex;
            red.EventSource = this;
            if (FindAllEvent != null)
                FindAllEvent(red);
        }
        public async void FindByCondition(Query queryObj)
        {
            T t = null;
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = typeof(T);
                AVQuery<AVObject> query = AVObject.GetQuery(type.Name);
                IEnumerable<AVObject> obj = await AVQuery<AVObject>.DoCloudQuery(queryObj.QuerySQL<T>());
                if (obj.Count() == 0)
                {
                    ex.IsSuccess = false;
                    ex.ErroeMessage = "该条件未能获得查询对象";
                }
                else
                {
                    t = ToEntity(obj.First());
                    ex.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ex.ErroeMessage = e.Message;
                ex.IsSuccess = false;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.Data = t;
            red.Result = ex;
            red.EventSource = this;
            if (FindEvent != null)
                FindEvent(red);
        }
        public async void FindAllByCondition(Query queryObj)
        {
            List<T> list = null;
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = typeof(T);
                AVQuery<AVObject> query = AVObject.GetQuery(type.Name);
                IEnumerable<AVObject> obj = await AVQuery<AVObject>.DoCloudQuery(queryObj.QuerySQL<T>());
                if (obj.Count() == 0)
                {
                    ex.IsSuccess = false;
                    ex.ErroeMessage = "该条件未能获得查询对象";
                }
                else
                {
                    list = ToEntityList(obj);
                    ex.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ex.ErroeMessage = e.Message;
                ex.IsSuccess = false;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.Data = list;
            red.Result = ex;
            red.EventSource = this;
            if (FindAllEvent != null)
                FindAllEvent(red);
        }
        public async void FindAllByCondition(string Sql)
        {
            List<T> list = null;
            ExcuteResult ex = new ExcuteResult();
            try
            {
                Type type = typeof(T);
                AVQuery<AVObject> query = AVObject.GetQuery(type.Name);
                IEnumerable<AVObject> obj = await AVQuery<AVObject>.DoCloudQuery(Sql);
                if (obj.Count() == 0)
                {
                    ex.IsSuccess = false;
                    ex.ErroeMessage = "该条件未能获得查询对象";
                }
                else
                {
                    list = ToEntityList(obj);
                    ex.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ex.ErroeMessage = e.Message;
                ex.IsSuccess = false;
            }
            RepositoryEventData red = new RepositoryEventData();
            red.Data = list;
            red.Result = ex;
            red.EventSource = this;
            if (FindAllEvent != null)
                FindAllEvent(red);
        }
        private T ToEntity(AVObject obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic["objectId"] = obj.ObjectId;
            dic["createdAt"] = obj.CreatedAt;
            dic["updatedAt"] = obj.UpdatedAt;
            foreach (string key in obj.Keys)
            {
                dic[key] = obj[key];
            }
            string jsonStr = JsonHelper.SerializeObject(dic);
            T t = JsonHelper.DeserializeJsonToObject<T>(jsonStr);
            return t;
        }
        private List<T> ToEntityList(IEnumerable<AVObject> objList)
        {
            List<T> list = new List<T>();
            foreach(var obj in objList)
            {
                list.Add(ToEntity(obj));
            }
            return list;
        }
        
    }
}
