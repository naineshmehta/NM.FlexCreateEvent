/*
' Copyright (c) 2017  Nainesh Mehta
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Linq;
using System.Web.UI.WebControls;
using NM.Modules.NM.FlexCreateEvent.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;

namespace NM.Modules.NM.FlexCreateEvent
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from NM.FlexCreateEventModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : FlexCreateEventModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    LocationController lc = new LocationController();
                    drpLocation.DataSource = lc.GetLocations(722).OrderBy(x => x.Name);
                    drpLocation.DataTextField = "Name";
                    drpLocation.DataValueField = "ItemId";
                    drpLocation.DataBind();

                    drpLocation.Items.Insert(0, new ListItem("==Please select a location==", "-1"));
                }
                else
                {
                    HandlePostBack();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void HandlePostBack()
        {
            try
            {
                var uploadedEvent = new Event();
                uploadedEvent.Title = Convert.ToString(Request.Form["txtEventName"]);
                uploadedEvent.ShortDescription = Convert.ToString(Request.Form["txtShortDesc"]);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}