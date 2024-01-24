using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace Vertexes.Models
{
    [Table("Events")]

    /// <summary>    
    /// Represents the custom data properties.    
    /// </summary>    
    public class MyEvent
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [ForeignKey(typeof(Vertex))]
        public int VertexId { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? EventName { get; set; }
        public string? Notes { get; set; }

        public MyEvent() { }

        
        /*
        public bool IsAllDay { get; set; }
        public TimeZoneInfo StartTimeZone { get; set; }
        public TimeZoneInfo EndTimeZone { get; set; }
        public Brush Background { get; set; }
        public object RecurrenceId { get; set; }
        public string RecurrenceRule { get; set; }
        public ObservableCollection<DateTime> RecurrenceExceptions { get; set; }
        */
    }
}
