<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.ForgotPassword>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Recover password | DEAL.STEAL.UNREAL.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="post-414 page type-page status-publish hentry" id="post-414">
	<h2 class="post-title">Recover password</h2>

	<div class="post-content">
	    <p><strong>
	</strong></p><div id="wppb_recover_password" class="wppb_holder"><strong>


						<p class="warning"><%: Html.ValidationSummary("Unable to reset password")%></p><!-- .warning -->  
                        <% using (Html.BeginForm("RecoverPassword", "Account", FormMethod.Post, new { Class = "user-forms", enctype = "multipart/form-data" }))
                           { %>                     
                        Please enter your username or email address.<br>You will receive a link to create a new password via email.<br><br>
							<p class="username_email">
								<%: Html.LabelFor(m => m.UsernameOrEmail)%>
                                <%: Html.TextBoxFor(m => m.UsernameOrEmail, new { Class = "input", size = "20" })%>                                
							</p><!-- .username_email -->	
						<p class="form-submit">
							<input type="submit" value="Get New Password" class="submit button" id="recover_password" name="recover_password" />
						</p><!-- .form-submit -->					
                        <!-- #recover_password -->
                        <% } %>
		</strong></div><strong>
	
</strong><p></p>

	    <div class="clear"></div>

	    	</div><!-- END .post-content -->

    <div class="clear"></div>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CustomHeader" runat="server">
    <link rel="stylesheet" href="<%=Url.Content("~/css/front.end.white.css") %>" type="text/css" media="all" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyClass" runat="server">
    page page-template-default custom-background one-sidebar-right gecko-browser windows-os customize-support
</asp:Content>
