<%@ Page Language="C#" %>
<%
// Reference: http://benfoster.io/blog/aspnet-mvc-custom-error-pages
Response.StatusCode = 500;
Response.WriteFile(MapPath("~/500.html"));
if (Request.IsLocal)
{
    Response.Write("<p>500.aspx</p>");
}
%>
