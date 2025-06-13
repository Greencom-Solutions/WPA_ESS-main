using Latest_Staff_Portal.CustomSecurity;
using Latest_Staff_Portal.Models;
using Latest_Staff_Portal.Utils;
using Latest_Staff_Portal.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Latest_Staff_Portal.Controllers
{
    [CustomeAuthentication]
    [CustomAuthorization(Role = "ALLUSERS")]
    public class StaffClaimController : Controller
    {
        public ActionResult StaffClaims(string status)
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
        public PartialViewResult StaffClaimsList(string status)
        {
            try
            {
                string staffNo = Session["Username"].ToString();
                List<StaffClaims> staffClaims = new List<StaffClaims>();
                var employee = Session["EmployeeData"] as EmployeeView;
                var station = employee?.GlobalDimension1Code;
                var page = "";
                if (string.IsNullOrEmpty(status))
                {
                    page = "StaffClaim?$filter=Account_No eq '" + staffNo + "' and Document_Type eq 'Staff Claims'&format=json";
                }
                else
                {
                    page = "StaffClaim?$filter=Shortcut_Dimension_1_Code eq '" + station + "' and Document_Type eq 'Staff Claims' and Status eq '"+status+"'&$format=json";

                }
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;
    
                        var claim = new StaffClaims
                        {
                            No = (string)config["No"],
                            Date = (string)config["Date"],
                            AccountType = (string)config["Account_Type"],
                            AccountNo = (string)config["Account_No"],
                            AccountName = (string)config["Account_Name"],
                            PayingBankAccount = (string)config["Paying_Bank_Account"],
                            BankName = (string)config["Bank_Name"],
                            Payee = (string)config["Payee"],
                            PaymentNarration = (string)config["Payment_Narration"],
                            ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"],
                            DepartmentName = (string)config["Department_Name"],
                            ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"],
                            ProjectName = (string)config["Project_Name"],
                            TotalAmountLCY = (decimal)config["Total_Amount_LCY"],
                            StrategicPlan = (string)config["Strategic_Plan"],
                            ReportingYearCode = (string)config["Reporting_Year_Code"],
                            WorkplanCode = (string)config["Workplan_Code"],
                            ActivityCode = (string)config["Activity_Code"],
                            ExpenditureRequisitionCode = (string)config["Expenditure_Requisition_Code"],
                            Status = (string)config["Status"],
                            DimensionSetId = (string)config["Dimension_Set_ID"],
                            Posted = (string)config["Posted"],
                        };
                        staffClaims.Add(claim);
                    }

                }
                return PartialView("~/Views/StaffClaim/PartialViews/StaffClaimsList.cshtml", staffClaims.OrderByDescending(x => x.No));
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        [HttpPost]
        public ActionResult StaffClaimDocumentView(string DocNo)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                StaffClaims claim = new StaffClaims();
                string page = "StaffClaim?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;
    
                        claim = new StaffClaims
                        {
                            No = (string)config["No"],
                            Date = (string)config["Date"],
                            AccountType = (string)config["Account_Type"],
                            AccountNo = (string)config["Account_No"],
                            AccountName = (string)config["Account_Name"],
                            PayingBankAccount = (string)config["Paying_Bank_Account"],
                            BankName = (string)config["Bank_Name"],
                            Payee = (string)config["Payee"],
                            PaymentNarration = (string)config["Payment_Narration"],
                            ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"],
                            DepartmentName = (string)config["Department_Name"],
                            ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"],
                            ProjectName = (string)config["Project_Name"],
                            TotalAmountLCY = (decimal)config["Total_Amount_LCY"],
                            StrategicPlan = (string)config["Strategic_Plan"],
                            ReportingYearCode = (string)config["Reporting_Year_Code"],
                            WorkplanCode = (string)config["Workplan_Code"],
                            ActivityCode = (string)config["Activity_Code"],
                            ExpenditureRequisitionCode = (string)config["Expenditure_Requisition_Code"],
                            Status = (string)config["Status"],
                            DimensionSetId = (string)config["Dimension_Set_ID"],
                            Posted = (string)config["Posted"],
                            AvailableAmount = ((decimal)config["Available_Amount"]).ToString("C", new CultureInfo("sw-KE")),
                            CommittedAmount = ((decimal)config["Committed_Amount"]).ToString("C", new CultureInfo("sw-KE")),
                            AieReceipt = (string)config["AIE_Receipt"],
                        };
                    }

                }
                return View(claim);
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public PartialViewResult StaffClaimLines(string DocNo, string Status)
        {
            try
            {
                List<StaffClaimLines> claimLines = new List<StaffClaimLines>();
                string pageLine = "StaffClaimLines?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
                using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;

                        StaffClaimLines claim = new StaffClaimLines
                        {
                            No = (string)config["No"],
                            LineNo = (int)config["Line_No"],
                            Type = (string)config["Type"],
                            AccountType = (string)config["Account_Type"],
                            AccountNo = (string)config["Account_No"],
                            AccountName = (string)config["Account_Name"],
                            Amount = (int)config["Amount"],
                            AmountLCY = (int)config["Amount_LCY"],
                            ValidatedBankName = (string)config["rbankName"],
                        };

                        claimLines.Add(claim);
                    }
                }
                return PartialView("~/Views/StaffClaim/PartialViews/StaffClaimLines.cshtml", claimLines);
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public JsonResult DeleteStaffClaimExpense(string DocNo, int lineNo)
        {
            try
            {
                EmployeeView employee = Session["EmployeeData"] as EmployeeView;
                string empNo = employee?.No;
                var userId = employee?.UserID;
                Credentials.ObjNav.deleteStaffClaimLine(empNo, DocNo, lineNo);
                LogHelper.Log(DocNo, userId, "Delete staff claim lines");

                return Json(new { message = "Staff claim Line Deleted Successfully", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenerateFo22Report(string docNo)
        {
            try
            {
                var staffNo = Session["Username"].ToString();
                var _filename = staffNo.Replace("/", "");
                var message = "";
                bool success = false, view = false;

                var employeeView = Session["EmployeeData"] as EmployeeView;
                var dimension = employeeView?.GlobalDimension1Code;

                message = Credentials.ObjNav.GenerateClaimsReport(docNo);
                if (string.IsNullOrEmpty(message))
                {
                    success = false;
                    message = "File Not Found";
                }
                else
                {
                    success = true;
                }

                return Json(new { message, success, view }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SendStaffClaimsDocForApproval(string DocNo)
        {
            try
            {
                var staffNo = Session["Username"].ToString();
                var _filename = staffNo.Replace("/", "");
                var message = "";
                bool success = false, view = false;

                var employeeView = Session["EmployeeData"] as EmployeeView;

                message = Credentials.ObjNav.sendStaffClaimApproval(staffNo, DocNo);

                if (string.IsNullOrEmpty(message))
                {
                    success = false;
                    message = "File Not Found";
                }
                else
                {
                    success = true;
                }

                return Json(new { message, success, view }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CancelStaffClaimsDocApproval(string DocNo)
        {
            try
            {
                var staffNo = Session["Username"].ToString();
                var _filename = staffNo.Replace("/", "");
                var message = "";
                bool success = false, view = false;

                var employeeView = Session["EmployeeData"] as EmployeeView;

                message = Credentials.ObjNav.CancelStaffClaimRecordApproval(staffNo, DocNo, "");

                if (string.IsNullOrEmpty(message))
                {
                    success = false;
                    message = "File Not Found";
                }
                else
                {
                    success = true;
                }

                return Json(new { message, success, view }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}