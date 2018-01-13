<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Adam_and_Christine_C.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <link href="css/login.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:Panel ID="pnlLoginForm" runat="server" CssClass="loginForm">
        <asp:Login ID="lgnForm" runat="server" RenderOuterTable="false">
            <LayoutTemplate>
                <div class ="loginRow">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="lbl">User Name:</asp:Label>
                </div>
                <div class ="loginRow">
                    <asp:TextBox ID="UserName" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1" Display="None" />
                </div>
                <div class ="loginRow">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="lbl">Password:</asp:Label>
                </div>
                <div class ="loginRow">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1" Display="None" />
                </div>
                <div class ="loginRow">
                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." CssClass="chkBox" />
                </div>
                <div class ="errorSummary">
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                </div>
                <div class ="loginRow">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" CssClass="btn btn-primary btn-custom" />
                </div>
            </LayoutTemplate>
        </asp:Login>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContentPlaceHolder" runat="server">
</asp:Content>
