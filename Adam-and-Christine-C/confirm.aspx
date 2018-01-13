<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="confirm.aspx.cs" Inherits="Adam_and_Christine_C.confirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="confirmSection">
        <div class="confirmRow">
            <div class="fullRow">
                <asp:Label ID="lblThankYouHeader" runat="server" CssClass="heading" />
            </div>
        </div>

        <asp:Panel ID="pnlRSVPThankYouFullText" runat="server">
            <div class="confirmRow">
                <div class="fullRow">
                    <asp:Label ID="lblThankYou" runat="server" CssClass="lbl" />
                    <asp:HyperLink ID="lnkRSVP" runat="server" CssClass="lbl" />

                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="confirmSection">
        <asp:Panel ID="pnlRSVPInfo" runat="server" CssClass="confirmRow">
            <div class="left">
                <asp:Label ID="lblRSVPInfo" runat="server" CssClass="heading" />
            </div>
            <div class="right">
                <asp:HyperLink ID="btnMistakeLink" runat="server" Font-Bold="true" CssClass="mistake">
                    <asp:Label ID="lblMistake" runat="server" />
                </asp:HyperLink>
            </div>
        </asp:Panel>

        <div class="confirmRow">
            <div class="fullRow">
                <asp:Label ID="lblName" runat="server" CssClass="lbl" />
            </div>
            <div class="fullRow">
                <asp:Label ID="lblAttending" runat="server" CssClass="lbl" /><asp:Label ID="lblAttendingAnswer" runat="server" CssClass="lbl" />
            </div>
            <asp:Panel ID="pnlEvent" runat="server" CssClass="fullRow">
                <asp:Label ID="lblEventConfirm" runat="server" CssClass="lbl" /><asp:Label ID="lblEventResponse" runat="server" CssClass="lbl" />
            </asp:Panel>
            <asp:Panel ID="pnlNumPeople" runat="server" CssClass="fullRow">
                <asp:Label ID="lblNumPeopleConfirm" runat="server" CssClass="lbl" /><asp:Label ID="lblNumPeopleResponse" runat="server" CssClass="lbl" />
            </asp:Panel>
            <div class="fullRow">
                <asp:Label ID="lblSubmitted" runat="server" CssClass="lbl" /><asp:Label ID="lblSubmittedResponse" runat="server" CssClass="lbl" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlAMSInfo" runat="server" CssClass="confirmSection">
        <div class="confirmRow">
            <div class="fullRow">
                <asp:Label ID="lblAMSInfoHeader" runat="server" CssClass="heading" />
            </div>
        </div>
        
            <asp:Repeater ID="repAMSInfo" runat="server">
                <ItemTemplate>
                    <div class="confirmRow">
                        <div class="fullRow">
                            <asp:Label ID="lblAMSInfoBody" runat="server" CssClass="lbl" Text='<%# Eval("Transtext")%>' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        
    </asp:Panel>

    <asp:Panel ID="pnlLINInfo" runat="server" CssClass="confirmSection">
        <div class="confirmRow">
            <div class="fullRow">
                <asp:Label ID="lblLINInfoHeader" runat="server" CssClass="heading" />
            </div>
        </div>

        <asp:Repeater ID="repLINInfo" runat="server">
            <ItemTemplate>
                <div class="confirmRow">
                    <div class="fullRow">
                        <asp:Label ID="lblLINInfoBody" runat="server" CssClass="lbl" Text='<%# Eval("Transtext")%>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContentPlaceHolder" runat="server">
</asp:Content>
