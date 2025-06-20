﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using Latest_Staff_Portal.CustomSecurity;
using Latest_Staff_Portal.Models;
using Latest_Staff_Portal.ViewModel;
using Newtonsoft.Json.Linq;

namespace Latest_Staff_Portal.Controllers
{
    [CustomeAuthentication]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");

            try
            {
                var EmpView = Session["EmployeeData"] as EmployeeView;
                /*string StaffNo = EmpView.No;*/
                var leaveType = "ANNUAL";
                var leaveBalances = GetLeaveBalance(leaveType);
                int assignedAssetsCount = GetAssignedAssetsCount();
                var imprestCount = GetImprestCount();
                var approvalsCount = GetDocumentApprovalsCount();
                ViewBag.leaveBalances = leaveBalances;
                ViewBag.assignedAssetsCount = assignedAssetsCount;
                ViewBag.imprestCount = imprestCount;
                ViewBag.approvalsCount = approvalsCount;
                return View();
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public ActionResult PersonalProfile()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                var StaffNo = Session["Username"].ToString();
                var EmpView = Session["EmployeeData"] as EmployeeView;


                return View(EmpView);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public PartialViewResult ProfilePicture(string gender)
        {
            var EmpView = new EmployeeView();
            EmpView.Gender = gender;
            return PartialView("~/Views/Dashboard/ProfilePic.cshtml", EmpView);
        }
        /*public PartialViewResult EmployeeQualification()
        {
            try
            {
                var StaffNo = Session["Username"].ToString();
                var employeeQualifications = new List<EmployeeQualification>();
                var employeeQualificationPage = "EmployeeQualifications?$filter=Employee_No eq '" + StaffNo + "'&$format=json";
                *//* var employeeQualificationPage = "EmployeeQualifications?$format=json";*//*
                var httpResponseQual = Credentials.GetOdataData(employeeQualificationPage);
                using (var streamReader = new StreamReader(httpResponseQual.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (JObject config in details["value"])
                    {
                        EmployeeQualification qualification = new EmployeeQualification
                        {
                            Employee_No = (string)config["Employee_No"],
                            Line_No = (int)config["Line_No"],
                            Qualification_Code = (string)config["Qualification_Code"],
                            From_Date = (string)config["From_Date"],
                            To_Date = (string)config["To_Date"],
                            Type = (string)config["Type"],
                            Description = (string)config["Line_No"],
                            Institution_Company = (string)config["Institution_Company"],




                        };
                        employeeQualifications.Add(qualification);
                    }
                }
                return PartialView("~/Views/Dashboard/PartialView/EmployeeQualification.cshtml", employeeQualifications);
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error
                {
                    Message = ex.Message
                };
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }*/
        /*public ActionResult AddQualification()
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    EmployeeQualification qualification = new EmployeeQualification();
                    Session["httpResponse"] = null;
                    EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

                    #region Qualifications
                    List<DropdownList> Qualification = new List<DropdownList>();
                    string pageCategory = "Qualifications?$format=json";

                    HttpWebResponse httpResponseCategory = Credentials.GetOdataData(pageCategory);
                    using (var streamReader = new StreamReader(httpResponseCategory.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var details = JObject.Parse(result);
                        foreach (JObject config in details["value"])
                        {
                            DropdownList dropdownList = new DropdownList();
                            dropdownList.Text = (string)config["Description"] + " (" + (string)config["Code"] + ")";
                            dropdownList.Value = (string)config["Code"];
                            Qualification.Add(dropdownList);
                        }
                    }
                    #endregion

                    qualification.ListOfQualifications = Qualification.Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Text,
                               Value = x.Value
                           }).ToList();

                    return PartialView("~/Views/Dashboard/PartialView/AddQualification.cshtml", qualification);
                }
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }*/
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SubmitQualification(EmployeeQualification qualification)
        {
            try
            {

                EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
                string Responsible_Employee_No = employeeView.No;
                string UserID = employeeView.UserID;
                bool status = false;

                /* Credentials.ObjNav.fnUpdateHelpDeskIssue(
                     qualification.Job_No, 
                     issue.HelpDesk_Category,
                     issue.Helpdesk_subcategory, 
                     issue.ICT_Inventory,
                     issue.Description_of_the_issue
                 );*/

                if (!status)
                {
                    string Redirect = "Success";
                    return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "Error submitting record. Try again.", success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public string GetLeaveBalance(string LvType)
        {
            var StaffNo = Session["Username"].ToString();
            var employee = Session["EmployeeData"] as EmployeeView;
            var newBal = new LeaveBalance();

            var Lvbal = new List<DropDownBalance>();
            decimal availableDays = 0;
            var harshpdays = "";
            var s = new decimal[5];
            if (LvType.Contains("ANNUAL"))
            {
                s = CommonClass.GetLeaveBal(StaffNo, LvType);
                if (s[0] > 1)
                    availableDays = s[0];
            }
            else if (availableDays == 0)
            {
                var page = "LeaveTypes?$select=Days&$filter=Code eq '" + LvType + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                        availableDays = availableDays + (int)config["Days"];
                }
            }


            return availableDays.ToString();
        }
        public int GetAssignedAssetsCount()
        {
            var StaffNo = Session["Username"].ToString();

            var count = 0;
            var page = $"FixedAssetCard?$filter=Responsible_Employee eq '{StaffNo}'&$format=json";

            var httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    count += 1;
                }
            }
            return count;
        }
        public int GetImprestCount()
        {

            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            var StaffNo = Session["Username"].ToString();
            var UserID = Session["UserID"].ToString();

            int imprestCount = 0;

            var page = $"SafariImprest?$filter=Requestor eq '{StaffNo}'&$format=json";

            var httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    imprestCount += 1;
                }
            }
            return imprestCount;

        }
        public int GetDocumentApprovalsCount()
        {
            var userID = Session["UserID"].ToString();
            int approvalsCount = 0;

            var page = "ApprovalEntries?$filter=Approver_ID eq '" + userID + "' and Status eq 'Open'&$format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();

            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                approvalsCount += 1;

            }
            return approvalsCount;
        }

    }
}