<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= sourcekey %>" ></script>    
    <h2><strong>Registration</strong></h2>
    <h4><strong>Your Personal Info</strong></h4>
    <table style="width:100%;">
        <tr>
            <td style="width: 145px">First Name</td>
            <td class="modal-sm" style="width: 180px">
                <asp:TextBox ID="fname_tb" runat="server"></asp:TextBox>
            </td>
            <td style="width: 131px">&nbsp;</td>
            <td rowspan="1" style="width: 329px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px">Last Name</td>
            <td class="modal-sm" style="width: 180px">
                <asp:TextBox ID="lname_tb" runat="server"></asp:TextBox>
            </td>
            <td style="width: 131px">&nbsp;</td>
            <td style="width: 329px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px">DOB</td>
            <td class="modal-sm" style="width: 180px">
                <asp:TextBox ID="dob_tb" runat="server" TextMode="Date"></asp:TextBox>
            </td>
            <td style="width: 131px">&nbsp;</td>
            <td style="width: 329px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px">Email</td>
            <td class="modal-sm" style="width: 180px">
                <asp:TextBox ID="email_tb" runat="server" TextMode="Email"></asp:TextBox>
            </td>
            <td style="width: 131px">&nbsp;</td>
            <td style="width: 329px">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 145px; height: 25px;">Password</td>
            <td class="modal-sm" style="width: 180px; height: 25px;">
                <asp:TextBox ID="pw1_tb" runat="server" ToolTip="" TextMode="Password"></asp:TextBox>
            </td>
            <td style="width: 131px; height: 25px;">Confirm Password</td>
            <td style="width: 329px; height: 25px;">
                <asp:TextBox ID="pw2_tb" runat="server" TextMode="Password"></asp:TextBox>
            &nbsp;<asp:Label ID="pwd_match" runat="server" Font-Bold="True" ForeColor="Red">Passwords must match!</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;" colspan="4">Password Complexity: <span id="complexity_rating" style="font-weight: bold;"></span></td>
            <td class="modal-sm" style="width: 180px; height: 20px;" colspan="4">
            </td>
            <td style="width: 138px; height: 20px;" colspan="4"></td>
            <td colspan="4" style="height: 20px">
            </td>
        </tr>
    </table>
    <asp:Label ID="error_lb" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    <br />
    <br />
    Your password must meet the following criteria:<br />
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
    <br />
    <h4><strong>Your Credit Card Info</strong></h4>
        <table style="width:100%;">
            <tr>
                <td style="width: 117px; height: 22px;">Name on Card</td>
                <td style="width: 182px; height: 22px;">
                <asp:TextBox ID="name_cc" runat="server"></asp:TextBox>
                </td>
                <td style="width: 138px; height: 22px;"></td>
                <td style="height: 22px; width: 329px;"></td>
            </tr>
            <tr>
                <td style="width: 117px">Card No</td>
                <td style="width: 182px">
                <asp:TextBox ID="cardno_cc" runat="server" style="margin-left: 0" MaxLength="19"></asp:TextBox>
                </td>
                <td style="width: 138px">&nbsp;</td>
                <td style="width: 329px">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 117px">CVV</td>
                <td style="width: 182px">
                <asp:TextBox ID="cvv_cc" runat="server" MaxLength="3"></asp:TextBox>
                </td>
                <td style="width: 138px">Expiry Date (MM/YY)</td>
                <td style="width: 329px">
                <asp:TextBox ID="expiry_cc" runat="server" MaxLength="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4"><strong>
                    <asp:Label ID="error2_lb" runat="server" ForeColor="Red"></asp:Label>
                    </strong></td>
                <td style="width: 182px" colspan="4">
                    &nbsp;</td>
                <td style="width: 138px" colspan="4">&nbsp;</td>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
        </table>
<br />
    <br />
<asp:Button ID="register_btn" runat="server" OnClick="register_btn_Click" Text="Register" />

<input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />

    <script type="text/javascript">
        document.getElementById("<%=pwd_match.ClientID %>").style.display = "none";

        function pwdChecker() {
            var pwd = document.getElementById("<%=pw1_tb.ClientID %>").value;
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
            var pwd1 = document.getElementById("<%=pw1_tb.ClientID %>").value;
            var pwd2 = document.getElementById("<%=pw2_tb.ClientID %>").value;

            if (pwd1 != pwd2) {
                document.getElementById("<%=pwd_match.ClientID %>").style.display = "";
            } else {
                document.getElementById("<%=pwd_match.ClientID %>").style.display = "none";
            }
        }

        function expiryFormatter() {
            var str = document.getElementById("<%=expiry_cc.ClientID %>");

            if (str.value.length == 2) {
                str.value = str.value.concat("/");
            }
        }

        function cardFormatter() {
            var str = document.getElementById("<%=cardno_cc.ClientID %>");

            if (str.value.length == 4) {
                str.value = str.value.concat(" ");
            }
            if (str.value.length == 9) {
                str.value = str.value.concat(" ");
            }
            if (str.value.length == 14) {
                str.value = str.value.concat(" ");
            }
        }

        grecaptcha.ready(function () {
            grecaptcha.execute('<%= sitekey %>', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token
            });
        });
    </script>
</asp:Content>
