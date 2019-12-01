<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Query._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Movie Title Query Website</h1>
        
        <p class="lead">
            <asp:Label ID="Label3" runat="server" Text="Movie Title:"></asp:Label><asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
        </p>
        <p class="lead">
            <asp:Button ID="Button3" runat="server" Text="Search this movie"  OnClick="Button3_Click" />
        </p>
        <p class="lead">
            <asp:Label ID="Label1" runat="server" Text="Query result"></asp:Label>
        &nbsp;</p>
        <p class="lead">
            <asp:Label ID="Results1" runat="server" style="width:300px;" maximunsize="300px" autosize="true"></asp:Label>
        </p>
        <p class="lead">
            <asp:Image ID="Image1" runat="server" autosize="true"></asp:Image>
        </p>
    </div>

    <div class="row">
    </div>

</asp:Content>
