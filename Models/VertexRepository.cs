using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertexes.Models
{
    public class VertexRepository
    {
        SQLiteConnection database;
        public VertexRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Vertex>();
            database.CreateTable<Edge>();
            database.CreateTable<Link>();
            database.CreateTable<MyEvent>();

        }
        public IEnumerable<T> GetAll<T>()
        {
            if (typeof(T) == typeof(Vertex))
            {
                return (IEnumerable<T>)database.Table<Vertex>();//.ToList();
            }
            if (typeof(T) == typeof(Edge))
            {
                return (IEnumerable<T>)database.Table<Edge>();//.ToList();
            }
            if (typeof(T) == typeof(Link))
            {
                return (IEnumerable<T>)database.Table<Link>();//.ToList();
            }
            if (typeof(T) == typeof(MyEvent))
            {
                return (IEnumerable<T>)database.Table<MyEvent>();//.ToList();
            }

            else return null;
        }

        public object GetItem<T>(int id)
        {
            if (typeof(T) == typeof(Vertex))
            {
                return database.Get<Vertex>(id);
            }
            if (typeof(T) == typeof(Edge))
            {
                return database.Get<Edge>(id);
            }
            if (typeof(T) == typeof(Link))
            {
                return database.Get<Link>(id);
            }
            if (typeof(T) == typeof(MyEvent))
            {
                return database.Get<MyEvent>(id);
            }
            else return null;

        }



        public int DeleteItem<T>(int id)
        {
            if (typeof(T) == typeof(Vertex))
            {
                return database.Delete<Vertex>(id);
            }
            if (typeof(T) == typeof(Edge))
            {
                return database.Delete<Edge>(id);
            }
            if (typeof(T) == typeof(Link))
            {
                return database.Delete<Link>(id);
            }
            if (typeof(T) == typeof(MyEvent))
            {
                return database.Delete<MyEvent>(id);
            }

            else return 0;

        }


        public int SaveVertex(Vertex item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
        public int SaveEdge(Edge item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
        public int SaveLink(Link item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }

        public int SaveEvent(MyEvent item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }



        
        public IEnumerable<Edge> GetEdges()
        {
            return database.Table<Edge>();//.ToList();
        }
        public Edge GetEdge(int id)
        {
            return database.Get<Edge>(id);
        }
        public int DeleteEdge(int id)
        {
            return database.Delete<Edge>(id);
        }


        public IEnumerable<Link> GetConns()
        {
            return database.Table<Link>();//.ToList();
        }
        public Link GetConn(int id)
        {
            return database.Get<Link>(id);
        }
        public int DeleteConn(int id)
        {
            return database.Delete<Link>(id);
        }
    }
}
