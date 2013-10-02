<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master"
    Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.Wrappers.UserDeals>" %>
<%@ Import Namespace="dealstealunreal.com.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MY PROFILE | DEAL.STEAL.UNREAL.
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="post-356" class="post-356 page type-page status-publish hentry">
<h2 class="post-title">MY PROFILE</h2>
<div class="post-content">
    <div id="newmydeals" class="my_deal">
        <%:Model.User.UserName%>'s Deals
    </div>
    <% foreach (var deal in Model.Deals)
       { %>
    <div class="post-<%:deal.DealID %> post type-post status-publish format-standard hentry my-deals mydealborder"
        id="post-<%:deal.DealID %>">
        <% if (Model.IsCurrentUser)
           { %>
        <div title="Delete Deal" class="deal_delete">
            <% using (Html.BeginForm("DealDelete", "Deal", FormMethod.Post))
               { %>
                <%:Html.Hidden("dealId", deal.DealID)%>
                <input type="submit" value="Delete" name="delete_deal" class="newdelete" />
            <% } %>
        </div>
        <% } %>
        <h2 class="mydeal-title">
            <a target="_blank" href="<%:deal.Url %>"><%:deal.Title %>- <b>£<%:deal.Price %></b>
                @ <%:deal.Retailer %></a></h2>
        <div id="frontpostmeta" class="post-meta">
            <span class="post-author">by <%:Model.User.UserName %> </span>
            <!-- .post-author -->
            <span class="post-date">on <%:deal.Date.ToString("dd MMM yyyy") %> , </span>
            <!-- .post-author -->
        </div>
        <!-- END .post-meta -->
        <div class="post-content">
            <div class="mydeal_description">
                <%:deal.Description %></div>
            <div class="mydeal_img">
                <a target="_blank" href="<%:deal.Url %>">
                    <img width="50%" src="<%:deal.ImageUrl %>">
                </a>
            </div>
            <% if (Model.IsCurrentUser)
               { %>
            <div class="deal_status">
                <% using (Html.BeginForm("DealActive", "Deal", FormMethod.Post))
               { %>
                    <%:Html.Hidden("dealId", deal.DealID)%>
                    <% if (deal.Active)
                       { %>
                   <%:Html.Hidden("active", "false")%>                   
                    <input type="submit" value="Deactivate"
                    name="deal_deactive" class="deacive" />
                    <% }
                       else
                       { %>
                       <%:Html.Hidden("active", "true")%>                   
                    <input type="submit" value="Activate"
                    name="deal_active" class="acive" />
                    <%} %>
                <% } %>
            </div>
            <% } %>
        </div>
        <div class="clear">
        </div>
    </div>
    <% } %>
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomHeader" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyClass" runat="server">
    page page-id-356 page-parent page-template-default logged-in admin-bar custom-background
    one-sidebar-right gecko-browser windows-os customize-support
</asp:Content>
