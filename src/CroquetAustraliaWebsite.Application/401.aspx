<%@ Page Language="C#" %>
<%
// Reference: http://benfoster.io/blog/aspnet-mvc-custom-error-pages
Response.StatusCode = 401;
Response.WriteFile(MapPath("~/401.html"));
if (Request.IsLocal)
{
    Response.Write("<p>401.aspx</p>");
}
%>
