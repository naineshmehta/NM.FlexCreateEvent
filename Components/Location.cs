using System.Collections.Generic;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Data;

namespace NM.Modules.NM.FlexCreateEvent.Components
{
    [TableName("FlexLocation")]
    [Scope("ModuleId")]
    class Location
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public int ModuleId { get; set; }        
    }

    class LocationController
    {
        public IEnumerable<Location> GetLocations(int moduleId)
        {

            IEnumerable<Location> locations;

            using (IDataContext ctx = DataContext.Instance())
            {
                var loc = ctx.GetRepository<Location>();
                locations = loc.Get(moduleId);
            }
            return locations;
        }
    }
}