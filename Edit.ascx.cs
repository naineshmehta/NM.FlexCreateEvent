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
using DotNetNuke.Entities.Users;
using NM.Modules.NM.FlexCreateEvent.Components;
using DotNetNuke.Services.Exceptions;

namespace NM.Modules.NM.FlexCreateEvent
{
    /// -----------------------------------------------------------------------------
    /// <summary>   
    /// The Edit class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from NM.FlexCreateEventModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : FlexCreateEventModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Implement your edit logic for your module
                if (!Page.IsPostBack)
                {
                    //get a list of users to assign the user to the Object
                    ddlAssignedUser.DataSource = UserController.GetUsers(PortalId);
                    ddlAssignedUser.DataTextField = "Username";
                    ddlAssignedUser.DataValueField = "UserId";
                    ddlAssignedUser.DataBind();

                    //check if we have an ID passed in via a querystring parameter, if so, load that item to edit.
                    //EventId is defined in the ItemModuleBase.cs file
                    if (ItemId > 0)
                    {
                        var tc = new EventController();

                        var t = tc.GetItem(ItemId, ModuleId);
                        if (t != null)
                        {
                            txtName.Text = t.Title;
                            txtDescription.Text = t.ShortDescription;
                            ddlAssignedUser.Items.FindByValue(t.AssignedUserId.ToString()).Selected = true;
                        }
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var t = new Event();
            var tc = new EventController();

            if (ItemId > 0)
            {
                t = tc.GetItem(ItemId, ModuleId);
                t.Title = txtName.Text.Trim();
                t.ShortDescription = txtDescription.Text.Trim();
                t.LastModifiedByUserId = UserId;
                t.LastModifiedOnDate = DateTime.Now;
                t.AssignedUserId = Convert.ToInt32(ddlAssignedUser.SelectedValue);
            }
            else
            {
                t = new Event()
                {
                    AssignedUserId = Convert.ToInt32(ddlAssignedUser.SelectedValue),
                    CreatedByUserId = UserId,
                    CreatedOnDate = DateTime.Now,
                    Title = txtName.Text.Trim(),
                    ShortDescription = txtDescription.Text.Trim(),

                };
            }

            t.LastModifiedOnDate = DateTime.Now;
            t.LastModifiedByUserId = UserId;
            t.ModuleId = ModuleId;

            if (t.EventId > 0)
            {
                tc.UpdateItem(t);
            }
            else
            {
                tc.CreateItem(t);
            }
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}