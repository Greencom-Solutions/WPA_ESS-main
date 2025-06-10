using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Latest_Staff_Portal.Models;
using Latest_Staff_Portal.ViewModel;
using Newtonsoft.Json.Linq;

namespace Latest_Staff_Portal.Controllers
{
    public class PettyCashController : Controller
    {
        // GET: PettyCash
        public ActionResult PettyCashVouchersList(string status)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                ViewBag.Status = status;
                return View();
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public PartialViewResult PettyCashVouchersListPartialView(string status)
        {
            try
            {
                string staffNo = Session["Username"].ToString();
                List<PettyCashVouchers> pettyCashVouchersList = new List<PettyCashVouchers>();
                var employee = Session["EmployeeData"] as EmployeeView;
                var station = employee?.GlobalDimension1Code;
                var page = "";

                page = "PettyCashVouchers?$format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;

                        var pettyCashVoucher = new PettyCashVouchers
                        {
                            No = (string)config["No"],
                            Date = (string)config["Date"],
                            Pay_Mode = (string)config["Pay_Mode"],
                            Cheque_No = (string)config["Cheque_No"],
                            Cheque_Date = (string)config["Cheque_Date"],
                            Paying_Bank_Account = (string)config["Paying_Bank_Account"],
                            Payee = (string)config["Payee"],
                            Total_Amount_LCY = (int)(config["Total_Amount_LCY"] ?? 0),
                            Currency_Code = (string)config["Currency_Code"],
                            Created_By = (string)config["Created_By"],
                            Status = (string)config["Status"],
                            Posted = (bool)(config["Posted"] ?? false),
                            Posted_By = (string)config["Posted_By"],
                            Posted_Date = (string)config["Posted_Date"],
                            Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                            Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"]
                        };
                        pettyCashVouchersList.Add(pettyCashVoucher);
                    }
                }
                return PartialView("~/Views/PettyCash/PartialViews/PettyCashVouchersListPartialView.cshtml", pettyCashVouchersList.OrderByDescending(x => x.No));
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        [HttpPost]
        public ActionResult PettyCashVouchersDocumentView(string DocNo)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                PettyCashVouchers pettyCash = new PettyCashVouchers();
                string page = "PettyCashVouchers?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;

                        pettyCash = new PettyCashVouchers
                        {
                            No = (string)config["No"],
                            Date = (string)config["Date"],
                            Pay_Mode = (string)config["Pay_Mode"],
                            Cheque_No = (string)config["Cheque_No"],
                            Cheque_Date = (string)config["Cheque_Date"],
                            Paying_Bank_Account = (string)config["Paying_Bank_Account"],
                            Payee = (string)config["Payee"],
                            Total_Amount_LCY = (int)(config["Total_Amount_LCY"] ?? 0),
                            Currency_Code = (string)config["Currency_Code"],
                            Created_By = (string)config["Created_By"],
                            Status = (string)config["Status"],
                            Posted = (bool)(config["Posted"] ?? false),
                            Posted_By = (string)config["Posted_By"],
                            Posted_Date = (string)config["Posted_Date"],
                            Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                            Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"]

                        };
                    }

                }
                return View(pettyCash);
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public PartialViewResult PettyCashVouchersLines(string DocNo, string Status)
        {
            try
            {
                List<PettyCashVoucherLines> pettyLines = new List<PettyCashVoucherLines>();
                string pageLine = "PCVLines?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
                using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;

                        PettyCashVoucherLines claim = new PettyCashVoucherLines
                        {
                            No = (string)config["No"],
                            Line_No = (int)config["Line_No"],
                            Bounced_Pv_No = (string)config["Bounced_Pv_No"],
                            Type = (string)config["Type"],
                            Account_Type = (string)config["Account_Type"],
                            Account_No = (string)config["Account_No"],
                            Account_Name = (string)config["Account_Name"],
                            Amount = (int)config["Amount"],
                            Net_Amount = (int)config["Net_Amount"],
                            Remaining_Amount = (int)config["Remaining_Amount"],
                            VAT_Rate = (int)config["VAT_Rate"],
                            VAT_Six_Percent_Rate = (int)config["VAT_Six_Percent_Rate"],
                            VAT_Withheld_Code = (string)config["VAT_Withheld_Code"],
                            VAT_Withheld_Amount = (int)config["VAT_Withheld_Amount"],
                            Budgetary_Control_A_C = (bool)config["Budgetary_Control_A_C"],
                            Advance_Recovery = (int)config["Advance_Recovery"],
                            Retention_Amount = (int)config["Retention_Amount"],
                            Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                            Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"],
                            Claim_Doc_No = (string)config["Claim_Doc_No"],
                            VAT_Code = (string)config["VAT_Code"],
                            W_Tax_Code = (string)config["W_Tax_Code"],
                            W_T_VAT_Code = (string)config["W_T_VAT_Code"],
                            VAT_Amount = (int)config["VAT_Amount"],
                            W_Tax_Amount = (int)config["W_Tax_Amount"],
                            Total_Net_Pay = (int)config["Total_Net_Pay"],
                            Status = (string)config["Status"]

                        };

                        pettyLines.Add(claim);
                    }
                }
                return PartialView("~/Views/PettyCash/PartialViews/PettyCashVouchersLines.cshtml", pettyLines);
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
    }
}