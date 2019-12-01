using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Query
{
    public partial class _Default : Page
    {
		private static MovieDB db = new MovieDB();

		protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        
        // query method
        protected void Button3_Click(object sender, EventArgs e)
        {
            MovieEntity entity = db.Query(TextBox1.Text);
            Results1.Text = entity.ToString();
            Image1.ImageUrl = entity.PosterUrl;
        }
    }
}
 