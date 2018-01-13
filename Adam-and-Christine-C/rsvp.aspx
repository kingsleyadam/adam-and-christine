<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="rsvp.aspx.cs" Inherits="Adam_and_Christine_C.rsvp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="updatePanelHeader" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAlreadySubmitted" runat="server" CssClass="formContent" Visible="false">
                <div class="formRowTop">
                    <asp:Label ID="lblAlreadySubmitText" runat="server" />
                    <asp:Label ID="lblAlreadySubmitSpace" runat="server" Text=" " />
                    <asp:HyperLink ID="lnkConfirmFwd" runat="server" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="rsvpRow">
                <asp:Label ID="lblHeader" runat="server" CssClass="heading" />
            </div>

            <asp:Panel ID="pnlLangSelect" runat="server" CssClass="languageSelect">
                <asp:LinkButton ID="lnkUS" runat="server" BorderWidth="0px" CommandArgument="US" OnClick="lnkFlag_Click">
                    <asp:Image ID="imgUS" runat="server" CssClass="grayscale" ImageUrl="~/img/flags/128/us.png" BorderWidth="0px" ToolTip="English" />
                </asp:LinkButton>
                <asp:LinkButton ID="lnkNL" runat="server" BorderWidth="0px" CommandArgument="NL" OnClick="lnkFlag_Click">
                    <asp:Image ID="imgNL" runat="server" CssClass="grayscale" ImageUrl="~/img/flags/128/nl.png" BorderWidth="0px" ToolTip="Nederland" />
                </asp:LinkButton>
                <asp:LinkButton ID="lnkIT" runat="server" BorderWidth="0px" CommandArgument="IT" OnClick="lnkFlag_Click">
                    <asp:Image ID="imgIT" runat="server" CssClass="grayscale" ImageUrl="~/img/flags/128/it.png" BorderWidth="0px" ToolTip="Italiano" />
                </asp:LinkButton>
                <asp:LinkButton ID="lnkDE" runat="server" BorderWidth="0px" CommandArgument="DE" OnClick="lnkFlag_Click" Visible="false">
                    <asp:Image ID="imgDE" runat="server" CssClass="grayscale" ImageUrl="~/img/flags/128/de.png" BorderWidth="0px" ToolTip="Deutsch" />
                </asp:LinkButton>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlRSVPForm" runat="server">
                <div class="rsvpRow">
                    <asp:Label ID="lblFillForm" runat="server" CssClass="heading" />
                </div>
                <div class="formContent">
                    <div class="formRow">
                        <div class="left">
                            <asp:Label ID="lblYesNo" runat="server" CssClass="lbl" />
                        </div>
                        <div class="right">
                            <asp:DropDownList ID="ddYesNo" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddYesNo_SelectedIndexChanged" />
                        </div>
                    </div>

                    <asp:Panel ID="pnlEvent" runat="server" CssClass="formRow">
                        <div class="left">
                            <asp:Label ID="lblEvent" runat="server" CssClass="lbl" />
                        </div>
                        <div class="right">
                            <asp:DropDownList ID="ddEvent" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="reqEvent" runat="server" Display="None" ControlToValidate="ddEvent" InitialValue="0" ValidationGroup="rsvpForm"></asp:RequiredFieldValidator>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlNumPeople" runat="server" CssClass="formRow">
                        <div class="left">
                            <asp:Label ID="lblNumPeople" runat="server" CssClass="lbl" />
                        </div>
                        <div class="right">
                            <asp:DropDownList ID="ddNumPeople" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="reqNumPeople" runat="server" Display="None" ControlToValidate="ddNumPeople" InitialValue="0" ValidationGroup="rsvpForm"></asp:RequiredFieldValidator>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlComment" runat="server" CssClass="formRow">
                        <div class="lbl">
                            <asp:Label ID="lblComment" runat="server" />
                        </div>
                        <div>
                            <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="5000" Height="100px" />
                        </div>
                    </asp:Panel>

                    <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="rsvpForm" CssClass="alert alert-danger" />

                    <asp:Panel ID="pnlSubmitRow" runat="server" CssClass="submitRow">
                        <div class="left">
                            <div class="loading">
                                <asp:UpdateProgress ID="updateProgress" runat="server">
                                    <ProgressTemplate>
                                        <asp:Image ID="imgLoading" runat="server" ImageUrl="~/img/ajax-loader.gif" CssClass="img" />
                                        <asp:Label ID="lblLoading" runat="server" CssClass="lbl" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                        <div class="right">
                            <asp:LinkButton ID="lnkText" runat="server" CssClass="btn btn-primary btn-custom" CausesValidation="true" ValidationGroup="rsvpForm" OnClick="lnkText_Click" />
                        </div>
                    </asp:Panel>

                </div>
            </asp:Panel>
            <asp:Label ID="lblError" runat="server" Text="AllOk" Style="display: none;"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContentPlaceHolder" runat="server">
</asp:Content>
