﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ariline.ascx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.UserControl.Ariline" %>
<asp:TextBox runat="server" ID="txtAriline" Width="40px" Height="22px" onkeyup="SelectItem(this,'-')" MaxLength="3" CssClass="text fl"></asp:TextBox>
<asp:DropDownList runat="server" CssClass="text" ID="ddlAirlins" Width="130px" onchange="ShowKey(this,'-',0)"></asp:DropDownList>