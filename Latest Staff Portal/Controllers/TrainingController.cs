using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Latest_Staff_Portal.Models;
using Latest_Staff_Portal.ViewModel;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Tls;

namespace Latest_Staff_Portal.Controllers
{
    public class TrainingController : Controller
    {
        // Training Requisition
        public ActionResult TrainingRequisitionList()
        {
            try
            {
                if (Session["Username"] == null)
                    return RedirectToAction("Login", "Login");
                return View();
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public PartialViewResult TrainingRequisitionListPartialView()
        {
            try
            {
                var UserID = Session["UserID"].ToString();
                var StaffNo = Session["Username"].ToString(); 
                var employeeView = Session["EmployeeData"] as EmployeeView;

                var TrainingReqList = new List<TrainingRequisition>();
               
                var role = Session["ESSRoleSetup"] as ESSRoleSetup;
                var page = $"TrainingRequisition?$filter=Created_By eq '{UserID}'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (JObject config in details["value"])
                    {
                        var reqList = new TrainingRequisition
                        {
                            Code = (string)config["Code"],
                            Training_Plan_No = (string)config["Training_Plan_No"],
                            Employee_Department = (string)config["Employee_Department"],
                            Course_Title = (string)config["Course_Title"],
                            Description = (string)config["Description"],
                            Start_DateTime = (string)config["Start_DateTime"],
                            End_DateTime = (string)config["End_DateTime"],
                            Duration = (int?)config["Duration"] ?? 0,
                            Duration_Type = (string)config["Duration_Type"],
                            Training_Region = (string)config["Training_Region"],
                            Cost = (int?)config["Cost"] ?? 0,
                            Training_Venue_Region_Code = (string)config["Training_Venue_Region_Code"],
                            Training_Venue_Region = (string)config["Training_Venue_Region"],
                            Training_Responsibility_Code = (string)config["Training_Responsibility_Code"],
                            Training_Responsibility = (string)config["Training_Responsibility"],
                            Location = (string)config["Location"],
                            Provider = (string)config["Provider"],
                            Provider_Name = (string)config["Provider_Name"],
                            Training_Type = (string)config["Training_Type"],
                            Training_Duration_Hrs = (int?)config["Training_Duration_Hrs"] ?? 0,
                            Planned_Budget = (int?)config["Planned_Budget"] ?? 0,
                            Planned_No_to_be_Trained = (int?)config["Planned_No_to_be_Trained"] ?? 0,
                            No_of_Participants = (int?)config["No_of_Participants"] ?? 0,
                            Total_Procurement_Cost = (int?)config["Total_Procurement_Cost"] ?? 0,
                            Other_Costs = (int?)config["Other_Costs"] ?? 0,
                            Employee_No = (string)config["Employee_No"],
                            Employee_Name = (string)config["Employee_Name"],
                            Created_By = (string)config["Created_By"],
                            Created_On = (string)config["Created_On"],
                            Status = (string)config["Status"],
                            Bonded = (bool?)config["Bonded"] ?? false,
                            Bonding_Period = (string)config["Bonding_Period"],
                            Expected_Bond_End = (string)config["Expected_Bond_End"],
                            Expected_Bond_Start = (string)config["Expected_Bond_Start"],
                            Bond_Penalty = (int?)config["Bond_Penalty"] ?? 0
                        };

                        TrainingReqList.Add(reqList);
                    }
                }

                return PartialView("~/Views/TRaining/PartialViews/TrainingRequisitionListPartialView.cshtml", TrainingReqList.OrderByDescending(x => x.Code));
            }
            catch (Exception ex)
            {
                var erroMsg = new Error
                {
                    Message = ex.Message.Replace("'", "")
                };
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }


        public ActionResult NewTrainingRequisition()
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                var trainingRequisition = new TrainingRequisition();

                #region TrainingPlanNo
                var TrainingPlanList = new List<DropdownList>();
                var pageTPN = $"ApprovedTrainingPlans?$format=json";
                var httpResponseTPN = Credentials.GetOdataData(pageTPN);
                using (var streamReader = new StreamReader(httpResponseTPN.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = (string)config1["Description"] + "-" + (string)config1["No"],
                            Value = (string)config1["No"]
                        };
                        TrainingPlanList.Add(dropdownList);
                    }
                }
                #endregion

                #region ResponsibilityCenters
                var ResponsibilityCentersList = new List<DropdownList>();
                var pageResponsibilityCenters = $"ResponsibilityCenters?$format=json";
                var httpResponseResponsibilityCenters = Credentials.GetOdataData(pageResponsibilityCenters);
                using (var streamReader = new StreamReader(httpResponseResponsibilityCenters.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = $"{(string)config1["Code"]}",
                            Value = (string)config1["Code"]
                        };
                        ResponsibilityCentersList.Add(dropdownList);
                    }
                }
                #endregion

