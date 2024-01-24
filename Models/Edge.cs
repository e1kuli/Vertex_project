using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertexes.Models
{
    [Table("Edges")]

    public class Edge
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string NameParent { get; set; }
        public string? NameChild { get; set; }

        public DateTime Date { get; set; }

        public void Save()
        {
            App.Database.SaveEdge(this);
        }

        public void Delete()
        {
            App.Database.DeleteEdge(this.Id);
        }

        public Edge()
        {

        }
        public static Edge Load(string tid)
        {

            return App.Database.GetEdge(Int32.Parse(tid));
        }

        public static Edge Load(int id)
        {

            return App.Database.GetEdge(id);
        }

        public static IEnumerable<Edge> LoadAll(int a)
        {

            return App.Database.GetAll<Edge>();
        }
    }
}
