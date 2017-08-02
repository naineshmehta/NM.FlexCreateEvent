/*
' Copyright (c) 2017 Nainesh Mehta
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/
using System.Collections.Generic;
using DotNetNuke.Data;

namespace NM.Modules.NM.FlexCreateEvent.Components
{
    class EventController
    {
        public void CreateItem(Event t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Event>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(Event t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Event>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Event> GetItems(int moduleId)
        {
            IEnumerable<Event> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Event>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Event GetItem(int itemId, int moduleId)
        {
            Event t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Event>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(Event t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Event>();
                rep.Update(t);
            }
        }

    }
}
