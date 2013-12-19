<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.Wrappers.UserDeals>" %>

<div class="deal_profile">
    <div class="left"> 
        <div class="d_username"><%:Model.User.UserName %></div> 
        <div class="deal_posted">Deals Posted:  <%:Model.Deals.Count() %></div> 
        <% if (Model.IsCurrentUser)
           { %>
        <div class="points">Points:  <%:Model.User.Points%></div>
        <% } %>
    </div>
    <div class="pro_img">
        <%:Html.GravatarImage(Model.User.ProfilePicture, new Dictionary<string, string> { { "class", "avatar avatar-80 photo" }, { "width", "80" }, { "height", "80" } }, 80, GravatarHtmlHelper.DefaultImage.Default, Request.Url.Authority + Url.Content(Model.User.ProfilePicture))%>
    </div>
</div>
