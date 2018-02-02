﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class ProcessRefund : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    string lockErrorMsg;
                    if (Lock(applyformId, LockRole.Platform, "退/废票处理", out lockErrorMsg))
                    {
                        if (applyform == null)
                        {
                            showErrorMessage("退/废票申请单不存在");
                        }
                        else
                        {
                            RefundApplyform refundApplyform = applyform as RefundApplyform;
                            if (refundApplyform != null && (refundApplyform.RefundType == RefundType.Involuntary || refundApplyform.RefundType == RefundType.SpecialReason))
                            {
                                bindAttachment(applyform.Id);
                            }
                            bindData(applyform);
                        }
                    }
                    else
                    {
                        showErrorMessage("锁定申请单失败。原因:" + lockErrorMsg);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }

        private void bindAttachment(decimal applyformId)
        {
            var attachment = ApplyformQueryService.QueryApplyAttachmentView(applyformId);
            UserControl.OutPutImage outPutImage = LoadControl(ResolveUrl("~/UserControl/OutPutImage.ascx")) as UserControl.OutPutImage;
            outPutImage.ApplyAttachment = attachment;
            outPutImage.IsPlatform = true;
            outPutImage.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            outPutImage.EnableTheming = false;
            outPutImage.ApplyformId = applyformId;
            divApplyAttachment.InnerHtml = string.Format("<h3 class=\"titleBg\">附件</h3><div id=\"divOutPutImage\" class=\"clearfix\">{0}</div>", outPutImage.Binddata());
        }

        private void bindData(RefundOrScrapApplyform applyform)
        {
            bindButtonView(applyform);
            bindHeader(applyform);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindPolicyRemark(applyform);
            //bindNewPriceInputer(applyform);
        }

        private void bindNewPriceInputer(RefundOrScrapApplyform applyform)
        {
            ModPriceInfo.Visible = applyform.RequireRevisePrice;
        }

        private void bindButtonView(RefundOrScrapApplyform applyform)
        {
            switch (applyform.Status)
            {
                case RefundApplyformStatus.AppliedForCancelReservation:
                    RedircetToProvider.Visible = true;
                    break;
                case RefundApplyformStatus.DeniedByProviderBusiness:
                    btnDenyRefund.Visible = true;
                    btnReDeal.Visible = true;
                    if (applyform.IsSpecial && applyform is RefundApplyform && ((RefundApplyform)applyform).RefundType == RefundType.SpecialReason)
                    {
                        SetServiceCharge.Visible = true;
                    }
                    break;
                case RefundApplyformStatus.AppliedForPlatform:
                    RedircetToProviderRefund.Visible = true;
                    SetServiceCharge.Visible = true;
                    break;
            }

        }

        private void bindHeader(RefundOrScrapApplyform applyform)
        {
            RefundApplyform refundForm = applyform as RefundApplyform;
            var orderRole = OrderRole.Platform;
            lblApplyformId.Text = applyform.Id.ToString();
            linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = string.Format("{0} {1}", applyform,
                refundForm != null ? (string.Format("({0})", refundForm.RefundType.GetDescription())) : string.Empty);
            var product = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription();
            }
            lblStatus.Text = StatusService.GetRefundApplyformStatus(applyform.Status, orderRole);
            if (applyform.Order.Provider != null && applyform.Order.Provider.Product is Service.Order.Domain.CommonProductInfo)
            {
                this.lblTicketType.Text = (applyform.Order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            }
            else
            {
                this.lblTicketType.Text = "-";
            }
            lblPNR.Text = AppendPNR(applyform.NewPNR, string.Empty);
            lblPNR.Text += AppendPNR(applyform.OriginalPNR, string.IsNullOrWhiteSpace(lblPNR.Text) ? string.Empty : "原编码：");

            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm:ss");
            lblAppliedReason.Text = applyform.ApplyRemark;
            if (applyform.ProcessedTime.HasValue)
            {
                this.lblProcessedTime.Text = applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm");
                this.lblProcessedResult.Text = StatusService.GetRefundApplyformStatus(applyform.Status, orderRole) + " " + applyform.ProcessedFailedReason;
            }
        }
        List<PNRPair> RenderedPNR = new List<PNRPair>();
        string AppendPNR(PNRPair pnr, string tip)
        {
            if (PNRPair.IsNullOrEmpty(pnr)) return string.Empty;
            if (RenderedPNR.Any(pnr.Equals)) return string.Empty;
            var result = new StringBuilder(" ");
            result.Append(tip);
            if (!string.IsNullOrWhiteSpace(pnr.PNR)) result.AppendFormat(PNRFORMAT, pnr.PNR.ToUpper(), "小");
            result.Append(" ");
            if (!string.IsNullOrWhiteSpace(pnr.BPNR)) result.AppendFormat(PNRFORMAT, pnr.BPNR.ToUpper(), "大");
            RenderedPNR.Add(pnr);
            return result.ToString();
        }
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";



        private void bindVoyages(RefundOrScrapApplyform applyform)
        {
            this.voyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight));
            ChangedServiceCharge.Value = renderedServiceCharge.InnerText = Math.Round(getServiceCharge(applyform.Passengers.First(), applyform.Order.IsSpecial), 2).ToString();
        }

        private void bindPassengers(RefundOrScrapApplyform applyform)
        {
            this.passengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(f => f.OriginalFlight));
            PassengerCount = applyform.Passengers.Count();
        }

        private void bindPolicyRemark(RefundOrScrapApplyform applyform)
        {
            divPolicyRemark.Visible = true;
            var product = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            divPolicyRemarkContent.InnerHtml =
                string.Format("{0} <span class='systemEndFix'>{1}</span>", product == null ? string.Empty : product.Remark, SystemParamService.PolicyRemark);
        }

        //protected void btnAgree_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        decimal applyformId;
        //        if (decimal.TryParse(Request.QueryString["id"], out applyformId))
        //        {
        //            ApplyformProcessService.PlatformProcessRefundApplyform(applyformId, true, CurrentUser.UserName);
        //            ReleaseLock(applyformId);
        //            RegisterScript(this, "alert('同意退/废票成功！');location.href='" + ReturnUrl + "'", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowExceptionMessage(ex, "同意退/废票");
        //    }
        //}

        //protected void btnNotAgree_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        decimal applyformId;
        //        if (decimal.TryParse(Request.QueryString["id"], out applyformId))
        //        {
        //            ApplyformProcessService.PlatformProcessRefundApplyform(applyformId, false, CurrentUser.UserName);
        //            ReleaseLock(applyformId);
        //            RegisterScript(this, "alert('不同意退/废票成功！');location.href='" + ReturnUrl + "'", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowExceptionMessage(ex, "不同意退/废票");
        //    }
        //}

        protected void btnReleaseLockAndBack_Click(object sender, EventArgs e)
        {
            decimal applyformId;
            if (decimal.TryParse(Request.QueryString["id"], out applyformId))
            {
                ReleaseLock(applyformId);
                RegisterScript(this, "location.href='" + ReturnUrl + "'", true);
            }
        }

        protected void RefundInfos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var Passengers = e.Item.FindControl("Passengers") as Repeater;
            if (Passengers != null)
            {
                Passengers.DataSource = DataBinder.Eval(e.Item.DataItem, "Passengers");
                Passengers.DataBind();
            }
        }
        private void setBackButton()
        {
            var returnUrl = ReturnUrl;
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }

        private string ReturnUrl
        {
            get
            {
                var returnUrl = Request.QueryString["returnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = "ChangeProcessList.aspx";
                }
                if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
                return returnUrl;
            }
        }

        protected int PassengerCount
        {
            get
            {
                if (ViewState["pCount"] == null)
                {
                    return 1;
                }
                return (int)ViewState["pCount"];
            }
            set
            {
                ViewState["pCount"] = value;
            }
        }

        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected void RedircetToProvider_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    ApplyformProcessService.ReservationCanceled(applyformId, CurrentUser.UserName);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('指向出票方成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "指向出票方");
            }

        }

        protected void btnDenyRefund_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    ApplyformProcessService.DenyRefundOrScrapByPlatform(applyformId, Reason.Text.Trim(), CurrentUser.UserName);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('拒绝退/废票成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "拒绝退/废票");
            }

        }

        protected void btnReDeal_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    var applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    //特殊原因特殊退票 提交修改服务费
                    if (applyform.IsSpecial && applyform is RefundApplyform && ((RefundApplyform)applyform).RefundType == RefundType.SpecialReason)
                    {
                        ApplyformProcessService.ReRefund(applyformId, applyform.Flights.Select(f => new PlatformProcessRefundView
                        {
                            AirportPair = new AirportPair(f.OriginalFlight.Departure.Code, f.OriginalFlight.Arrival.Code),
                            ServiceCharge = decimal.Parse(ChangedServiceCharge.Value)
                        }
                    ), ChangeServiceChargeReason.Text, CurrentUser.UserName);
                    }
                    else
                    {
                        ApplyformProcessService.ReRefund(applyformId, CurrentUser.UserName);
                    }
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('重新退/废票成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "重新退/废票");
            }

        }

        /// <summary>
        /// 修改服务费并指向出票方退票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToRefund(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    var applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    ApplyformProcessService.PlatformProcessRefundApplyform(applyformId, applyform.Flights.Select(f => new PlatformProcessRefundView
                    {
                        AirportPair = new AirportPair(f.OriginalFlight.Departure.Code, f.OriginalFlight.Arrival.Code),
                        ServiceCharge = decimal.Parse(ChangedServiceCharge.Value)
                    }
                    ), getIncreasing(applyform.Passengers.First()), ChangeServiceChargeReason.Text, CurrentUser.UserName);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('修改服务费成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改服务费");
            }

        }

        private decimal getServiceCharge(Passenger passenger, bool isSpecial)
        {
            var result = 0M;
            if (isSpecial)
            {
                result += (from ticket in passenger.Tickets
                           from flight in ticket.Flights
                           where flight.Bunk is Service.Order.Domain.Bunk.SpecialBunk
                           select (flight.Bunk as Service.Order.Domain.Bunk.SpecialBunk).ServiceCharge).Sum();
            }
            return result;
        }

        private decimal getIncreasing(Passenger passenger)
        {
            var result = 0M;
            result += (from ticket in passenger.Tickets
                       from flight in ticket.Flights
                       where flight.Bunk is Service.Order.Domain.Bunk.SpecialBunk
                       select flight.Increasing).Sum();
            return result;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            decimal attachmentId;
            string id = Request.QueryString["id"];
            if (decimal.TryParse(id, out attachmentId))
            {
                try
                {
                    string filePath = FileService.Upload(fileAttachment, "RefundApplyformView", "(jpg)|(bmp)|(png)", 600 * 1024);
                    List<ApplyAttachmentView> list = new List<ApplyAttachmentView>();
                    var bytes = ChinaPay.B3B.Service.FileService.GetFileBytes(filePath);
                    Thumbnail thumbnail = new Thumbnail();
                    list.Add(new ApplyAttachmentView
                    {
                        Id = Guid.NewGuid(),
                        ApplyformId = attachmentId,
                        FilePath = filePath,
                        Thumbnail = thumbnail.MakeThumb(100, bytes),
                        Time = DateTime.Now
                    });
                    ApplyformQueryService.AddApplyAttachmentView(list, CurrentUser.UserName);
                    bindAttachment(attachmentId);
                    ShowMessage("上传成功");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "上传");
                }
            }
        }
    }
}