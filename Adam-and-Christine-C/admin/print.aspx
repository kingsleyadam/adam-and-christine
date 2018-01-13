<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print.aspx.cs" Inherits="Adam_and_Christine_C.admin.print" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Invitations</title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="/css/print.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <asp:Repeater ID="repInvites" runat="server" OnItemDataBound="repInvites_ItemDataBound">
            <ItemTemplate>
                <div class="invite">
                    <div class="left">
                        <asp:Label ID="lblInviteID" runat="server" Text='<%#Eval("InviteID")%>' Visible="false"></asp:Label>
                        <asp:Label ID="lblInviteName" runat="server" Text='<%#Eval("InviteName")%>' CssClass="name"></asp:Label>
                        <asp:Image ID="imgAC" runat="server" CssClass="acImage" ImageUrl="~/img/a-and-c-cropped.jpg" />
                    </div>
                    <div class="right">
                        <h4 style="margin-top:0;padding-top:5px;"><asp:Label ID="Label2" runat="server" Text="RSVP with:"></asp:Label></h4>
                        <asp:Image ID="imgQRCode" runat="server" CssClass="qrImage" />
                        <h5><asp:Label ID="lblOr" runat="server" Text="or"></asp:Label></h5>
                        <h4 style="margin-bottom:0;"><asp:Label ID="lblRSVPLink" runat="server" Text='<%#Eval("RSVPLink")%>'></asp:Label></h4>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>

    <script src="/js/jquery-2.1.1.min.js"></script>
    <script src="/js/bootstrap.min.js"></script>
    <script src="/js/javascript.js"></script>
</body>
</html>
