<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebApplication.aspx.cs" Inherits="LoanBrokerWeb.WebApplication" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LoanBroker</title>
</head>

<body>

    <form id="form1" runat="server">
    <div>
    
        Amount:
        <asp:TextBox ID="TextBoxAmount" runat="server" Width="250px"></asp:TextBox><br /><br />
        Duration (months):
        <asp:TextBox ID="TextBoxDuration" runat="server" Width="250px"></asp:TextBox><br /><br />
        SSN (xxxxxx-xxxx)
        <asp:TextBox ID="TextBoxSSN" runat="server" Width="250px"></asp:TextBox><br /><br />

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="Button1" runat="server" Text="Submit"
                        OnClick="ButtonLoanRequest_Click" />
                        <asp:UpdateProgress  runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                Searching....
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        
        <br /><br />
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>    
    </div>
    </form>
</body>
</html>