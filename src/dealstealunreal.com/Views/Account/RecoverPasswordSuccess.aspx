<%@ Page Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="profileUpdatedSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register</h2>
    <p>
        Your password has been reset and an email sent to your account. Click <asp:HyperLink ID="link" NavigateUrl="~/Account/LogOn" runat="server" Text="here " /> to login.
    </p>
</asp:Content>
