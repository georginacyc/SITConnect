<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Change Password</h4>
    <p>
        <table style="width:100%;">
            <tr>
                <td class="modal-sm" style="width: 166px">Current Password</td>
                <td>
                    <asp:TextBox ID="current_tb" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="modal-sm" style="width: 166px">New Password</td>
                <td>
                    <asp:TextBox ID="new_tb" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="modal-sm" style="width: 166px">Confirm New Password</td>
                <td>
                    <asp:TextBox ID="new2_tb" runat="server" TextMode="Password"></asp:TextBox>
                    <strong>&nbsp;<asp:Label ID="pwd_match" runat="server" ForeColor="Red">Passwords must match!</asp:Label>
                    </strong></td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p>
    Password Complexity: <span id="complexity_rating" style="font-weight: bold;"></span>
    <p><strong>
        <asp:Label ID="error_lb" runat="server" ForeColor="Red"></asp:Label>
        </strong></p>
    <p>Your password must meet the following criteria:<br />
&nbsp;&nbsp;&nbsp; -
    <strong>
    <asp:Label ID="pwd_length" runat="server" ForeColor="Red" Text="At least 8 characters"></asp:Label>
    </strong>
    <br />
&nbsp;&nbsp;&nbsp; -
    <strong>
    <asp:Label ID="pwd_case" runat="server" ForeColor="Red" Text="Mix of upper and lowercase characters"></asp:Label>
    </strong>
    <br />
&nbsp;&nbsp;&nbsp; -
    <strong>
    <asp:Label ID="pwd_num" runat="server" ForeColor="Red" Text="At least 1 number"></asp:Label>
    </strong>
    <br />
&nbsp;&nbsp;&nbsp; -
    <strong>
    <asp:Label ID="pwd_char" runat="server" ForeColor="Red" Text="At least 1 special character"></asp:Label>
    </strong>
    <br />
    </p>
    <p>
        <asp:Button ID="submit_btn" runat="server" Text="Submit" OnClick="submit_btn_Click" />
    </p>
    <script type="text/javascript">
        document.getElementById("<%=pwd_match.ClientID %>").style.display = "none";

        function pwdChecker() {
            var pwd = document.getElementById("<%=new_tb.ClientID %>").value;
            var score = 0;
            var rating = document.getElementById("complexity_rating");
            rating.innerHTML = "";

            if (pwd.length >= 8) {
                document.getElementById("<%=pwd_length.ClientID %>").style.color = "Green";
                document.getElementById("<%=pwd_length.ClientID %>").style.fontWeight = "normal";
                score += 1;
            } else {
                document.getElementById("<%=pwd_length.ClientID %>").style.color = "Red";
                document.getElementById("<%=pwd_length.ClientID %>").style.fontWeight = "bold";
            }

            var caseRegex = /.*[A-Z]+.*[a-z]+.*/;
            if (caseRegex.test(pwd)) {
                document.getElementById("<%=pwd_case.ClientID %>").style.color = "Green";
                document.getElementById("<%=pwd_case.ClientID %>").style.fontWeight = "normal";
                score += 1;
            } else {
                document.getElementById("<%=pwd_case.ClientID %>").style.color = "Red";
                document.getElementById("<%=pwd_case.ClientID %>").style.fontWeight = "bold";
            }

            var numRegex = /.*[0-9]+.*/;
            if (numRegex.test(pwd)) {
                document.getElementById("<%=pwd_num.ClientID %>").style.color = "Green";
                document.getElementById("<%=pwd_num.ClientID %>").style.fontWeight = "normal";
                score += 1;
            } else {
                document.getElementById("<%=pwd_num.ClientID %>").style.color = "Red";
                document.getElementById("<%=pwd_num.ClientID %>").style.fontWeight = "bold";
            }

            var charRegex = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
            if (charRegex.test(pwd)) {
                document.getElementById("<%=pwd_char.ClientID %>").style.color = "Green";
                document.getElementById("<%=pwd_char.ClientID %>").style.fontWeight = "normal";
                score += 1;
            } else {
                document.getElementById("<%=pwd_char.ClientID %>").style.color = "Red";
                document.getElementById("<%=pwd_char.ClientID %>").style.fontWeight = "bold";
            }

            switch (score) {
                case 1:
                    rating.innerHTML = "Very Weak";
                    rating.style.color = "Red";
                    break;
                case 2:
                    rating.innerHTML = "Weak";
                    rating.style.color = "Red";
                    break;
                case 3:
                    rating.innerHTML = "Medium";
                    rating.style.color = "Red";
                    break;
                case 4:
                    rating.innerHTML = "Strong";
                    rating.style.color = "Green";
                    break;
                default:
                    rating.innerHTML = "Very Weak";
                    rating.style.color = "Red";
                    break;

            }
        }

        function pwdMatcher() {
            var pwd1 = document.getElementById("<%=new_tb.ClientID %>").value;
            var pwd2 = document.getElementById("<%=new2_tb.ClientID %>").value;

            if (pwd1 != pwd2) {
                document.getElementById("<%=pwd_match.ClientID %>").style.display = "";
            } else {
                document.getElementById("<%=pwd_match.ClientID %>").style.display = "none";
            }
        }
    </script>
</asp:Content>
