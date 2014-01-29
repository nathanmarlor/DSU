<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    DEAL.STEAL.UNREAL.
</asp:Content>

<asp:Content ID="ClassContent3" ContentPlaceHolderID="BodyClass" runat="server">
    home blog custom-background one-sidebar-right gecko-browser windows-os
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderAction("Deals", new { Controller = "Deal" }); %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CustomFooter" runat="server">
    <% if (Request.IsAuthenticated)
       { %>
        <% Html.RenderAction("SubmitDeal", "Deal"); %>
    <% }
       else
       { %>
    <div style="float: left; left: -50px; position: fixed; top: 155px; z-index: 10;">
        <br>
        <a style="display: block; box-shadow: none; width: 220px; height: 45px; cursor: pointer;
            float: right; margin: 20px;" onclick="loginFirst('<%: Url.Action("LogOn","Account") %>','Please Log in to Submit Your Deal.')"
            title="Submit Deal" href="javascript:void(0);" id="submit-deal"></a>
    </div>
    <% } %>
</asp:Content>

<asp:Content ID="HeaderContent1" ContentPlaceHolderID="CustomHeader" runat="server">

    <script type="text/javascript" src="<%=Url.Content("~/js/validate.js") %>"></script>
</asp:Content>