<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><strong>Your Profile</strong></h2>
    <h4><strong>Your Personal Info</strong></h4>
    <table style="width:100%;">
        <tr>
            <td style="width: 145px; height: 20px;">First Name</td>
            <td class="modal-sm" style="width: 180px; height: 20px;">
                <asp:Label ID="fname_lb" runat="server"></asp:Label>
            </td>
            <td style="width: 150px; height: 20px;"></td>
            <td rowspan="1" style="width: 329px; height: 20px;">
                </td>
        </tr>
        <tr>
            <td style="width: 145px">Last Name</td>
            <td class="modal-sm" style="width: 180px">
                <asp:Label ID="lname_lb" runat="server"></asp:Label>
            </td>
            <td style="width: 150px">&nbsp;</td>
            <td style="width: 329px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px">DOB</td>
            <td class="modal-sm" style="width: 180px">
                <asp:Label ID="dob_lb" runat="server"></asp:Label>
            </td>
            <td style="width: 150px">&nbsp;</td>
            <td style="width: 329px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px">Email</td>
            <td class="modal-sm" style="width: 180px">
                <asp:Label ID="email_lb" runat="server"></asp:Label>
            </td>
            <td style="width: 150px">&nbsp;</td>
            <td style="width: 329px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px; height: 20px;"></td>
            <td class="modal-sm" style="width: 180px; height: 20px;">
                </td>
            <td style="width: 150px; height: 20px;"></td>
            <td style="width: 329px; height: 20px;">
            </td>
        </tr>
        <tr>
            <td style="height: 20px;" colspan="4">
<asp:Button ID="pass_btn" runat="server" Text="Change Password" OnClick="pass_btn_Click" />
            </td>
            <td class="modal-sm" style="width: 180px; height: 20px;">
            </td>
            <td style="width: 138px; height: 20px;"></td>
            <td style="height: 20px">
            </td>
        </tr>
    </table>
    <h4>&nbsp;</h4>
    <h4><strong>Your Credit Card Info</strong></h4>
        <table style="width:100%;">
            <tr>
                <td style="width: 117px; height: 22px;">Name on Card</td>
                <td style="width: 182px; height: 22px;">
                    <asp:Label ID="cardname_lb" runat="server"></asp:Label>
                </td>
                <td style="width: 138px; height: 22px;"></td>
                <td style="height: 22px; width: 329px;"></td>
            </tr>
            <tr>
                <td style="width: 117px">Card No</td>
                <td style="width: 182px">
                    <asp:Label ID="cardnum_lb" runat="server"></asp:Label>
                </td>
                <td style="width: 138px">&nbsp;</td>
                <td style="width: 329px">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 117px">CVV</td>
                <td style="width: 182px">
                    <asp:Label ID="cvv_lb" runat="server"></asp:Label>
                </td>
                <td style="width: 138px">Expiry Date (MM/YY)</td>
                <td style="width: 329px">
                    <asp:Label ID="expiry_lb" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">&nbsp;</td>
                <td style="width: 182px" colspan="4">
                    &nbsp;</td>
                <td style="width: 138px" colspan="4">&nbsp;</td>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
        </table>
    <br />
<asp:Button ID="logout_btn" runat="server" Text="Logout" OnClick="logout_btn_Click" />
</asp:Content>
