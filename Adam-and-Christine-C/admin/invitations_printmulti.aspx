<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="invitations_printmulti.aspx.cs" Inherits="Adam_and_Christine_C.admin.invitations_printmulti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminMainContentPlaceHolder" runat="server">
    <asp:Panel ID="pnlSearch" runat="server" CssClass="adminRow" DefaultButton="btnSearch">
        <div class="form-inline">
           
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search" onFocus="this.select()"></asp:TextBox>
                <div class="input-group-btn">
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-default" OnClick="btnClear_Click"><span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                    <asp:Button ID="btnSearch" runat="server" Text="Search All Fields" CssClass="btn btn-default" OnClick="btnSearch_Click" CommandArgument="All" />
                </div>
            </div>
             <asp:Button ID="btnPrintAll" runat="server" Text="Print Selected" CssClass="btn btn-primary" OnClick="btnPrintAll_Click" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlGridViewInvites" runat="server" CssClass="adminRow">
        <asp:GridView ID="grdInvites" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="InviteID" runat="server" GridLines="None" OnPreRender="grdInvites_PreRender" OnSorting="grdInvites_Sorting">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkPrint" runat="server" CssClass="chkBox" />
                    </ItemTemplate>
                    <HeaderTemplate>
                        <asp:ImageButton ID="chkAll" runat="server" ImageUrl="~/img/select-all.gif" CssClass="selectAll" AlternateText="Select All" OnClick="chkAll_Click" />
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="InviteID" HeaderText="InviteID" SortExpression="InviteID" />
                <asp:BoundField DataField="FullName" HeaderText="Full Name" SortExpression="FullName" ItemStyle-Width="120px" />
                <asp:BoundField DataField="EventID" HeaderText="Event ID" SortExpression="EventID" Visible="false" />
                <asp:BoundField DataField="EventName" HeaderText="Event Name" SortExpression="EventName" ItemStyle-Width="86px" />
                <asp:BoundField DataField="NumPeople" HeaderText="Number of People" SortExpression="NumPeople" />
                <asp:BoundField DataField="SubmitDate" HeaderText="Submit Date" SortExpression="SubmitDate" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                <asp:BoundField DataField="LangID" HeaderText="LangID" InsertVisible="False" ReadOnly="True" SortExpression="LangID" Visible="False" />
                <asp:BoundField DataField="LangName" HeaderText="Language" SortExpression="LangName" />
                <asp:BoundField DataField="iso" HeaderText="iso" SortExpression="iso" Visible="False" />
                <asp:BoundField DataField="Attending" HeaderText="Attending" SortExpression="Attending" />
                <asp:BoundField DataField="RSVPLink" HeaderText="RSVP Link" SortExpression="RSVPLink" Visible="False" />
            </Columns>
            <SelectedRowStyle BackColor="#eeeeee" />
            <PagerStyle CssClass="tableFooter" />
            <EmptyDataTemplate>
                <div class="well well-sm">No invites were found for this query.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
