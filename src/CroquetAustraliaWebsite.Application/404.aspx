<%@ Page Language="C#" %>
<%
// Reference: http://benfoster.io/blog/aspnet-mvc-custom-error-pages
Response.StatusCode = 404;
Response.WriteFile(MapPath("~/404.html"));
if (Request.IsLocal)
{
    Response.Write("<p>404.aspx</p>");
}
%>
