<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="translations.aspx.cs" Inherits="Adam_and_Christine_C.admin.translations" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminMainContentPlaceHolder" runat="server">
    <asp:Panel ID="pnlLanguageSelect" runat="server" CssClass="adminRowUnderline noTopMargin">
        <div class="form-inline">
            <asp:Label ID="lblSection" runat="server" Text="Section: "></asp:Label><asp:DropDownList ID="ddGlossaryGrp" runat="server" Width="120px" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddGlossaryGrp_SelectedIndexChanged" />
            <asp:Label ID="lblEngText" runat="server" Text="English Text: "></asp:Label><asp:DropDownList ID="ddEngText" runat="server" Width="220px" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddEngText_SelectedIndexChanged" />
            <asp:Label ID="lblLang" runat="server" Text="Language: "></asp:Label><asp:DropDownList ID="ddLang" runat="server" Width="133px" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddLang_SelectedIndexChanged" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlEnglishText" runat="server" CssClass="adminRowUnderline">
        <asp:Label ID="lblFullEnglishText" runat="server" Font-Size="14px"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="pnlTranstext" runat="server" CssClass="adminRowUnderline">
        <asp:TextBox ID="txtFullTranstext" runat="server" Width="100%" Height="150px" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqFullTranstext" runat="server" ErrorMessage="Translated Text Required" Display="None" ControlToValidate="txtFullTranstext" ValidationGroup="Update"></asp:RequiredFieldValidator>
    </asp:Panel>
    <asp:Panel ID="pnlSubmit" runat="server" CssClass="adminRowUnderline form-inline">
        <asp:Button ID="btnSubmit" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="Update" />
        <asp:Button ID="btnAddNew" runat="server" Text="Add" CssClass="btn btn-primary" ValidationGroup="Update" OnClick="btnAddNew_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-primary" ValidationGroup="Update" OnClick="btnDelete_Click"  OnClientClick='return confirm("Are you sure you want to delete this text?");' />
    </asp:Panel>
</asp:Content>
