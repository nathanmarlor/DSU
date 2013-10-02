<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul id="top-menu" class="menu">
<li id="menu-item-384" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-home menu-item-384 <% if(ViewContext.Controller.ValueProvider.GetValue("Action").RawValue.ToString().Equals("Index")){ %>current_page_item<% } %>"><%: Html.ActionLink("DEALS", "Index", "Home")%></li>
<%
    if (Request.IsAuthenticated) {
%>
<li id="menu-item-378" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-378 <% if(ViewContext.Controller.ValueProvider.GetValue("Action").RawValue.ToString().Equals("ShowProfile")){ %>current_page_item<% } %>"><%: Html.ActionLink("MY PROFILE", "ShowProfile", "Account")%>
<ul class="sub-menu sf-js-enabled sf-shadow">
	<li id="menu-item-421" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-421" style="white-space: normal; float: none; width: 100%;"><%: Html.ActionLink("Edit", "EditProfile", "Account")%></li>
</ul>
</li>
<li id="menu-item-382" class="reward menu-item menu-item-type-post_type menu-item-object-page menu-item-382 <% if(ViewContext.Controller.ValueProvider.GetValue("Action").RawValue.ToString().Equals("OurRewardSystem")){ %>current_page_item<% } %>"><%: Html.ActionLink("REWARDS", "OurRewardSystem", "Home")%></li>
<%
    }
    else {
%> 
<li id="menu-item-381" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-381 <% if(ViewContext.Controller.ValueProvider.GetValue("Action").RawValue.ToString().Equals("JoinTheTeam")){ %>current_page_item<% } %>"><%: Html.ActionLink("HOW IT WORKS", "JoinTheTeam", "Home")%></li>
<%
    }
%>
<li id="menu-item-383" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-383 <% if(ViewContext.Controller.ValueProvider.GetValue("Action").RawValue.ToString().Equals("AboutUs")){ %>current_page_item<% } %>"><%: Html.ActionLink("ABOUT US", "AboutUs", "Home")%></li>
<li id="menu-item-385" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-385"><% Html.RenderPartial("LogOnUserControl"); %>
<%
    if (!Request.IsAuthenticated) {
%>
<ul class="sub-menu">
	<li id="menu-item-419" class="Register menu-item menu-item-type-post_type menu-item-object-page menu-item-419"><%: Html.ActionLink("REGISTER", "Register", "Account")%></li>
	<li id="menu-item-420" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-420"><%: Html.ActionLink("RECOVER PASSWORD", "RecoverPassword", "Account")%></li>
</ul>
<%
    }
%>
</li>
</ul>