﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ComingEdit.aspx.cs" Inherits="PartB.ComingEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>Movie Title: <asp:TextBox ID="movieTitle" runat="server"></asp:TextBox></div>
    <div>Short Description: <asp:TextBox ID="shortDescription" runat="server"></asp:TextBox></div>
    <div>Long Description: <asp:TextBox ID="longDescription" runat="server"></asp:TextBox></div>
    <div>Movie Image: <asp:TextBox ID="movieImage" runat="server" Enabled="false" Text="MovieComingSoon.png"></asp:TextBox></div>
    <div><asp:Button ID="submit" OnClick="Submit_Click" runat="server" Text="Submit" /> |
         <asp:Button ID="cancel" OnClick="Cancel_Click" runat="server" Text="Cancel" />
    </div>
    <div><pre><asp:Label ID="Label1" runat="server" Text=""></asp:Label></pre></div>
</asp:Content>
