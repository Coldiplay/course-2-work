using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruh.Model.DBs
{
    //public class Test
    //{
    //    public void Main()
    //    {
    //        DB<DbOperation>.GetDb().Insert();
    //    }
    //}

    public interface ISampleDB
    {
        public bool Insert(object obj);
        public bool Update(object obj);
    }

    public class DB<T> where T : ISampleDB, new()
    {
        static List<ISampleDB> sampleDBs = new List<ISampleDB>();

        public static ISampleDB GetDb()
        {
            ISampleDB db = sampleDBs.FirstOrDefault(s => s.GetType() == typeof(T));
            if (db == null)
            {
                db = new T();
                sampleDBs.Add(db);
            }
            return db;
        }
    }
}
