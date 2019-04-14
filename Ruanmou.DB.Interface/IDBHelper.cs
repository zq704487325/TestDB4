using System.Collections.Generic;
using Ruanmou.Model;

namespace Ruanmou.DB.Interface
{
    public interface IDBHelper
    {
        bool Delete<T>(int id) where T : BaseModel;
        T Find<T>(int id) where T : BaseModel;
        bool Insert<T>(T t) where T : BaseModel;
        List<T> SelectAll<T>() where T : BaseModel;
        void ShowProNameAndVal<T>(T t);
        bool Update<T>(T t) where T : BaseModel;
    }
}