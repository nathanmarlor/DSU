<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.Wrappers.OrderedDeals>" %>

<% foreach (var deal in Model.Deals)
   { %>
<div class="post-<%:deal.DealID %> post type-post status-publish format-standard hentry deals-steal"
    id="post-<%:deal.DealID %>">
    <% if (!Model.CurrentUsername.Equals(deal.UserName))
       { %>
    <div class="deal_bt">
    <% if (Request.IsAuthenticated)
       {
           if (deal.UserName != Model.CurrentUsername && deal.CanVote)
           { %>
        <div class="neg_deal">
            <% using (Html.BeginForm("Voting", "Deal", FormMethod.Post))
               { %>
            <%: Html.Hidden("vote", dealstealunreal.com.Models.Vote.NegativeVote)%>
            <%: Html.Hidden("dealId", deal.DealID)%>
            <input type="submit" class="no_deal" name="no_deal_x" />
            <% } %>
        </div>
        <div class="pos_deal">
            <% using (Html.BeginForm("Voting", "Deal", FormMethod.Post))
               { %>
            <%: Html.Hidden("vote", dealstealunreal.com.Models.Vote.PositiveVote)%>
            <%: Html.Hidden("dealId", deal.DealID)%>
            <input type="submit" class="deal" name="deal_x" />
            <% } %>
        </div>
        <% }
           else
           {
           %><%
           }
       }
       else
       { %>
       <div class="neg_deal">
        <a class="no_deal" onclick="loginFirst('<%: Url.Action("LogOn","Account") %>','Please log in to vote.')" href="javascript:void(0);"></a>
       </div>
        <div class="pos_deal">
            <a class="deal" onclick="loginFirst('<%: Url.Action("LogOn","Account") %>','Please log in to vote.')" href="javascript:void(0);"></a>
       </div>
        <% } %>
    </div>
    <% } %>
    <h2 class="deal-title">
        <a target="_blank" href="<%: deal.Url %>"><%: deal.Title %>- <b>£<%: deal.Price %></b>
            @ <%: deal.Retailer %></a></h2>
    <div id="frontpostmeta" class="post-meta">
        <span class="post-author">by <%: Html.ActionLink(deal.UserName, "ShowProfile", new { Controller="Account", UserID=deal.UserName })%> </span>
        <!-- .post-author -->
        <span class="post-date">on <%: deal.Date.ToShortDateString() %> , </span>
        <!-- .post-author -->
        <a onclick="openBoxId('description<%:deal.DealID %>')" title="View Description" id="<%:deal.DealID %>" class="a-description"
            href="javascript:void(0);">DESCRIPTION</a> <a onclick="openBoxId('comment<%:deal.DealID %>')" title="View Comments"
                id="<%:deal.DealID %>" class="a-comment" href="javascript:void(0);">COMMENTS</a>
    </div>
    <!-- END .post-meta -->
    <div class="post-content">
        <div id="description<%:deal.DealID %>" class="deal_description white_content" style="display: none;">
            <a style="float: right; text-decoration: none; color: #000;" onclick="closeBoxId('description<%:deal.DealID %>')"
                href="javascript:void(0)">Close</a>
                <% if (Model.CurrentUsername.Equals(deal.UserName))
                   { %>
            <a style="float:left; text-decoration:none;color:#000;" onclick="closeOpenBoxId('description<%:deal.DealID %>','textarea<%:deal.DealID %>')" class="a-edit" href="javascript:void(0)">Edit</a>   
            <%} %> 
                <p class="description">
                    <%: deal.Description %></p>
        </div>
        <% Html.RenderAction("Comment", new {DealID=deal.DealID});%>
        <div id="textarea<%:deal.DealID %>" style="display: none;" class="white_content deal_edit">
            <a style="float: right; text-decoration: none; color: #000;" onclick="closeBoxId('textarea<%:deal.DealID %>')"
                href="javascript:void(0)">Close</a>
                <% using (Html.BeginForm("EditDescription", "Deal", FormMethod.Post))
                   { %>
            Description:
            <%:Html.TextArea("DealDescriptionEdit", deal.Description, new { rows="6", Class="inputstyle", style="resize: none;", id="deal_description_edit_"+deal.DealID })%>
            <span id="dealDescriptionError<%:deal.DealID %>" style="display: none; max-width: 150px" class="error1">
                Max 100 words</span>
                <%:Html.Hidden("dealId",deal.DealID) %>
            <input type="submit" value="Submit" class="Btn" id="submit"
                name="submit" />
            <% } %>
        </div>
        <div class="deal_img">
            <a target="_blank" href="<%:deal.Url %>">
                <img width="70%" src="<%:deal.ImageUrl %>">
            </a>
        </div>
        <div class="maintitebar">
            <div class="nodeal_bar bar">
                THIS IS...</div>
            <div class="deal_bar bar">
            </div>
            <div class="steal_bar bar">
            </div>
            <div class="unreal_bar bar">
            </div>
        </div>
        <div class="mygridcontainer">
            <div style="width: <%:deal.Votes %>%" class="blocks">
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<% } %>
