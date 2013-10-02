<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.Wrappers.DealComments>" %>

<div id="comment<%:Model.Deal.DealID %>" class="deal_comments white_content" style="display: none;">
    <a style="float: right; text-decoration: none; color: #000;" onclick="closeBoxId('comment<%:Model.Deal.DealID%>')"
        href="javascript:void(0)">Close</a><div id="comments">
        </div>
        <% if (Model.Comments.Any())
           { %><ul class="commentlist">
           <% foreach(var comment in Model.Comments) { %>            
                <li id="li-comment-<%:comment.Key.UserName %>" class="comment byuser comment-author-<%:comment.Key.UserName %> even thread-even depth-1">
                    <table class="comment-wrapper" id="comment-<%:comment.Key.UserName %>">
                        <tbody>
                            <tr>
                                <td class="comment-avatar">
                                    <div style="position: relative;">
                                        <img width="30" height="30" class="avatar avatar-30 photo" src="<%:comment.Key.ProfilePicture %>"
                                            alt="">
                                            <%:Html.GravatarImage(comment.Key.Email, new Dictionary<string, string> {{"width","30"},{"height","30"},{"class","avatar avatar-30 photo"}},30,GravatarHtmlHelper.DefaultImage.Default,comment.Key.ProfilePicture)%>
                                        <span class="comment-avatar-overlay"></span>
                                    </div>
                                </td>
                                <!-- END .comment-avatar -->
                                <td class="comment-main">
                                    <div class="comment-meta">
                                        <span class="comment-author"><%:Html.ActionLink(comment.Key.UserName, "ShowProfile", new { Controller="Account",UserID=comment.Key.UserName })%></span>
                                    </div>
                                    <!-- END .comment-meta -->
                                    <div class="comment-content">
                                        <p>
                                            <%:comment.Value.CommentString %></p>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <!-- END .comment-content -->
                                    <span class="comment-date"><%:comment.Value.Date.ToString("MMMM dd,yyyy") %> at <%:comment.Value.Date.ToShortTimeString().ToLower() %></span>
                                    <div class="clear">
                                    </div>
                                    <div class="clear">
                                    </div>
                                </td>
                                <!-- END .comment-main -->
                            </tr>
                        </tbody>
                    </table>
                    <!-- #comment-27  -->
                    <div class="clear">
                    </div>
                </li>
                
        <% } %>
        <!-- #comment-## -->
        </ul>
        <%
        } else
           { %>
    <div class="comments-box">
        <p>
            No comments yet!</p>
    </div>
    <%} %>
    <% if (Request.IsAuthenticated && Model.CurrentUsername != string.Empty)
       { %>
    <div id="cForm<%:Model.Deal.DealID %>">
        <div class="comment-respond" id="respond">
            <h3 class="comment-reply-title" id="reply-title">
                COMMENTS: <small><a style="display: none;" href="/#respond" id="cancel-comment-reply-link"
                    rel="nofollow">Cancel reply</a></small></h3>
            <% using (Html.BeginForm("Comment", "Deal", FormMethod.Post, new { Class = "comment-form", id = "commentform" }))
               { %>
            <p class="logged-in-as">
                
                Logged in as
                <%: Html.ActionLink(Model.CurrentUsername, "ShowProfile", "Account")%>.
            </p>
            <p class="comment-form-comment">
                <%:Html.LabelFor(m => m.NewComment, "Comment")%>
                <%:Html.TextAreaFor(m => m.NewComment, 8, 45, new Dictionary<string, Object> { { "aria-required", "true" }, { "id", "comment" } })%>
                </p>
            <p class="form-allowed-tags">
                You may use these
                <abbr title="HyperText Markup Language">
                    HTML</abbr>
                tags and attributes: <code>&lt;a href="" title=""&gt; &lt;abbr title=""&gt; &lt;acronym
                    title=""&gt; &lt;b&gt; &lt;blockquote cite=""&gt; &lt;cite&gt; &lt;code&gt; &lt;del
                    datetime=""&gt; &lt;em&gt; &lt;i&gt; &lt;q cite=""&gt; &lt;strike&gt; &lt;strong&gt;
                </code>
            </p>
            <p class="form-submit">
                <%:Html.HiddenFor(m => m.Deal.DealID)%>
                <input type="submit" value="Post Comment" id="submit" name="submit" />
            </p>
            <% } %>
        </div>
        <!-- #respond -->
    </div>
    <!-- END #cForm -->
    <% }
       else
       { %>
    <div id="cForm123">
        <div class="comment-respond" id="Div1">
            <h3 class="comment-reply-title" id="H1">
                COMMENTS: <small><a style="display: none;" href="/#respond" id="A1" rel="nofollow">Cancel
                    reply</a></small></h3>
            <p class="must-log-in">
                You must be
                <%: Html.ActionLink("logged in", "LogOn", "Account")%>
                to post a comment.</p>
        </div>
        <!-- #respond -->
    </div>
    <% } %>
</div>
