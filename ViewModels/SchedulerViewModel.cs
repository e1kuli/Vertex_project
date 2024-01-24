using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertexes.Models;

namespace Vertexes.ViewModels
{
    /// <summary>
    /// The data binding View Model.
    /// </summary>
    public class SchedulerViewModel
    {
        public SchedulerViewModel()
        {
            this.IntializeAppoitments();
        }
        public ObservableCollection<MyEvent> Events { get; set; }
        private void IntializeAppoitments()
        {

            this.Events = new ObservableCollection<MyEvent>(App.Database.GetAll<MyEvent>());
        }

    }
}
