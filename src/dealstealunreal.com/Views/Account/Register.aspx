<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.Register>" %>
<%@ Import Namespace="Recaptcha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Register | DEAL.STEAL.UNREAL.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="post-407 page type-page status-publish hentry" id="post-407">
	<h2 class="post-title">Register</h2>

	<div class="post-content">
	    <p style="text-align: left;"><strong>	</strong></p>
        <div id="wppb_register" class="wppb_holder">
        <p class="error"><%: Html.ValidationMessage("Recaptcha")%>
        <%: Html.ValidationMessage("System")%>
        <%: Html.ValidationMessageFor(m => m.Email) %>
        <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
        <%: Html.ValidationMessageFor(m => m.Password) %>
        <%: Html.ValidationMessageFor(m => m.ProfilePicture) %>
        <%: Html.ValidationMessageFor(m => m.UserName) %></p>
        <% using (Html.BeginForm("Register", "Account", FormMethod.Post, new { Class = "user-forms", enctype = "multipart/form-data", id="adduser" }))
                           { %>  
<input type="hidden" value="10485760" name="MAX_FILE_SIZE" />

	<p class="registerNameHeading"><strong>PERSONAL DETAILS</strong></p>
				<p class="form-username">
                    <%: Html.Raw(HttpUtility.HtmlDecode(Html.LabelFor(m => m.UserName,"Username<font color=\"red\" title=\"This field is required for registration.\">*</font>").ToHtmlString()))%>
                    <%: Html.TextBoxFor(m => m.UserName, new { Class = "text-input" })%>
				</p><!-- .form-username -->
    <p class="registerContactInfoHeading"><strong>LOGIN DETAILS</strong></p>
				<p class="form-email">
                    <%: Html.Raw(HttpUtility.HtmlDecode(Html.LabelFor(m => m.Email, "E-mail<font color=\"red\" title=\"This field is marked as required by the administrator\">*</font>").ToHtmlString()))%>
                    <%: Html.TextBoxFor(m => m.Email, new { Class = "text-input" })%>
				</p><!-- .form-email -->
             <p class="registerAboutYourselfHeader"><strong>About Yourself</strong></p>
			<p class="form-password">
                <%: Html.Raw(HttpUtility.HtmlDecode(Html.LabelFor(m => m.Password, "Password<font color=\"red\" title=\"This field is required for registration.\">*</font>").ToHtmlString()))%>
                <%: Html.TextBoxFor(m => m.Password, new { Class = "text-input", type="password" })%>
			</p><!-- .form-password -->
				 
			<p class="form-password">
                <%: Html.Raw(HttpUtility.HtmlDecode(Html.LabelFor(m => m.ConfirmPassword, "Repeat Password<font color=\"red\" title=\"This field is required for registration.\">*</font>").ToHtmlString()))%>
                <%: Html.TextBoxFor(m => m.ConfirmPassword, new { Class = "text-input", type="password" })%>
			</p><!-- .form-password -->
			<p class="form-upload1">
                <%: Html.Raw(HttpUtility.HtmlDecode(Html.LabelFor(m => m.ProfilePicture, "Profile Picture").ToHtmlString()))%>
                <%: Html.TextBoxFor(m => m.ProfilePicture, new { size = "30", type="file" })%>

				<span class="wppb-max-upload">(max upload size 10Mb)</span>
				<span class="wppb-description-delimiter">	
			</span></p><!-- .form-upload1 -->
			<p class="form-checkbox2">
					<label for="agreeToTerms2">I have read the terms &amp; conditions<font color="red" title="This field is marked as required by the administrator.">*</font></label>
                    <input type="checkbox" id="agreeToTerms2" name="agreeToTerms2" value="agree"><span class="agreeToTerms"></span>
                    <br/><br/>
                    </p>                   
                    <!-- .form-checkbox2 --><div class="form-reCAPTCHA">
                    <strong><label class="form-reCAPTCHA-label" for="Anti-Spam">Anti-Spam</label></strong>
                    <style type="text/css">#recaptcha_area{line-height:0;}</style>
                    <%: Html.Raw(Html.GenerateCaptcha()) %>
                    </div>                   
                    <!-- .form-reCAPTCHA -->							
						<p class="form-submit">
						    <input type="submit" value="Register" class="submit button" id="addusersub" name="adduser"/>
						    <input type="hidden" value="adduser" id="action" name="action"/>
						    <input type="hidden" value="register" name="formName"/>
						</p><!-- .form-submit -->					
<!-- #adduser -->
<% } %>
	
	</div><strong>
</strong><p></p>

	    <div class="clear"></div>

	    	</div><!-- END .post-content -->

    <div class="clear"></div>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CustomHeader" runat="server">
    <link rel="stylesheet" href="<%=Url.Content("~/css/front.end.white.css") %>" type="text/css" media="all" />
</asp:Content>
