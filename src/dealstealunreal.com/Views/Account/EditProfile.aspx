<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViewMasterPageLayout.Master" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.EditProfile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit profile | DEAL.STEAL.UNREAL.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="post-412 page type-page status-publish hentry" id="post-412">
	<h2 class="post-title">Edit profile</h2>

	<div class="post-content">
	    <p><strong>	</strong></p><div id="wppb_modify" class="wppb_holder">
	
        <% using (Html.BeginForm("EditProfile", "Account", FormMethod.Post, new { Class = "user-forms", enctype = "multipart/form-data", id="edituser" }))
                           { %>
<input type="hidden" value="10485760" name="MAX_FILE_SIZE"><!-- set the MAX_FILE_SIZE to the server's current max upload size in bytes -->		

	<p class="nameHeader"><strong>PERSONAL DETAILS</strong></p>
			<p class="username">
                <%: Html.LabelFor(m => m.UserName,"Username")%>
                <%: Html.TextBoxFor(m => m.UserName, new { Class = "text-input", disabled = "disabled" })%>
                <span class="wppb-description-delimiter"> The usernames cannot be changed.</span>
			</p><!-- .first_name -->
            <p class="contactInfoHeader"><strong>LOGIN DETAILS</strong></p>
				<p class="form-email">
                    <%: Html.Raw(HttpUtility.HtmlDecode(Html.LabelFor(m => m.Email, "E-mail<font color=\"red\" title=\"This field is marked as required by the administrator\">*</font>").ToHtmlString()))%>
                    <%: Html.TextBoxFor(m => m.Email, new { Class = "text-input" })%>
					<span class="wppb-description-delimiter">(required)</span>
				</p><!-- .form-email -->
            <p class="aboutYourselfHeader"><strong>CHANGE PASSWORD?</strong></p>
			<p class="form-password">
                <%: Html.LabelFor(m => m.Password, "New Password")%>
                <%: Html.TextBoxFor(m => m.Password, new { Class = "text-input", type="password" })%>
			</p><!-- .form-password -->

			<p class="form-password">
                <%: Html.LabelFor(m => m.ConfirmPassword, "Repeat Password")%>
                <%: Html.TextBoxFor(m => m.ConfirmPassword, new { Class = "text-input", type="password" })%>
			</p><!-- .form-password -->
			<p class="form-upload1">
                <%: Html.LabelFor(m => m.ProfilePicture, "Profile Picture")%>
                <%: Html.TextBoxFor(m => m.ProfilePicture, new { size = "30", type="file" })%>
                <span class="wppb-max-upload">(max upload size 10Mb)</span>
				<span class="wppb-description-delimiter"></span>
                <span class="wppb-description-delimiter"><u>Current avatar</u>: </span>
                <span class="wppb-description-delimiter">
                <% if (Model.ProfilePicturePath.Equals("~/images/default_user_profile.jpg"))
                   { %>
                   <i>No uploaded avatar</i>
                <%}
                   else
                   { %>
                   <%:Html.GravatarImage(Model.Email, new Dictionary<string, string> { { "class", "avatar avatar-80 photo" }, { "width", "80" }, { "height", "80" } }, 80, GravatarHtmlHelper.DefaultImage.Default, Request.Url.Authority + Url.Content(Model.ProfilePicturePath))%>
                <% } %>
                
                	
			</span></p><!-- .form-upload1 -->				
				<p class="form-submit">
					<input type="submit" value="UPDATE" class="submit button" id="updateuser" name="updateuser">
				</p><!-- .form-submit -->							
                <!-- #edituser -->
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

<asp:Content ID="Content4" ContentPlaceHolderID="BodyClass" runat="server">
    page page-template-default custom-background one-sidebar-right gecko-browser windows-os customize-support
</asp:Content>
