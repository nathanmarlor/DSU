<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dealstealunreal.com.Models.Deals.Deal>" %>

<div style="float: left; left: -50px; position: fixed; top: 173px; z-index: 10;">
    <!--<span style="font-family: 'Abel',sans-serif;float: left;font-family: 'Abel',sans-serif; left: 17px;position: relative;">Login to submit your Deal</span><br>-->
    <a style="display: block; box-shadow: none; width: 216px; height: 60px; cursor: pointer;
        float: right; margin: 20px;" onclick="openBox()" title="Submit Deal" href="javascript:void(0);"
        id="submit-deal"></a>
</div>
<div id="underlay">
</div>
<div class="white_content" id="light" style="display: none;">
    <a style="float: right; text-decoration: none; color: #000;" 
        <% if (Request.IsAuthenticated){ %> onclick="document.getElementById('light').style.display='none';document.getElementById('fade').style.display='none'"<% } else { %>
        onclick="loginFirst('http://www.DealStealUnreal.com','Please log in to submit deal.')"<% } %>
        href="javascript:void(0)">Close</a>
    <%
        if (Request.IsAuthenticated)
        {
            using (Ajax.BeginForm(null, null, new AjaxOptions { HttpMethod = "Post" }, new { id = "submitdealform", name = "submitdealform" }))
            { %>
    <table width="535" border="0">
        <tbody>
            <tr>
                <td colspan="3">
                    <b><u>SUBMIT DEAL</u></b>
                    <span id="topError" style="display: none; max-width: 200px" class="error1">Error!</span>
                </td>
            </tr>
            <tr>
                <td width="108">
                    Title:<span class="asterisk">*</span>
                </td>
                <td width="217">
                    <%: Html.TextBoxFor(m => m.Title, new { Class = "inputstyle", id = "deal_title" })%>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="108">
                    Retailer:<span class="asterisk">*</span>
                </td>
                <td width="217">
                    <%: Html.TextBoxFor(m => m.Retailer, new { Class = "inputstyle", id = "deal_retailer" })%>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="108">
                    Deal URL:<span class="asterisk">*</span>
                </td>
                <td width="217">
                    <%: Html.TextBoxFor(m => m.Url, new { Class = "inputstyle", id = "deal_url" })%>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Price (£):<span class="asterisk">*</span>
                </td>
                <td>
                    <%: Html.TextBoxFor(m => m.Price, new { Class = "inputstyle", id = "deal_price" })%>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Image URL:
                </td>
                <td>
                    <%: Html.TextBoxFor(m => m.ImageUrl, new { Class = "inputstyle", id = "deal_image_url" })%>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="108" valign="top">
                    Description:
                </td>
                <td width="217">
                    <%: Html.TextAreaFor(m => m.Description, new { Class = "inputstyle", style = "resize: none;", id = "deal_description" })%>
                </td>
                <td width="121">
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <input type="button" onclick="addDeal();" value="Submit" class="Btn" id="submit" name="submit" />
                    &nbsp;
                    <input type="button" onclick="clearFields();" value="Reset" class="Btn" id="reset" name="reset" />
                </td>
                <td>
                </td>
            </tr>
        </tbody>
    </table>
    <% }
    }
    %>
</div>
<div class="black_overlay" id="fade" style="display: none;">
</div>
