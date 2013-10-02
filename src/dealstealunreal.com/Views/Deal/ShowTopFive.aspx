<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<dealstealunreal.com.Models.Deals.Deal>>" %>

<div id="sidebar">
    <div id="top5">
        <h3 class="top5">
            DEALS OF THE WEEK</h3>
        <ul class="top">
        <% foreach(var deal in Model){ %>
            <li>
                <div class="top_deal_img">
                    <a target="_blank" href="<%:deal.Url %>">
                        <img width="50px" src="<%:deal.ImageUrl %>">
                    </a>
                </div>
                <div class="top_deal_title">
                    <a target="_blank" href="<%:deal.Url %>"><%:deal.Title %></a></div>
            </li>
            <% } %>
        </ul>
    </div>
</div>