using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LoanBrokerWeb
{
    public partial class WebApplication : System.Web.UI.Page
    {
        ServiceReferences.Service1Client service = new ServiceReferences.Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["interestrate"] != null)
            {
                Label1.Text = "Best loan " + Request["bank"].ToString() + " with interest rate " + Request["interestrate"].ToString() + "%";
                TextBoxAmount.Text = Session["amount"].ToString();
                TextBoxDuration.Text = Session["duration"].ToString();
                TextBoxSSN.Text = Session["ssn"].ToString();
            }
            
        }

        protected void ButtonLoanRequest_Click(object sender, EventArgs e)
        {
            if (this.PageIsValid())
            {
                string ssn = TextBoxSSN.Text;
                double amount = Convert.ToDouble(TextBoxAmount.Text);
                int duration = Convert.ToInt32(TextBoxDuration.Text);

                try
                {
                    string message = service.LoanRequest(ssn, amount, duration);
                    string[] messageArray = message.Split('#');
                    string bank = messageArray[1];
                    string interestrate = messageArray[2];

                    Session["amount"] = TextBoxAmount.Text;
                    Session["duration"] = TextBoxDuration.Text;
                    Session["ssn"] = TextBoxSSN.Text;
                    Response.Redirect("WebApplication.aspx?interestrate=" + interestrate + "&bank=" + bank);
                }
                catch (TimeoutException te) { }
            }
            else
            {
                Response.Redirect("WebApplication.aspx?error=emptyFields");
            }
        }

        private Boolean PageIsValid()
        {

            if (TextBoxDuration.Text != "" && TextBoxAmount.Text != "" && TextBoxSSN.Text != "" && TextBoxSSN.Text != "")
            {
                return true;
            }

            return false;

        }
    }
}