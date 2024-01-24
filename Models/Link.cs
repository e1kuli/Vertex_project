using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;
using Vertexes.Models;

namespace Vertexes.Models;

[Table("Links")]

public class Link
{
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }
    [ForeignKey(typeof(Vertex))]
    public int FirstVertexId { get; set; }
    [ForeignKey(typeof(Edge))]
    public int EdgeId { get; set; }
    [ForeignKey(typeof(Vertex))]
    public int SecondVertexId { get; set; }
    //если процесс и объект = 0; если оба относятся к одному виду = 1
    public int SameGroupIndicator { get; set; }

    public void Save()
    {
        App.Database.SaveLink(this);
    }

    public void Delete()
    {
        App.Database.DeleteItem<Link>(this.Id);
    }

    public Link()
    {
    }
    public static Link Load(string tid)
    {
        return (Link)App.Database.GetItem<Link>(Int32.Parse(tid));
    }

    public static Link Load(int id)
    {
        return (Link)App.Database.GetItem<Link>(id);
    }

    public static IEnumerable<Link> LoadAll()
    {
        return App.Database.GetAll<Link>();
    }
    
}
