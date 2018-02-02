﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="low_price_policy_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.low_price_policy_manage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户特价政策管理</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />

<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        特价政策管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col class="w30" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">航空公司：</span>
                                <asp:DropDownList ID="ddlAirline" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">OFFICE号：</span>
                                <asp:DropDownList ID="ddlOffice" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input radio_reset">
                                <span class="name">票证类型：</span>
                                <asp:RadioButton ID="radall" runat="server" Text="所有" Checked="true" GroupName="one" />
                                <asp:RadioButton ID="radBSP" runat="server" Text="BSP" GroupName="one" />
                                <asp:RadioButton ID="radB2B" runat="server" Text="B2B" GroupName="one" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">出发城市：</span>
                                <uc:City runat="server" ID="txtDeparture"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">是否有效：</span>
                                <asp:DropDownList ID="ddlYouxiao" runat="server">
                                    <asp:ListItem Text="-请选择-" Value="99"></asp:ListItem>
                                    <asp:ListItem Text="仅显示有效" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="仅显示过期" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input radio_reset">
                                <span class="name">政策状态：</span>
                                <asp:RadioButton ID="radAuditAll" runat="server" Text="所有" Checked="true" GroupName="radioone" />
                                <asp:RadioButton ID="radAudit" runat="server" Text="审核" GroupName="radioone" />
                                <asp:RadioButton ID="radUnAudit" runat="server" Text="未审" GroupName="radioone" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">中转城市：</span>
                                <uc:City runat="server" ID="txtTransit"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">行程类型：</span>
                                <asp:DropDownList ID="ddlVoyage" runat="server">
                                    <asp:ListItem Value="" Text="-请选择-" />
                                    <asp:ListItem Value="1" Text="单程" />
                                    <asp:ListItem Value="2" Text="往返" />
                                    <asp:ListItem Value="8" Text="中转联程" />
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">修改日期：</span>
                                <asp:TextBox ID="txtPubStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '#F{$dp.$D(\'txtPubEndTime\')||\'2020-10-01\'}' })"></asp:TextBox> -
                                <asp:TextBox ID="txtPubEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({ isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtPubStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                      <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City runat="server" ID="txtArrival"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">舱位：</span> 
                                <asp:TextBox ID="txtBunks" runat="server" CssClass="text"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({  readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox> -
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({  readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="btns" colspan="4">
                            <asp:Button ID="btnQuery" runat="server" CssClass="btn class1" Text="查 询" OnClick="btnQuery_Click" />
                            <asp:Button ID="btnPublish" runat="server" CssClass="btn class2" Text="发 布" OnClick="btnPublish_Click" />
                            <asp:Button ID="btnAudited" runat="server" CssClass="btn class2" Text="审 核" OnClick="btnAudited_Click" />
                            <asp:Button ID="btnUnAudited" runat="server" CssClass="btn class2" Text="取消审核" OnClick="btnUnAudited_Click" />
                            <asp:Button ID="btnDel" runat="server" CssClass="btn class2" Text="删 除" OnClick="btnDel_Click" />
                            <input type="button" value="清空条件" class=" btn class2" onclick="ResetSearchOption();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_bargain" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false" OnRowCommand="grv_bargain_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input type="checkbox" class="chkAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" class="chkOne" id="chk" runat="server" value='<%#Eval("id") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="Airline" HeaderText="航空公司" />
                <asp:TemplateField HeaderText="出发城市">
                    <ItemTemplate>
                        <span class="Departure">
                            <%#Eval("Departure")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="中转城市">
                    <ItemTemplate>
                        <span class="Transit">
                            <%#Eval("Transit")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="票证行程">
                    <ItemTemplate>
                        <%#Eval("TicketType")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="排除日期">
                  <ItemTemplate>
                    <span class="DepartureDateFilter">
                    <%#Eval("DepartureDateFilter")%>
                    </span>
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="适用班期">
                    <ItemTemplate>
                        <span class="DepartureWeekFilter">
                            <%#Eval("DepartureWeekFilter")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="航班限制">
                    <ItemTemplate>
                        <span class="Include">
                            <%#Eval("Include")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="提前天数" DataField="BeforehandDays" />
                <asp:BoundField HeaderText="适用舱位" DataField="Berths" />
                <asp:BoundField HeaderText="价格/折扣" DataField="PriceInfo" />
                <asp:TemplateField HeaderText="政策返佣">
                    <ItemTemplate>
                        <%#Eval("Commission")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="去程日期">
                    <ItemTemplate>
                        <%#Eval("DepartureDates")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="发布者" DataField="Opearor" />
                <asp:BoundField HeaderText="政策状态" DataField="Sudit" />
                <asp:BoundField HeaderText="是否挂起" DataField="Hang" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("id") %>' CommandName='<%#Eval("SuditName") %>'
                            runat="server"><%#Eval("SuditTip")%></asp:LinkButton><br />
                        <a href="javascript:ModifyCommissionLow('<%#Eval("id") %>','<%#Eval("InternalCommission") %>','<%#Eval("SubordinateCommission") %>','<%#Eval("ProfessionCommission") %>','<%#Eval("Price") %>','<%#Eval("Discount") %>','<%#Eval("PriceTypes") %>','<%#Eval("IsInternal") %>','<%#Eval("IsPeer") %>','<%#Eval("VoyageType") %>');">
                            修改返佣</a><br />
                        <asp:LinkButton ID="LinkButton2" CommandArgument='<%#Eval("id") %>' CssClass="del_click"
                            CommandName='del' runat="server">删 除</asp:LinkButton><br />
                        <a href='low_price_policy_edit.aspx?Id=<%#Eval("id") %>&Type=Update'>修改详细</a><br /> <a
                            href='low_price_policy_edit.aspx?Id=<%#Eval("id") %>&Type=Copy'>政策复制</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="box" id="showempty" visible="false" runat="server">
        没有任何符合条件的查询结果</div>
    <asp:HiddenField ID="hidIds" runat="server" />
    <asp:HiddenField ID="hdDefaultDate" runat="server" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            特价政策修改返佣</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr id="price">
                <td class="title">
                    <input type="radio" id="radPrice" runat="server" name="pricetype" value="0" />
                    <label for="radPrice">
                        价格：</label><br />
                    <input type="radio" id="radDiscount" class="discount1" runat="server" name="pricetype"
                        value="1" />
                    <label for="radDiscount" class="discount1">
                        折扣：</label>
                    <input type="radio" id="radCommission" class="commission1" runat="server" name="pricetype"
                        value="3" />
                    <label for="radCommission" class="commission1">
                        按返佣</label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblSign" CssClass="priceInfo" runat="server">￥</asp:Label><asp:TextBox
                        ID="txtPrice" Width="50px" MaxLength="4" CssClass="text priceInfo" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtDisCount" Width="50px" MaxLength="3" CssClass="text discount discount1"
                        runat="server"></asp:TextBox><asp:Label ID="lblDiscount" CssClass="discount discount1"
                            runat="server">折</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    下级返佣
                </td>
                <td>
                    <asp:TextBox ID="txtSubordinateCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
                <td class="title  allowBrotherPurchase">
                    同行返佣
                </td>
                <td class="allowBrotherPurchase">
                    <asp:TextBox ID="txtProfessionCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
            </tr>
            <tr>
                <td class="title canHaveSubordinate">
                    内部返佣
                </td>
                <td class="canHaveSubordinate">
                    <asp:TextBox ID="txtInternalCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
                <td class="title">
                </td>
                <td>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保&nbsp;&nbsp;&nbsp;存"
                        OnClick="btnSave_Click" />
                    <input class="close btn class2" value="关&nbsp;&nbsp;&nbsp;闭" type="button" />
                </td>
            </tr>
        </table>
    </div>
    
    <a id="divChooice" style="display: none;" data="{type:'pop',id:'divIsAll'}"></a>
    <div id="divIsAll" class="form layer" style="display: none">
        <h2>
            确认选择范围</h2>
        <div style="text-align:center;">
            <input type="button" id="btnAll" class="btn class1" value="选择所有政策" />
            <input type="button" id="btnCurrt" class="btn class1" value="选择当前页政策" />
            <input type="button" id="btnCancel" class="close btn class2" value="取消" />
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidIsAll" />
    <asp:HiddenField runat="server" ID="hfdVoyageType" />
    <asp:HiddenField runat="server" ID="hfdIsPeer" />
    <asp:HiddenField runat="server" ID="hfdIsInternal" />
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/checkboxchooice.js?20130416" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/polciy_public.js?20121205" type="text/javascript"></script>
    <%-- <script src="/Scripts/FixTable.js" type="text/javascript"></script>--%>
    <script src="/Scripts/jquery.tipTip.minified.js?20121114" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119?20121114" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/LowPricePolicy.js?2012111401" type="text/javascript"></script>
</body>
</html>
