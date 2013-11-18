<%@ Page Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="profileUpdatedSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register</h2>
    <p>
        Your profile has been registered successfully. Click <asp:HyperLink ID="link" NavigateUrl="~/Account/ShowProfile" runat="server" Text="here " /> to view your profile.
    </p>
</asp:Content>
