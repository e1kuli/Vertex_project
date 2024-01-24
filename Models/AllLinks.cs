using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Vertexes.Models
{
    public class AllLinksClass
    {
        public string edge_name { get; set; }
        public string Name { get; set; }
        public int vertex_id { get; set; }

        public AllLinksClass(string edge_name, string vertex_name, int vertex_id)
        {
            this.edge_name = edge_name;
            this.Name = vertex_name;
            this.vertex_id = vertex_id;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class GroupLinksClass<K,T> : ObservableCollection<T>
    {
        public K Name { get; private set; }
        public GroupLinksClass(K name, IEnumerable<T> items) : base(items)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
