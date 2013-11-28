<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master"  Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.LogOn>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Login | DEAL.STEAL.UNREAL.
</asp:Content>

<asp:Content ID="HeaderContent1" ContentPlaceHolderID="CustomHeader" runat="server">
    <link rel="stylesheet" href="<%=Url.Content("~/css/front.end.white.css") %>" type="text/css" media="all" />
</asp:Content>

<asp:Content ID="ClassContent3" ContentPlaceHolderID="BodyClass" runat="server">
    page page-template-default custom-background one-sidebar-right gecko-browser windows-os customize-support
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="post-login" class="page hentry">
	    
	    <h2 class="post-title">Login</h2>

	    <div class="post-content">
	        <p><strong></strong></p>
            <div id="wppb_login">		
		    <% using (Html.BeginForm("LogOn","Account",FormMethod.Post,new {Class="sign-in"})) { %>
            <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
				    <p>
					    <%: Html.LabelFor(m => m.UserName) %>
                        <%: Html.TextBoxFor(m => m.UserName, new { Class="text-input" })%>
                        <%: Html.ValidationMessageFor(m => m.UserName) %>
				    </p><!-- .form-username -->
				    <p>
                        <%: Html.LabelFor(m => m.Password) %>
                        <%: Html.PasswordFor(m => m.Password, new { Class = "text-input" })%>
                        <%: Html.ValidationMessageFor(m => m.Password) %>
				    </p><!-- .form-password -->			<p class="login-form-submit">
								    <input name="submit" class="submit button" value="LOG IN" type="submit">
				
						    <%: Html.CheckBoxFor(m => m.RememberMe, new { Class="remember-me checkbox", Checked="true"})%>
                            <%: Html.LabelFor(m => m.RememberMe) %>

				    <input name="action" value="log-in" type="hidden">
				    <input name="button" value="page" type="hidden">
				    <input name="formName" value="login" type="hidden">
			    </p><!-- .form-submit -->
			
						    <p>
							    <strong><%: Html.ActionLink("Lost password?", "RecoverPassword", "Account")%></strong>
						    </p>		<% } %><!-- .sign-in -->
			    <div class="social_connect_ui">
					    <div style="margin-bottom: 3px;"><label>Connect with:</label></div>
				    <div title="Social Connect">
							    <a href="javascript:void(0);" title="Facebook">
							        <img alt="Facebook" src="../images/facebook-login-button.png">
							    </a>
					
		    </div>			    
    </div> <!-- End of social_connect_ui div -->
		
        </div><strong>
	    <br />
    </strong><p></p>

	        <div class="clear"></div>

	    	</div><!-- END .post-content -->

        <div class="clear"></div>
    </div><!-- #post-## -->

</asp:Content>
