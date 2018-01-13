<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="invitations.aspx.cs" Inherits="Adam_and_Christine_C.admin.invitations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminMainContentPlaceHolder" runat="server">

    <asp:Panel ID="pnlSearch" runat="server" CssClass="adminRow" DefaultButton="btnSearch">
        <div class="form-inline">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-default" OnClick="btnAddNew_Click" />
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search" onFocus="this.select()"></asp:TextBox>
                <div class="input-group-btn">
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click"><span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                    <asp:Button ID="btnSearch" runat="server" Text="Search in: All Fields" CssClass="btn btn-primary" OnClick="btnSearch_Click" CommandArgument="All" />

                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                        <span class="caret"></span>
                        <span class="sr-only">Toggle Dropdown</span>
                    </button>
                    <ul class="dropdown-menu" role="menu">
                        <asp:Repeater ID="repSearchFields" runat="server">
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="lnkSearchFld" runat="server" Text='<%# Bind("Display") %>' CommandArgument='<%# Bind("FldNum") %>' CommandName="UpdateSearchFld" OnClick="UpdateSearch_Click" />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlGridViewInvites" runat="server" CssClass="adminRow">

        <asp:GridView ID="grdInvites" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="InviteID" runat="server" GridLines="None" OnPreRender="grdInvites_PreRender" OnRowDataBound="grdInvites_RowDataBound" OnSelectedIndexChanged="grdInvites_SelectedIndexChanged" OnRowDeleting="grdInvites_RowDeleting" OnPageIndexChanged="grdInvites_PageIndexChanged" OnPageIndexChanging="grdInvites_PageIndexChanging" OnRowCommand="grdInvites_RowCommand" OnSorting="grdInvites_Sorting">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                    <ItemTemplate>
                        <asp:Button runat="server" ID="SelectButton" CommandName="Select" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="print-link"
                            CommandName="Print" CommandArgument='<%#Eval("InviteID")%>'>Print</asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="grdLink" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="btn btn-default">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server"
                            CommandName="Delete" OnClientClick='return confirm("Are you sure you want to delete this invite?");' Text="Delete" CommandArgument='<%#Eval("InviteID")%>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="grdLink" />
                </asp:TemplateField>
                <asp:BoundField DataField="FullName" HeaderText="Full Name" SortExpression="FullName" ItemStyle-Width="140px" />
                <asp:BoundField DataField="EventID" HeaderText="Event ID" SortExpression="EventID" Visible="false" />
                <asp:BoundField DataField="EventName" HeaderText="Event Name" SortExpression="EventName" ItemStyle-Width="90px"  />
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

        <div class="num-pages">
            <div class="left">
                <asp:Panel ID="pnlGridViewInfoHeader" runat="server" CssClass="leftlbl">
                    <asp:Label ID="lblAMSHeader" runat="server" Text="Amsterdam: " Font-Bold="True" />
                    <asp:Label ID="lblAMS" runat="server" Font-Bold="True" />
                    | 
                <asp:Label ID="lblNEBHeader" runat="server" Text="Nebraska: " Font-Bold="True" />
                    <asp:Label ID="lblNEB" runat="server" Font-Bold="True" />
                    | 
                <asp:Label ID="lblNAHeader" runat="server" Text="No Response: " Font-Bold="True" />
                    <asp:Label ID="lblNA" runat="server" Font-Bold="True" />
                    | 
                </asp:Panel>
            </div>
            <div class="right">
                <div class="input-group">
                    <span class="input-group-addon">Invites Per Page</span>
                    <asp:DropDownList ID="ddRows" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddRows_SelectedIndexChanged">
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="20" Value="20" />
                        <asp:ListItem Text="50" Value="50" />
                        <asp:ListItem Text="100" Value="100" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlInviteInfo" runat="server" CssClass="adminRow">
        <div class="form-horizontal form-border" role="form">
            <div class="form-group">
                <label id="lblFullName" runat="server" class="col-sm-4 control-label">Full Name</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtFullName" runat="server" class="form-control" placeholder="Full Name"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label id="lblInviteName" runat="server" class="col-sm-4 control-label">Invite Name</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtInviteName" runat="server" class="form-control" placeholder="Invite Name (the text to appear on the rsvp page)"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label id="lblEmail" runat="server" class="col-sm-4 control-label">Email</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Email"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label id="lblAttending" runat="server" class="col-sm-4 control-label">Attending</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddAttending" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddAttending_SelectedIndexChanged" />
                </div>
            </div>
            <asp:Panel ID="pnlEventNumPeople" runat="server" Visible="false">
                <div class="form-group">
                    <label id="lblEvent" runat="server" class="col-sm-4 control-label">Event</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddEvent" runat="server" class="form-control" />
                        <asp:RequiredFieldValidator ID="reqEvent" runat="server" Display="None" ControlToValidate="ddEvent" InitialValue="0" ErrorMessage="Event Required" ValidationGroup="Update" />
                    </div>
                </div>
                <div class="form-group">
                    <label id="lblNumPeople" runat="server" class="col-sm-4 control-label">Number Of People</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddNumPeople" runat="server" class="form-control" />
                        <asp:RequiredFieldValidator ID="reqNumPeople" runat="server" Display="None" ControlToValidate="ddNumPeople" InitialValue="0" ErrorMessage="Number Of People Required" ValidationGroup="Update" />
                    </div>
                </div>
            </asp:Panel>
            <div class="form-group">
                <label id="lblLangName" runat="server" class="col-sm-4 control-label">Language</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddLangName" runat="server" class="form-control" />
                </div>
            </div>
            <asp:Panel ID="pnlLabels" runat="server">
                <div class="form-group">
                    <label id="lblSubmitDateHeader" runat="server" class="col-sm-4 control-label">Submit Date</label>
                    <div class="col-sm-8">
                        <asp:Label ID="lblSubmitDate" runat="server" CssClass="control-label" style="display:inline-block;text-align:left;" />
                    </div>
                </div>
                <div class="form-group">
                    <label id="lblHyperlinkHeader" runat="server" class="col-sm-4 control-label">RSVP Link</label>
                    <div class="col-sm-8">
                        <asp:HyperLink ID="lnkRSVPLink" runat="server" Target="_blank">
                            <asp:Label ID="lblRSVPLink" runat="server" CssClass="control-label" style="display:inline-block;text-align:left;" />
                        </asp:HyperLink></div></div><div class="form-group">
                    <label id="lblCommentHeader" runat="server" class="col-sm-4 control-label">Comment</label> <div class="col-sm-8">
                        <asp:Label ID="lblComment" runat="server" CssClass="control-label" style="display:inline-block;text-align:left;" />
                    </div>
                </div>
                <div class="form-group">
                    <label id="lblQRCodeHeader" runat="server" class="col-sm-4 control-label">QR Code</label> <div class="col-sm-8">
                       <asp:Image ID="imgQRCode" runat="server" CssClass="control-label" style="display:inline-block;text-align:left;" Height="117px" />
                    </div>
                </div>
            </asp:Panel>
            <div class="form-group">
                <div class="col-sm-offset-4 col-sm-8">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" CommandArgument="Update" ValidationGroup="Update" />
                    <asp:Button ID="btnGenQRCode" runat="server" Text="Generate QR Code" CssClass="btn btn-default" OnClick="btnGenQRCode_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="cmd">
        <div class="left">
            <asp:Button ID="btnGenQRCodesAll" runat="server" Text="Generate QR Codes For All" CssClass="btn btn-default" OnClick="btnGenQRCodesAll_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="Print Multiple Invites" CssClass="btn btn-default" OnClick="btnPrint_Click" /><asp:Label ID="lblQRCodesMessage" runat="server" Visible="false" ForeColor="Green" Text="lblQRCodesMessage" CssClass="qrCode" />
        </div>
        <div class="right">
            <asp:LinkButton ID="btnExcelDownload" runat="server" OnClick="btnExcelDownload_Click"><asp:Image ID="imgExcelDownload" runat="server" ImageUrl="~/img/excel-icon-small.png" /></asp:LinkButton>
        </div>
    </div>
</asp:Content>
