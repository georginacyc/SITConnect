<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= sourcekey %>" ></script>
    <h2>Login</h2>
<p>
    <table style="width:100%;">
        <tr>
            <td style="width: 99px">Email</td>
            <td>
                <asp:TextBox ID="email_tb" runat="server" style="margin-left: 0" TextMode="Email"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 99px">Password</td>
            <td>
                <asp:TextBox ID="pwd_tb" runat="server" TextMode="Password"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" style="width: 153px"><strong>
                <asp:Label ID="error_lb" runat="server" ForeColor="Red"></asp:Label>
                </strong></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</p>
<input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
<p>
    <asp:Button ID="login_btn" runat="server" OnClick="login_btn_Click" Text="Login" />
</p>
    <script>
        grecaptcha.ready(function () {
                grecaptcha.execute('<%= sitekey %>', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token
            });
        });
    </script>
</asp:Content>
