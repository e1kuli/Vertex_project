using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Runtime.CompilerServices;

namespace Vertexes.Models;

[Table("Vertex")]
public class Vertex
{
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }
    public int? MyEventId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Text { get; set; }
    //Type = 0 - object
    //Type = 1 - process
    public int Type { get; set; }
    
    public void Save()
    {
        App.Database.SaveVertex(this);
    }

    public void Delete()
    {
        App.Database.DeleteItem<Vertex>(this.Id);
    }

    public Vertex()
    {
    }
    public static Vertex Load(string tid)
    {
        return (Vertex)App.Database.GetItem<Vertex>(Int32.Parse(tid));
    }

    public static Vertex Load(int id)
    {
        return (Vertex)App.Database.GetItem<Vertex>(id);
    }

    public static IEnumerable<Vertex> LoadAll()
    {
        return App.Database.GetAll<Vertex>();
    }

}