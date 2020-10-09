using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = new[]
            {
                new
                {
                   Delete = false,
                   ColumnPosition = 1,
                   FieldLength = 1,
                   FieldName = "First Name"
                }
            };

            GridView1.AutoGenerateEditButton = true;

            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {

        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {

        }
    }
}