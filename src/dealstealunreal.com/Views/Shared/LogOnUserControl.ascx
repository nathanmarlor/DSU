<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <%: Html.ActionLink("LOG OUT", "LogOff", "Account") %> 
<%
    }
    else {
%> 
         <%: Html.ActionLink("LOG IN", "LogOn", "Account") %> 
<%
    }
%>
