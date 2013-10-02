<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%
    if (Request.IsAuthenticated) {
%>
<div class="deal_profile">
    <div class="left"> 
        <div class="d_username"></div> 
        <div class="deal_posted"></div> 
        <div class="points"></div>
    </div>
    <div class="pro_img">
        
    </div>
</div>
<%
    }
%>