                #region TrainingCourses
                var TrainingCoursesList = new List<DropdownList>();
                var pageTrainingCourses = $"TrainingPlanLines?$format=json";
                var httpTrainingCourses = Credentials.GetOdataData(pageTrainingCourses);
                using (var streamReader = new StreamReader(httpTrainingCourses.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = (string)config1["Course_Description"] + "-" + (string)config1["Course_Title"],
                            Value = (string)config1["Course_Title"]
                        };
                        TrainingCoursesList.Add(dropdownList);
                    }
                }
                #endregion

                #region Locations
                var LocationsList = new List<DropdownList>();
                var pageLocations = $"Locations?$format=json";
                var httpLocations = Credentials.GetOdataData(pageLocations);
                using (var streamReader = new StreamReader(httpLocations.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = (string)config1["Code"] + "" + (string)config1["Name"],
                            Value = (string)config1["Code"]
                        };
                        LocationsList.Add(dropdownList);
                    }
                }
                #endregion

                #region Region
                var RegionsList = new List<DropdownList>();
                var pageRegions = $"DimensionValueList?$format=json";
                var httpResponseRegions = Credentials.GetOdataData(pageRegions);
                using (var streamReader = new StreamReader(httpResponseRegions.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = $"{(string)config1["Name"]} - {(string)config1["Code"]}",
                            Value = (string)config1["Code"]
                        };
                        RegionsList.Add(dropdownList);
                    }
                }
                #endregion

                #region Provider
                var ProviderList = new List<DropdownList>();
                var pageProvider = $"Providers?$format=json";
                var httpResponseProvider = Credentials.GetOdataData(pageProvider);
                using (var streamReader = new StreamReader(httpResponseProvider.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = (string)config1["Name"] + " - " + (string)config1["No"],
                            Value = (string)config1["No"]
                        };
                        ProviderList.Add(dropdownList);
                    }
                }
                #endregion



                trainingRequisition.ListOfTrainingPlan = TrainingPlanList.Select(x =>
                  new SelectListItem
                  {
                      Text = x.Text,
                      Value = x.Value
                  }).ToList();

                trainingRequisition.ListOfResponsibilityCenters = ResponsibilityCentersList.Select(x => 
                 new SelectListItem
                 {
                     Text = x.Text,
                     Value = x.Value
                 }).ToList();

                trainingRequisition.ListOfTrainingCourses = TrainingCoursesList.Select(x =>
                new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

                trainingRequisition.ListOfLocations = LocationsList.Select(x =>
                new SelectListItem
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                trainingRequisition.ListOfRegions = RegionsList.Select(x =>
                   new SelectListItem
                   {
                       Text = x.Text,
                       Value = x.Value
                   }).ToList();

                trainingRequisition.ListOfProviders = ProviderList.Select(x =>
                  new SelectListItem
                  {
                      Text = x.Text,
                      Value = x.Value
                  }).ToList();



                return PartialView("~/Views/Training/PartialViews/NewTrainingRequisition.cshtml",
                    trainingRequisition);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public JsonResult SubmitTrainingRequisition(TrainingRequisition trainingRequisition)
        {
            try
            {
                var UserID = Session["UserID"].ToString();
                var StaffNo = Session["Username"].ToString();
                var employeeView = Session["EmployeeData"] as EmployeeView;

                bool res = false;
                string res2= Credentials.ObjNav.createTrainingRequisition(
                    trainingRequisition.Training_Plan_No,
                    trainingRequisition.Course_Title, 
                    trainingRequisition.Training_Venue_Region_Code, 
                    trainingRequisition.Training_Responsibility_Code,
                    trainingRequisition.Location,
                    trainingRequisition.Provider,
                    StaffNo,
                    trainingRequisition.Description
                    );

                if (res2!="")
                {
                    var redirect = res2;

                    return Json(new { message = redirect, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var redirect = "Error adding record. Try again";
                    return Json(new { message = redirect, success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TrainingRequisitionDocumentView(string DocNo)
        {
            try
            {
                if (Session["Username"] == null) return RedirectToAction("Login", "Login");
                var StaffNo = Session["Username"].ToString();
                var TrainingReqDoc = new TrainingRequisition();
                var page = "TrainingRequisition?$filter=Code eq '" + DocNo + "'&$format=json";
                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        TrainingReqDoc = new TrainingRequisition
                        {
                            Code = (string)config["Code"],
                            Training_Plan_No = (string)config["Training_Plan_No"],
                            Employee_Department = (string)config["Employee_Department"],
                            Course_Title = (string)config["Course_Title"],
                            Description = (string)config["Description"],
                            Start_DateTime = (string)config["Start_DateTime"],
                            End_DateTime = (string)config["End_DateTime"],
                            Duration = (int?)config["Duration"] ?? 0,
                            Duration_Type = (string)config["Duration_Type"],
                            Training_Region = (string)config["Training_Region"],
                            Cost = (int?)config["Cost"] ?? 0,
                            Training_Venue_Region_Code = (string)config["Training_Venue_Region_Code"],
                            Training_Venue_Region = (string)config["Training_Venue_Region"],
                            Training_Responsibility_Code = (string)config["Training_Responsibility_Code"],
                            Training_Responsibility = (string)config["Training_Responsibility"],
                            Location = (string)config["Location"],
                            Provider = (string)config["Provider"],
                            Provider_Name = (string)config["Provider_Name"],
                            Training_Type = (string)config["Training_Type"],
                            Training_Duration_Hrs = (int?)config["Training_Duration_Hrs"] ?? 0,
                            Planned_Budget = (int?)config["Planned_Budget"] ?? 0,
                            Planned_No_to_be_Trained = (int?)config["Planned_No_to_be_Trained"] ?? 0,
                            No_of_Participants = (int?)config["No_of_Participants"] ?? 0,
                            Total_Procurement_Cost = (int?)config["Total_Procurement_Cost"] ?? 0,
                            Other_Costs = (int?)config["Other_Costs"] ?? 0,
                            Employee_No = (string)config["Employee_No"],
                            Employee_Name = (string)config["Employee_Name"],
                            Created_By = (string)config["Created_By"],
                            Created_On = (string)config["Created_On"],
                            Status = (string)config["Status"],
                            Bonded = (bool?)config["Bonded"] ?? false,
                            Bonding_Period = (string)config["Bonding_Period"],
                            Expected_Bond_End = (string)config["Expected_Bond_End"],
                            Expected_Bond_Start = (string)config["Expected_Bond_Start"],
                            Bond_Penalty = (int?)config["Bond_Penalty"] ?? 0
                        };
                    }
                }

                return View(TrainingReqDoc);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error
                {
                    Message = ex.Message.Replace("'", "")
                };
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public PartialViewResult TrainingRequisitionLinesPartialView(string DocNo, string status, string Training_Venue_Region_Code)
        {
            try
            {
                var UserID = Session["UserID"].ToString();
                var StaffNo = Session["Username"].ToString();
                var TrainingParticipantsLines = new List<TrainingParticipantsList>();
                var employeeView = Session["EmployeeData"] as EmployeeView;
                var role = Session["ESSRoleSetup"] as ESSRoleSetup;
                var page = $"TrainingParticipantsList?$filter=Training_Code eq '{DocNo}'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (JObject config in details["value"])
                    {
                        var TrainingParticipantsLine = new TrainingParticipantsList
                        {
                            Line_No = (int)(config["Line_No"] ?? 0),
                            Training_Code = (string)(config["Training_Code"] ?? ""),
                            Employee_Code = (string)(config["Employee_Code"] ?? ""),
                            Training_Responsibility_Code = (string)(config["Training_Responsibility_Code"] ?? ""),
                            Employee_Name = (string)(config["Employee_Name"] ?? ""),
                            Type = (string)(config["Type"] ?? ""),
                            Destination = (string)(config["Destination"] ?? ""),
                            No_of_Days = (int)(config["No_of_Days"] ?? 0),
                            Total_Amount = (int)(config["Total_Amount"] ?? 0),
                            Training_Responsibility = (string)(config["Training_Responsibility"] ?? ""),
                            Global_Dimension_1_Code = (string)(config["Global_Dimension_1_Code"] ?? ""),
                            Witness = (string)(config["Witness"] ?? ""),
                            Witness_Name = (string)(config["Witness_Name"] ?? ""),
                            Charge_Levy = (bool)(config["Charge_Levy"] ?? false),
                            Requestor = (string)(config["Requestor"] ?? "")
                        };

                        TrainingParticipantsLines.Add(TrainingParticipantsLine);
                    }
                }
                ViewBag.DocNo = DocNo;
                ViewBag.TrainingVenue = Training_Venue_Region_Code;
                ViewBag.status = status;
                return PartialView("~/Views/Training/PartialViews/TrainingRequisitionLinesPartialView.cshtml", TrainingParticipantsLines.OrderByDescending(x => x.Line_No));
            }
            catch (Exception ex)
            {
                var erroMsg = new Error
                {
                    Message = ex.Message.Replace("'", "")
                };
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public ActionResult NewTrainingRequisitionLine(string docNo, string TrainingVenue)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                var trainingReqLine = new TrainingParticipantsList();
                trainingReqLine.Destination = TrainingVenue;


                #region Employees
                var EmpleyeeList = new List<DropdownList>();
                var pageWp = $"EmployeeList?$format=json";
                var httpResponseWp = Credentials.GetOdataData(pageWp);
                using (var streamReader = new StreamReader(httpResponseWp.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = $"{(string)config1["FullName"]} - {(string)config1["No"]}",
                            Value = (string)config1["No"]
                        };
                        EmpleyeeList.Add(dropdownList);
                    }
                }
                #endregion

                #region TypeOfExpense
                var ExpenseTypeList = new List<DropdownList>();
                var pageExpenseType = $"ReceiptsandPaymentTypes?$format=json";
                var httpResponseExpenseType = Credentials.GetOdataData(pageExpenseType);
                using (var streamReader = new StreamReader(httpResponseExpenseType.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = $"{(string)config1["Code"]} - {(string)config1["Description"]}",
                            Value = (string)config1["Code"]
                        };
                        ExpenseTypeList.Add(dropdownList);
                    }
                }
                #endregion

                #region Region
                var RegionsList = new List<DropdownList>();
                var pageRegions = $"DimensionValueList?$format=json";
                var httpResponseRegions = Credentials.GetOdataData(pageRegions);
                using (var streamReader = new StreamReader(httpResponseRegions.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (var jToken in details["value"])
                    {
                        var config1 = (JObject)jToken;
                        var dropdownList = new DropdownList
                        {
                            Text = $"{(string)config1["Name"]} - {(string)config1["Code"]}",
                            Value = (string)config1["Code"]
                        };
                        RegionsList.Add(dropdownList);
                    }
                }
                #endregion

                trainingReqLine.ListOfEmployees = EmpleyeeList.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                trainingReqLine.ListOfExpenseTypes = ExpenseTypeList.Select(x =>
                   new SelectListItem
                   {
                       Text = x.Text,
                       Value = x.Value
                   }).ToList();

                trainingReqLine.ListOfRegions = RegionsList.Select(x =>
                  new SelectListItem
                  {
                      Text = x.Text,
                      Value = x.Value
                  }).ToList();

                return PartialView("~/Views/Training/PartialViews/NewTrainingRequisitionLine.cshtml",
                    trainingReqLine);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public JsonResult SubmitTrainingRequisitionLine(TrainingParticipantsList trainingParticipant)
        {
            try
            {
                var docNo = trainingParticipant.Training_Code;
                bool res = false;
                string res1 = Credentials.ObjNav.addTrainingParticipants(
                    trainingParticipant.Type,
                    trainingParticipant.Employee_Code,
                    trainingParticipant.Destination, //not needed
                    trainingParticipant.No_of_Days,  //not needed
                    docNo
                 );


                if (res)
                {
                    var redirect = docNo;

                    return Json(new { message = redirect, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var redirect = "Error adding record. Try again";
                    return Json(new { message = redirect, success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SendTrainingReqDocForApproval(string DocNo)
        {
            try
            {

                var employee = Session["EmployeeData"] as EmployeeView;
                var staffNo = employee?.No;
                var userId = employee?.UserID;
                bool res = false;
              /*  Credentials.ObjNav.FullUtilVoucherApproval(docNo);
                Credentials.ObjNav.UpdateApprovalEntrySenderID(57000, docNo, employee?.UserID);*/

                if (res)
                {
                    var redirect = res;

                    return Json(new { message = redirect, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var redirect = "Error adding record. Try again";
                    return Json(new { message = redirect, success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CancelTrainingReqApproval(string DocNo)
        {
            try
            {
                var employee = Session["EmployeeData"] as EmployeeView;
                var staffNo = employee.No;
               /* Credentials.ObjNav.cancelImprestApprovalRequest(staffNo, DocNo);*/
                return Json(new { message = "Training requisition approval cancelled Successfully", success = true },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CoursesJSon(string Training_Plan_No)
        {
            try
            {
                List<object> courses = new List<object>();

                string page = "TrainingPlanLines?$filter=Training_Plan_Code eq '" + Training_Plan_No + "'&format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);

                    foreach (JObject config in details["value"])
                    {
                        courses.Add(new
                        {
                            Course_Title = (string)config["Course_Title"],
                            Course_Description = (string)config["Course_Description"]
                        });
                    }
                }

                return Json(courses, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}