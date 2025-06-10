using iTextSharp.text.pdf.parser;
using Latest_Staff_Portal.CustomSecurity;
using Latest_Staff_Portal.Models;
using Latest_Staff_Portal.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static iTextSharp.text.pdf.PdfDocument;
using static System.Net.WebRequestMethods;

namespace Latest_Staff_Portal.Controllers;

[CustomeAuthentication]

public class PurchaseController : Controller
{
    // GET: Purchase
    public ActionResult PurchaseRequisitionList()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    /* add invitation to tenders*/
    public ActionResult InvitationToTender()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public ActionResult ApprovedTenderInvitation()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public ActionResult PublishedInvitationsForSupply()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult PublishedInvitationListPartialView()
    {
        try
        {
            string userId = Session["UserId"]?.ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            if (employeeView == null || string.IsNullOrEmpty(userId))
            {
                return PartialView("~/Views/Shared/Error.cshtml",
                    new Error { Message = "Session data is missing." });
            }

            List<InvitationToTender> invitationList = new List<InvitationToTender>();
            string empNo = employeeView.No;
            string page = "InvitationToTender?$filter=Document_Status eq 'Published'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    var invitation = new InvitationToTender
                    {
                        Code = (string)config["Code"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Solicitation_Type = (string)config["Solicitation_Type"],
                        External_Document_No = (string)config["External_Document_No"],
                        Procurement_Type = (string)config["Procurement_Type"],
                        Procurement_Category_ID = (string)config["Procurement_Category_ID"],
                        Project_ID = (string)config["Project_ID"],
                        Assigned_Procurement_Officer = (string)config["Assigned_Procurement_Officer"],
                        Road_Code = (string)config["Road_Code"],
                        Description = (string)config["Description"],
                        Currency_Code = (string)config["Currency_Code"]
                    };
                    invitationList.Add(invitation);
                }
            }

            return PartialView("~/Views/Purchase/InvitationToTenderList.cshtml",
                invitationList.OrderByDescending(x => x.Code));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return PartialView("~/Views/Shared/Error.cshtml",
                new Error { Message = "An error occurred while processing your request." });
        }
    }

    public PartialViewResult ApprovedInvitationListPartialView()
    {
        try
        {
            List<ApprovedTenderInvitation> approvedInvitationList = new List<ApprovedTenderInvitation>();
            string page = "ApprovedOpenTenderInvitationList?$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    var approvedInvitation = new ApprovedTenderInvitation()
                    {
                        Code = (string)config["Code"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Solicitation_Type = (string)config["Solicitation_Type"],
                        External_Document_No = (string)config["External_Document_No"],
                        Procurement_Type = (string)config["Procurement_Type"],
                        Procurement_Category_ID = (string)config["Procurement_Category_ID"],
                        Project_ID = (string)config["Project_ID"],
                        Assigned_Procurement_Officer = (string)config["Assigned_Procurement_Officer"],
                        Road_Code = (string)config["Road_Code"],
                        Description = (string)config["Description"],
                        Currency_Code = (string)config["Currency_Code"]
                    };
                    approvedInvitationList.Add(approvedInvitation);
                }
            }

            return PartialView("~/Views/Purchase/ApprovedTenderInvitationList.cshtml", approvedInvitationList);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /* add invitation to tenders*/

    public PartialViewResult InvitationToTenderListPartialView()
    {
        try
        {
            string userId = Session["UserId"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            List<InvitationToTender> invitationList = new List<InvitationToTender>();
            string empNo = employeeView.No;
            string page = "InvitationToTender?$filter=Document_Status eq 'Draft'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    var invitation = new InvitationToTender
                    {
                        Code = (string)config["Code"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Solicitation_Type = (string)config["Solicitation_Type"],
                        External_Document_No = (string)config["External_Document_No"],
                        Procurement_Type = (string)config["Procurement_Type"],
                        Procurement_Category_ID = (string)config["Procurement_Category_ID"],
                        Project_ID = (string)config["Project_ID"],
                        Assigned_Procurement_Officer = (string)config["Assigned_Procurement_Officer"],
                        Road_Code = (string)config["Road_Code"],
                        Description = (string)config["Description"],
                        Currency_Code = (string)config["Currency_Code"]

                    };
                    invitationList.Add(invitation);
                }
            }

            return PartialView("~/Views/Purchase/InvitationToTenderList.cshtml",
                invitationList.OrderByDescending(x => x.Code));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public ActionResult InvitationDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                InvitationToTender invitation = new InvitationToTender();

                string page = "InvitationToTender?$filter=Code eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        invitation.odataetag = (string)config["odataetag"];
                        invitation.Code = (string)config["Code"];
                        invitation.Invitation_Notice_Type = (string)config["Invitation_Notice_Type"];
                        invitation.Document_Date = (string)config["Document_Date"];
                        invitation.Tender_Name = (string)config["Tender_Name"];
                        invitation.Tender_Summary = (string)config["Tender_Summary"];
                        invitation.External_Document_No = (string)config["External_Document_No"];
                        invitation.Stage_1_EOI_Invitation = (string)config["Stage_1_EOI_Invitation"];
                        invitation.PRN_No = (string)config["PRN_No"];
                        invitation.Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"];
                        invitation.Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"];
                        invitation.Location_Code = (string)config["Location_Code"];
                        invitation.Description = (string)config["Description"];
                        invitation.Procurement_Type = (string)config["Procurement_Type"];
                        invitation.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                        invitation.Procurement_Category_ID = (string)config["Procurement_Category_ID"];
                        invitation.Lot_No = (string)config["Lot_No"];
                        invitation.Target_Bidder_Group = (string)config["Target_Bidder_Group"];
                        invitation.Solicitation_Type = (string)config["Solicitation_Type"];
                        invitation.Bid_Selection_Method = (string)config["Bid_Selection_Method"];
                        invitation.Requisition_Template_ID = (string)config["Requisition_Template_ID"];
                        invitation.Bid_Scoring_Template = (string)config["Bid_Scoring_Template"];
                        invitation.Bid_Opening_Committe = (string)config["Bid_Opening_Committe"];
                        invitation.Bid_Evaluation_Committe = (string)config["Bid_Evaluation_Committe"];
                        invitation.Procurement_Method = (string)config["Procurement_Method"];
                        invitation.Enforce_Mandatory_Pre_bid_Visi = (bool)config["Enforce_Mandatory_Pre_bid_Visi"];
                        invitation.Mandatory_Pre_bid_Visit_Date = (string)config["Mandatory_Pre_bid_Visit_Date"];
                        invitation.Prebid_Meeting_Address = (string)config["Prebid_Meeting_Address"];
                        invitation.Prebid_Meeting_Register_ID = (string)config["Prebid_Meeting_Register_ID"];
                        invitation.Bid_Envelop_Type = (string)config["Bid_Envelop_Type"];
                        invitation.Sealed_Bids = (bool)config["Sealed_Bids"];
                        invitation.Tender_Validity_Duration = (string)config["Tender_Validity_Duration"];
                        invitation.Tender_Validity_Expiry_Date = (string)config["Tender_Validity_Expiry_Date"];
                        invitation.Purchaser_Code = (string)config["Purchaser_Code2"];
                        invitation.Language_Code = (string)config["Language_Code"];
                        invitation.Mandatory_Special_Group_Reserv = (bool)config["Mandatory_Special_Group_Reserv"];
                        invitation.Bid_Tender_Security_Required = (bool)config["Bid_Tender_Security_Required"];
                        invitation.Bid_Security_Percent = (int)config["Bid_Security_Percent"];
                        invitation.Bid_Security_Amount_LCY = (int)config["Bid_Security_Amount_LCY"];
                        invitation.Bid_Security_Validity_Duration =
                            (string)config["Bid_Security_Validity_Duration"];
                        invitation.Bid_Security_Expiry_Date = (string)config["Bid_Security_Expiry_Date"];
                        invitation.Insurance_Cover_Required = (bool)config["Insurance_Cover_Required"];
                        invitation.Performance_Security_Required = (bool)config["Performance_Security_Required"];
                        invitation.Performance_Security_Percent = (int)config["Performance_Security_Percent"];
                        invitation.Advance_Payment_Security_Req = (bool)config["Advance_Payment_Security_Req"];
                        invitation.Advance_Payment_Security_Percent =
                            (int)config["Advance_Payment_Security_Percent"];
                        invitation.Advance_Amount_Limit_Percent = (int)config["Advance_Amount_Limit_Percent"];
                        invitation.Appointer_of_Bid_Arbitrator = (string)config["Appointer_of_Bid_Arbitrator"];
                        invitation.Status = (string)config["Status"];
                        invitation.Published = (bool)config["Published"];
                        invitation.Bid_Submission_Method = (string)config["Bid_Submission_Method"];
                        invitation.Cancel_Reason_Code = (string)config["Cancel_Reason_Code"];
                        invitation.Cancellation_Date = (string)config["Cancellation_Date"];
                        invitation.Cancellation_Secret_Code = (string)config["Cancellation_Secret_Code"];
                        invitation.No_of_Submission = (int)config["No_of_Submission"];
                        invitation.Created_Date_Time = (string)config["Created_Date_Time"];
                        invitation.Created_by = (string)config["Created_by"];
                        invitation.Submission_Start_Date = (string)config["Submission_Start_Date"];
                        invitation.Submission_Start_Time = (string)config["Submission_Start_Time"];
                        invitation.Submission_End_Date = (string)config["Submission_End_Date"];
                        invitation.Submission_End_Time = (string)config["Submission_End_Time"];
                        invitation.Bid_Opening_Date = (string)config["Bid_Opening_Date"];
                        invitation.Bid_Opening_Time = (string)config["Bid_Opening_Time"];
                        invitation.Bid_Opening_Venue = (string)config["Bid_Opening_Venue"];
                        invitation.Procuring_Entity_Name_Contact = (string)config["Procuring_Entity_Name_Contact"];
                        invitation.Address = (string)config["Address"];
                        invitation.Address_2 = (string)config["Address_2"];
                        invitation.Post_Code = (string)config["Post_Code"];
                        invitation.City = (string)config["City"];
                        invitation.Country_Region_Code = (string)config["Country_Region_Code"];
                        invitation.Phone_No = (string)config["Phone_No"];
                        invitation.E_Mail = (string)config["E_Mail"];
                        invitation.Tender_Box_Location_Code = (string)config["Tender_Box_Location_Code"];
                        invitation.Primary_Tender_Submission = (string)config["Primary_Tender_Submission"];
                        invitation.Primary_Engineer_Contact = (string)config["Primary_Engineer_Contact"];
                        invitation.Requesting_Region = (string)config["Requesting_Region"];
                        invitation.Requesting_Directorate = (string)config["Requesting_Directorate"];
                        invitation.Requesting_Department = (string)config["Requesting_Department"];
                        invitation.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                        invitation.Procurement_Plan_Entry_No = (int)config["Procurement_Plan_Entry_No"];
                        invitation.Job = (string)config["Job"];
                        invitation.Job_Task_No = (string)config["Job_Task_No"];
                        invitation.PP_Planning_Category = (string)config["PP_Planning_Category"];
                        invitation.Document_Status = (string)config["Document_Status"];
                        invitation.PP_Funding_Source_ID = (string)config["PP_Funding_Source_ID"];
                        invitation.PP_Total_Budget = (double)config["PP_Total_Budget"];
                        invitation.PP_Total_Actual_Costs = (int)config["PP_Total_Actual_Costs"];
                        invitation.PP_Total_Commitments = (int)config["PP_Total_Commitments"];
                        invitation.PP_Total_Available_Budget = (double)config["PP_Total_Available_Budget"];
                        invitation.PP_Preference_Reservation_Code =
                            (string)config["PP_Preference_Reservation_Code"];
                        invitation.Financial_Year_Code = (string)config["Financial_Year_Code"];
                    }
                }

                return View(invitation);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    /*end of Tenders invitation */

    public PartialViewResult PurchaseRequisitionListPartialView()
    {
        try
        {
            string userId = Session["UserId"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            List<PurchaseRequisition> purchaseRequisitions = new List<PurchaseRequisition>();
            string empNo = employeeView.No;
            string page = "StandardPurchaseRequisition?$filter=Request_By_No eq '" + empNo + "'&$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PurchaseRequisition purchase = new PurchaseRequisition();
                    purchase.No = (string)config["No_"];
                    purchase.Description = (string)config["Request_Description"];
                    purchase.Document_Type = (string)config["Document_Type"];
                    purchase.No = (string)config["No"];
                    purchase.PRN_Type = (string)config["PRN_Type"];
                    purchase.Document_Date = ((DateTime)config["Document_Date"]).ToString("dd/MM/yyyy");
                    purchase.Location_Code = (string)config["Location_Code"];
                    purchase.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                    purchase.Description = (string)config["Description"];
                    purchase.Priority_Level = (string)config["Priority_Level"];
                    purchase.Requester_ID = (string)config["Requester_ID"];
                    purchase.Request_By_No = (string)config["Request_By_No"];
                    purchase.Request_By_Name = (string)config["Request_By_Name"];
                    purchase.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                    purchase.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                    purchase.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                    purchase.PP_Planning_Category = (string)config["PP_Planning_Category"];
                    purchase.Job = (string)config["Job"];
                    purchase.Job_Task_No = (string)config["Job_Task_No"];
                    purchase.PP_Total_Budget = (decimal)config["PP_Total_Budget"];
                    //purchase.PP_Total_Commitments = (decimal)config["PP_Total_Commitments"];
                    //purchase.PP_Total_Available_Budget = (decimal)config["PP_Total_Available_Budget"];
                    purchase.Requisition_Amount = (decimal)config["Requisition_Amount"];
                    purchase.Status = (string)config["Status"] == "Released" || (string)config["Status"] == "Posted"
                        ? "Approved"
                        : (string)config["Status"];



                    if ((string)config["Status"] == "Released" || (string)config["Status"] == "Posted")
                    {
                        purchase.Status = "Approved";
                    }
                    else
                    {
                        purchase.Status = (string)config["Status"];
                    }

                    purchaseRequisitions.Add(purchase);
                }
            }

            return PartialView("~/Views/Purchase/PRVReqListView.cshtml",
                purchaseRequisitions.OrderByDescending(x => x.No));
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public ActionResult NewPurchaseRequest()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                string Dim1 = "", Dim2 = "";
                PurchaseRequisition newPurchaseRequisition = new PurchaseRequisition();
                Session["httpResponse"] = null;

                #region Employee Data

                string pageData = "EmployeeList?$filter=No eq '" + StaffNo + "'&$format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(pageData);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);

                    if (details["value"].Count() > 0)
                    {
                        foreach (JObject config in details["value"])
                        {
                            Dim1 = (string)config["Global_Dimension_1_Code"];
                            Dim2 = (string)config["Department_Name"];
                        }
                    }
                }

                #endregion

                EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
                Dim1 = employeeView.GlobalDimension1Code;
                Dim2 = employeeView.Department_Code;
                //if (Dim1 == "")
                //{
                //    Error erroMsg = new Error();
                //    erroMsg.Message = "Your Station is not set. Contact HR";
                //    return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
                //}
                //else if (Dim2 != "")
                //{
                //    Error erroMsg = new Error();
                //    erroMsg.Message = "Your Department is not set. Contact HR";
                //    return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
                //}
                //else
                //{

                #region Dim1 List

                List<DropdownList> dim1 = new List<DropdownList>();
                string pageDim1 =
                    "DimensionValues?$filter=Global_Dimension_No eq 1 and Blocked eq false&$format=json";


                HttpWebResponse httpResponseDepartment = Credentials.GetOdataData(pageDim1);
                using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropDown = new DropdownList();
                        dropDown.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropDown.Value = (string)config["Code"];
                        dim1.Add(dropDown);
                    }
                }

                #endregion

                #region dim2

                List<DropdownList> dim2 = new List<DropdownList>();
                string pageDim2 =
                    "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$format=json";

                HttpWebResponse httpResponseDivision = Credentials.GetOdataData(pageDim2);
                using (var streamReader = new StreamReader(httpResponseDivision.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        dim2.Add(dropdownList);
                    }
                }

                #endregion

                #region procurment plan

                List<DropdownList> procurmentPlan = new List<DropdownList>();
                string procPlan = "ProcurementPlan?$format=json";

                HttpWebResponse httpResponseproc = Credentials.GetOdataData(procPlan);
                using (var streamReader = new StreamReader(httpResponseproc.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        dropdownList.Value = (string)config["Code"];
                        dim2.Add(dropdownList);
                    }
                }

                #endregion

                #region procurment plan

                List<DropdownList> procurementCategories = new List<DropdownList>();
                string categories = "ProcurementCategories?$format=json";

                HttpWebResponse httpResponseCategories = Credentials.GetOdataData(categories);
                using (var streamReader = new StreamReader(httpResponseCategories.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        dropdownList.Value = (string)config["Code"];
                        procurementCategories.Add(dropdownList);
                    }
                }

                #endregion

                #region Locations

                List<DropdownList> locations = new List<DropdownList>();
                string loc = "Locations?$format=json";

                HttpWebResponse httpResponseLoc = Credentials.GetOdataData(loc);
                using (var streamReader = new StreamReader(httpResponseLoc.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        DropdownList d = new DropdownList();
                        d.Text = (string)config["Name"];
                        d.Value = (string)config["Code"];
                        locations.Add(d);
                    }
                }

                #endregion



                newPurchaseRequisition.ListOfDim1 = dim1.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                newPurchaseRequisition.ListOfPlanningCategories = procurementCategories.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                newPurchaseRequisition.ListOfDim2 = dim2.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                newPurchaseRequisition.ListOfLocations = locations.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();


                return View("~/Views/Purchase/NewPurchaseRequest.cshtml", newPurchaseRequisition);

            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public PartialViewResult NewPurchaseLine(string planId)
    {
        try
        {
            // LocationList locationList = new LocationList();
            PurchaseRequisitionLine purchaseLine = new PurchaseRequisitionLine();

            #region ProcurementPlanEntry

            List<DropdownList> procurementPlans = new List<DropdownList>();
            string page = $"ProcurementPlanEntry?$filter=ProcurementPlanID eq '{planId}'&$format=json";


            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Value = (string)config["EntryNo"];
                    dropDown.Text = (string)config["PlanningCategory"] + "-" + (string)config["Description"];
                    procurementPlans.Add(dropDown);
                }
            }

            #endregion

            //locationList = new LocationList


            purchaseLine.ListOfProcurementPlanEntries = procurementPlans.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            return PartialView("~/Views/Purchase/PartialViews/NewPurchaseLine.cshtml", purchaseLine);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public JsonResult SubmitPurchaseRequisition(PurchaseRequisition requisition)
    {
        try
        {
            string StaffNo = Session["Username"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            // string userId = employeeView.UserID;

            //string DocNo = Credentials.ObjNav.createstandardpurchasereq(requisition.PrnType,  requisition.Location_Code, requisition.Product_Group_,
            //    requisition.Shortcut_Dimension_1_Code, requisition.Shortcut_Dimension_2_Code, requisition.PP_Planning_Category, userId, StaffNo);
            string DocNo = Credentials.ObjNav.createPurchaseReq(StaffNo, "", requisition.Location_Code, "", 0,
                requisition.PrnType, "", requisition.PP_Planning_Category,
                0, "", "", "");

            if (DocNo != "")
            {
                string Redirect = "/Purchase/PurchaseDocumentView?DocNo=" + DocNo;

                return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Document not created. Please try again later...", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult PurchaseDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                PurchaseRequisition purchase = new PurchaseRequisition();

                string page = "PurchaseRequisition?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {

                        purchase.No = (string)config["No"];
                        purchase.PRN_Type = (string)config["PRN_Type"];
                        purchase.Document_Date = ((DateTime)config["Document_Date"]).ToString("dd/MM/yyyy");
                        purchase.Location_Code = (string)config["Location_Code"];
                        purchase.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                        purchase.Description = (string)config["Description"];
                        purchase.Priority_Level = (string)config["Priority_Level"];
                        purchase.Requester_ID = (string)config["Requester_ID"];
                        purchase.Request_By_No = (string)config["Request_By_No"];
                        purchase.Request_By_Name = (string)config["Request_By_Name"];
                        purchase.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                        purchase.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                        purchase.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                        purchase.PP_Planning_Category = (string)config["PP_Planning_Category"];
                        purchase.Job = (string)config["Job"];
                        purchase.Job_Task_No = (string)config["Job_Task_No"];
                        purchase.PP_Total_Budget = (decimal)config["PP_Total_Budget"];
                        //purchase.PP_Total_Commitments = (decimal)config["PP_Total_Commitments"];
                        //purchase.PP_Total_Available_Budget = (decimal)config["PP_Total_Available_Budget"];
                        purchase.Requisition_Amount = (decimal)config["Requisition_Amount"];
                        purchase.Status = (string)config["Status"];
                        if ((string)config["Status"] == "Released" || (string)config["Status"] == "Posted")
                        {
                            purchase.Status = "Approved";
                        }
                        else
                        {
                            purchase.Status = (string)config["Status"];
                        }
                    }
                }

                purchase.LocationName =
                    DimensinValuesList.GetDimensionValueName(purchase.Shortcut_Dimension_1_Code);
                purchase.AdminUnitName =
                    DimensinValuesList.GetDimensionValueName(purchase.Shortcut_Dimension_2_Code);


                return View(purchase);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult PurchaseDocumentLines(string DocNo)
    {
        try
        {
            #region Purchase Lines

            //var reqtype = GetRequisitionType(DocNo);
            List<PurchaseRequisitionLine> purchaseRequisitionLines = new List<PurchaseRequisitionLine>();
            string pageLine = "PurchaseRequisitionLines?$filter=Document_No eq '" + DocNo + "'&$format=json";
            /* string pageLine = "ApprovedPRNLines?$filter=Document_No eq '" + DocNo + "'&$format=json";*/
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PurchaseRequisitionLine requisitionLine = new PurchaseRequisitionLine();
                    requisitionLine.Document_Type = (string)config["Document_Type"];
                    requisitionLine.Document_No = (string)config["Document_No"];
                    requisitionLine.Line_No = (int)config["Line_No"];
                    requisitionLine.ProcurementPlanID = (string)config["ProcurementPlanID"];
                    requisitionLine.Type = (string)config["Type"];
                    requisitionLine.ProcurementPlanItemNo = (int)config["ProcurementPlanItemNo"];
                    requisitionLine.Control6 = (string)config["Control6"];
                    requisitionLine.No = (string)config["No"];
                    requisitionLine.Description = (string)config["Description"];
                    requisitionLine.TechnicalSpecifications = (string)config["TechnicalSpecifications"];
                    requisitionLine.UnitofMeasureCode = (string)config["UnitofMeasureCode"];
                    requisitionLine.QuantityInStore = (int)config["QuantityInStore"];
                    requisitionLine.Quantity = (int)config["Quantity"];
                    requisitionLine.DirectUnitCostInclVAT = (string)config["DirectUnitCostInclVAT"];
                    requisitionLine.Amount = (decimal)config["Amount"];
                    requisitionLine.AmountIncludingVAT = (decimal)config["AmountIncludingVAT"];
                    requisitionLine.CurrencyCode = (string)config["CurrencyCode"];
                    requisitionLine.LocationCode = (string)config["LocationCode"];
                    requisitionLine.UnitCostLCY = (decimal)config["UnitCostLCY"];
                    requisitionLine.VATProdPostingGroup = (string)config["VATProdPostingGroup"];
                    requisitionLine.ItemCategoryCode = (string)config["ItemCategoryCode"];
                    requisitionLine.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                    requisitionLine.Budget = (string)config["Budget"];
                    requisitionLine.Budget_Line = (string)config["Budget_Line"];
                    requisitionLine.PPTotalAvailableBudget = (decimal)config["PPTotalAvailableBudget"];
                    requisitionLine.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                    requisitionLine.PPFundingSourceID = (string)config["PPFundingSourceID"];
                    requisitionLine.PPTotalBudget = (decimal)config["PPTotalBudget"];
                    requisitionLine.PPTotalActualCosts = (decimal)config["PPTotalActualCosts"];
                    requisitionLine.PPTotalCommitments = (decimal)config["PPTotalCommitments"];
                    requisitionLine.PPSolicitationType = (string)config["PPSolicitationType"];
                    requisitionLine.PPProcurementMethod = (string)config["PPProcurementMethod"];
                    requisitionLine.PPPreferenceReservationCode = (string)config["PPPreferenceReservationCode"];
                    requisitionLine.PRNConversionProcedure = (string)config["PRNConversionProcedure"];
                    requisitionLine.Women_Percent = (int)config["Women_Percent"];
                    requisitionLine.Women_Amount = (int)config["Women_Amount"];
                    requisitionLine.Youth_Percent = (int)config["Youth_Percent"];
                    requisitionLine.Youth_Amount = (int)config["Youth_Amount"];
                    requisitionLine.AGPO_Percent = (int)config["AGPO_Percent"];
                    requisitionLine.AGPO_Amount = (int)config["AGPO_Amount"];
                    requisitionLine.PWD_Percent = (int)config["PWD_Percent"];
                    requisitionLine.PWD_Amount = (int)config["PWD_Amount"];

                    purchaseRequisitionLines.Add(requisitionLine);
                }
            }

            #endregion



            return PartialView("~/Views/Purchase/PurchaseDocumentLineView.cshtml", purchaseRequisitionLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public JsonResult SendPurchaseAppForApproval(string DocNo)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            string staffNo = Session["Username"].ToString();
            string msg = "";
            bool valSucc = false;
            string userId = employeeView.UserID;



            string sent = Credentials.ObjNav.sendPurchaseRequisitionApproval(staffNo, DocNo);

            if (sent != "")
            {
                Credentials.ObjNav.UpdateApprovalEntrySenderID(38, DocNo, userId);

                base.Session["SuccessMsg"] = string.Concat("Purchase Requisition, Document No ", DocNo,
                    " send for approval Successfully");
            }

            msg = "Purchase Requisition,Document No," + DocNo + " sent for approval Successfully";
            valSucc = true;

            return Json(new { message = msg, success = valSucc }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult CancelPurchaseAppForApproval(string DocNo)
    {
        try
        {
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string empNo = employee.No;
            Credentials.ObjNav.fnCancelPurchaseRequisitionApproval(empNo, DocNo);
            return Json(new { message = "Purchase Requisition approval cancelled Successfully", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]


    protected string GetRequisitionType(string DocNo)
    {
        string ReqType = "";
        string page = "PurchaseRequisition?$filter=No_ eq '" + DocNo + "'&format=json";
        HttpWebResponse httpResponse = Credentials.GetOdataData(page);
        using var streamReader = new StreamReader(httpResponse.GetResponseStream());
        var result = streamReader.ReadToEnd();

        var details = JObject.Parse(result);
        foreach (JObject config in details["value"])
        {
            ReqType = (string)config["Requisition_Type"];
        }

        return ReqType;
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SubmitPurchaseLine(string DocNo, PurchaseRequisitionLine requisitionLine)
    {
        try
        {
            string documentNumber = DocNo;

            string planningCategory = requisitionLine.ProcurementPlanID;

            int itemno = requisitionLine.ProcurementPlanItemNo;

            int Qnty = requisitionLine.Quantity;




            string status =
                Credentials.ObjNav.createPurchaseReqLine(documentNumber, planningCategory, Qnty, itemno);

            if (!string.IsNullOrEmpty(status))
            {
                string DocNetAmount = GetDocNetAmount(DocNo);
                return Json(
                    new { NetAmout = DocNetAmount, message = "Purchase Line Added successfully", success = true },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    ////protected int GetDocumentCount(string DocNo)
    //{
    //    int count = 0;
    //    string pageLine = "PurchaseLines?$select=Line_No_&$orderby=Line_No_ desc&$top=1&$filter=Document_No_ eq '" + DocNo + "'&$format=json";
    //    HttpWebResponse httpResponse = Credentials.GetOdataData(pageLine);
    //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //    {
    //        var result = streamReader.ReadToEnd();

    //        var details = JObject.Parse(result);
    //        if (details["value"].Count() > 0)
    //        {
    //            foreach (JObject config in details["value"])
    //            {
    //                count = (int)config["Line_No_"];
    //            }
    //        }
    //    }
    //    return count;
    //}
    //public JsonResult UpdatePurchaseLine(PurchaseRequisitionLine prvLine)
    //{
    //    try
    //    {
    //        //Credentials.ObjNav.PurchaseRequistionLineUpdate(Convert.ToInt32(prvLine.LnNo), Convert.ToInt32(prvLine.Qnty), Convert.ToInt32(prvLine.Amount), prvLine.DocNo, prvLine.Description2, Convert.ToInt32(prvLine.NoofDays));
    //        string DocNetAmount = GetDocNetAmount(prvLine.DocNo);
    //        return Json(new { NetAmout = DocNetAmount, message = "Purchase Line Updated successfully", success = true }, JsonRequestBehavior.AllowGet);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(new { message = ex.Message.Replace("'", ""), success = false }, JsonRequestBehavior.AllowGet);
    //    }
    //}
    public JsonResult RemovePurchaseLine(string DocNo, string LnNo)
    {
        try
        {
            // Credentials.ObjNav.PurchaseRequsitionRemoveLine(Convert.ToInt32(LnNo), DocNo);
            string DocNetAmount = GetDocNetAmount(DocNo);

            return Json(
                new { NetAmout = DocNetAmount, message = "Purchase Line Deleted successfully", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    [AcceptVerbs(HttpVerbs.Get)]
    public PartialViewResult FileUploadForm()
    {
        return PartialView("~/Views/Purchase/FileAttachmentForm.cshtml");
    }

    protected string GetDocNetAmount(string DocNo)
    {
        string amount = "";
        string page = "PurchaseRequisition?$filter=No eq '" + DocNo + "'&format=json";
        HttpWebResponse httpResponse = Credentials.GetOdataData(page);
        using var streamReader = new StreamReader(httpResponse.GetResponseStream());
        var result = streamReader.ReadToEnd();

        var details = JObject.Parse(result);
        if (details["value"].Count() > 0)
        {
            foreach (JObject config in details["value"])
            {
                amount = Convert.ToDecimal((string)config["Amount"]).ToString("#,##0.00");
            }
        }

        return amount;
    }

    //-----------------FUNCTIONAL PROCUREMENT LINE FUNCTIONS----------------//

    public ActionResult FunctionalProcurementPlan()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }
    public PartialViewResult ProcurementPlanList()
    {
        var procurementPlans = new List<ProcurementPlan>();
        var employeeView = Session["EmployeeData"] as EmployeeView;

        if (employeeView == null)
        {
            return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanList.cshtml", procurementPlans);
        }

        var geographical = employeeView.GlobalDimension1Code;
        var adminUnit = employeeView.AdministrativeUnit;
        var employeeDims = employeeView.EmployeeDimensions;

        try
        {
            if (employeeDims != null && employeeDims.Count > 0)
            {
                foreach (var query in employeeDims.Select(dim =>
                             $"DptProcurementPlan?$filter=Global_Dimension_1_Code eq '{dim.GlobalDim1}' " +
                             $"and Global_Dimension_2_Code eq '{dim.GlobalDim2}' " +
                             $"and Document_Type eq 'Procurement Plan' and Plan_Type eq 'Functional'&$format=json"))
                {
                    procurementPlans.AddRange(FetchProcurementPlans(query));
                }
            }
            else
            {
                var query = "";
                if (geographical == "00000001")
                {
                    query = $"DptProcurementPlan?$filter=Global_Dimension_2_Code eq '{adminUnit}' " +
                            $"and Document_Type eq 'Procurement Plan' and Plan_Type eq 'Functional'&$format=json";
                }
                else
                {
                    query = $"DptProcurementPlan?$filter=Global_Dimension_1_Code eq '{geographical}' " +
                            $"and Document_Type eq 'Procurement Plan' and Plan_Type eq 'Functional'&$format=json";
                }

                procurementPlans.AddRange(FetchProcurementPlans(query));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }

        return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanList.cshtml",
            procurementPlans.OrderByDescending(x => x.Code));
    }

    private IEnumerable<ProcurementPlan> FetchProcurementPlans(string odataQuery)
    {
        var procurementPlans = new List<ProcurementPlan>();

        try
        {
            var httpResponse = Credentials.GetOdataData(odataQuery);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            procurementPlans.AddRange(from JObject config in details["value"]
                                      select new ProcurementPlan
                                      {
                                          Code = (string)config["Code"],
                                          Description = (string)config["Description"],
                                          ConsolidatedProcurementPlan = (string)config["Consolidated_Procurement_Plan"],
                                          DocumentDate = DateTime.Parse((string)config["Document_Date"]),
                                          CorporateStrategicPlanID = (string)config["Corporate_Strategic_Plan_ID"],
                                          FinancialBudgetID = (string)config["Financial_Budget_ID"],
                                          FinancialYearCode = (string)config["Financial_Year_Code"],
                                          StartDate = DateTime.Parse((string)config["Start_Date"]),
                                          EndDate = DateTime.Parse((string)config["End_Date"]),
                                          GlobalDimension1Code = (string)config["Global_Dimension_1_Code"],
                                          GlobalDimension2Code = (string)config["Global_Dimension_2_Code"],
                                          Admin_Unit_Name = (string)config["Admin_Unit_Name"],
                                          GlobalDimension3Code = (string)config["Global_Dimension_3_Code"],
                                          TotalBudgetGoods = decimal.Parse((string)config?["Total_Budget_Goods"]),
                                          TotalBudgetWorks = decimal.Parse((string)config?["Total_Budget_Works"]),
                                          TotalBudgetServices = decimal.Parse((string)config?["Total_Budget_Services"]),
                                          TotalBudgetedSpend = decimal.Parse((string)config?["Total_Budgeted_Spend"]),
                                          TotalActualGoods = decimal.Parse((string)config["Total_Actual_Goods"]),
                                          TotalActualWorks = decimal.Parse((string)config["Total_Actual_Works"]),
                                          TotalActualServices = decimal.Parse((string)config["Total_Actual_Services"]),
                                          TotalActualSpend = decimal.Parse((string)config["Total_Actual_Spend"]),
                                          SpendPercent = decimal.Parse((string)config["Spend_Percent"]),
                                          ApprovalStatus = (string)config["Approval_Status"],
                                          CreatedBy = (string)config["Created_By"],
                                          DateCreated = DateTime.Parse((string)config["Date_Created"]),
                                          TimeCreated = TimeSpan.Parse((string)config["Time_Created"])
                                      });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }

        return procurementPlans;
    }


    public ActionResult FunctionalProcurementPlanDocument(string docNo)
    {
        try
        {
            string userId = Session["UserId"].ToString();
            ProcurementPlan procurement = new ProcurementPlan();

            string page = $"DptProcurementPlan?$filter=Code eq '{docNo}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    procurement.Code = (string)config["Code"];
                    procurement.Description = (string)config["Description"];
                    procurement.ConsolidatedProcurementPlan = (string)config["Consolidated_Procurement_Plan"];
                    procurement.DocumentDate = DateTime.Parse((string)config["Document_Date"]);
                    procurement.CorporateStrategicPlanID = (string)config["Corporate_Strategic_Plan_ID"];
                    procurement.FinancialBudgetID = (string)config["Financial_Budget_ID"];
                    procurement.FinancialYearCode = (string)config["Financial_Year_Code"];
                    procurement.StartDate = DateTime.Parse((string)config["Start_Date"]);
                    procurement.EndDate = DateTime.Parse((string)config["End_Date"]);
                    procurement.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                    procurement.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                    procurement.Admin_Unit_Name = (string)config["Admin_Unit_Name"];
                    procurement.GlobalDimension3Code = (string)config["Global_Dimension_3_Code"];
                    procurement.TotalBudgetGoods = decimal.Parse((string)config["Total_Budget_Goods"]);
                    procurement.TotalBudgetWorks = decimal.Parse((string)config["Total_Budget_Works"]);
                    procurement.TotalBudgetServices = decimal.Parse((string)config["Total_Budget_Services"]);
                    procurement.TotalBudgetedSpend = decimal.Parse((string)config["Total_Budgeted_Spend"]);
                    procurement.TotalActualGoods = decimal.Parse((string)config["Total_Actual_Goods"]);
                    procurement.TotalActualWorks = decimal.Parse((string)config["Total_Actual_Works"]);
                    procurement.TotalActualServices = decimal.Parse((string)config["Total_Actual_Services"]);
                    procurement.TotalActualSpend = decimal.Parse((string)config["Total_Actual_Spend"]);
                    procurement.SpendPercent = decimal.Parse((string)config["Spend_Percent"]);
                    procurement.ApprovalStatus = (string)config["Approval_Status"];
                    procurement.CreatedBy = (string)config["Created_By"];
                    procurement.DateCreated = DateTime.Parse((string)config["Date_Created"]);
                    procurement.TimeCreated = TimeSpan.Parse((string)config["Time_Created"]);


                }
            }

            return View(procurement);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult ProcurementPlanLines(string DocNo, string Status, string StartDate, string EndDate)
    {
        try
        {
            List<ProcurementPlanLine> procurementPlanLines = new List<ProcurementPlanLine>();
            string pageLine = "ProcurementPlanLines?$filter=Procurement_Plan_ID eq '" + DocNo +
                              "' and Total_Budgeted_Cost gt 0&$format=json";

            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ProcurementPlanLine procurementPlanLine = new ProcurementPlanLine();
                    procurementPlanLine.PPLineNo = (int)config["PP_Line_No"];
                    procurementPlanLine.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    procurementPlanLine.DocumentType = (string)config["Document_Type"];
                    procurementPlanLine.PlanningCategory = (string)config["Planning_Category"];
                    procurementPlanLine.Description = (string)config["Description"];
                    procurementPlanLine.TotalQuantity = (decimal)config["Total_Quantity"];
                    procurementPlanLine.TotalBudgetedCost = (decimal)config["Total_Budgeted_Cost"];
                    procurementPlanLine.StartDate = StartDate;
                    procurementPlanLine.EndDate = EndDate;
                    procurementPlanLine.Status = Status;
                    procurementPlanLines.Add(procurementPlanLine);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanLines.cshtml", procurementPlanLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult ProcurementTask(string procurementNo, string planningCategory, string Status, string StartDate, string EndDate)
    {
        Employee emp = new Employee();
        try
        {
            List<ProcurementTask> procurementTasks = new List<ProcurementTask>();
            /* string pageLine = "ProcurementPlanRevisionEntries?$filter=ProcurementPlanID eq '" + procurementNo +
                               "' and Planning_Category eq '" + planningCategory + "' and Document_Type eq 'Revision Voucher'&$format=json";*/
            string pageLine = "ProcurementTask?$filter=Procurement_Plan_ID eq '" + procurementNo +
                              "' and Planning_Category eq '" + planningCategory + "'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            LoadProcurementMethods();
            LoadSolicitationTypes();
            LoadProcurementTypes();
            LoadVotes();
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    ProcurementTask procurementTask = new ProcurementTask();
                    procurementTask.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    procurementTask.EntryNo = (int)config["Entry_No"];
                    procurementTask.PlanningCategory = (string)config["Planning_Category"];
                    procurementTask.DocumentType = (string)config["Document_Type"];
                    procurementTask.ItemType = (string)config["Item_Type"];
                    procurementTask.ItemNo = (string)config["Item_No"];
                    procurementTask.Description = (string)config["Description"];
                    procurementTask.ProcurementType = (string)config["Procurement_Type"];
                    procurementTask.SolicitationType = (string)config["Solicitation_Type"];
                    procurementTask.ProcurementMethod = (string)config["Procurement_Method"];
                    procurementTask.AlternativeProcurementMethods =
                        (string)config["Alternative_Procurement_Methods"];
                    procurementTask.PreferenceReservationCode = (string)config["Preference_Reservation_Code"];
                    procurementTask.FundingSourceID = (string)config["Funding_Source_ID"];
                    procurementTask.RequisitionProductGroup = (string)config["Requisition_Product_Group"];
                    procurementTask.ProcurementUse = (string)config["Procurement_Use"];
                    procurementTask.InvitationNoticeType = (string)config["Invitation_Notice_Type"];
                    procurementTask.PlanningDate = DateTime
                        .ParseExact((string)config["Planning_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        .ToString("dd-MM-yyyy");
                    procurementTask.Quantity = (decimal)config["Quantity"];
                    procurementTask.UnitCost = (decimal)config["Unit_Cost"];
                    procurementTask.LineBudgetCost = (decimal)config["Line_Budget_Cost"];
                    procurementTask.TotalActualCosts = (decimal)config["Total_Actual_Costs"];
                    procurementTask.TotalPRNCommitments = (decimal)config["Total_PRN_Commitments"];
                    procurementTask.AvailableProcurementBudget = (decimal)config["Available_Procurement_Budget"];
                    procurementTask.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                    procurementTask.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                    procurementTask.ShortcutDimension3Code = (string)config["Shortcut_Dimension_3_Code"];
                    procurementTask.ShortcutDimension4Code = (string)config["Shortcut_Dimension_4_Code"];
                    procurementTask.ShortcutDimension5Code = (string)config["Shortcut_Dimension_5_Code"];
                    procurementTask.ShortcutDimension6Code = (string)config["Shortcut_Dimension_6_Code"];
                    procurementTask.ShortcutDimension7Code = (string)config["Shortcut_Dimension_7_Code"];
                    procurementTask.ShortcutDimension8Code = (string)config["Shortcut_Dimension_8_Code"];
                    procurementTask.ProcurementStartDate = StartDate;
                    procurementTask.ProcurementEndDate = EndDate;
                    procurementTask.ProcurementDurationDays = (int)config["Procurement_Duration_Days"];
                    procurementTask.UserID = (string)config["User_ID"];
                    procurementTask.LastDateModified = DateTime.ParseExact((string)config["Last_Date_Modified"],
                        "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    procurementTask.BudgetControlJobNo = (string)config["Budget_Control_Job_No"];
                    procurementTask.BudgetControlJobTaskNo = (string)config["Budget_Control_Job_Task_No"];
                    procurementTask.BudgetAccount = (string)config["Budget_Account"];
                    procurementTask.Q1Quantity = (decimal)config["Q1_Quantity"];
                    procurementTask.Q1Amount = (decimal)config["Q1_Amount"];
                    procurementTask.Q1Budget = (decimal)config["Q1_Budget"];
                    procurementTask.Q2Quantity = (decimal)config["Q2_Quantity"];
                    procurementTask.Q2Amount = (decimal)config["Q2_Amount"];
                    procurementTask.Q2Budget = (decimal)config["Q2_Budget"];
                    procurementTask.Q3Quantity = (decimal)config["Q3_Quantity"];
                    procurementTask.Q3Amount = (decimal)config["Q3_Amount"];
                    procurementTask.Q3Budget = (decimal)config["Q3_Budget"];
                    procurementTask.Q4Quantity = (decimal)config["Q4_Quantity"];
                    procurementTask.Q4Amount = (decimal)config["Q4_Amount"];
                    procurementTask.Q4Budget = (decimal)config["Q4_Budget"];
                    procurementTask.WorkPlanProjectID = (string)config["Work_Plan_Project_ID"];
                    procurementTask.WorkplanTaskNo = (string)config["Workplan_Task_No"];
                    procurementTask.ActivityNo = (string)config["Activity_No"];
                    procurementTask.SubActivityNo = (string)config["Sub_Activity_No"];
                    procurementTask.AGPOPercent = (decimal)config["AGPO_Percent"];
                    procurementTask.AGPOAmount = (decimal)config["AGPO_Amount"];
                    procurementTask.PWDPercent = (decimal)config["PWD_Percent"];
                    procurementTask.PWDAmount = (decimal)config["PWD_Amount"];
                    procurementTask.WomenPercent = (decimal)config["Women_Percent"];
                    procurementTask.WomenAmount = (decimal)config["Women_Amount"];
                    procurementTask.YouthPercent = (decimal)config["Youth_Percent"];
                    procurementTask.YouthAmount = (decimal)config["Youth_Amount"];
                    procurementTask.MarginOfPreference = (decimal)config["Margin_of_Preference"];
                    procurementTask.CitizenContractorsPercent = (decimal)config["Citizen_Contractors_Percent"];
                    procurementTask.ActionTaken = (string)config["Action_Taken"] ?? "";
                    procurementTask.Status = (string)config["Status"];
                    procurementTask.DocumentStatus = Status;
                    procurementTasks.Add(procurementTask);
                }
            }
            ViewBag.DocumentStatus = Status;
            ViewBag.procurementPlanID = procurementNo;
            ViewBag.Plan_No = "";
            ViewBag.planningCategory = planningCategory;
            ViewBag.AdminUnit = emp.GlobalDimension1Code;
            return PartialView("ProcurementTask", procurementTasks);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    //For approver
    public ActionResult ApproverFunctionalProcurementPlanDocument(string docNo)
    {
        try
        {
            string userId = Session["UserId"].ToString();
            ProcurementPlan procurement = new ProcurementPlan();

            string page = $"DptProcurementPlan?$filter=Code eq '{docNo}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    procurement.Code = (string)config["Code"];
                    procurement.Description = (string)config["Description"];
                    procurement.ConsolidatedProcurementPlan = (string)config["Consolidated_Procurement_Plan"];
                    procurement.DocumentDate = DateTime.Parse((string)config["Document_Date"]);
                    procurement.CorporateStrategicPlanID = (string)config["Corporate_Strategic_Plan_ID"];
                    procurement.FinancialBudgetID = (string)config["Financial_Budget_ID"];
                    procurement.FinancialYearCode = (string)config["Financial_Year_Code"];
                    procurement.StartDate = DateTime.Parse((string)config["Start_Date"]);
                    procurement.EndDate = DateTime.Parse((string)config["End_Date"]);
                    procurement.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                    procurement.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                    procurement.Admin_Unit_Name = (string)config["Admin_Unit_Name"];
                    procurement.GlobalDimension3Code = (string)config["Global_Dimension_3_Code"];
                    procurement.TotalBudgetGoods = decimal.Parse((string)config["Total_Budget_Goods"]);
                    procurement.TotalBudgetWorks = decimal.Parse((string)config["Total_Budget_Works"]);
                    procurement.TotalBudgetServices = decimal.Parse((string)config["Total_Budget_Services"]);
                    procurement.TotalBudgetedSpend = decimal.Parse((string)config["Total_Budgeted_Spend"]);
                    procurement.TotalActualGoods = decimal.Parse((string)config["Total_Actual_Goods"]);
                    procurement.TotalActualWorks = decimal.Parse((string)config["Total_Actual_Works"]);
                    procurement.TotalActualServices = decimal.Parse((string)config["Total_Actual_Services"]);
                    procurement.TotalActualSpend = decimal.Parse((string)config["Total_Actual_Spend"]);
                    procurement.SpendPercent = decimal.Parse((string)config["Spend_Percent"]);
                    procurement.ApprovalStatus = (string)config["Approval_Status"];
                    procurement.CreatedBy = (string)config["Created_By"];
                    procurement.DateCreated = DateTime.Parse((string)config["Date_Created"]);
                    procurement.TimeCreated = TimeSpan.Parse((string)config["Time_Created"]);


                }
            }

            return View(procurement);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult ApproverProcurementPlanLines(string DocNo, string Status, string StartDate, string EndDate)
    {
        try
        {
            List<ProcurementPlanLine> procurementPlanLines = new List<ProcurementPlanLine>();
            string pageLine = "ProcurementPlanLines?$filter=Procurement_Plan_ID eq '" + DocNo +
                              "' and Total_Budgeted_Cost gt 0&$format=json";

            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ProcurementPlanLine procurementPlanLine = new ProcurementPlanLine();
                    procurementPlanLine.PPLineNo = (int)config["PP_Line_No"];
                    procurementPlanLine.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    procurementPlanLine.DocumentType = (string)config["Document_Type"];
                    procurementPlanLine.PlanningCategory = (string)config["Planning_Category"];
                    procurementPlanLine.Description = (string)config["Description"];
                    procurementPlanLine.TotalQuantity = (decimal)config["Total_Quantity"];
                    procurementPlanLine.TotalBudgetedCost = (decimal)config["Total_Budgeted_Cost"];
                    procurementPlanLine.StartDate = StartDate;
                    procurementPlanLine.EndDate = EndDate;
                    procurementPlanLine.Status = Status;
                    procurementPlanLines.Add(procurementPlanLine);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanLines.cshtml", procurementPlanLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult ApproverProcurementTask(string procurementNo, string planningCategory, string Status, string StartDate, string EndDate)
    {
        Employee emp = new Employee();
        try
        {
            List<ProcurementTask> procurementTasks = new List<ProcurementTask>();
            string pageLine = "ProcurementPlanRevisionEntries?$filter=ProcurementPlanID eq '" + procurementNo +
                              "' and Planning_Category eq '" + planningCategory + "' and Document_Type eq 'Revision Voucher'&$format=json";
            /* string pageLine = "ProcurementTask?$filter=Procurement_Plan_ID eq '" + procurementNo +
                               "' and Planning_Category eq '" + planningCategory + "'&$format=json";*/
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            LoadProcurementMethods();
            LoadSolicitationTypes();
            LoadProcurementTypes();
            LoadVotes();
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    ProcurementTask procurementTask = new ProcurementTask();
                    procurementTask.ProcurementPlanID = (string)config["ProcurementPlanID"];
                    procurementTask.EntryNo = (int)config["EntryNo"];
                    procurementTask.PlanningCategory = (string)config["Planning_Category"];
                    procurementTask.DocumentType = (string)config["Document_Type"];
                    procurementTask.ItemType = (string)config["PlanItemType"];
                    procurementTask.ItemNo = (string)config["PlanItemNo"];
                    procurementTask.Description = (string)config["Description"];
                    procurementTask.ProcurementType = (string)config["Procurement_Type"];
                    procurementTask.SolicitationType = (string)config["Solicitation_Type"];
                    procurementTask.ProcurementMethod = (string)config["Procurement_Method"];
                    procurementTask.AlternativeProcurementMethods =
                        (string)config["Alternative_Procurement_Methods"];
                    procurementTask.PreferenceReservationCode = (string)config["Preference_Reservation_Code"];
                    procurementTask.FundingSourceID = (string)config["Funding_Source_ID"];
                    procurementTask.RequisitionProductGroup = (string)config["Requisition_Product_Group"];
                    procurementTask.ProcurementUse = (string)config["Procurement_Use"];
                    procurementTask.InvitationNoticeType = (string)config["Invitation_Notice_Type"];
                    procurementTask.PlanningDate = DateTime
                        .ParseExact((string)config["Planning_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        .ToString("dd-MM-yyyy");
                    procurementTask.Quantity = (decimal)config["Quantity"];
                    procurementTask.UnitCost = (decimal)config["Unit_Cost"];
                    procurementTask.LineBudgetCost = (decimal)config["Line_Budget_Cost"];
                    procurementTask.TotalActualCosts = (decimal)config["Total_Actual_Costs"];
                    procurementTask.TotalPRNCommitments = (decimal)config["Total_PRN_Commitments"];
                    procurementTask.AvailableProcurementBudget = (decimal)config["Available_Procurement_Budget"];
                    procurementTask.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                    procurementTask.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                    procurementTask.ShortcutDimension3Code = (string)config["Shortcut_Dimension_3_Code"];
                    procurementTask.ShortcutDimension4Code = (string)config["Shortcut_Dimension_4_Code"];
                    procurementTask.ShortcutDimension5Code = (string)config["Shortcut_Dimension_5_Code"];
                    procurementTask.ShortcutDimension6Code = (string)config["Shortcut_Dimension_6_Code"];
                    procurementTask.ShortcutDimension7Code = (string)config["Shortcut_Dimension_7_Code"];
                    procurementTask.ShortcutDimension8Code = (string)config["Shortcut_Dimension_8_Code"];
                    procurementTask.ProcurementStartDate = StartDate;
                    procurementTask.ProcurementEndDate = EndDate;
                    procurementTask.ProcurementDurationDays = (int)config["Procurement_Duration_Days"];
                    procurementTask.UserID = (string)config["User_ID"];
                    procurementTask.LastDateModified = DateTime.ParseExact((string)config["Last_Date_Modified"],
                        "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    procurementTask.BudgetControlJobNo = (string)config["Budget_Control_Job_No"];
                    procurementTask.BudgetControlJobTaskNo = (string)config["Budget_Control_Job_Task_No"];
                    procurementTask.BudgetAccount = (string)config["Budget_Account"];
                    procurementTask.Q1Quantity = (decimal)config["Q1_Quantity"];
                    procurementTask.Q1Amount = (decimal)config["Q1_Amount"];
                    procurementTask.Q1Budget = (decimal)config["Q1_Budget"];
                    procurementTask.Q2Quantity = (decimal)config["Q2_Quantity"];
                    procurementTask.Q2Amount = (decimal)config["Q2_Amount"];
                    procurementTask.Q2Budget = (decimal)config["Q2_Budget"];
                    procurementTask.Q3Quantity = (decimal)config["Q3_Quantity"];
                    procurementTask.Q3Amount = (decimal)config["Q3_Amount"];
                    procurementTask.Q3Budget = (decimal)config["Q3_Budget"];
                    procurementTask.Q4Quantity = (decimal)config["Q4_Quantity"];
                    procurementTask.Q4Amount = (decimal)config["Q4_Amount"];
                    procurementTask.Q4Budget = (decimal)config["Q4_Budget"];
                    procurementTask.WorkPlanProjectID = (string)config["Work_Plan_Project_ID"];
                    procurementTask.WorkplanTaskNo = (string)config["Workplan_Task_No"];
                    procurementTask.ActivityNo = (string)config["Activity_No"];
                    procurementTask.SubActivityNo = (string)config["Sub_Activity_No"];
                    procurementTask.AGPOPercent = (decimal)config["AGPO_Percent"];
                    procurementTask.AGPOAmount = (decimal)config["AGPO_Amount"];
                    procurementTask.PWDPercent = (decimal)config["PWD_Percent"];
                    procurementTask.PWDAmount = (decimal)config["PWD_Amount"];
                    procurementTask.WomenPercent = (decimal)config["Women_Percent"];
                    procurementTask.WomenAmount = (decimal)config["Women_Amount"];
                    procurementTask.YouthPercent = (decimal)config["Youth_Percent"];
                    procurementTask.YouthAmount = (decimal)config["Youth_Amount"];
                    procurementTask.MarginOfPreference = (decimal)config["Margin_of_Preference"];
                    procurementTask.CitizenContractorsPercent = (decimal)config["Citizen_Contractors_Percent"];
                    procurementTask.ActionTaken = (string)config["Action_Taken"];
                    procurementTask.Status = (string)config["Status"];
                    procurementTask.DocumentStatus = Status;
                    procurementTasks.Add(procurementTask);
                }
            }
            ViewBag.DocumentStatus = Status;
            ViewBag.procurementPlanID = procurementNo;
            ViewBag.Plan_No = "";
            ViewBag.planningCategory = planningCategory;
            ViewBag.AdminUnit = emp.GlobalDimension1Code;
            return PartialView("ProcurementTask", procurementTasks);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }







    public PartialViewResult NewProcurementPlan()
    {
        try
        {

            string StaffNo = Session["Username"].ToString();
            string Dim1 = "", Dim2 = "";

            ProcurementPlan newProcurementPlan = new ProcurementPlan();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            Dim1 = employeeView.GlobalDimension1Code;
            Dim2 = employeeView.Department_Code;
            newProcurementPlan.CreatedBy = employeeView.UserID;
            newProcurementPlan.Name = employeeView.FirstName;



            #region Dim1 List

            List<DropdownList> dim1 = new List<DropdownList>();
            string pageDim1 = "DimensionValues?$filter=Global_Dimension_No eq 1 and Blocked eq false&$format=json";


            HttpWebResponse httpResponseDepartment = Credentials.GetOdataData(pageDim1);
            using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Code"] + "-" + (string)config["Name"];
                    dropDown.Value = (string)config["Code"];
                    dim1.Add(dropDown);
                }
            }

            #endregion

            #region dim2

            List<DropdownList> dim2 = new List<DropdownList>();
            string pageDim2 = "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$format=json";

            HttpWebResponse httpResponseDivision = Credentials.GetOdataData(pageDim2);
            using (var streamReader = new StreamReader(httpResponseDivision.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                    dropdownList.Value = (string)config["Code"];
                    dim2.Add(dropdownList);
                }
            }

            #endregion

            #region procurment plan

            List<DropdownList> procurmentPlan = new List<DropdownList>();
            string procPlan = "ProcurementPlan?$format=json";

            HttpWebResponse httpResponseproc = Credentials.GetOdataData(procPlan);
            using (var streamReader = new StreamReader(httpResponseproc.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                    dropdownList.Value = (string)config["Code"];
                    dim2.Add(dropdownList);
                }
            }

            #endregion

            #region Corporate Stategic plan

            List<DropdownList> strategicPlans = new List<DropdownList>();
            string stPlans = "AllCSPS?$format=json";

            HttpWebResponse httpResponsePlans = Credentials.GetOdataData(stPlans);
            using (var streamReader = new StreamReader(httpResponsePlans.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                    dropdownList.Value = (string)config["Code"];
                    strategicPlans.Add(dropdownList);
                }
            }

            #endregion

            #region Financial Budget

            List<DropdownList> financialBudget = new List<DropdownList>();
            string categories = "GLBudgets?$format=json";

            HttpWebResponse httpResponseCategories = Credentials.GetOdataData(categories);
            using (var streamReader = new StreamReader(httpResponseCategories.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                    dropdownList.Value = (string)config["Code"];
                    financialBudget.Add(dropdownList);
                }
            }

            #endregion

            #region Financial Budget

            List<DropdownList> financialYears = new List<DropdownList>();
            string finYears = "GLBudgets?$format=json";

            HttpWebResponse httpResponseFinYrs = Credentials.GetOdataData(finYears);
            using (var streamReader = new StreamReader(httpResponseFinYrs.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Name"] + "-" + (string)config["Description"];
                    dropdownList.Value = (string)config["Name"];
                    financialYears.Add(dropdownList);
                }
            }

            #endregion

            #region vote

            List<DropdownList> votes = new List<DropdownList>();
            string pageVote = "DimensionValues?$filter=Global_Dimension_No eq 3 and Blocked eq false&$format=json";


            HttpWebResponse httpResponseVote = Credentials.GetOdataData(pageVote);
            using (var streamReader = new StreamReader(httpResponseVote.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Code"] + "-" + (string)config["Name"];
                    dropDown.Value = (string)config["Code"];
                    votes.Add(dropDown);
                }
            }

            #endregion

            newProcurementPlan.ListOfDim1 = dim1.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newProcurementPlan.ListofStrategicPlans = strategicPlans.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newProcurementPlan.ListOfFinancialBudgets = financialBudget.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newProcurementPlan.ListOfFinancialYears = financialYears.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newProcurementPlan.ListOfDim2 = dim2.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newProcurementPlan.ListOfDim3 = votes.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();


            return PartialView("~/Views/Purchase/PartialViews/NewFunctionalProcurementPlan.cshtml",
                newProcurementPlan);

        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public JsonResult SubmitProcurementPlan(ProcurementPlan requisition)
    {
        try
        {
            string StaffNo = Session["Username"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string DocNo = Credentials.ObjNav.InsertProcurementPlan(StaffNo, requisition.Description,
                requisition.CorporateStrategicPlanID, requisition.GlobalDimension1Code
                , requisition.GlobalDimension2Code, requisition.GlobalDimension3Code);
            if (DocNo != "")
            {
                string Redirect = "/Purchase/FunctionalProcurementPlanDocument?DocNo=" + DocNo;

                return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Document not created. Please try again later...", success = false },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult NewProcurementPlanLine()
    {
        try
        {
            // LocationList locationList = new LocationList();
            PurchaseRequisitionLine purchaseLine = new PurchaseRequisitionLine();

            #region Procurement Categories

            List<DropdownList> procurementPlans = new List<DropdownList>();
            string page = $"ProcurementCategories?&$format=json";


            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Value = (string)config["Code"];
                    dropDown.Text = (string)config["Description"];
                    procurementPlans.Add(dropDown);
                }
            }

            #endregion


            purchaseLine.ListOfProcurementCategories = procurementPlans.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            return PartialView("~/Views/Purchase/PartialViews/NewProcurementPlanLine.cshtml", purchaseLine);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SubmitProcurementLine(string DocNo, PurchaseRequisitionLine requisitionLine)
    {
        try
        {
            string documentNumber = DocNo;
            string planningCategory = requisitionLine.ProcurementPlanID;
            int itemno = requisitionLine.ProcurementPlanItemNo;

            int Qnty = requisitionLine.Quantity;

            Credentials.ObjNav.InsertProcurementPlanLines(documentNumber, planningCategory);

            string DocNetAmount = GetDocNetAmount(DocNo);
            return Json(
                new
                {
                    NetAmout = DocNetAmount,
                    message = "Procurement Plan Line Added successfully",
                    success = true
                }, JsonRequestBehavior.AllowGet);


        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    [HttpPost]
    public JsonResult SubmitUpdatedTask(ProcurementTask planEntry,
        string planningCategory)
    {

        string Procurement_Type;
        string Procurement_Method;

        if (planEntry.AlternativeProcurementMethods == null)
        {
            planEntry.AlternativeProcurementMethods = "N/A";
        }

        switch (planEntry.ProcurementType)
        {
            case "GOODS": // Handles both "Edited" and "0"
                Procurement_Type = "0";
                break;


            case "SERVICES": // Handles both "Delete" and "1"
                Procurement_Type = "1";
                break;

            case "WORKS":
                Procurement_Type = "2";
                break;

            case "CONSULTANCY":
                Procurement_Type = "3";
                break;

            case "ASSETS":
                Procurement_Type = "4";
                break;
            default:
                Procurement_Type = "0";
                break;
        }


        switch (planEntry.ProcurementMethod)
        {
            case "Open Tender":
                Procurement_Method = "1";
                break;

            case "RFQ":
                Procurement_Method = "2";
                break;

            case "RFP":
                Procurement_Method = "3";
                break;

            case "Two Stage Tender":
                Procurement_Method = "4";
                break;


            case "Design Competition Tender":
                Procurement_Method = "5";
                break;

            case "Restricted Tender":
                Procurement_Method = "6";
                break;


            case "Direct Procurement":
                Procurement_Method = "7";
                break;

            case "Low Value Procurement":
                Procurement_Method = "8";
                break;

            case "Force Account":
                Procurement_Method = "9";
                break;

            case "Framework Agreement":
                Procurement_Method = "10";
                break;

            case "Reverse Auction":
                Procurement_Method = "11";
                break;

            default:
                Procurement_Method = "0";
                break;
        }


        try
        {
            string StaffNo = Session["Username"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            bool res = false;

            /* res =
                 Credentials.ObjNav.FnUpdateLinesProcplanLines(
                     planEntry.ProcurementPlanID,
                     planEntry.PlanningCategory,
                     0, //null- this is for document type int.Parse(planEntry.docType)
                     planEntry.SolicitationType,
                     *//*planEntry.PlanningCategory, //this is for GL account*//*
                     planEntry.docNo,
                     planEntry.EntryNo,
                     1, //int.Parse(planEntry.PlanItemType),-------- For PlanItemType
                     planEntry.ItemNo,
                     planEntry.ProcurementType,
                     int.Parse(Procurement_Method),
                     "DONOR", //funding source ID
                     int.Parse(Procurement_Type), *//*-------int.Parse(planEntry.Requisition_Product_Group),*//*
                     planEntry.Quantity,
                     planEntry.UnitCost,
                     employeeView.GlobalDimension1Code,
                     employeeView.GlobalDimension2Code,
                     planEntry.ShortcutDimension3Code, // Vote
                     planEntry.ShortcutDimension4Code, // Sub Program
                     planEntry.ShortcutDimension5Code, //class
                     planEntry.ShortcutDimension6Code, //funding source Dim
                     planEntry.Q1Quantity,
                     planEntry.Q2Quantity,
                     planEntry.Q3Quantity,
                     planEntry.Q4Quantity,

                     planEntry.Q1Amount,
                     planEntry.Q2Amount,
                     planEntry.Q3Amount,
                     planEntry.Q4Amount,

                     planEntry.AGPOPercent,
                     planEntry.PWDPercent,
                     planEntry.WomenPercent,
                     planEntry.YouthPercent,
                     planEntry.CitizenContractorsPercent,
                     1,
                     employeeView.UserID,
                     planEntry.AlternativeProcurementMethods
                 );*/

            if (res)
            {
                string Redirect = planEntry.docNo;
                return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Record not updated. Please try again later...", success = false },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult NewProcurementPlanEntry(string planLine, string category)
    {
        try
        {
            LocationList locationList = new LocationList();
            ViewBag.Category = category;

            #region Location List

            List<Locations> Locations = new List<Locations>();
            string page = "SpecialVendor?$filter=Blocked eq false&format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    Locations LocList = new Locations();
                    LocList.Code = (string)config["Code"];
                    LocList.Name = (string)config["Description"];
                    Locations.Add(LocList);
                }
            }

            #endregion

            #region ProcurementType

            List<DropdownList> procurementType = new List<DropdownList>();
            string pageType = "ProcurementTypes?$&format=json";

            HttpWebResponse httpResponseType = Credentials.GetOdataData(pageType);
            using (var streamReader = new StreamReader(httpResponseType.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList LocList = new DropdownList();
                    LocList.Text = (string)config["Code"];
                    LocList.Value = (string)config["Description"];
                    procurementType.Add(LocList);
                }
            }

            #endregion

            #region Solicitation Type

            List<DropdownList> solicitationType = new List<DropdownList>();
            string pageSolicitation = "SolicitationType?$&format=json";

            HttpWebResponse httpResponseSol = Credentials.GetOdataData(pageSolicitation);
            using (var streamReader = new StreamReader(httpResponseSol.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList LocList = new DropdownList();
                    LocList.Text = (string)config["Code"];
                    LocList.Value = (string)config["Description"];
                    solicitationType.Add(LocList);
                }
            }

            #endregion

            #region Vendor Category

            List<DropdownList> vendorType = new List<DropdownList>();
            string pageVendor = "VendorCategory?$&format=json";

            HttpWebResponse httpResponseVendor = Credentials.GetOdataData(pageVendor);
            using (var streamReader = new StreamReader(httpResponseVendor.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList LocList = new DropdownList();
                    LocList.Text = (string)config["Code"];
                    LocList.Value = (string)config["Description"];
                    vendorType.Add(LocList);
                }
            }

            #endregion

            #region Funding Source

            List<DropdownList> funding = new List<DropdownList>();
            string pagefunding = "FundingSource?$&format=json";

            HttpWebResponse httpResponsefunding = Credentials.GetOdataData(pagefunding);
            using (var streamReader = new StreamReader(httpResponsefunding.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList LocList = new DropdownList();
                    LocList.Text = (string)config["Code"];
                    LocList.Value = (string)config["Description"];
                    funding.Add(LocList);
                }
            }

            #endregion

            #region Contracts

            List<DropdownList> contracts = new List<DropdownList>();
            string pagecontracts = "PerfomanceContracts?$&format=json";

            HttpWebResponse httpResponsecontracts = Credentials.GetOdataData(pagecontracts);
            using (var streamReader = new StreamReader(httpResponsecontracts.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList LocList = new DropdownList();
                    LocList.Text = (string)config["No"];
                    LocList.Value = (string)config["Description"];
                    contracts.Add(LocList);
                }
            }

            #endregion

            locationList = new LocationList
            {
                Code = planLine,
                ListOfLocations = Locations.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList(),
                ListOfProcurementTypes = procurementType.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList(),
                ListOfSolicitationType = solicitationType.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList(),
                ListOfVendorCategory = vendorType.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList(),
                ListOfFundingSource = funding.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList(),
                ListOfContracts = contracts.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList(),
            };
            return PartialView("~/Views/Purchase/NewProcurementPlanEntry.cshtml", locationList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    protected void LoadProcurementMethods()
    {
        try
        {
            List<DropdownList> dropDownList = new List<DropdownList>
            {
                new DropdownList { Value = "Open Tender", Text = "Open Tender" },
                new DropdownList { Value = "RFQ", Text = "RFQ" },
                new DropdownList { Value = "RFP", Text = "RFP" },
                new DropdownList { Value = "Two-Stage Tender", Text = "Two-Stage Tender" },
                new DropdownList { Value = "Design Competition Tender", Text = "Design Competition Tender" },
                new DropdownList { Value = "Restricted Tender", Text = "Restricted Tender" },
                new DropdownList { Value = "Direct Procurement", Text = "Direct Procurement" },
                new DropdownList { Value = "Low Value Procurement", Text = "Low Value Procurement" },
                new DropdownList { Value = "Force Account", Text = "Force Account" },
                new DropdownList { Value = "Framework Agreement", Text = "Framework Agreement" },
                new DropdownList { Value = "Reverse Auction", Text = "Reverse Auction" }
            };

            ViewBag.ProcurementMethods = dropDownList;
        }
        catch (Exception exception)
        {
            // Handle exception
            exception.Data.Clear();
        }
    }
    protected void LoadProcurementTypes()
    {
        try
        {
            #region ProcurementType

            List<DropdownList> procurementType = new List<DropdownList>();
            string pageType = "ProcurementTypes?$&format=json";

            HttpWebResponse httpResponseType = Credentials.GetOdataData(pageType);
            using (var streamReader = new StreamReader(httpResponseType.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    DropdownList LocList = new DropdownList
                    {
                        Value = (string)config["Code"], // Assuming "Code" should be the Value
                        Text = (string)config["Description"] // Assuming "Description" should be the Text
                    };
                    procurementType.Add(LocList);
                }
            }

            ViewBag.ProcurementTypeList = procurementType; // Assign to ViewBag

            #endregion
        }
        catch (Exception exception)
        {
            // Handle the exception
            exception.Data.Clear();
        }
    }
    protected void LoadSolicitationTypes()
    {
        try
        {
            #region SolicitationType

            List<DropdownList> solicitationType = new List<DropdownList>();
            string pageSolicitation = "SolicitationType?$&format=json";

            HttpWebResponse httpResponseSol = Credentials.GetOdataData(pageSolicitation);
            using (var streamReader = new StreamReader(httpResponseSol.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    DropdownList LocList = new DropdownList
                    {
                        Value = (string)config["Code"], // Assuming "Code" should be the Value
                        Text = (string)config["Description"] // Assuming "Description" should be the Text
                    };
                    solicitationType.Add(LocList);
                }
            }

            ViewBag.SolicitationTypeList = solicitationType; // Assign to ViewBag

            #endregion
        }
        catch (Exception exception)
        {
            // Handle the exception
            exception.Data.Clear();
        }
    }
    protected void LoadVotes()
    {
        try
        {
            #region votes
            List<DropdownList> votes = new List<DropdownList>();
            string pageVotes = "SolicitationType?$&format=json";

            HttpWebResponse httpResponseVotes = Credentials.GetOdataData(pageVotes);
            using (var streamReader = new StreamReader(httpResponseVotes.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    DropdownList VoteList = new DropdownList
                    {
                        Value = (string)config["Code"], // Assuming "Code" should be the Value
                        Text = (string)config["Description"] // Assuming "Description" should be the Text
                    };
                    votes.Add(VoteList);
                }
            }

            ViewBag.VotesList = votes; // Assign to ViewBag

            #endregion
        }
        catch (Exception exception)
        {
            // Handle the exception
            exception.Data.Clear();
        }
    }


    public JsonResult SubmitPlanEntry(ProcurementTask planEntry)
    {
        try
        {
            // bool success = Credentials.ObjNav.insertPlanEntries(planEntry.ItemNo,planEntry.ItemType);
            /* if (true)
             {
                 return Json(new { message = "Procurement Plan Entry added successfully", success = true },
                     JsonRequestBehavior.AllowGet);
             }*/

            return Json(
                new { message = "Procurement Plan Entry not added. Please try again later", success = false },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult ExpensePRNLine(string docNo, string glAccount, int lineNo, string Status)
    {
        try
        {
            List<ExpensePRNLine> expensePRNLines = new List<ExpensePRNLine>();
            string pageLine =
                $"ExpensePRNLine?$filter=Document_No eq '{docNo}' and Source_Line_No eq {lineNo}&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ExpensePRNLine expensePRN = new ExpensePRNLine();
                    expensePRN.DocumentNo = (string)config["Document_No"];
                    expensePRN.LineNo = Convert.ToInt32(config["Line_No"]);
                    expensePRN.ProcPlanEntryNo = Convert.ToInt32(config["ProcPlan_Entry_No"]);
                    expensePRN.ProcType = (string)config["Proc_Type"];
                    expensePRN.No = (string)config["No"];
                    expensePRN.Name = (string)config["Name"];
                    expensePRN.UnitOfMeasure = (string)config["Unit_of_Measure"];
                    expensePRN.Quantity = Convert.ToInt32(config["Quantity"]);
                    expensePRN.Rate = Convert.ToDecimal(config["Rate"]);
                    expensePRN.Total = Convert.ToDecimal(config["Total"]);
                    expensePRN.ExpenseDescription = (string)config["Expense_Description"];
                    expensePRN.GLAccount = (string)config["G_L_Account"];
                    expensePRN.Status = (string)config["Status"];
                    expensePRN.RecalledBy = (string)config["Recalled_By"];
                    expensePRN.RecalledOn = DateTime.Parse((string)config["Recalled_On"]);
                    expensePRN.SourceLineNo = (string)config["Source_Line_No"];
                    expensePRNLines.Add(expensePRN);
                }
            }

            ExpensePRNLineList Lines = new ExpensePRNLineList
            {
                Status = Status,
                ListOfExpensePRNLine = expensePRNLines
            };

            return PartialView("~/Views/Purchase/PartialViews/ExpensePRNLines.cshtml", Lines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    [CustomAuthorization(Role = "PROCUREMENT")]
    public ActionResult ApprovedPurchaseRequisition()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }
    public PartialViewResult ApprovedPurchaseRequisitionList()
    {
        var purchaseRequisitions = new List<ApprovedPurchaseRequisition>();
        var employeeView = Session["EmployeeData"] as EmployeeView;

        if (employeeView == null)
        {
            return PartialView("PartialViews/ApprovedPurchaseRequisitionList", purchaseRequisitions);
        }

        var geographical = employeeView.GlobalDimension1Code;
        var adminUnit = employeeView.AdministrativeUnit;
        var employeeDims = employeeView.EmployeeDimensions;

        try
        {
            if (employeeDims != null && employeeDims.Count > 0)
            {
                foreach (var query in employeeDims.Select(dim =>
                             $"ApprovedPurchaseRequisition?$filter=Shortcut_Dimension_1_Code eq '{dim.GlobalDim1}' " +
                             $"and Shortcut_Dimension_2_Code eq '{dim.GlobalDim2}' and Status eq 'Released'&$format=json"))
                {
                    purchaseRequisitions.AddRange(FetchApprovedPurchaseRequisitions(query));
                }
            }
            else
            {
                var query = "";
                if (geographical == "00000001")
                {
                    query = $"ApprovedPurchaseRequisition?$filter=Shortcut_Dimension_2_Code eq '{adminUnit}' " +
                            $"and Status eq 'Released'&$format=json";
                }
                else
                {
                    query = $"ApprovedPurchaseRequisition?$filter=Shortcut_Dimension_1_Code eq '{geographical}' " +
                            $"and Status eq 'Released'&$format=json";
                }
                purchaseRequisitions.AddRange(FetchApprovedPurchaseRequisitions(query));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }

        return PartialView("PartialViews/ApprovedPurchaseRequisitionList", purchaseRequisitions.OrderByDescending(x => x.No));
    }

    private IEnumerable<ApprovedPurchaseRequisition> FetchApprovedPurchaseRequisitions(string odataQuery)
    {
        var purchaseRequisitions = new List<ApprovedPurchaseRequisition>();

        try
        {
            var httpResponse = Credentials.GetOdataData(odataQuery);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            foreach (JObject config in details["value"])
            {
                purchaseRequisitions.Add(new ApprovedPurchaseRequisition
                {
                    DocumentType = config["Document_Type"]?.ToString() ?? string.Empty,
                    No = config["No"]?.ToString() ?? string.Empty,
                    PRNType = config["PRN_Type"]?.ToString() ?? string.Empty,
                    RequesterID = config["Requester_ID"]?.ToString() ?? string.Empty,
                    RequestByNo = config["Request_By_No"]?.ToString() ?? string.Empty,
                    RequestByName = config["Request_By_Name"]?.ToString() ?? string.Empty,
                    ShortcutDimension2Code = config["Shortcut_Dimension_2_Code"]?.ToString() ?? string.Empty,
                    AssignedUserID = config["Assigned_User_ID"]?.ToString() ?? string.Empty,
                    DepartmentName = config["Department_Name"]?.ToString() ?? string.Empty,
                    ProjectName = config["Project_Name"]?.ToString() ?? string.Empty,
                    ApprovedRequisitionAmount = config.ContainsKey("Approved_Requisition_Amount")
                        ? Convert.ToDecimal(config["Approved_Requisition_Amount"])
                        : 0M,
                    Status = config["Status"]?.ToString() == "Released" ? "Approved" : config["Status"]?.ToString() ?? string.Empty,
                    AssignedOfficer = config["Assigned_Officer"]?.ToString() ?? string.Empty,
                    LocationCode = config["Location_Code"]?.ToString() ?? string.Empty,
                    OrderDate = config.ContainsKey("Order_Date")
                        ? DateTime.Parse(config["Order_Date"]?.ToString() ?? "0001-01-01")
                        : DateTime.MinValue,
                    RequisitionAmount = config.ContainsKey("Requisition_Amount")
                        ? Convert.ToDecimal(config["Requisition_Amount"])
                        : 0M,
                    ShortcutDimension1Code = config["Shortcut_Dimension_1_Code"]?.ToString() ?? string.Empty,
                    ProcurementType = config["Procurement_Type"]?.ToString() ?? string.Empty,
                    ProcessType = config["Process_Type"]?.ToString() ?? string.Empty,
                    PurchaseType = config["Purchase_Type"]?.ToString() ?? string.Empty,
                    Description = config["Description"]?.ToString() ?? string.Empty,
                    Job = config["Job"]?.ToString() ?? string.Empty,
                    JobName = config["Job_Name"]?.ToString() ?? string.Empty,
                    LocationName = DimensinValuesList.GetDimensionValueName(config["Location_Code"]?.ToString() ?? string.Empty),
                    AdminUnitName = DimensinValuesList.GetDimensionValueName(config["Shortcut_Dimension_2_Code"]?.ToString() ?? string.Empty),
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }

        return purchaseRequisitions;
    }

    public ActionResult ApprovedPurchaseRequisitionDocument(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                ApprovedPurchaseRequisition purchase = new ApprovedPurchaseRequisition();

                string page = "PurchaseRequisitions?$filter=No_ eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {

                        purchase.DocumentType = (string)config["Document_Type"];
                        purchase.No = (string)config["No_"];
                        purchase.PRNType = (string)config["PRN_Type"];
                        purchase.DocumentDate = DateTime.Parse(config["Document_Date"].ToString());
                        purchase.LocationCode = (string)config["Location_Code"];
                        purchase.RequisitionNo = (string)config["Requisition_No"];
                        purchase.RequisitionProductGroup = (string)config["Requisition_Product_Group"];
                        purchase.RequesterID = (string)config["Requester_ID"];
                        purchase.RequestByNo = (string)config["Request_By_No_"];
                        purchase.RequestByName = (string)config["Request_By_Name"];
                        purchase.ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"];
                        purchase.ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"];
                        purchase.AssignedOfficer = (string)config["Assigned_Officer"];
                        purchase.AssignedUserID = (string)config["Assigned_User_ID"];
                        purchase.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                        purchase.PurchaserCode2 = (string)config["Purchaser_Code2"];
                        purchase.PPPlanningCategory = (string)config["PP_Planning_Category"];
                        purchase.Description = (string)config["Description"];
                        purchase.PPTotalBudget = Convert.ToDecimal(config["PP_Total_Budget"]);
                        purchase.PPSolicitationType = (string)config["PP_Solicitation_Type"];
                        purchase.OtherProcurementMethods = (string)config["Other_Procurement_Methods"];
                        purchase.PPInvitationNoticeType = (string)config["PP__Invitation_Notice_Type"];
                        purchase.PPPreferenceReservationCode = (string)config["PP_Preference_Reservation_Code"];
                        purchase.PPTotalActualCosts = Convert.ToDecimal(config["PP_Total_Actual_Costs"]);
                        purchase.PRNConversionProcedure = (string)config["PRN_Conversion_Procedure"];
                        purchase.Status = (string)config["Status"];
                        purchase.LinkedIFSNo = (string)config["Linked_IFS_No_"];
                        purchase.LinkedLPONo = (string)config["Linked_LPO_No_"];
                        purchase.ConsolidateToIFSNo = (string)config["Consolidate_to_IFS_No_"];
                        purchase.OrderedPRN = Convert.ToBoolean(config["Ordered_PRN"]);

                    }
                }

                purchase.LocationName = DimensinValuesList.GetDimensionValueName(purchase.LocationCode);
                purchase.AdminUnitName = DimensinValuesList.GetDimensionValueName(purchase.ShortcutDimension2Code);


                #region vendorCategories

                List<DropdownList> vendorCategories = new List<DropdownList>();
                string pageVC = $"VendorCategory?&$format=json";

                HttpWebResponse httpResponseVC = Credentials.GetOdataData(pageVC);
                using (var streamReader = new StreamReader(httpResponseVC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        list.Value = (string)config["Code"];
                        vendorCategories.Add(list);

                    }

                }

                #endregion

                #region SolicitationTypes

                List<DropdownList> solicitationTypes = new List<DropdownList>();
                string pageST = $"SolicitationType?&$format=json";

                HttpWebResponse httpResponseST = Credentials.GetOdataData(pageST);
                using (var streamReader = new StreamReader(httpResponseST.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        list.Value = (string)config["Code"];
                        solicitationTypes.Add(list);

                    }

                }

                #endregion

                #region IFSNos

                List<DropdownList> IFS = new List<DropdownList>();
                string pageIF = $"SolicitationType?&$format=json";

                HttpWebResponse httpResponseIF = Credentials.GetOdataData(pageIF);
                using (var streamReader = new StreamReader(httpResponseIF.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        list.Value = (string)config["Code"];
                        solicitationTypes.Add(list);

                    }

                }

                #endregion

                purchase.ListOfSolisitationTypes = solicitationTypes.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                purchase.ListOfPPPreferenceReservationCodes = vendorCategories.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                purchase.ListOfIFSNos = IFS.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                return View(purchase);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult ApprovedPRNLine(string docNo, string Status)
    {
        try
        {
            List<ApprovedPRNLine> approvedPRNLines = new List<ApprovedPRNLine>();
            string pageLine = $"ApprovedPRNLines?$filter=Document_No eq '{docNo}'&$format=json";
            //string pageLine = $"ExpensePRNLine?$filter=Document_No eq '{docNo}' &$format=json";

            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ApprovedPRNLine approvedPRN = new ApprovedPRNLine();

                    approvedPRN.DocumentNo = (string)config["Document_No"];
                    approvedPRN.LineNo = Convert.ToInt32(config["Line_No"]);
                    approvedPRN.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    approvedPRN.ProcurementPlanEntryNo = Convert.ToInt32(config["Procurement_Plan_Entry_No"]);
                    approvedPRN.PPSolicitationType = (string)config["PP_Solicitation_Type"];
                    approvedPRN.PPProcurementMethod = (string)config["PP_Procurement_Method"];
                    approvedPRN.PPPreferenceReservationCode = (string)config["PP_Preference_Reservation_Code"];
                    approvedPRN.Description = (string)config["Description"];
                    approvedPRN.UnitOfMeasureCode = (string)config["Unit_of_Measure_Code"];
                    approvedPRN.Quantity = Convert.ToInt32(config["Quantity"]);
                    approvedPRN.DirectUnitCost = Convert.ToDecimal(config["Direct_Unit_Cost"]);
                    approvedPRN.Amount = Convert.ToDecimal(config["Amount"]);
                    approvedPRN.AmountIncludingVAT = Convert.ToDecimal(config["Amount_Including_VAT"]);
                    approvedPRN.CurrencyCode = (string)config["Currency_Code"];
                    approvedPRN.LocationCode = (string)config["Location_Code"];
                    approvedPRN.UnitCostLCY = Convert.ToDecimal(config["Unit_Cost_LCY"]);



                    approvedPRNLines.Add(approvedPRN);
                }
            }

            ApprovedPRNLineList Lines = new ApprovedPRNLineList
            {
                Status = Status,
                ListOfApprovedPRNLine = approvedPRNLines
            };

            return PartialView("~/Views/Purchase/PartialViews/ApprovedPRNLine.cshtml", Lines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    //Request for quotations

    public ActionResult RequestForQuotations()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }
    public PartialViewResult RequestForQuotationsList()
    {
        try
        {
            List<RequestForQuotationsList> RequestForQuotationsList = new List<RequestForQuotationsList>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string station = employee.GlobalDimension1Code;
            string adminUnit = employee.AdministrativeUnit;
            ESSRoleSetup role = Session["ESSRoleSetup"] as ESSRoleSetup;
            string staffNo = employee.No;
            string page = "";
            /*if (role.Procurement_officer)
            {*/
                page =
                    $"RequestForQuotationCard?$filter=Purchaser_Code eq '{staffNo}'&$orderby=Code desc&$format=json";
           /* }
            else if (role.HQ_Procurement_Officer)
            {
                page = "RequestForQuotationCard?$orderby=Code desc&$format=json";
            }*/
            /* else if (employee.HeadofStation || role.Station_Procurement_Office)
             {
                 page =
                     $"RequestForQuotationCard?$filter=Global_Dimension_1_Code eq '{station}'&$orderby=Code desc&$format=json";
             }*/
            /*else if (employee.HOD)
            {
                page =
                    $"RequestForQuotationCard?$filter=Global_Dimension_2_Code eq '{adminUnit}'&$orderby=Code desc&$format=json";
            }
*/

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    RequestForQuotationsList quotation = new RequestForQuotationsList
                    {
                        Code = (string)config["Code"],
                        Document_Date = (string)config["Document_Date"],
                        External_Document_No = (string)config["External_Document_No"],
                        PRN_No = (string)config["PRN_No"],
                        Location_Code = (string)config["Location_Code"],
                        Description = (string)config["Description"],
                        Submission_End_Date = (string)config["Submission_End_Date"],
                        Submission_End_Time = (string)config["Submission_End_Time"],
                        Bid_Opening_Date = (string)config["Bid_Opening_Date"],
                        Bid_Opening_Time = (string)config["Bid_Opening_Time"],
                        Procurement_Type = (string)config["Procurement_Type"],
                        Requisition_Product_Group = (string)config["Requisition_Product_Group"],
                        Procurement_Category_ID = (string)config["Procurement_Category_ID"],
                        Target_Bidder_Group = (string)config["Target_Bidder_Group"],
                        Solicitation_Type = (string)config["Solicitation_Type"],
                        Bid_Submission_Method = (string)config["Bid_Submission_Method"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Sealed_Bids = (bool)config["Sealed_Bids"],
                        Tender_Validity_Duration = (string)config["Tender_Validity_Duration"],
                        Tender_Validity_Expiry_Date = (string)config["Tender_Validity_Expiry_Date"],
                        Purchaser_Code = (string)config["Purchaser_Code2"],
                        Purchaser_Name = (string)config["Purchaser_Name"],
                        Mandatory_Special_Group_Reserv = (bool)config["Mandatory_Special_Group_Reserv"],
                        Appointer_of_Bid_Arbitrator = (string)config["Appointer_of_Bid_Arbitrator"],
                        Bid_Scoring_Template = (string)config["Bid_Scoring_Template"],
                        Bid_Opening_Committe = (string)config["Bid_Opening_Committe"],
                        Bid_Evaluation_Committe = (string)config["Bid_Evaluation_Committe"],
                        Document_Status = (string)config["Document_Status"],
                        Published = (bool)config["Published"],
                        Date_Time_Published = (string)config["Date_Time_Published"],
                        Status = (string)config["Status"],
                        No_of_Submission = (int)config["No_of_Submission"],
                        Created_Date_Time = (string)config["No_of_Submission"],
                        Created_by = (string)config["Created_by"],
                        Procurement_Plan_ID = (string)config["Procurement_Plan_ID"],
                        Procurement_Plan_Entry_No = (int)config["Procurement_Plan_Entry_No"],
                        PP_Planning_Category = (string)config["PP_Planning_Category"],
                        PP_Funding_Source_ID = (string)config["PP_Funding_Source_ID"],
                        PP_Total_Budget = (int)config["PP_Total_Budget"],
                        PP_Total_Actual_Costs = (int)config["PP_Total_Actual_Costs"],
                        PP_Total_Commitments = (int)config["PP_Total_Commitments"],
                        PP_Total_Available_Budget = (int)config["PP_Total_Available_Budget"],
                        PP_Preference_Reservation_Code = (string)config["PP_Preference_Reservation_Code"]
                    };

                    if ((string)config["Status"] == "Released" || (string)config["Status"] == "Posted")
                    {
                        quotation.Status = "Approved";
                    }
                    else
                    {
                        quotation.Status = (string)config["Status"];
                    }

                    RequestForQuotationsList.Add(quotation);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/RequestForQuotationsList.cshtml",
                RequestForQuotationsList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }
    public ActionResult RequestForQuotationsDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                RequestForQuotationCard quotation = new RequestForQuotationCard();
                string page = "RequestForQuotationCard?$filter=Code eq '" + DocNo + "'&format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        quotation.Code = (string)config["Code"];
                        quotation.Document_Date = (string)config["Document_Date"];
                        quotation.External_Document_No = (string)config["External_Document_No"];
                        quotation.Location_Code = (string)config["Location_Code"];
                        quotation.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                        quotation.PRN_No = (string)config["PRN_No"];
                        quotation.Location_Code = (string)config["Location_Code"];
                        quotation.Description = (string)config["Description"];
                        quotation.Submission_End_Date = (string)config["Submission_End_Date"];
                        quotation.Submission_End_Time = (string)config["Submission_End_Time"];
                        quotation.Bid_Opening_Date = (string)config["Bid_Opening_Date"];
                        quotation.Bid_Opening_Time = (string)config["Bid_Opening_Time"];
                        quotation.Procurement_Type = (string)config["Procurement_Type"];
                        quotation.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                        quotation.Procurement_Category_ID = (string)config["Procurement_Category_ID"];
                        quotation.Target_Bidder_Group = (string)config["Target_Bidder_Group"];
                        quotation.Solicitation_Type = (string)config["Solicitation_Type"];
                        quotation.Bid_Submission_Method = (string)config["Bid_Submission_Method"];
                        quotation.Procurement_Method = (string)config["Procurement_Method"];
                        quotation.Sealed_Bids = (bool)config["Sealed_Bids"];
                        quotation.Status = (string)config["Status"];
                        quotation.Tender_Validity_Duration = (string)config["Tender_Validity_Duration"];
                        quotation.Tender_Validity_Expiry_Date = (string)config["Tender_Validity_Expiry_Date"];
                        quotation.Purchaser_Code = (string)config["Purchaser_Code2"];
                        quotation.Purchaser_Name = (string)config["Purchaser_Name"];
                        quotation.Mandatory_Special_Group_Reserv = (bool)config["Mandatory_Special_Group_Reserv"];
                        quotation.Appointer_of_Bid_Arbitrator = (string)config["Appointer_of_Bid_Arbitrator"];
                        quotation.Bid_Scoring_Template = (string)config["Bid_Scoring_Template"];
                        quotation.Bid_Opening_Committe = (string)config["Bid_Opening_Committe"];
                        quotation.Bid_Evaluation_Committe = (string)config["Bid_Evaluation_Committe"];
                        quotation.Document_Status = (string)config["Document_Status"];
                        quotation.Published = (bool)config["Published"];
                        quotation.Date_Time_Published = (string)config["Date_Time_Published"];
                        quotation.Status = (string)config["Status"];
                        quotation.No_of_Submission = (int)config["No_of_Submission"];
                        quotation.Created_Date_Time = (string)config["Created_Date_Time"];
                        quotation.Created_by = (string)config["Created_by"];
                        quotation.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                        quotation.Procurement_Plan_Entry_No = (int)config["Procurement_Plan_Entry_No"];
                        quotation.PP_Planning_Category = (string)config["PP_Planning_Category"];
                        quotation.PP_Funding_Source_ID = (string)config["PP_Funding_Source_ID"];
                        quotation.PP_Total_Budget = (int)config["PP_Total_Budget"];
                        quotation.PP_Total_Actual_Costs = (int)config["PP_Total_Actual_Costs"];
                        quotation.PP_Total_Commitments = (int)config["PP_Total_Commitments"];
                        quotation.PP_Total_Available_Budget = (int)config["PP_Total_Available_Budget"];
                        quotation.PP_Preference_Reservation_Code = (string)config["PP_Preference_Reservation_Code"];
                        quotation.Financial_Year_Code = (string)config["Financial_Year_Code"];

                        quotation.Bid_Opening_Venue = (string)config["Bid_Opening_Venue"];
                        quotation.Bid_Selection_Method = (string)config["Bid_Selection_Method"];
                        quotation.Tender_Box_Location_Code = (string)config["Tender_Box_Location_Code"];

                        quotation.GlobalDim1 = (string)config["Global_Dimension_1_Name"];
                        quotation.GlobalDim2 = (string)config["Global_Dimension_2_Name"];
                        quotation.Specification = (string)config["Specification"];
                        quotation.No_Of_Invited_Bidders = (int)config["No_of_Invited_Bidders"];
                        quotation.Procurement_Entity_Name = (string)config["Procuring_Entity_Name_Contact"];
                        quotation.PreBid_Meeting_Required = (bool)config["PreBid_Meeting_Required"];
                        /* quotation.PreBid_Meeting_Required = config["PreBid_Meeting_Required"] != null
                          ? Convert.ToBoolean(config["PreBid_Meeting_Required"])
                          : false;*/

                        quotation.PreBid_Meeting_Venue = (string)config["PreBid_Meeting_Venue"];
                        quotation.PreBid_Meeting_Date = (string)config["PreBid_Meeting_Date"];
                        quotation.PreBid_Meeting_Time = (string)config["PreBid_Meeting_Time"];
                        quotation.Requires_Technical_Evaluation = (string)config["Requires_Technical_Evaluation"];

                        if ((string)config["Status"] == "Released" || (string)config["Status"] == "Posted")
                        {
                            quotation.Status = "Approved";
                        }
                        else
                        {
                            quotation.Status = (string)config["Status"];
                        }
                    }
                }

                #region BidEvaluationTemplate

                List<DropdownList> bidEvaluationTemplate = new List<DropdownList>();
                string pageVC = $"BidEvaluationTemplate?&$format=json";

                HttpWebResponse httpResponseVC = Credentials.GetOdataData(pageVC);
                using (var streamReader = new StreamReader(httpResponseVC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        list.Value = (string)config["Code"];
                        bidEvaluationTemplate.Add(list);

                    }

                }

                #endregion

                #region OpeningTenderCommittees

                List<DropdownList> openingCmm = new List<DropdownList>();
                string pageTC =
                    $"TenderCommittees?$filter=CommitteeType eq 'OPENING' and ApprovalStatus eq 'Released'&format=json";

                HttpWebResponse httpResponseOp = Credentials.GetOdataData(pageTC);
                using (var streamReader = new StreamReader(httpResponseOp.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text =
                            $"{(string)config["CommitteeType"]}({(string)config["DocumentNo"]})-{(string)config["TenderName"]}";
                        list.Value = (string)config["DocumentNo"];
                        openingCmm.Add(list);

                    }

                }

                #endregion

                #region EvaluationTenderCommittees

                List<DropdownList> evaluationCom = new List<DropdownList>();
                string pageTEva =
                    $"TenderCommittees?$filter=CommitteeType eq 'EVALUATION' and ApprovalStatus eq 'Released'&format=json";

                HttpWebResponse httpResponseEva = Credentials.GetOdataData(pageTEva);
                using (var streamReader = new StreamReader(httpResponseEva.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text =
                            $"{(string)config["CommitteeType"]}({(string)config["DocumentNo"]})-{(string)config["TenderName"]}";
                        list.Value = (string)config["DocumentNo"];
                        evaluationCom.Add(list);

                    }

                }

                #endregion


                #region ProcurementTypes

                List<DropdownList> procurementTypes = new List<DropdownList>();
                string pagePT = $"ProcurementTypes?&$format=json";

                HttpWebResponse httpResponsePT = Credentials.GetOdataData(pagePT);
                using (var streamReader = new StreamReader(httpResponsePT.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Description"];
                        list.Value = (string)config["Code"];
                        procurementTypes.Add(list);

                    }

                }
                #endregion

                quotation.ListOfScoringTemplate = bidEvaluationTemplate.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                quotation.ListOfEvaluatin = evaluationCom.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                quotation.ListOfOpening = openingCmm.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();


                quotation.ListOfProcurementTypes = procurementTypes.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();



                return View(quotation);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }
    public PartialViewResult GetCommitteeList(string DocNo)
    {
        try
        {
            List<IFSTenderCommitteeList> IFSTenderCommitteeList = new List<IFSTenderCommitteeList>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string page = $"IFSTenderCommittee?$filter=IFS_Code eq '{DocNo}'&$orderby=Document_No desc&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSTenderCommitteeList IFSTender = new IFSTenderCommitteeList
                    {
                        Document_No = (string)config["Document_No"],
                        Committee_Type = (string)config["Committee_Type"],
                        Description = (string)config["Description"],
                        Appointment_Effective_Date = (string)config["Appointment_Effective_Date"],
                        Appointing_Authority = (string)config["Appointing_Authority"],
                        Tender_Name = (string)config["Tender_Name"],
                        Approval_Status = (string)config["Approval_Status"],
                        Primary_Region = (string)config["Primary_Region"],
                        Primary_Directorate = (string)config["Primary_Directorate"],
                        Primary_Department = (string)config["Primary_Department"],
                        Blocked = (bool)config["Blocked"],
                        Created_By = (string)config["Created_By"],
                        Created_Date = (string)config["Created_Date"],
                        Created_Time = (string)config["Created_Time"],
                        IFS_Code = (string)config["IFS_Code"],
                        Document_Date = (string)config["Document_Date"],

                    };
                    IFSTenderCommitteeList.Add(IFSTender);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/IFSTenderCommitteeList.cshtml",
                IFSTenderCommitteeList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }
    public PartialViewResult RequiredDocumentsList(string DocNo)
    {
        try
        {
            List<RequiredDocuments> RequiredDocumentsList = new List<RequiredDocuments>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string page = $"IFSRequiredDocuments?$filter=Document_No eq '{DocNo}'&$orderby=Document_No desc&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    RequiredDocuments document = new RequiredDocuments
                    {
                        Document_No = (string)config["Document_No"],
                        Procurement_Document_Type_ID = (string)config["Procurement_Document_Type_ID"],
                        Description = (string)config["Description"],
                        Track_Certificate_Expiry = (bool)config["Track_Certificate_Expiry"],
                        Requirement_Type = config["Requirement_Type"]?.ToString() == "0" ? "Mandatory" : "Optional"

                    };
                    RequiredDocumentsList.Add(document);


                }
            }
            ViewBag.DocNo = DocNo;
            return PartialView("~/Views/Purchase/PartialViews/RequiredDocumentsList.cshtml",
                RequiredDocumentsList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }
    public PartialViewResult NewDocument(string DocNo)
    {


        RequiredDocuments RequiredDocuments = new RequiredDocuments();

        #region ProcurementDocumentTypeLookup     
        List<DropdownList> ProdurementDocumentTypeList = new List<DropdownList>();
        string pageProcDoc = "ProcurementDocumentType?&$format=json";
        HttpWebResponse httpResponseProcDoc = Credentials.GetOdataData(pageProcDoc);
        using (var streamReader = new StreamReader(httpResponseProcDoc.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                DropdownList dropdownList = new DropdownList();
                dropdownList.Text = (string)config["Code"];
                dropdownList.Value = (string)config["Code"];
                ProdurementDocumentTypeList.Add(dropdownList);
            }
        }
        #endregion

        RequiredDocuments.RFQ = DocNo;
        RequiredDocuments.ListOfDocumentTypes = ProdurementDocumentTypeList.Select(x =>
            new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

        return PartialView("~/Views/Purchase/PartialViews/NewDocument.cshtml", RequiredDocuments);
    }

    public JsonResult GetDocumentDescription(string Procurement_Document_Type_ID)
    {
        try
        {
            string pageProcDoc = $"ProcurementDocumentType?$filter=Code eq '{Procurement_Document_Type_ID}'&$format=json";

            HttpWebResponse httpResponseProcDoc = Credentials.GetOdataData(pageProcDoc);
            using (var streamReader = new StreamReader(httpResponseProcDoc.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                if (details["value"] != null && details["value"].Any())
                {
                    var document = details["value"].First(); // Get the first matching document
                    string description = document["Description"]?.ToString() ?? "No description available";

                    return Json(new { success = true, description = description }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, description = "Document not found" }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message.Replace("'", "") }, JsonRequestBehavior.AllowGet);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SubmitDocument(RequiredDocuments document)
    {
        try
        {

            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;

            bool result = false;
            /* result = Credentials.ObjNav.fnUpdateRequiredDocs(
                 document.RFQ,
                 document.Procurement_Document_Type_ID,
                 int.Parse(document.Requirement_Type),
                 document.Track_Certificate_Expiry,
                 0
             );*/


            if (result)
            {
                string redirectUrl = "/Purchase/RequestForQuotationsDocumentView?DocNo=" + document.RFQ;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    public PartialViewResult RFQResponsesList(string DocNo)
    {
        try
        {
            List<PurchaseQuote> RFQResponseList = new List<PurchaseQuote>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            /*string page = $"PurchaseQuotesList?$filter=Document_Type eq 'Quote' and IFS_Code eq '{DocNo}'&$format=json"; */
            string page = $"PurchaseQuotesList?$filter=No eq '{DocNo}' and Document_Type eq 'Quote'&$format=json";


            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PurchaseQuote RFQResponse = new PurchaseQuote
                    {
                        No = (string)config["No"],
                        Buy_from_Vendor_No = (string)config["Buy_from_Vendor_No"],
                        Buy_from_Vendor_Name = (string)config["Buy_from_Vendor_Name"],
                        Location_Code = (string)config["Location_Code"],
                        Assigned_User_ID = (string)config["Assigned_User_ID"]
                    };
                    RFQResponseList.Add(RFQResponse);

                    ViewBag.DocNo = DocNo;
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/RFQResponsesList.cshtml",
                RFQResponseList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }
    public PartialViewResult TenderDocumentSourcesList(string DocNo)
    {
        try
        {
            List<IFSTenderDocumentSource> DocumentSourceList = new List<IFSTenderDocumentSource>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string page = $"IFSTenderDocumentSource?$filter=Document_No eq '{DocNo}'&$format=json";


            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSTenderDocumentSource source = new IFSTenderDocumentSource
                    {
                        Document_No = (string)config["Document_No"],
                        Tender_Source_ID = (string)config["Tender_Source_ID"],
                        Description = (string)config["Description"],
                        Url_Link = (string)config["Url_Link"],
                        Date_Published = (string)config["Date_Published"]
                    };
                    DocumentSourceList.Add(source);

                    ViewBag.DocNo = DocNo;
                }
            }
            ViewBag.DocNo = DocNo;
            return PartialView("~/Views/Purchase/PartialViews/TenderDocumentSourcesList.cshtml",
                DocumentSourceList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }
    public PartialViewResult NewTenderDocumentSource(string DocNo)
    {
        IFSTenderDocumentSource DocumentSource = new IFSTenderDocumentSource();

        #region TenderDocumentSourceLookup     
        List<DropdownList> TenderDocumentSourceList = new List<DropdownList>();
        string pageSec = $"TenderDocumentSource?$format=json";
        HttpWebResponse httpResponseSec = Credentials.GetOdataData(pageSec);
        using (var streamReader = new StreamReader(httpResponseSec.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                DropdownList dropdownList = new DropdownList();
                dropdownList.Text = (string)config["Code"];
                dropdownList.Value = (string)config["Code"];
                TenderDocumentSourceList.Add(dropdownList);
            }
        }
        #endregion

        DocumentSource.Document_No = DocNo;
        DocumentSource.ListOfDocumentSources = TenderDocumentSourceList.Select(x =>
            new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

        return PartialView("~/Views/Purchase/PartialViews/NewTenderDocumentSource.cshtml", DocumentSource);
    }

    public JsonResult GetDocumentSourceDescription(string Procurement_Document_Type_ID)
    {
        try
        {
            string pageProcDoc = $"TenderDocumentSource?$filter=Code eq '{Procurement_Document_Type_ID}'&$format=json";

            HttpWebResponse httpResponseProcDoc = Credentials.GetOdataData(pageProcDoc);
            using (var streamReader = new StreamReader(httpResponseProcDoc.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                if (details["value"] != null && details["value"].Any())
                {
                    var document = details["value"].First(); // Get the first matching document
                    string description = document["Description"]?.ToString() ?? "No description available";

                    return Json(new { success = true, description = description }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, description = "Document not found" }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message.Replace("'", "") }, JsonRequestBehavior.AllowGet);
        }
    }


    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SubmitTenderDocumentSource(IFSTenderDocumentSource documentSource)
    {
        try
        {

            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            var url_link = documentSource.Url_Link ?? "";
            bool result = false;
            /* result = Credentials.ObjNav.fnUpdateTenderDocSource(
                 documentSource.Document_No,
                 documentSource.Tender_Source_ID,
                 url_link,
                 DateTime.ParseExact(documentSource.Date_Published, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                 0
             );*/

            if (result)
            {
                string redirectUrl = "/Purchase/RequestForQuotationsDocumentView?DocNo=" + documentSource.Document_No;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult IFSSecurityList(string DocNo)
    {
        try
        {
            List<IFSSecurities> SecurityList = new List<IFSSecurities>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string page = $"IFSSecurities?$filter=IFS_Code eq '{DocNo}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSSecurities security = new IFSSecurities
                    {
                        IFS_Code = (string)config["IFS_Code"],
                        Form_of_Security = (string)config["Form_of_Security"],
                        Security_Type = (string)config["Security_Type"],
                        Required_at_Bid_Submission = (bool)config["Required_at_Bid_Submission"],
                        Description = (string)config["Description"],
                        Security_Amount_LCY = (int)config["Security_Amount_LCY"],
                        Bid_Security_Validity_Expiry = (string)config["Bid_Security_Validity_Expiry"]

                    };
                    SecurityList.Add(security);


                }
            }
            ViewBag.DocNo = DocNo;
            return PartialView("~/Views/Purchase/PartialViews/IFSSecurityList.cshtml",
                SecurityList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }
    public PartialViewResult NewIFSSecurity(string DocNo)
    {


        IFSSecurities IFSSecurity = new IFSSecurities();

        #region FormOfSecurityLookup     
        List<DropdownList> FormOfSecurityList = new List<DropdownList>();
        string pageSec = "TenderSecurityTypes?&$format=json";
        HttpWebResponse httpResponseSec = Credentials.GetOdataData(pageSec);
        using (var streamReader = new StreamReader(httpResponseSec.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                DropdownList dropdownList = new DropdownList();
                dropdownList.Text = (string)config["Code"];
                dropdownList.Value = (string)config["Code"];
                FormOfSecurityList.Add(dropdownList);
            }
        }
        #endregion

        IFSSecurity.IFS_Code = DocNo;
        IFSSecurity.ListOfSecurityForms = FormOfSecurityList.Select(x =>
            new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

        return PartialView("~/Views/Purchase/PartialViews/NewIFSSecurity.cshtml", IFSSecurity);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SubmitIFSSecurity(IFSSecurities security)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            bool result = false;
            /*result = Credentials.ObjNav.fnUpdateTenderSecurities(
                security.IFS_Code,
                security.Form_of_Security,
                int.Parse(security.Security_Type),
                security.Required_at_Bid_Submission,
                security.Description,
                security.Security_Amount_LCY,
                DateTime.ParseExact(security.Bid_Security_Validity_Expiry, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                0
            );*/

            if (result)
            {
                string redirectUrl = "/Purchase/RequestForQuotationsDocumentView?DocNo=" + security.IFS_Code;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult RequestForQuotationsLines(string DocNo, string Status)
    {
        try
        {
            List<RequestForQuotationsLines> QuotationsLines = new List<RequestForQuotationsLines>();

            string page = "RequestForQuotationLines?$filter=Standard_Purchase_Code eq '" + DocNo + "'&format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    RequestForQuotationsLines quotationLine = new RequestForQuotationsLines
                    {
                        Standard_Purchase_Code = (string)config["Standard_Purchase_Code"],
                        Line_No = (int)config["Line_No"],
                        Type = (string)config["Type"],
                        FilteredTypeField = (string)config["FilteredTypeField"],
                        No = (string)config["No"],
                        Description = (string)config["Description"],
                        Variant_Code = (string)config["Variant_Code"],
                        Quantity = (int)config["Quantity"],
                        Unit_of_Measure_Code = (string)config["Unit_of_Measure_Code"],
                        Amount_Excl_VAT = (int)config["Amount_Excl_VAT"],
                        Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                        Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"],
                        ShortcutDimCode_x005B_3_x005D_ = (string)config["ShortcutDimCode_x005B_3_x005D_"],
                        ShortcutDimCode_x005B_4_x005D_ = (string)config["ShortcutDimCode_x005B_4_x005D_"],
                        ShortcutDimCode_x005B_5_x005D_ = (string)config["ShortcutDimCode_x005B_5_x005D_"],
                        ShortcutDimCode_x005B_6_x005D_ = (string)config["ShortcutDimCode_x005B_6_x005D_"],
                        ShortcutDimCode_x005B_7_x005D_ = (string)config["ShortcutDimCode_x005B_7_x005D_"],
                        Item_Category = (string)config["Item_Category"],
                        ShortcutDimCode_x005B_8_x005D_ = (string)config["ShortcutDimCode_x005B_8_x005D_"],
                    };


                    QuotationsLines.Add(quotationLine);
                }
            }

            RequestForQuotationsLinesList Lines = new RequestForQuotationsLinesList
            {
                Status = Status,
                ListOfRequestForQuotationsLines = QuotationsLines
            };
            return PartialView("~/Views/Purchase/PartialViews/RequestForQuotationsLines.cshtml", Lines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult InvitedBiders(string DocNo, string status, string description)
    {
        try
        {
            List<RecurringPurchaseLine> invitedBiders = new List<RecurringPurchaseLine>();

            string page = "RecurringPuchaseLines?$filter=Code eq '" + DocNo + "'&format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    RecurringPurchaseLine bider = new RecurringPurchaseLine();
                    bider.Vendor_No = (string)config["Vendor_No"];
                    bider.Code = (string)config["Code"];
                    bider.Vendor_Name = (string)config["Vendor_Name"];
                    bider.Vendor_Phone_No = (string)config["Vendor_Phone_No"];
                    bider.Primary_Email = (string)config["Primary_Email"];
                    bider.Description = (string)config["Description"];
                    bider.Category = (string)config["Category"];
                    invitedBiders.Add(bider);

                }
            }

            RecurringPurchaseLinesList Lines = new RecurringPurchaseLinesList
            {
                Status = status,
                ListOfBiders = invitedBiders
            };
            ViewBag.Document_Status = status;
            ViewBag.description = description;
            return PartialView("~/Views/Purchase/PartialViews/InvitedBiders.cshtml", Lines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public JsonResult InvitedBidersJSon(string DocNo, string status, string description)
    {
        try
        {
            List<object> invitedBiders = new List<object>();

            string page = "RecurringPuchaseLines?$filter=Code eq '" + DocNo + "'&format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    invitedBiders.Add(new
                    {
                        Vendor_No = (string)config["Vendor_No"],
                        Vendor_Name = (string)config["Vendor_Name"]
                    });
                }
            }

            return Json(invitedBiders, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        }
    }




    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SendRFQ(string DocNo)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            bool result = false;
            /*result = Credentials.ObjNav.fnSendRFQ(DocNo);*/

            if (result)
            {
                string redirectUrl = "/Purchase/RequestForQuotationsDocumentView?DocNo=" + DocNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult PublishInvitation(string DocNo)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            bool result = false;
            /*result = Credentials.ObjNav.fnPublishITT(DocNo);*/

            if (result)
            {
                string redirectUrl = "/Purchase/RequestForQuotationsDocumentView?DocNo=" + DocNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult PreviewRFQ(string DocNo, string VendorNo)
    {
        try
        {
            string message = "";
            bool success = false, view = false;
            /* message = Credentials.ObjNav.PreviewRFQ(DocNo, VendorNo);*/
            if (message == "")
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }
            return Json(new { message = message, success, view }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }


    //Tender Committee
    public ActionResult IFSTenderCommittee()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }

    }
    public PartialViewResult IFSTenderCommitteeList()
    {

        try
        {
            List<IFSTenderCommitteeList> IFSTenderCommitteeList = new List<IFSTenderCommitteeList>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            //string page = "IFSTenderCommittee?$filter=Created_By eq '" + employee.UserID + "'&$format=json";
            string page = "IFSTenderCommittee?$orderby=Document_No desc&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSTenderCommitteeList IFSTender = new IFSTenderCommitteeList
                    {
                        Document_No = (string)config["Document_No"],
                        Committee_Type = (string)config["Committee_Type"],
                        Description = (string)config["Description"],
                        Appointment_Effective_Date = (string)config["Appointment_Effective_Date"],
                        Appointing_Authority = (string)config["Appointing_Authority"],
                        Tender_Name = (string)config["Tender_Name"],
                        Approval_Status = (string)config["Approval_Status"],
                        Primary_Region = (string)config["Primary_Region"],
                        Primary_Directorate = (string)config["Primary_Directorate"],
                        Primary_Department = (string)config["Primary_Department"],
                        Blocked = (bool)config["Blocked"],
                        Created_By = (string)config["Created_By"],
                        Created_Date = (string)config["Created_Date"],
                        Created_Time = (string)config["Created_Time"],
                        IFS_Code = (string)config["IFS_Code"],
                        Document_Date = (string)config["Document_Date"],

                    };
                    IFSTenderCommitteeList.Add(IFSTender);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/IFSTenderCommitteeList.cshtml",
                IFSTenderCommitteeList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public PartialViewResult IFSTenderCommitteeLines(string DocNo, string status)
    {

        try
        {
            List<IFSTenderCommitteeLines> IFSTenderCommitteeLines = new List<IFSTenderCommitteeLines>();
            string pageLine = "IFSTenderCommitteeLines?$filter=Document_No eq '" + DocNo + "'&$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSTenderCommitteeLines IFSTenderLine = new IFSTenderCommitteeLines
                    {
                        Document_No = (string)config["Document_No"],
                        Committee_Type = (string)config["Committee_Type"],
                        Line_No = (int)config["Line_No"],
                        Role = (string)config["Role"],
                        Member_No = (string)config["Member_No"],
                        Member_Name = (string)config["Member_Name"],
                        Designation = (string)config["Designation"],
                        Member_Email = (string)config["Member_Email"],
                    };
                    IFSTenderCommitteeLines.Add(IFSTenderLine);
                }
            }

            IFSTenderCommitteeLineList tenderCommitteeLineList = new IFSTenderCommitteeLineList();
            tenderCommitteeLineList.Status = status;
            tenderCommitteeLineList.ListOfTenderCommitteeLine = IFSTenderCommitteeLines;

            ViewBag.Document_Status = status;
            return PartialView("~/Views/Purchase/PartialViews/IFSTenderCommitteeLines.cshtml",
                tenderCommitteeLineList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public ActionResult IFSTenderCommitteeDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                //string doc = EncryptionHelper.DecryptDocumentNo(DocNo);
                IFSTenderCommitteeCard IFSTender = new IFSTenderCommitteeCard();
                string page = "IFSTenderCommitteeCard?$filter=Document_No eq '" + DocNo + "'&format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        IFSTender.Document_No = (string)config["Document_No"];
                        IFSTender.Committee_Type = (string)config["Committee_Type"];
                        IFSTender.IFS_Code = (string)config["IFS_Code"];
                        IFSTender.Description = (string)config["Description"];
                        IFSTender.Document_Date = (string)config["Document_Date"];
                        IFSTender.Appointment_Effective_Date = (string)config["Appointment_Effective_Date"];
                        IFSTender.Appointing_Authority = (string)config["Appointing_Authority"];
                        IFSTender.Tender_Name = (string)config["Tender_Name"];
                        IFSTender.Duration = (int)config["Duration"];
                        IFSTender.Raised_By = (string)config["Raised_By"];
                        IFSTender.Name = (string)config["Name"];
                        IFSTender.Staff_Id = (string)config["Staff_Id"];
                        IFSTender.Designation = (string)config["Designation"];
                        IFSTender.Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"];
                        IFSTender.Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"];
                        IFSTender.Min_Required_No_of_Members = (int)config["Min_Required_No_of_Members"];
                        IFSTender.Actual_No_of_Committee_Memb = (int)config["Actual_No_of_Committee_Memb"];
                        IFSTender.Approval_Status = (string)config["Approval_Status"];
                        IFSTender.Created_By = (string)config["Raised_By"];
                        IFSTender.Created_Date = (string)config["Created_Date"];
                        IFSTender.Created_Time = (string)config["Created_Time"];
                    }
                }
                #region ProcurementCommitteeTypes    
                List<DropdownList> ProcurementCommitteeTypesList = new List<DropdownList>();
                string pageCommitteeTypes = "ProcurementCommitteeTypes?&$format=json";
                HttpWebResponse httpResponseCommitteeTypes = Credentials.GetOdataData(pageCommitteeTypes);
                using (var streamReader = new StreamReader(httpResponseCommitteeTypes.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Committee_Type"];
                        dropdownList.Value = (string)config["Committee_Type"];
                        ProcurementCommitteeTypesList.Add(dropdownList);
                    }
                }
                #endregion

                #region IfsCode    
                List<DropdownList> IfsCodeList = new List<DropdownList>();
                string pageIfsCode = "SourcingProcessList?$format=json";
                HttpWebResponse httpResponseIfsCode = Credentials.GetOdataData(pageIfsCode);
                using (var streamReader = new StreamReader(httpResponseIfsCode.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"];
                        dropdownList.Value = (string)config["Code"];
                        IfsCodeList.Add(dropdownList);
                    }
                }
                #endregion

                #region Dim1    
                List<DropdownList> Dim1List = new List<DropdownList>();
                string pageDim1 = "DimensionValueList?$filter= Dimension_Code eq 'GEOGRAPHICAL'&$format=json";
                HttpWebResponse httpResponseDim1 = Credentials.GetOdataData(pageDim1);
                using (var streamReader = new StreamReader(httpResponseDim1.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        Dim1List.Add(dropdownList);
                    }
                }
                #endregion

                #region Dim2    
                List<DropdownList> Dim2List = new List<DropdownList>();
                string pageDim2 = "DimensionValueList?$filter= Dimension_Code eq 'ADMINISTRATIVE'&$format=json";
                HttpWebResponse httpResponseDim2 = Credentials.GetOdataData(pageDim2);
                using (var streamReader = new StreamReader(httpResponseDim2.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        Dim2List.Add(dropdownList);
                    }
                }
                #endregion

                IFSTender.ListOfCommitteeTypes = ProcurementCommitteeTypesList.Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Text,
                              Value = x.Value
                          }).ToList();

                IFSTender.ListOfIFSCodes = IfsCodeList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.Text,
                            Value = x.Value
                        }).ToList();


                IFSTender.ListOfDim1 = Dim1List.Select(x =>
                       new SelectListItem()
                       {
                           Text = x.Text,
                           Value = x.Value
                       }).ToList();

                IFSTender.ListOfDim2 = Dim2List.Select(x =>
                       new SelectListItem()
                       {
                           Text = x.Text,
                           Value = x.Value
                       }).ToList();

                return View(IFSTender);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult updateIFSTenderCommitteeHeader(IFSTenderCommitteeCard updatedHeader)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            bool result = false;
            var Document_No = updatedHeader.Document_No;
            var IFS_Code = updatedHeader.IFS_Code;
            var Committee_Type = updatedHeader.Committee_Type;
            var Description = updatedHeader.Description;
            var Tender_Name = updatedHeader.Tender_Name;
            var appointmentEffective_Date = DateTime.ParseExact(updatedHeader.Appointment_Effective_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
            var Appointing_Authority = updatedHeader.Appointing_Authority;
            var Duration = updatedHeader.Duration;

            // result = Credentials.ObjNav.UpdateTenderCommitees(
            //     Document_No,
            //     IFS_Code,
            //     Committee_Type,
            //     Description,
            //     Tender_Name,
            //     appointmentEffective_Date,
            //     Appointing_Authority,
            //     Duration,
            // );



            if (result)
            {
                string redirectUrl = "/Purchase/IFSTenderCommittee?DocNo=" + updatedHeader.Document_No;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public ActionResult NewIFSTenderCommitteeLine(string docNo, string commiteeType)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                IFSTenderCommitteeLines tenderCommittee = new IFSTenderCommitteeLines();
                Session["httpResponse"] = null;
                EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
                var geolocation = employeeView.GlobalDimension1Code;
                var adminUnit = employeeView.GlobalDimension2Code;
                tenderCommittee.Document_No = docNo;
                tenderCommittee.Committee_Type = commiteeType;

                #region memberNo

                List<DropdownList> memberNo = new List<DropdownList>();
                string pagememberNo = $"PoolCommitteeMembers?$filter=Global_Dimension_1_Code eq '{geolocation}' and Global_Dimension_2_Code eq '{adminUnit}' and Status eq 'Approved'&$format=json";

                HttpWebResponse httpResponseMemberNo = Credentials.GetOdataData(pagememberNo);
                using (var streamReader = new StreamReader(httpResponseMemberNo.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Member_No"] + "-" + (string)config["Member_Name"];
                        dropdownList.Value = (string)config["Member_No"];
                        memberNo.Add(dropdownList);
                    }
                }

                #endregion


                tenderCommittee.ListOfmemberNo = memberNo.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                return PartialView("~/Views/Purchase/PartialViews/NewIFSTenderCommitteeLine.cshtml",
                    tenderCommittee);

            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public ActionResult NewIFSTenderCommittee(string Code, string Committee_Type)
    {
        try
        {

            IFSTenderCommitteeCard tenderCommittee = new IFSTenderCommitteeCard();
            Session["httpResponse"] = null;

            tenderCommittee.IFS_Code = Code;


            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            tenderCommittee.Created_By = employeeView.UserID;
            tenderCommittee.Global_Dimension_1_Code = employeeView.GlobalDimension1Code;
            List<DropdownList> committeeTypesList = new List<DropdownList>();
            string pageCommitteeTypes = "ProcurementCommitteeTypes?&$format=json";
            HttpWebResponse httpResponseCT = Credentials.GetOdataData(pageCommitteeTypes);
            using (var streamReader = new StreamReader(httpResponseCT.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Committee_Type"];
                    dropdownList.Value = (string)config["Committee_Type"];
                    committeeTypesList.Add(dropdownList);
                }
            }



            IFSTenderCommitteeCard IFSCodes = new IFSTenderCommitteeCard();
            Session["httpResponse"] = null;
            tenderCommittee.Raised_By = employeeView.UserID;
            tenderCommittee.Location = DimensinValuesList.GetDimensionValueName(employeeView.GlobalDimension1Code);
            tenderCommittee.AdminUnit = DimensinValuesList.GetDimensionValueName(employeeView.AdministrativeUnit);

            tenderCommittee.Name = employeeView.FirstName + " " + employeeView.LastName;
            tenderCommittee.Staff_Id = employeeView.No;
            List<DropdownList> IFSCodesList = new List<DropdownList>();
            string pageIFSCodes = $"RequestForQuotationCard?$filter=Global_Dimension_1_Code eq '{employeeView.GlobalDimension1Code}'&$format=json";
            HttpWebResponse httpResponseIFSCodes = Credentials.GetOdataData(pageIFSCodes);
            using (var streamReader = new StreamReader(httpResponseIFSCodes.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"];
                    dropdownList.Value = (string)config["Code"];
                    IFSCodesList.Add(dropdownList);
                }
            }


            tenderCommittee.ListOfCommitteeTypes = committeeTypesList.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();


            tenderCommittee.ListOfIFSCodes = IFSCodesList.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();



            return PartialView("~/Views/Purchase/PartialViews/NewIFSTenderCommittee.cshtml", tenderCommittee);

        }

        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public JsonResult GetMemberDetails(string Member_No)
    {
        try
        {
            string memberName = "";

            #region MemebersLookup

            string pageMembers = $"Resources?$filter=No eq '{Member_No}'&$format=json";

            HttpWebResponse httpResponseWorkPlan = Credentials.GetOdataData(pageMembers);
            using (var streamReader = new StreamReader(httpResponseWorkPlan.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    memberName = (string)config["Name"];

                }
            }

            #endregion

            // Create and return the JSON result
            var response = new
            {
                MemberName = memberName,
                success = true
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    public JsonResult DeleteTenderCommitteeMember(string DocNo, string LnNo, string committeeType)
    {
        try
        {
            bool result = false;
            /*result = Credentials.ObjNav.DeleteTenderCommiteeMembers(DocNo, Convert.ToInt32(LnNo), committeeType);*/

            if (result)
            {
                string redirectUrl = "/Purchase/IFSTEnderCommittee?DocNo=" + DocNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                string redirectUrl = "/Purchase/IFSTEnderCommittee?DocNo=" + DocNo;
                return Json(new { message = "Error deleting record. try again", success = false }, JsonRequestBehavior.AllowGet);

            }


        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }





    //pool Committee
    public ActionResult PoolCommittee()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }

    }
    public PartialViewResult PoolCommitteeList()
    {

        try
        {
            List<PoolCommittee> PoolCommitteeList = new List<PoolCommittee>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;


            var commType = "POOL";
            var Appointing_Officer = FetchAppointingOfficer(commType);



            string page = "PoolCommittee?$orderby=Document_No desc&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PoolCommittee poolCommittee = new PoolCommittee
                    {
                        Document_No = (string)config["Document_No"],
                        Description = (string)config["Description"],
                        Document_Date = (string)config["Document_Date"],
                        Appointment_Effective_Date = (string)config["Appointment_Effective_Date"],
                        Appointing_Authority = Appointing_Officer,
                        Name = (string)config["Name"],
                        Staff_Id = (string)config["Staff_Id"],
                        Designation = (string)config["Designation"],
                        Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"],
                        Global_Dimension_1_Name = (string)config["Global_Dimension_1_Name"],
                        Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"],
                        Global_Dimension_2_Name = (string)config["Global_Dimension_2_Name"],
                        Status = (string)config["Status"],
                        Created_By = (string)config["Created_By"],
                        Created_Date = (string)config["Created_Date"],
                        Created_Time = (string)config["Created_Time"]

                    };
                    PoolCommitteeList.Add(poolCommittee);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/PoolCommitteeList.cshtml",
                PoolCommitteeList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public ActionResult NewPoolCommittee()
    {
        try
        {

            PoolCommittee poolCommittee = new PoolCommittee();

            Session["httpResponse"] = null;
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            poolCommittee.Created_By = employeeView.UserID;
            poolCommittee.Global_Dimension_1_Code = employeeView.GlobalDimension1Code;
            poolCommittee.Global_Dimension_2_Code = employeeView.GlobalDimension2Code;

            List<DropdownList> committeeTypesList = new List<DropdownList>();
            string pageCommitteeTypes = "ProcurementCommitteeTypes?&$format=json";
            HttpWebResponse httpResponseCT = Credentials.GetOdataData(pageCommitteeTypes);
            using (var streamReader = new StreamReader(httpResponseCT.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Committee_Type"];
                    dropdownList.Value = (string)config["Committee_Type"];
                    committeeTypesList.Add(dropdownList);
                }
            }

            poolCommittee.ListOfCommitteeTypes = committeeTypesList.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            return PartialView("~/Views/Purchase/PartialViews/NewPoolCommittee.cshtml", poolCommittee);

        }

        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public JsonResult GetAppointingOfficer(string commType)
    {
        try
        {
            var Appointing_Officer = "";
            string pagePoolComm = $"ProcurementCommitteeTypes?filter=Committee_Type eq '{commType}'&$format=json";
            HttpWebResponse httpResponsePoolComm = Credentials.GetOdataData(pagePoolComm);

            using (var streamReader = new StreamReader(httpResponsePoolComm.GetResponseStream()))
            {
                var results = streamReader.ReadToEnd();
                var details = JObject.Parse(results);

                foreach (JObject config in details["value"])
                {
                    Appointing_Officer = (string)config["Title_of_Appointing_Officer"];
                }
            }

            var response = new
            {
                Appointing_Officer = Appointing_Officer
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { Message = ex.Message.Replace("'", ""), Success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public string FetchAppointingOfficer(string commType)
    {
        var Appointing_Officer = "";
        string pagePoolComm = $"ProcurementCommitteeTypes?filter=Committee_Type eq '{commType}'&$format=json";
        HttpWebResponse httpResponsePoolComm = Credentials.GetOdataData(pagePoolComm);

        using (var streamReader = new StreamReader(httpResponsePoolComm.GetResponseStream()))
        {
            var results = streamReader.ReadToEnd();
            var details = JObject.Parse(results);

            foreach (JObject config in details["value"])
            {
                Appointing_Officer = (string)config["Title_of_Appointing_Officer"];
            }
        }

        return Appointing_Officer;
    }

    [HttpPost]
    public JsonResult SubmitPoolCommittee(PoolCommittee newPoolCommittee)
    {
        try
        {
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;

            string DocNo = "";
            /* DocNo = Credentials.ObjNav.fnInsertPoolCommittee(employee.UserID, newPoolCommittee.Description, DateTime.ParseExact(newPoolCommittee.Appointment_Effective_Date.Replace("-", "/"), "dd/MM/yyyy",
                 CultureInfo.InvariantCulture));*/

            if (DocNo != "")
            {
                string redirectUrl = "/Purchase/PoolCommitteeDocumentView?DocNo=" + DocNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Record Not created. Please try again later...", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    public PartialViewResult PoolCommitteeLines(string DocNo, string status)
    {

        try
        {

            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            var geolocation = employee.GlobalDimension1Code;
            var adminUnit = employee.GlobalDimension2Code;
            List<PoolCommitteeLines> PoolCommitteeLines = new List<PoolCommitteeLines>();
            /*  string pageLine = $"PoolCommitteeMembers?$filter=Document_No eq '" + DocNo + "'&$format=json";*/
            string pageLine = $"PoolCommitteeMembers?$filter=Document_No eq '{DocNo}' and Global_Dimension_1_Code eq '{geolocation}' and Global_Dimension_2_Code eq '{adminUnit}'&$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PoolCommitteeLines PoolCommitteeLine = new PoolCommitteeLines
                    {
                        Document_No = (string)config["Document_No"],
                        Line_No = (int)config["Line_No"],
                        Role = (string)config["Role"],
                        Member_No = (string)config["Member_No"],
                        Member_Name = (string)config["Member_Name"],
                        Telephone_No = (string)config["Telephone_No"],
                        Member_Email = (string)config["Member_Email"],
                        Staff_No = (string)config["Staff_No"],
                        Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"],
                        Global_Dimension_2_Code = (string)config["Global_Dimension_2_CodeSSS"],
                    };
                    PoolCommitteeLines.Add(PoolCommitteeLine);
                }
            }
            ViewBag.Document_Status = status;
            return PartialView("~/Views/Purchase/PartialViews/PoolCommitteeLines.cshtml",
                PoolCommitteeLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public ActionResult PoolCommitteeDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                //string doc = EncryptionHelper.DecryptDocumentNo(DocNo);
                PoolCommittee PoolCommittee = new PoolCommittee();
                string page = "PoolCommittee?$filter=Document_No eq '" + DocNo + "'&format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        PoolCommittee.Document_No = (string)config["Document_No"];
                        PoolCommittee.Description = (string)config["Description"];
                        PoolCommittee.Document_Date = (string)config["Document_Date"];
                        PoolCommittee.Appointment_Effective_Date = (string)config["Appointment_Effective_Date"];
                        PoolCommittee.Appointing_Authority = (string)config["Appointing_Authority"];
                        PoolCommittee.Name = (string)config["Name"];
                        PoolCommittee.Staff_Id = (string)config["Staff_Id"];
                        PoolCommittee.Designation = (string)config["Designation"];
                        PoolCommittee.Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"];
                        PoolCommittee.Global_Dimension_1_Name = (string)config["Global_Dimension_1_Name"];
                        PoolCommittee.Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"];
                        PoolCommittee.Global_Dimension_2_Name = (string)config["Global_Dimension_2_Name"];
                        PoolCommittee.Status = (string)config["Status"];
                        PoolCommittee.Created_By = (string)config["Created_By"];
                        PoolCommittee.Created_Date = (string)config["Created_Date"];
                        PoolCommittee.Created_Time = (string)config["Created_Time"];


                    }
                }
                #region ProcurementCommitteeTypes    
                List<DropdownList> ProcurementCommitteeTypesList = new List<DropdownList>();
                string pageCommitteeTypes = "ProcurementCommitteeTypes?&$format=json";
                HttpWebResponse httpResponseCommitteeTypes = Credentials.GetOdataData(pageCommitteeTypes);
                using (var streamReader = new StreamReader(httpResponseCommitteeTypes.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Committee_Type"];
                        dropdownList.Value = (string)config["Committee_Type"];
                        ProcurementCommitteeTypesList.Add(dropdownList);
                    }
                }
                #endregion

                #region IfsCode    
                List<DropdownList> IfsCodeList = new List<DropdownList>();
                string pageIfsCode = "RecurringPurchaseLines?$format=json";
                HttpWebResponse httpResponseIfsCode = Credentials.GetOdataData(pageIfsCode);
                using (var streamReader = new StreamReader(httpResponseIfsCode.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        dropdownList.Value = (string)config["Code"];
                        IfsCodeList.Add(dropdownList);
                    }
                }
                #endregion

                #region Dim1    
                List<DropdownList> Dim1List = new List<DropdownList>();
                string pageDim1 = "DimensionValueList?$filter= Dimension_Code eq 'GEOGRAPHICAL'&$format=json";
                HttpWebResponse httpResponseDim1 = Credentials.GetOdataData(pageDim1);
                using (var streamReader = new StreamReader(httpResponseDim1.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        Dim1List.Add(dropdownList);
                    }
                }
                #endregion

                #region Dim2    
                List<DropdownList> Dim2List = new List<DropdownList>();
                string pageDim2 = "DimensionValueList?$filter= Dimension_Code eq 'ADMINISTRATIVE'&$format=json";
                HttpWebResponse httpResponseDim2 = Credentials.GetOdataData(pageDim2);
                using (var streamReader = new StreamReader(httpResponseDim2.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        Dim2List.Add(dropdownList);
                    }
                }
                #endregion

                /* PoolCommittee.ListOfCommitteeTypes = ProcurementCommitteeTypesList.Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Text,
                               Value = x.Value
                           }).ToList();

                 PoolCommittee.ListOfIFSCodes = IfsCodeList.Select(x =>
                         new SelectListItem()
                         {
                             Text = x.Text,
                             Value = x.Value
                         }).ToList();


                 PoolCommittee.ListOfDim1 = Dim1List.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.Text,
                            Value = x.Value
                        }).ToList();

                 PoolCommittee.ListOfDim2 = Dim2List.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.Text,
                            Value = x.Value
                        }).ToList();*/

                return View(PoolCommittee);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }
    public ActionResult NewPoolCommitteeLine(string docNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                PoolCommitteeLines poolCommittee = new PoolCommitteeLines();
                Session["httpResponse"] = null;
                EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
                poolCommittee.Document_No = docNo;
                //poolCommittee.Committee_Type = commiteeType;



                #region memberNo

                List<DropdownList> memberNo = new List<DropdownList>();
                string pagememberNo = $"Resources?$filter=Type eq 'Person'&$format=json";

                HttpWebResponse httpResponseMemberNo = Credentials.GetOdataData(pagememberNo);
                using (var streamReader = new StreamReader(httpResponseMemberNo.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["No"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["No"];
                        memberNo.Add(dropdownList);
                    }
                }

                #endregion


                poolCommittee.ListOfmemberNo = memberNo.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                return PartialView("~/Views/Purchase/PartialViews/NewPoolCommitteeLine.cshtml",
                    poolCommittee);

            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public JsonResult SubmitPoolCommiteeMember(PoolCommitteeLines newMember)
    {
        try
        {

            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string docNo = newMember.Document_No;
            int role = int.Parse(newMember.Role);
            string staffNumber = newMember.Member_No;

            bool inserted = false;
           /* inserted = Credentials.ObjNav.fnInsertPoolCommitteeMember(docNo, role, staffNumber, employee.GlobalDimension1Code, employee.GlobalDimension2Code);*/

            if (inserted)
            {
                string msg = "Member successfully added";
                return Json(new { message = msg, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Member Not Inserted. Please try again later...", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    public JsonResult DeletePoolCommitteeMember(string DocNo, string LnNo)
    {
        try
        {
            bool result = false;
           /* result = Credentials.ObjNav.fnDeletePoolCommitteeMember(DocNo, Convert.ToInt32(LnNo));*/

            if (result)
            {
                string redirectUrl = "/Purchase/IFSTEnderCommittee?DocNo=" + DocNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                string redirectUrl = "/Purchase/IFSTEnderCommittee?DocNo=" + DocNo;
                return Json(new { message = "Error deleting record. try again", success = false }, JsonRequestBehavior.AllowGet);

            }


        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    public ActionResult SendPoolCommitteeApproval(string docNo)
    {
        try
        {

            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            string staffNo = Session["Username"].ToString();
            string userId = employeeView.UserID;

            bool result = false;
            /*result=Credentials.ObjNav.SendPoolCommiteeForApproval(docNo);*/

            if (result)
            {
                Credentials.ObjNav.UpdateApprovalEntrySenderID(50162, docNo, userId);
                return Json(new { message = "Document successfully sent for approval .", success = true },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Failed to send document for approval.", success = false },
                JsonRequestBehavior.AllowGet);
            }


        }
        catch (Exception ex)
        {

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }
    public ActionResult CancelPoolCommitteeApproval(string docNo)
    {
        try
        {

            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            bool result = false;
            /*result=Credentials.ObjNav.CancelPoolCommiteeForApproval(docNo);*/

            if (result)
            {
                return Json(new { message = "Document successfully sent for approval .", success = true },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Failed to send document for approval.", success = false },
                JsonRequestBehavior.AllowGet);
            }


        }
        catch (Exception ex)
        {

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult updatePoolCommitteeHeader(PoolCommittee updatedHeader)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            bool result = false;

            var Document_No = updatedHeader.Document_No;
            var Description = updatedHeader.Description;
            var appointmentEffective_Date = DateTime.ParseExact(updatedHeader.Appointment_Effective_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);

           /* result = Credentials.ObjNav.fnUpdatePoolCommittee(
                Document_No,
                Description,
                appointmentEffective_Date
            );*/

            if (result)
            {
                string redirectUrl = "/Purchase/IFSTenderCommittee?DocNo=" + updatedHeader.Document_No;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }





    //Framework Contract
    public ActionResult FrameworkContract()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult FrameworkContractList()
    {

        try
        {
            List<FrameworkContractList> FrameworkContractList = new List<FrameworkContractList>();

            string page = "FrameworkAgreementsList?$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    FrameworkContractList FrameworkContract = new FrameworkContractList
                    {
                        Code = (string)config["Code"],
                        Tender_Name = (string)config["Tender_Name"],
                        Status = (string)config["Status"],
                        PRN_No = (string)config["PRN_No"],
                        Awarded_Bidder_No = (string)config["Awarded_Bidder_No"],
                        Awarded_Bidder_Name = (string)config["Awarded_Bidder_Name"],
                        RFQ_Sent = (bool)config["RFQ_Sent"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Awarded_Quote_No = (string)config["Awarded_Quote_No"],
                        Awarded_Bidder_Sum = (int)config["Awarded_Bidder_Sum"],
                        External_Document_No = (string)config["External_Document_No"],
                        Tender_Summary = (string)config["Tender_Summary"],
                        Procurement_Category_ID = (string)config["Procurement_Category_ID"],

                    };
                    FrameworkContractList.Add(FrameworkContract);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/FrameworkContractList.cshtml", FrameworkContractList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }

    }

    public ActionResult FrameworkContractDocumentView(string Code)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                FrameworkContractCard FrameworkContract = new FrameworkContractCard();
                string page = "FrameworkRequestCard?$filter=Code eq '" + Code + "'&format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        FrameworkContract.Code = (string)config["Code"];
                        FrameworkContract.External_Document_No = (string)config["External_Document_No"];
                        FrameworkContract.PRN_No = (string)config["PRN_No"];
                        FrameworkContract.Responsibility_Center = (string)config["Responsibility_Center"];
                        FrameworkContract.Location_Code = (string)config["Location_Code"];
                        FrameworkContract.Tender_Name = (string)config["Tender_Name"];
                        FrameworkContract.Tender_Summary = (string)config["Tender_Summary"];
                        FrameworkContract.Procurement_Type = (string)config["Procurement_Type"];
                        FrameworkContract.Status = (string)config["Status"];
                        FrameworkContract.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                        FrameworkContract.Procurement_Category_ID = (string)config["Procurement_Category_ID"];
                        FrameworkContract.Procurement_Category = (string)config["Procurement_Category"];
                        FrameworkContract.Solicitation_Type = (string)config["Solicitation_Type"];
                        FrameworkContract.Procurement_Method = (string)config["Procurement_Method"];
                        FrameworkContract.Purchaser_Code = (string)config["Purchaser_Code2"];
                        FrameworkContract.Language_Code = (string)config["Language_Code"];
                        FrameworkContract.Created_Date_Time = (string)config["Created_Date_Time"];
                        FrameworkContract.Created_by = (string)config["Created_by"];
                        FrameworkContract.Framework_Agreement_No = (string)config["Framework_Agreement_No"];
                        FrameworkContract.Awarded_Quote_No = (string)config["Awarded_Quote_No"];
                        FrameworkContract.Awarded_Bidder_No = (string)config["Awarded_Bidder_No"];
                        FrameworkContract.Awarded_Bidder_Name = (string)config["Awarded_Bidder_Name"];
                        FrameworkContract.Awarded_Bidder_Sum = (int)config["Awarded_Bidder_Sum"];
                        FrameworkContract.Requesting_Region = (string)config["Requesting_Region"];
                        FrameworkContract.Requesting_Directorate = (string)config["Requesting_Directorate"];
                        FrameworkContract.Requesting_Department = (string)config["Requesting_Department"];
                        FrameworkContract.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                        FrameworkContract.Procurement_Plan_Entry_No = (int)config["Procurement_Plan_Entry_No"];
                        FrameworkContract.Job = (string)config["Job"];
                        FrameworkContract.Job_Task_No = (string)config["Job_Task_No"];
                        FrameworkContract.PP_Planning_Category = (string)config["PP_Planning_Category"];
                        FrameworkContract.PP_Funding_Source_ID = (string)config["PP_Funding_Source_ID"];
                        FrameworkContract.PP_Total_Budget = (int)config["PP_Total_Budget"];
                        FrameworkContract.PP_Total_Actual_Costs = (int)config["PP_Total_Actual_Costs"];
                        FrameworkContract.PP_Total_Commitments = (int)config["PP_Total_Commitments"];
                        FrameworkContract.PP_Total_Available_Budget = (int)config["PP_Total_Available_Budget"];
                        FrameworkContract.PP_Preference_Reservation_Code =
                            (string)config["PP_Preference_Reservation_Code"];
                        FrameworkContract.Financial_Year_Code = (string)config["Financial_Year_Code"];
                        FrameworkContract.Project_ID = (string)config["Project_ID"];

                    }
                }

                return View(FrameworkContract);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult FrameworkContractLines()
    {
        try
        {
            List<RequestForQuotationsLines> FrameworkContractLines = new List<RequestForQuotationsLines>();
            string Standard_Purchase_Code = "FITT0002";
            string page =
                $"RequestForQuotationLines?$filter=Standard_Purchase_Code eq '{Standard_Purchase_Code}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    RequestForQuotationsLines FrameworkContractLine = new RequestForQuotationsLines
                    {
                        Standard_Purchase_Code = (string)config["Standard_Purchase_Code"],
                        Line_No = (int)config["Line_No"],
                        Type = (string)config["Type"],
                        FilteredTypeField = (string)config["FilteredTypeField"],
                        No = (string)config["No"],
                        Description = (string)config["Description"],
                        Variant_Code = (string)config["Variant_Code"],
                        Quantity = (int)config["Quantity"],
                        Unit_of_Measure_Code = (string)config["Unit_of_Measure_Code"],
                        Amount_Excl_VAT = (int)config["Amount_Excl_VAT"],
                        Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                        Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"],
                        ShortcutDimCode_x005B_3_x005D_ = (string)config["ShortcutDimCode_x005B_3_x005D_"],
                        ShortcutDimCode_x005B_4_x005D_ = (string)config["ShortcutDimCode_x005B_4_x005D_"],
                        ShortcutDimCode_x005B_5_x005D_ = (string)config["ShortcutDimCode_x005B_5_x005D_"],
                        ShortcutDimCode_x005B_6_x005D_ = (string)config["ShortcutDimCode_x005B_6_x005D_"],
                        ShortcutDimCode_x005B_7_x005D_ = (string)config["ShortcutDimCode_x005B_7_x005D_"],
                        Item_Category = (string)config["Item_Category"],
                        ShortcutDimCode_x005B_8_x005D_ = (string)config["ShortcutDimCode_x005B_8_x005D_"],
                    };
                    FrameworkContractLines.Add(FrameworkContractLine);
                }
            }

            FrameworkContractLinesList Lines = new FrameworkContractLinesList
            {
                ListOfFrameworkContractLines = FrameworkContractLines
            };

            return PartialView("~/Views/Purchase/PartialViews/FrameworkContractLines.cshtml", Lines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public ActionResult NewFrameworkContract()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                string Dim1 = "", Dim2 = "";
                FrameworkContractCard newFrameworkContract = new FrameworkContractCard();
                Session["httpResponse"] = null;

                #region ResponsibilityCenters

                List<DropdownList> responsibilityCenters = new List<DropdownList>();
                string pageRS = "ResponsibilityCenters?$format=json";

                HttpWebResponse httpResponseRS = Credentials.GetOdataData(pageRS);
                using (var streamReader = new StreamReader(httpResponseRS.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList rs = new DropdownList();
                        rs.Text = (string)config["Name"];
                        rs.Value = (string)config["Code"];
                        responsibilityCenters.Add(rs);

                    }
                }

                #endregion

                #region Locations

                List<DropdownList> locationCodes = new List<DropdownList>();
                string pageLC = "LocationCodes?$format=json";

                HttpWebResponse httpResponseLC = Credentials.GetOdataData(pageLC);
                using (var streamReader = new StreamReader(httpResponseLC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList lc = new DropdownList();
                        lc.Text = (string)config["Code"] + " - " + (string)config["Name"];
                        lc.Value = (string)config["Code"];
                        locationCodes.Add(lc);

                    }
                }

                #endregion

                #region ProcurementTypes

                List<DropdownList> procurementTypes = new List<DropdownList>();
                string pagePT = "ProcurementTypes?$format=json";

                HttpWebResponse httpResponsePT = Credentials.GetOdataData(pagePT);
                using (var streamReader = new StreamReader(httpResponsePT.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList lc = new DropdownList();
                        lc.Text = (string)config["Description"];
                        lc.Value = (string)config["Code"];
                        procurementTypes.Add(lc);

                    }
                }

                #endregion

                #region procurementCategory

                List<DropdownList> procurementCategory = new List<DropdownList>();
                string pagePC = "ProcurementCategories?$format=json";

                HttpWebResponse httpResponsePC = Credentials.GetOdataData(pagePC);
                using (var streamReader = new StreamReader(httpResponsePC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList lc = new DropdownList();
                        lc.Text = (string)config["Description"];
                        lc.Value = (string)config["Code"];
                        procurementCategory.Add(lc);

                    }
                }

                #endregion

                #region purchasers

                List<DropdownList> purchaserCodes = new List<DropdownList>();
                string pagePurchC = "Purchasers?$format=json";

                HttpWebResponse httpResponsePurchC = Credentials.GetOdataData(pagePurchC);
                using (var streamReader = new StreamReader(httpResponsePurchC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList lc = new DropdownList();
                        lc.Text = (string)config["Code"] + " - " + (string)config["Name"];
                        lc.Value = (string)config["Code"];
                        purchaserCodes.Add(lc);

                    }
                }

                #endregion

                #region Languages

                List<DropdownList> languages = new List<DropdownList>();
                string pageLang = "Languages?$format=json";

                HttpWebResponse httpResponseLang = Credentials.GetOdataData(pageLang);
                using (var streamReader = new StreamReader(httpResponseLang.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList lc = new DropdownList();
                        lc.Text = (string)config["Name"] + " (" + (string)config["Code"] + ")";
                        lc.Value = (string)config["Code"];
                        languages.Add(lc);

                    }
                }

                #endregion

                newFrameworkContract.ListOfResponsibilityCentres = responsibilityCenters.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                newFrameworkContract.ListOfLocationCodes = locationCodes.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                newFrameworkContract.ListOfProcurementTypes = procurementTypes.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                newFrameworkContract.ListOfProcurementCategories = procurementCategory.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                newFrameworkContract.ListOfPurchaserCodes = purchaserCodes.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();

                newFrameworkContract.ListOfLanguageCodes = languages.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                return View("~/Views/Purchase/PartialViews/NewFrameworkContract.cshtml", newFrameworkContract);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult SubmitFrameworkContract(FrameworkContractCard newFrameworkContract)
    {
        try
        {
            // Retrieve employee details from the session
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;

            // Extract and validate the incoming data from the FrameworkContractCard
            string responsibilityCenter = newFrameworkContract.Responsibility_Center ?? "";
            string locationCode = newFrameworkContract.Location_Code ?? "";
            string tenderName = newFrameworkContract.Tender_Name ?? "";
            string tenderSummary = newFrameworkContract.Tender_Summary ?? "";
            string procurementType = newFrameworkContract.Procurement_Type ?? "";
            string requisitionProductGroup = newFrameworkContract.Requisition_Product_Group ?? "";
            string procurementCategory = newFrameworkContract.Procurement_Category ?? "";
            string purchaserCode = newFrameworkContract.Purchaser_Code ?? "";
            string languageCode = newFrameworkContract.Language_Code ?? "";


            string docNo = "";
            /* docNo = Credentials.ObjNav.InsertFrameworkContract(
                 tenderName,
                 tenderSummary,
                 procurementType,
                 requisitionProductGroup,
                 procurementCategory,
                 purchaserCode,
                 languageCode,
                 responsibilityCenter,
                 locationCode,
                 responsibleEmployeeNo,
                 userID
             );
   */
            if (!string.IsNullOrEmpty(docNo))
            {
                string redirectUrl = "/Purchase/FrameworkContractDocumentView?No=" + docNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    //Direct procurements

    public ActionResult DirectProcurements()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult DirectProcurementsList()
    {
        try
        {
            List<DirectProcurementsList> DirectProcurementsList = new List<DirectProcurementsList>();

            string page = "DirectProcurement?$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DirectProcurementsList DPList = new DirectProcurementsList
                    {
                        Code = (string)config["Code"],
                        Tender_Name = (string)config["Tender_Name"],
                        Status = (string)config["Status"],
                        PRN_No = (string)config["PRN_No"],
                        Awarded_Bidder_No = (string)config["Awarded_Bidder_No"],
                        Awarded_Bidder_Name = (string)config["Awarded_Bidder_Name"],
                        RFQ_Sent = (bool)config["RFQ_Sent"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Awarded_Quote_No = (string)config["Awarded_Quote_No"],
                        Awarded_Bidder_Sum = (int)config["Awarded_Bidder_Sum"],
                        External_Document_No = (string)config["External_Document_No"],
                        Tender_Summary = (string)config["Tender_Summary"],
                        Procurement_Category_ID = (string)config["Procurement_Category_ID"],
                    };
                    DirectProcurementsList.Add(DPList);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/DirectProcurementsList.cshtml",
                DirectProcurementsList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public ActionResult DirectProcurementsDocumentView(string Code)
    {
        return View();
    }

    [CustomAuthorization(Role = "PROCUREMENT")]
    public ActionResult AssignedPurchaseRequisition()
    {
        try
        {
            if (Session["Username"] == null || Session["Username"].ToString() == "")
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult AssignedPurchaseRequisitionList()
    {
        try
        {
            string page = "";
            string userId = Session["UserId"].ToString();
            List<ApprovedPurchaseRequisition> purchaseRequisitions = new List<ApprovedPurchaseRequisition>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string staffNo = employee.No;
            ESSRoleSetup role = Session["ESSRoleSetup"] as ESSRoleSetup;

            page = "PurchaseRequisitions?$filter=Purchaser_Code2 eq '" + staffNo +
                   "' and Status eq 'Released' and Ordered_PRN eq false and Document_Type eq 'Purchase Requisition' &$format=json";
            //page = $"AssignedProcurementRequistion?$filter=Requester_ID eq '{userId}'&$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ApprovedPurchaseRequisition purchase = new ApprovedPurchaseRequisition();

                    purchase.DocumentType = (string)config["Document_Type"];
                    purchase.No = (string)config["No_"];
                    purchase.PRNType = (string)config["PRN_Type"];
                    purchase.DocumentDate = DateTime.Parse(config["Document_Date"].ToString());
                    purchase.LocationCode = (string)config["Location_Code"];
                    purchase.RequisitionNo = (string)config["Requisition_No"];
                    purchase.RequisitionProductGroup = (string)config["Requisition_Product_Group"];
                    purchase.RequesterID = (string)config["Requester_ID"];
                    purchase.RequestByNo = (string)config["Request_By_No_"];
                    purchase.RequestByName = (string)config["Request_By_Name"];
                    purchase.ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"];
                    purchase.ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"];
                    purchase.AssignedOfficer = (string)config["Assigned_Officer"];
                    purchase.AssignedUserID = (string)config["Assigned_User_ID"];
                    purchase.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    purchase.PurchaserCode2 = (string)config["Purchaser_Code2"];
                    purchase.PPPlanningCategory = (string)config["PP_Planning_Category"];
                    purchase.Description = (string)config["Description"];
                    purchase.PPTotalBudget = Convert.ToDecimal(config["PP_Total_Budget"]);
                    purchase.PPSolicitationType = (string)config["PP_Solicitation_Type"];
                    purchase.OtherProcurementMethods = (string)config["Other_Procurement_Methods"];
                    purchase.PPInvitationNoticeType = (string)config["PP__Invitation_Notice_Type"];
                    purchase.PPPreferenceReservationCode = (string)config["PP_Preference_Reservation_Code"];
                    purchase.PPTotalActualCosts = Convert.ToDecimal(config["PP_Total_Actual_Costs"]);
                    purchase.PRNConversionProcedure = (string)config["PRN_Conversion_Procedure"];
                    purchase.Status = (string)config["Status"];
                    purchase.LinkedIFSNo = (string)config["Linked_IFS_No_"];
                    purchase.LinkedLPONo = (string)config["Linked_LPO_No_"];
                    purchase.ConsolidateToIFSNo = (string)config["Consolidate_to_IFS_No_"];
                    purchase.OrderedPRN = Convert.ToBoolean(config["Ordered_PRN"]);
                    purchaseRequisitions.Add(purchase);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/AssignedPurchaseRequisitionList.cshtml",
                purchaseRequisitions.OrderByDescending(x => x.No));
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    [HttpPost]
    public ActionResult AssignedPurchaseRequisitionDocument(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                ApprovedPurchaseRequisition purchase = new ApprovedPurchaseRequisition();
                /*
                                string page2 = "PurchaseRequisitions?$filter=No_ eq '" + DocNo + "'&format=json";*/
                string page = "PurchaseRequisition?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {

                        purchase.DocumentType = (string)config["Document_Type"];
                        purchase.No = (string)config["No"];
                        purchase.PRNType = (string)config["PRN_Type"];
                        purchase.DocumentDate = DateTime.Parse(config["Document_Date"].ToString());
                        purchase.LocationCode = (string)config["Location_Code"];
                        purchase.RequisitionNo = (string)config["Requisition_No"];
                        purchase.RequisitionProductGroup = (string)config["Requisition_Product_Group"];
                        purchase.RequesterID = (string)config["Requester_ID"];
                        purchase.RequestByNo = (string)config["Request_By_No_"];
                        purchase.RequestByName = (string)config["Request_By_Name"];
                        purchase.ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"];
                        purchase.ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"];
                        purchase.AssignedOfficer = (string)config["Assigned_Officer"];
                        purchase.AssignedUserID = (string)config["Assigned_User_ID"];
                        purchase.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                        purchase.PurchaserCode2 = (string)config["Purchaser_Code2"];
                        purchase.PPPlanningCategory = (string)config["PP_Planning_Category"];
                        purchase.Description = (string)config["Description"];
                        purchase.PPTotalBudget = Convert.ToDecimal(config["PP_Total_Budget"]);
                        purchase.PPSolicitationType = (string)config["PP_Solicitation_Type"];
                        purchase.PPProcurementMethod = (string)config["PP_Procurement_Method"];
                        purchase.OtherProcurementMethods = (string)config["Other_Procurement_Methods"]; //this
                        purchase.PPInvitationNoticeType = (string)config["PP_Invitation_Notice_Type"]; //this
                        purchase.PPPreferenceReservationCode = (string)config["PP_Preference_Reservation_Code"];
                        purchase.PPTotalActualCosts = Convert.ToDecimal(config["PP_Total_Actual_Costs"]);
                        purchase.PRNConversionProcedure = (string)config["PRN_Conversion_Procedure"];
                        purchase.Status = (string)config["Status"];
                        purchase.LinkedIFSNo = (string)config["Linked_IFS_No_"];
                        purchase.LinkedLPONo = (string)config["Linked_LPO_No_"];
                        purchase.ConsolidateToIFSNo = (string)config["Consolidate_to_IFS_No_"];
                        purchase.OrderedPRN = Convert.ToBoolean(config["Ordered_PRN"]);
                    }


                }

                #region vendorCategories

                List<DropdownList> vendorCategories = new List<DropdownList>();
                string pageVC = $"VendorCategory?&$format=json";

                HttpWebResponse httpResponseVC = Credentials.GetOdataData(pageVC);
                using (var streamReader = new StreamReader(httpResponseVC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Description"];
                        list.Value = (string)config["Code"];
                        vendorCategories.Add(list);

                    }

                }

                #endregion

                #region SolicitationTypes

                List<DropdownList> solicitationTypes = new List<DropdownList>();
                string pageST = $"SolicitationType?&$format=json";

                HttpWebResponse httpResponseST = Credentials.GetOdataData(pageST);
                using (var streamReader = new StreamReader(httpResponseST.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        list.Value = (string)config["Code"];
                        solicitationTypes.Add(list);

                    }

                }

                #endregion

                #region IFSNos

                List<DropdownList> IFS = new List<DropdownList>();
                string pageIF = $"SolicitationType?&$format=json";

                HttpWebResponse httpResponseIF = Credentials.GetOdataData(pageIF);
                using (var streamReader = new StreamReader(httpResponseIF.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList list = new DropdownList();
                        list.Text = (string)config["Code"] + "-" + (string)config["Description"];
                        list.Value = (string)config["Code"];
                        solicitationTypes.Add(list);

                    }

                }

                #endregion

                purchase.ListOfSolisitationTypes = solicitationTypes.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                purchase.ListOfPPPreferenceReservationCodes = vendorCategories.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                purchase.ListOfIFSNos = IFS.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Text,
                        Value = x.Value
                    }).ToList();
                return View(purchase);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult AssignedPurchaseReqLine(string docNo, string Status)
    {
        try
        {
            List<ApprovedPRNLine> approvedPRNLines = new List<ApprovedPRNLine>();
            string pageLine = $"ApprovedPRNLines?$filter=Document_No eq '{docNo}'&$format=json";
            //string pageLine = $"ExpensePRNLine?$filter=Document_No eq '{docNo}' &$format=json";

            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ApprovedPRNLine approvedPRN = new ApprovedPRNLine();

                    approvedPRN.DocumentNo = (string)config["Document_No"];
                    approvedPRN.LineNo = Convert.ToInt32(config["Line_No"]);
                    approvedPRN.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    approvedPRN.ProcurementPlanEntryNo = Convert.ToInt32(config["Procurement_Plan_Entry_No"]);
                    approvedPRN.PPSolicitationType = (string)config["PP_Solicitation_Type"];
                    approvedPRN.PPProcurementMethod = (string)config["PP_Procurement_Method"];
                    approvedPRN.PPPreferenceReservationCode = (string)config["PP_Preference_Reservation_Code"];
                    approvedPRN.Description = (string)config["Description"];
                    approvedPRN.UnitOfMeasureCode = (string)config["Unit_of_Measure_Code"];
                    approvedPRN.Quantity = Convert.ToInt32(config["Quantity"]);
                    approvedPRN.DirectUnitCost = Convert.ToDecimal(config["Direct_Unit_Cost"]);
                    approvedPRN.Amount = Convert.ToDecimal(config["Amount"]);
                    approvedPRN.AmountIncludingVAT = Convert.ToDecimal(config["Amount_Including_VAT"]);
                    approvedPRN.CurrencyCode = (string)config["Currency_Code"];
                    approvedPRN.LocationCode = (string)config["Location_Code"];
                    approvedPRN.UnitCostLCY = Convert.ToDecimal(config["Unit_Cost_LCY"]);



                    approvedPRNLines.Add(approvedPRN);
                }
            }

            ApprovedPRNLineList Lines = new ApprovedPRNLineList
            {
                Status = Status,
                ListOfApprovedPRNLine = approvedPRNLines
            };

            return PartialView("~/Views/Purchase/PartialViews/ApprovedPRNLine.cshtml", Lines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public ActionResult PublishedIfs()
    {
        return View("~Views/Purchase/PublishedIfs.cshtml");
    }

    public JsonResult GetPurchasers()
    {
        try
        {
            List<DropdownList> purchasers = new List<DropdownList>();
            string pageWPA = $"Purchasers?$format=json";
            HttpWebResponse httpResponseWPA = Credentials.GetOdataData(pageWPA);

            using (var streamReader = new StreamReader(httpResponseWPA.GetResponseStream()))
            {
                var results = streamReader.ReadToEnd();
                var details = JObject.Parse(results);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList
                    {
                        Text = (string)config["Name"],
                        Value = (string)config["Code"]
                    };
                    purchasers.Add(dropdownList);
                }
            }

            var response = new
            {
                ListOfPurchasers = purchasers.Select(x => new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList(),
                Success = true
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { Message = ex.Message.Replace("'", ""), Success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult AssignProcurentOfficer(string docNo, string purchaserNumber)
    {
        bool successVal = false;
        try
        {

            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string assigned = "";
            string StaffNo = Session["Username"].ToString();
            Credentials.ObjNav.AssignProcurementOfficer(docNo, purchaserNumber);
            string page = "PurchaseRequisitions?$filter=No_ eq '" + docNo + "'&format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {


                    assigned = (string)config["Purchaser_Code2"];


                }
            }

            return NotifyOfficer(assigned);

        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult NotifyOfficer(string officerNumber)
    {
        bool successVal = false;
        try
        {

            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string StaffNo = Session["Username"].ToString();
            bool notified = Credentials.ObjNav.NotifySupplyChain(officerNumber);

            if (notified)
            {
                return Json(new { message = "Notification sent successfully.", success = true },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Failed to send notification.", success = false },
                JsonRequestBehavior.AllowGet);


        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult UpdateProcurementDocument(string docNo, string purchaserNumber, string solicitationType,
        int procurementMethod, int invitationNoticeType, string preferenceReservationCode)
    {
        bool successVal = false;
        try
        {
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string StaffNo = Session["Username"].ToString();

            // Assuming your UpdateDocument method can take these parameters
            Credentials.ObjNav.ModifyPurchaseHeader(docNo, solicitationType, invitationNoticeType,
                procurementMethod, preferenceReservationCode, "");
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            if (!successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult CreateInvitationNotice(string docNo)
    {
        bool successVal = false;
        try
        {
            string UserID = Session["UserID"].ToString();

            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            Credentials.ObjNav.CreateInvitationNotice(docNo, false, UserID);
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);

        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult UpdateBidDetails(RequestForQuotationCard quotationCard)
    {
        bool successVal = false;
        try
        {
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string StaffNo = Session["Username"].ToString();

            //submission end date
            DateTime subendDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(quotationCard.Submission_End_Date))
            {
                subendDate = DateTime.ParseExact(quotationCard.Submission_End_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture); //okay
            }

            //submission end time
            DateTime sendTime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(quotationCard.Submission_End_Time))
            {
                DateTime.TryParse(quotationCard.Submission_End_Time, out sendTime); //okay
            }

            //Bid opening date
            DateTime bidOpDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(quotationCard.Bid_Opening_Date))
            {
                bidOpDate = DateTime.ParseExact(quotationCard.Bid_Opening_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
            }


            //Bid opening time
            DateTime bidopTime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(quotationCard.Bid_Opening_Time))
            {
                DateTime.TryParse(quotationCard.Bid_Opening_Time, out bidopTime);
            }


            var Tender_Box_Location_Code = quotationCard.Tender_Box_Location_Code ?? "N/A";

            /*//PreBid meeting date
            DateTime prebidMeetingDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(quotationCard.PreBid_Meeting_Date))
            {
                prebidMeetingDate = DateTime.ParseExact(quotationCard.PreBid_Meeting_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
            }*/


            //prebid meeting time
            DateTime prebidMeetingTime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(quotationCard.PreBid_Meeting_Time))
            {
                DateTime.TryParse(quotationCard.PreBid_Meeting_Time, out prebidMeetingTime);
            }


            var PrebidVenue = quotationCard.PreBid_Meeting_Venue ?? "N/A";

            var Bid_Opening_Committe = quotationCard.Bid_Opening_Committe ?? "";
            var Bid_Evaluation_Committe = quotationCard.Bid_Evaluation_Committe ?? "";

            // Assuming your UpdateDocument method can take these parameters
            /*Credentials.ObjNav.ModifyRFQ(
                 quotationCard.Code,
                 subendDate,
                 sendTime,
                 quotationCard.Tender_Validity_Duration,
                 quotationCard.Bid_Scoring_Template,
                 Bid_Opening_Committe,
                 Bid_Evaluation_Committe,
                 bidOpDate,
                 bidopTime,
                quotationCard.Description,
                quotationCard.Procurement_Type,
                int.Parse(quotationCard.Target_Bidder_Group),
                int.Parse(quotationCard.Bid_Submission_Method),
                quotationCard.Sealed_Bids,
                quotationCard.Tender_Validity_Duration,
                quotationCard.Bid_Opening_Venue,
               Tender_Box_Location_Code,
                quotationCard.PreBid_Meeting_Required,
                PrebidVenue,
                DateTime.ParseExact(quotationCard.PreBid_Meeting_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture),
                prebidMeetingTime,
                 quotationCard.Specification,
                 int.Parse(quotationCard.Requires_Technical_Evaluation)
            );*/

            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            if (!successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult AddBider(string docNo, string vendorNo, string category, string description)
    {
        try
        {
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (description == null)
            {
                description = "";
            }

           /* Credentials.ObjNav.CreateInvitedBidders(docNo, vendorNo, category, description);*/

            return Json(new { message = "Bidder added successfully.", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult DeleteInvitedBidder(string Code, string Vendor_No)
    {
        try
        {
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            bool result = false;
           /* result = Credentials.ObjNav.DeleteInvitedBidders(Code, Vendor_No);*/
            if (result)
            {
                return Json(new { message = "Bidder Deleted successfully.", success = true },
                               JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Failed Deleting bidder. Try again.", success = false },
                               JsonRequestBehavior.AllowGet);
            }

        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult GetVendors(string Category)
    {
        try
        {
            List<DropdownList> vendors = new List<DropdownList>();
            string pageWPA = $"ProcurementVendors?$filter=Supplier_Category eq '{Category}'&$format=json";
            HttpWebResponse httpResponseWPA = Credentials.GetOdataData(pageWPA);

            using (var streamReaders = new StreamReader(httpResponseWPA.GetResponseStream()))
            {
                var results = streamReaders.ReadToEnd();
                var detailss = JObject.Parse(results);

                foreach (JObject config1 in detailss["value"])
                {
                    DropdownList dropdownList = new DropdownList
                    {
                        Text = $"{(string)config1["Name"]}({(string)config1["No"]})",
                        Value = (string)config1["No"]
                    };
                    vendors.Add(dropdownList);
                }
            }

            // Create and return the JSON result
            var response = new
            {
                ListOfVendors = vendors.Select(x => new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList(),
                success = true
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult GetItems(string pPlanId, string glAccount)
    {
        try
        {
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string geoLoc = employee.GlobalDimension1Code;
            string adminUnit = employee.AdministrativeUnit;
            string item = "";
            string entryNo = "";
            decimal unitCost = 0;
            string page =
                $"ProcurementPlanEntry?$filter=ProcurementPlanID eq '{pPlanId}' and GlobalDimension1Code eq '{geoLoc}' and GlobalDimension2Code eq '{adminUnit}' and Budget_Account eq '{glAccount}' &$format=json";

            HttpWebResponse httpResponseWPA = Credentials.GetOdataData(page);

            using (var streamReaders = new StreamReader(httpResponseWPA.GetResponseStream()))
            {
                var results = streamReaders.ReadToEnd();
                var detailss = JObject.Parse(results);

                foreach (JObject config1 in detailss["value"])
                {
                    item = (string)config1["Description"];
                    entryNo = (string)config1["EntryNo"];
                    unitCost = (decimal)config1["UnitCost"];

                }
            }

            var response = new
            {
                Item = item,
                EntryNo = entryNo,
                UnitCost = unitCost
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult GetAppointingAuthority(string ifsCode)
    {
        try
        {
            string Appointing_Officer = "";
            string page = $"ProcurementCommitteeTypes?$format=json";

            HttpWebResponse httpResponseWPA = Credentials.GetOdataData(page);

            using (var streamReaders = new StreamReader(httpResponseWPA.GetResponseStream()))
            {
                var results = streamReaders.ReadToEnd();
                var detailss = JObject.Parse(results);

                foreach (JObject config1 in detailss["value"])
                {
                    Appointing_Officer = (string)config1["Title_of_Appointing_Officer"];
                }
            }

            var response = new
            {
                Appointing_Authority = Appointing_Officer,

            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult SubmitAppointmentRequest(IFSTenderCommitteeCard appointmentRequest)
    {
        try
        {
            DateTime appointmentEffectiveDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(appointmentRequest.Appointment_Effective_Date))
            {
                DateTime.TryParse(appointmentRequest.Appointment_Effective_Date, out appointmentEffectiveDate);
            }

            string committeeType = appointmentRequest.Committee_Type ?? string.Empty;
            string ifsCode = appointmentRequest.IFS_Code ?? string.Empty;
            string description = appointmentRequest.Description;
            string appointingAuthority = appointmentRequest.Appointing_Authority ?? string.Empty;
            decimal duration = 0;
            if (appointmentRequest.Duration != null)
            {
                duration = (int)appointmentRequest.Duration;
            }

            // Skipping fields like DocumentDate, Description, TenderName, Name, Location, AdminUnit as they are not used in InsertTenderCommitees
            // Add them if needed for further logic

            string staffNo = Session["Username"].ToString();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string globalDimension1Code = employee?.GlobalDimension1Code ?? string.Empty;
            string userId = employee.UserID;
            string redirectUrl = "";
            string docNo = "";
            /*docNo = Credentials.ObjNav.InsertTenderCommitees(
                ifsCode,
                committeeType,
                description,
                appointmentEffectiveDate,
                duration,
                userId
            );
*/
            if (!string.IsNullOrEmpty(docNo))
            {
                if (committeeType == "INSPECTION")
                {
                    redirectUrl = "/Purchase/InspectionCommitteeDocumentView?DocNo=" + docNo;
                }
                else
                {
                    redirectUrl = "/Purchase/IFSTenderCommitteeDocumentView?DocNo=" + docNo;
                }

                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Document not created. Please try again later...", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult SubmitCommiteeMember(IFSTenderCommitteeLines newMember)
    {
        try
        {


            string docNo = newMember.Document_No;
            string commiteeType = newMember.Committee_Type;
            int role = newMember.MRole;
            string staffNumber = newMember.Member_No;

            bool inserted = Credentials.ObjNav.InsertTenderCommiteeMembers(docNo, commiteeType, role, staffNumber);

            if (inserted)
            {
                string redirectUrl = "/Purchase/IFSTenderCommitteeDocumentView?DocNo=" + docNo;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Member Not Inserted. Please try again later...", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult SendRFQForApproval(string DocNo)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            string staffNo = Session["Username"].ToString();
            string msg = "";
            bool valSucc = false;
            string userId = employeeView.UserID;
            int bidders = 0;

            string page = "RecurringPuchaseLines?$filter=Code eq '" + DocNo + "'&format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    bidders++;

                }
            }

            if (bidders < 3)
            {
                msg = "You must input at least 3 bidders. The Request for Quotation Requisition, Document No: " + DocNo +
                      " was not sent for approval.";
                valSucc = false;
            }
            else
            {
                Credentials.ObjNav.SendRFQforApproval(DocNo);
                Credentials.ObjNav.UpdateApprovalEntrySenderID(99210, DocNo, userId);

                msg = "Purchase Requisition, Document No: " + DocNo + " sent for approval successfully.";
                valSucc = true;
            }


            return Json(new { message = msg, success = valSucc }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult CancelRFQForApproval(string DocNo)
    {
        try
        {
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string empNo = employee.No;
            Credentials.ObjNav.CancellRFQApproval(DocNo);
            return Json(new { message = "Purchase Requisition approval cancelled Successfully", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }



    public JsonResult DeletePRNExpense(string DocNo, int lineNo)
    {
        try
        {
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            string empNo = employee.No;
            Credentials.ObjNav.DeletePRNExpense(DocNo, lineNo);
            return Json(new { message = "PRN Line Deleted Successfully", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult SendIFSApproval(string docNo)
    {
        try
        {
            // Check session
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // Get user data from session
            var employee = Session["EmployeeData"] as EmployeeView;
            var staffNo = Session["Username"].ToString();

            // Variables to track roles
            int members = 0, sec = 0, chair = 0;
            string message = "";

            // Prepare OData request
            string page = $"IFSTenderCommitteeLines?$filter=Document_No eq '{docNo}'&$format=json";
            HttpWebResponse response = Credentials.GetOdataData(page);

            // Parse the response
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (var token in details["value"])
                {
                    var role = (string)token["Role"];
                    if (role == "Member") members++;
                    else if (role == "Secretary") sec++;
                    else if (role == "Chairperson") chair++;
                }
            }

            // Validation logic
            if (members + sec + chair < 4)
            {
                message =
                    "The total number of members, including the secretary and chairperson, must be at least 4.";
            }
            else if (members < 2)
            {
                message = "You must have at least 2 members.";
            }
            else if (sec == 0)
            {
                message = "The Committee must a secretary.";
            }
            else if (chair == 0)
            {
                message = "The Committee must have a chairperson.";
            }
            else
            {
                // Send for approval and update the approval entry
                Credentials.ObjNav.SendIFSTEnderCommiteeForApproval(docNo);
                Credentials.ObjNav.UpdateApprovalEntrySenderID(70141, docNo, employee.UserID);

                message = $"{docNo} sent for approval.";
                return Json(new { message, success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message, success = false }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult CancelIFSApproval(string docNo)
    {
        bool successVal = false;
        try
        {

            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string StaffNo = Session["Username"].ToString();
            Credentials.ObjNav.CancellIFSTEnderCommiteeForApproval(docNo);


            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);

        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult NotifyCommitteeMembers(string docNo)
    {
        bool successVal = false;
        try
        {
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string StaffNo = Session["Username"].ToString();
            Credentials.ObjNav.FnSendTenderCommitteNotification(docNo);


            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);

        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult SuggestedApprovedPRNLine(string DocNo)
    {
        try
        {
            List<ApprovedPRNLine> approvedPRNLines = new List<ApprovedPRNLine>();
            string pageLine = $"ApprovedPRNLines?$filter=Document_No eq '{DocNo}'&$format=json";
            //string pageLine = $"ExpensePRNLine?$filter=Document_No eq '{docNo}' &$format=json";

            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ApprovedPRNLine approvedPRN = new ApprovedPRNLine();

                    approvedPRN.DocumentNo = (string)config["Document_No"];
                    approvedPRN.LineNo = Convert.ToInt32(config["Line_No"]);
                    approvedPRN.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    approvedPRN.ProcurementPlanEntryNo = Convert.ToInt32(config["Procurement_Plan_Entry_No"]);
                    approvedPRN.PPSolicitationType = (string)config["PP_Solicitation_Type"];
                    approvedPRN.PPProcurementMethod = (string)config["PP_Procurement_Method"];
                    approvedPRN.OtherProcurementMethods = (string)config["Other_Procurement_Methods"];
                    approvedPRN.PPPreferenceReservationCode = (string)config["PP_Preference_Reservation_Code"];
                    approvedPRN.Description = (string)config["Description"];
                    approvedPRN.UnitOfMeasureCode = (string)config["Unit_of_Measure_Code"];
                    approvedPRN.Quantity = Convert.ToInt32(config["Quantity"]);
                    approvedPRN.DirectUnitCost = Convert.ToDecimal(config["Direct_Unit_Cost"]);
                    approvedPRN.Amount = Convert.ToDecimal(config["Amount"]);
                    approvedPRN.AmountIncludingVAT = Convert.ToDecimal(config["Amount_Including_VAT"]);
                    approvedPRN.CurrencyCode = (string)config["Currency_Code"];
                    approvedPRN.LocationCode = (string)config["Location_Code"];
                    approvedPRN.UnitCostLCY = Convert.ToDecimal(config["Unit_Cost_LCY"]);
                    approvedPRN.ContractNoToPay = (string)config["Contract_No_to_pay"];
                    approvedPRN.DocNo = DocNo;
                    approvedPRN.LPO_Created = (bool)config["LPO_Created"];
                    approvedPRN.Line_Selected = (bool)config["Line_Selected"];
                    approvedPRN.Awarded_Quantity = (decimal)config["Awarded_Quantity"];
                    approvedPRN.Awarded_Unit_Cost = (decimal)config["Awarded_Unit_Cost"];
                    approvedPRN.Awarded_Line_Amount = (decimal)config["Awarded_Line_Amount"];
                    approvedPRN.Women_Percent = (int)config["Women_Percent"];
                    approvedPRN.Women_Amount = (int)config["Women_Amount"];
                    approvedPRN.Youth_Percent = (int)config["Youth_Percent"];
                    approvedPRN.Youth_Amount = (int)config["Youth_Amount"];
                    approvedPRN.AGPO_Percent = (int)config["AGPO_Percent"];
                    approvedPRN.AGPO_Amount = (int)config["AGPO_Amount"];
                    approvedPRN.PWD_Percent = (int)config["PWD_Percent"];
                    approvedPRN.PWD_Amount = (int)config["PWD_Amount"];

                    if (approvedPRN.LPO_Created)
                    {
                        approvedPRN.Line_Selected = true;
                    }

                    approvedPRNLines.Add(approvedPRN);
                    ViewBag.PRN = DocNo;
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/SuggestedApprovedPRNLine.cshtml", approvedPRNLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult EditApprovedPRNLine(string DocNo, int LineNo)
    {

        try
        {
            List<SelectListItem> ContractDropdown = ContractToPayToDropDown();
            var ApprovedPRNLine = new ApprovedPRNLine
            {
                No = DocNo,
                LineNo = LineNo,

            };

            ViewBag.ContractDropdownList = ContractDropdown;


            return PartialView("~/Views/Purchase/PartialViews/EditApprovedPRNLine.cshtml", ApprovedPRNLine);
        }
        catch (Exception ex)
        {
            var erroMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult EditAssingedPRNLines(string DocNo, int LineNo, decimal AwardQnty, decimal AwardUntCst, decimal AwardLineAmount)
    {

        try
        {
            var AssingedPRNLines = new ApprovedPRNLineViewModel
            {

                DocNo = DocNo,
                LineNo = LineNo,
                AwardQnty = AwardQnty,
                AwardUntCst = AwardUntCst,
                AwardLineAmount = AwardLineAmount

            };



            return PartialView("~/Views/Purchase/PartialViews/EditAssingedPRNLines.cshtml", AssingedPRNLines);
        }
        catch (Exception ex)
        {
            var erroMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public ActionResult UpdateAwardedAmount(string DocNo, int LineNo, decimal AwardQnty, decimal AwardUntCst,
        decimal AwardLineAmount)
    {
        try
        {
            bool res = false;
            /*res=Credentials.ObjNav.FnUpdateAwardedAmount(DocNo, LineNo, AwardQnty, AwardUntCst);*/
            return Json(new { message = "Awarded Amounts updated  successfully.", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public List<SelectListItem> ContractToPayToDropDown()
    {
        List<SelectListItem> contractDropdown = new List<SelectListItem>();
        string page = $"PurchaseList?$filter=Document_Type eq 'Blanket Order'&$format=json";

        try
        {
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);

            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);


            foreach (JObject config in details["value"])
            {
                SelectListItem dropdownItem = new SelectListItem
                {
                    Text = (string)config["No"] + " - " + (string)config["Buy_from_Vendor_Name"],
                    Value = (string)config["No"]
                };
                contractDropdown.Add(dropdownItem);
            }
        }
        catch (Exception ex)
        {

            throw;
        }

        return contractDropdown;
    }

    public JsonResult UpdateContract(string DocNo, int LineNo, string ContractNo)
    {
        try
        {

            var employee = Session["EmployeeData"] as EmployeeView;
            if (employee == null)
            {
                return Json(new { message = "Session expired. Please log in again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }

            string empNo = employee.No;
            Credentials.ObjNav.AddContractNo(ContractNo, LineNo, DocNo);
            return Json(new { message = "Contract added successfully.", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult ContractPaymentDetailsLines(string PRN, int LineLineNo, string ContractToPay)
    {
        try
        {
            List<ContractPaymentDetailsLines> contractPaymentDetails = new List<ContractPaymentDetailsLines>();
            string pageLine =
                $"ContractPaymentDetailsLines?$filter=Document_No eq '{PRN}' and Document_No ne ''&$format=json";

            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    ContractPaymentDetailsLines paymentDetails = new ContractPaymentDetailsLines
                    {
                        Document_No = (string)config["Document_No"],
                        Line_No = Convert.ToInt32(config["Line_No"]),
                        Contract_No = (string)config["Contract_No"],
                        Item_Line_No = Convert.ToInt32(config["Item_Line_No"]),
                        Item_No = (string)config["Item_No"],
                        Description = (string)config["Description"],
                        Quantity = Convert.ToInt32(config["Quantity"]),
                        Unit_Price = Convert.ToInt32(config["Unit_Price"]),
                        Amount = Convert.ToInt32(config["Amount"]),
                        LineLineNo = LineLineNo,
                    };

                    contractPaymentDetails.Add(paymentDetails);
                }
            }

            ViewBag.LineLineNo = LineLineNo;
            ViewBag.PRN = PRN;
            ViewBag.ContractNoToPay = ContractToPay;

            return PartialView("~/Views/Purchase/PartialViews/ContractPaymentDetailsLines.cshtml",
                contractPaymentDetails);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error
            {
                Message = ex.Message
            };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public PartialViewResult NewContractPaymentDetailsLines(string ContractNoToPay, int LineLineNo, string PRN)
    {
        try
        {
            var ContractPaymentDetailsLines = new ContractPaymentDetailsLines
            {
                Document_No = PRN,
                LineLineNo = LineLineNo,
                Contract_No = ContractNoToPay,


            };

            ViewBag.ItemsDropDownList = ItemsDropDownList(ContractNoToPay);
            ;
            ViewBag.Document_No = PRN;
            ViewBag.LineLineNo = LineLineNo;
            ViewBag.Contract_No = ContractNoToPay;


            return PartialView("~/Views/Purchase/PartialViews/NewContractPaymentDetailsLines.cshtml",
                ContractPaymentDetailsLines);
        }
        catch (Exception ex)
        {
            var erroMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public List<SelectListItem> ItemsDropDownList(string ContractToPayToNo)
    {
        List<SelectListItem> ItemsDropDown = new List<SelectListItem>();
        string page =
            $"PurchaseLines?$filter=Document_No eq '{ContractToPayToNo}' and Document_Type eq 'Blanket Order'&$format=json";

        try
        {
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);

            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);


            foreach (JObject config in details["value"])
            {
                SelectListItem dropdownItem = new SelectListItem
                {
                    Text = (string)config["No"] + " - " + (string)config["Shortcut_Dimension_1_Code"] + " - " +
                           (string)config["Description"] + " - " + (string)config["Quantity"] + " - " +
                           (string)config["Line_Amount"],
                    Value = (string)config["Line_No"]
                };
                ItemsDropDown.Add(dropdownItem);
            }
        }
        catch (Exception ex)
        {

            throw;
        }

        return ItemsDropDown;
    }

    public JsonResult SubmitContractPaymentDetails(string DocNo, int LineNo, string ContractNo)
    {
        try
        {

            var employee = Session["EmployeeData"] as EmployeeView;
            if (employee == null)
            {
                return Json(new { message = "Session expired. Please log in again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }

            string empNo = employee.No;
            Credentials.ObjNav.AddContractLines(ContractNo, DocNo, LineNo);
            return Json(new { message = "Contract payment details added successfully.", success = true },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult SubmitCreateContractPO(string DocNo, string Contract_No)
    {
        bool successVal = false;
        string userId = Session["UserId"]?.ToString();
        try
        {

            string dcNo = Credentials.ObjNav.CreateLPODoc(Contract_No, DocNo, userId);
            return Json(new { message = "Success.", success = true, docNo = dcNo }, JsonRequestBehavior.AllowGet);

        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult CreateDirectLPO(string DocNo)
    {
        bool successVal = false;
        string userId = Session["UserId"]?.ToString();
        try
        {
            string PoDocNo = Credentials.ObjNav.createLPODirectly(DocNo, userId);
            return Json(new { message = "Success.", success = true, DocNo = PoDocNo },
                JsonRequestBehavior.AllowGet);

        }
        catch (Exception ex)
        {
            if (successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult PurchaseOrdersList(string stage)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.Stage = stage;
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult PurchaseOrdersPartialViewList(string stage)
    {
        try
        {
            string userId = Session["UserId"]?.ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            if (employeeView == null || string.IsNullOrEmpty(userId))
            {
                return PartialView("~/Views/Shared/Error.cshtml",
                    new Error { Message = "Session data is missing." });
            }

            List<PurchaseOrder> purchaseOrdersList = new List<PurchaseOrder>();
            string empNo = employeeView.No;

            var geographical = employeeView?.GlobalDimension1Code;
            string page = "";

            if (stage == "scm")
            {
                page = $"PurchaseOrdersList?$filter=Document_Type eq 'Order' and Assigned_User_ID eq  '{userId}' and Document_Review_Status eq 'SCM'&$format=json";
            }
            else if (stage == "accl")
            {
                page = $"PurchaseOrdersList?$filter=Document_Type eq 'Order' and Shortcut_Dimension_1_Code  eq  '{geographical}' and Document_Review_Status eq 'Accounts'&$format=json";
            }
            else
            {
                return PartialView("~/Views/Shared/Error.cshtml",
                    new Error { Message = "This Page doesn't Exist, Check Your Permissions and Login." });
            }

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    var purchaseOrder = new PurchaseOrder
                    {
                        Document_Type = (string)config["Document_Type"],
                        No = (string)config["No"],
                        Buy_from_Vendor_No = (string)config["Buy_from_Vendor_No"],
                        Order_Address_Code = (string)config["Order_Address_Code"],
                        Buy_from_Vendor_Name = (string)config["Buy_from_Vendor_Name"],
                        Vendor_Order_No = (string)config["Vendor_Order_No"],
                        Buy_from_Post_Code = (string)config["Buy_from_Post_Code"],
                        Buy_from_Country_Region_Code = (string)config["Buy_from_Country_Region_Code"],
                        Buy_from_Contact = (string)config["Buy_from_Contact"],
                        Pay_to_Name = (string)config["Pay_to_Name"],
                        Pay_to_Post_Code = (string)config["Pay_to_Post_Code"],
                        Pay_to_Country_Region_Code = (string)config["Pay_to_Country_Region_Code"],
                        Pay_to_Contact = (string)config["Pay_to_Contact"],
                        Ship_to_Code = (string)config["Ship_to_Code"],
                        Ship_to_Name = (string)config["Ship_to_Name"],
                        Ship_to_Post_Code = (string)config["Ship_to_Post_Code"],
                        Ship_to_Country_Region_Code = (string)config["Ship_to_Country_Region_Code"],
                        Ship_to_Contact = (string)config["Ship_to_Contact"],
                        Posting_Date = (string)config["Posting_Date"],
                        Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                        Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"],
                        Location_Code = (string)config["Location_Code"],
                        Purchaser_Code = (string)config["Purchaser_Code"],
                        Assigned_User_ID = (string)config["Assigned_User_ID"],
                        Currency_Code = (string)config["Currency_Code"],
                        Document_Date = (string)config["Document_Date"],
                        Status = (string)config["Status"],
                        Payment_Terms_Code = (string)config["Payment_Terms_Code"],
                        Due_Date = (string)config["Due_Date"],
                        Payment_Discount_Percent = (int)(config["Payment_Discount_Percent"] ?? 0),
                        Payment_Method_Code = (string)config["Payment_Method_Code"],
                        Shipment_Method_Code = (string)config["Shipment_Method_Code"],
                        Requested_Receipt_Date = (string)config["Requested_Receipt_Date"],
                        Job_Queue_Status = (string)config["Job_Queue_Status"],
                        Amount = (string)config["Amount"],
                        Amount_Including_VAT = (string)config["Amount_Including_VAT"],
                        Posting_Description = (string)config["Posting_Description"],
                        isAccounts = stage == "accl",
                        Returned = (bool)config["Returned"],
                    };

                    purchaseOrdersList.Add(purchaseOrder);
                }
            }

            return PartialView("~/Views/Purchase/PurchaseOrdersPartialViewList.cshtml", purchaseOrdersList);
        }
        catch (Exception ex)
        {
            return PartialView("~/Views/Shared/Error.cshtml",
                new Error { Message = "An error occurred while processing your request." });
        }
    }

    public ActionResult PurchaseOrdersDocumentView(string DocNo)
    {

        if (Session["Username"] == null)
        {
            return RedirectToAction("Login", "Login");
        }

        try
        {
            string userId = Session["UserId"]?.ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            List<PurchaseOrder> purchaseOrdersList = new List<PurchaseOrder>();
            string empNo = employeeView.No;
            string page = $"PurchaseOrder?$filter=No eq '{DocNo}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    var purchaseOrder = new PurchaseOrder
                    {
                        Document_Type = (string)config["Document_Type"],
                        No = (string)config["No"],
                        Buy_from_Vendor_No = (string)config["Buy_from_Vendor_No"],
                        Order_Address_Code = (string)config["Order_Address_Code"],
                        Buy_from_Vendor_Name = (string)config["Buy_from_Vendor_Name"],
                        Vendor_Order_No = (string)config["Vendor_Order_No"],
                        Buy_from_Post_Code = (string)config["Buy_from_Post_Code"],
                        Buy_from_Country_Region_Code = (string)config["Buy_from_Country_Region_Code"],
                        Buy_from_Contact = (string)config["Buy_from_Contact"],
                        Pay_to_Name = (string)config["Pay_to_Name"],
                        Pay_to_Post_Code = (string)config["Pay_to_Post_Code"],
                        Pay_to_Country_Region_Code = (string)config["Pay_to_Country_Region_Code"],
                        Pay_to_Contact = (string)config["Pay_to_Contact"],
                        Ship_to_Code = (string)config["Ship_to_Code"],
                        Ship_to_Name = (string)config["Ship_to_Name"],
                        Ship_to_Post_Code = (string)config["Ship_to_Post_Code"],
                        Ship_to_Country_Region_Code = (string)config["Ship_to_Country_Region_Code"],
                        Ship_to_Contact = (string)config["Ship_to_Contact"],
                        Posting_Date = (string)config["Posting_Date"],
                        Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"],
                        Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"],
                        Location_Code = (string)config["Location_Code"],
                        Purchaser_Code = (string)config["Purchaser_Code"],
                        Assigned_User_ID = (string)config["Assigned_User_ID"],
                        Currency_Code = (string)config["Currency_Code"],
                        Document_Date = (string)config["Document_Date"],
                        Status = (string)config["Status"],
                        Payment_Terms_Code = (string)config["Payment_Terms_Code"],
                        Due_Date = (string)config["Due_Date"],
                        Payment_Discount_Percent = (int)(config["Payment_Discount_Percent"] ?? 0),
                        Payment_Method_Code = (string)config["Payment_Method_Code"],
                        Shipment_Method_Code = (string)config["Shipment_Method_Code"],
                        Requested_Receipt_Date = (string)config["Requested_Receipt_Date"],
                        Job_Queue_Status = (string)config["Job_Queue_Status"],
                        Requisition_No = (string)config["Requisition_No"],
                        Vendor_Invoice_No = (string)config["Vendor_Invoice_No"],
                        PRN_No = (string)config["Requisition_No"],
                        Document_Review_Status = (string)config["Document_Review_Status"],
                        Posting_Description = (string)config["Posting_Description"],
                        Accounts_Rejections_Reason = (string)config["Accounts_Rejections_Reasons"]
                    };

                    ViewBag.VendorDropdownList = ProcurementVendorsDownList();
                    purchaseOrdersList.Add(purchaseOrder);
                }
            }

            var purchases = purchaseOrdersList.FirstOrDefault();
            if (purchases == null)
            {
                return HttpNotFound("Purchase order not found");
            }

            return View(purchases);

        }
        catch (Exception ex)
        {

            return new HttpStatusCodeResult(500, "An error occurred while processing your request.");
        }
    }




    public ActionResult PurchaseOrderLines(string DocNo, string Status)
    {
        var StaffNo = Session["Username"].ToString();

        var purchaseOrderLines = new List<PurchaseOrderLines>();

        var PurchaseOrderLinePage = "PurchaseOrderLines?$filter=Document_No eq '" + DocNo + "'&$format=json";

        var httpResponse = Credentials.GetOdataData(PurchaseOrderLinePage);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                var purchaseOrderLine = new PurchaseOrderLines();

                purchaseOrderLine.Document_Type = (string)config["Document_Type"];
                purchaseOrderLine.Document_No = (string)config["Document_No"];
                purchaseOrderLine.Line_No = (int?)config["Line_No"] ?? 0;
                purchaseOrderLine.Type = (string)config["Type"];
                purchaseOrderLine.No = (string)config["No"];
                purchaseOrderLine.Item_Reference_No = (string)config["Item_Reference_No"];
                purchaseOrderLine.IC_Partner_Code = (string)config["IC_Partner_Code"];
                purchaseOrderLine.IC_Partner_Ref_Type = (string)config["IC_Partner_Ref_Type"];
                purchaseOrderLine.IC_Partner_Reference = (string)config["IC_Partner_Reference"];
                purchaseOrderLine.Variant_Code = (string)config["Variant_Code"];
                purchaseOrderLine.Nonstock = (bool?)config["Nonstock"] ?? false;
                purchaseOrderLine.VAT_Percent = (int?)config["VAT_Percent"] ?? 0;
                purchaseOrderLine.VAT_Prod_Posting_Group = (string)config["VAT_Prod_Posting_Group"];
                purchaseOrderLine.Description = (string)config["Description"];
                purchaseOrderLine.Description_2 = (string)config["Description_2"];
                purchaseOrderLine.Drop_Shipment = (bool?)config["Drop_Shipment"] ?? false;
                purchaseOrderLine.Return_Reason_Code = (string)config["Return_Reason_Code"];
                purchaseOrderLine.Location_Code = (string)config["Location_Code"];
                purchaseOrderLine.Bin_Code = (string)config["Bin_Code"];
                purchaseOrderLine.Quantity = (int?)config["Quantity"] ?? 0;
                purchaseOrderLine.Unit_of_Measure_Code = (string)config["Unit_of_Measure_Code"];
                purchaseOrderLine.Reserved_Quantity = (int?)config["Reserved_Quantity"] ?? 0;
                purchaseOrderLine.Job_Remaining_Qty = (int?)config["Job_Remaining_Qty"] ?? 0;
                purchaseOrderLine.Unit_of_Measure = (string)config["Unit_of_Measure"];
                purchaseOrderLine.Approved_Requisition_Amount = (int?)config["Approved_Requisition_Amount"] ?? 0;
                purchaseOrderLine.Approved_Unit_Cost = (int?)config["Approved_Unit_Cost"] ?? 0;
                purchaseOrderLine.Direct_Unit_Cost = (decimal?)config["Direct_Unit_Cost"] ?? 0;
                purchaseOrderLine.Indirect_Cost_Percent = (int?)config["Indirect_Cost_Percent"] ?? 0;
                purchaseOrderLine.Unit_Cost_LCY = (int)config["Unit_Cost_LCY"];
                purchaseOrderLine.Unit_Price_LCY = (int?)config["Unit_Price_LCY"] ?? 0;
                purchaseOrderLine.Line_Amount = (int?)config["Line_Amount"] ?? 0;
                purchaseOrderLine.Tax_Liable = (bool?)config["Tax_Liable"] ?? false;
                purchaseOrderLine.Tax_Area_Code = (string)config["Tax_Area_Code"];
                purchaseOrderLine.Tax_Group_Code = (string)config["Tax_Group_Code"];
                purchaseOrderLine.Use_Tax = (bool?)config["Use_Tax"] ?? false;
                purchaseOrderLine.Line_Discount_Percent = (int?)config["Line_Discount_Percent"] ?? 0;
                purchaseOrderLine.Line_Discount_Amount = (int?)config["Line_Discount_Amount"] ?? 0;
                purchaseOrderLine.Procurement_Plan = (string)config["Procurement_Plan"];
                purchaseOrderLine.Procurement_Plan_Item = (string)config["Procurement_Plan_Item"];
                purchaseOrderLine.Prepayment_Percent = (int?)config["Prepayment_Percent"] ?? 0;
                purchaseOrderLine.Prepmt_Line_Amount = (int?)config["Prepmt_Line_Amount"] ?? 0;
                purchaseOrderLine.Prepmt_Amt_Inv = (int?)config["Prepmt_Amt_Inv"] ?? 0;
                purchaseOrderLine.Allow_Invoice_Disc = (bool?)config["Allow_Invoice_Disc"] ?? false;
                purchaseOrderLine.Inv_Discount_Amount = (int?)config["Inv_Discount_Amount"] ?? 0;
                purchaseOrderLine.Inv_Disc_Amount_to_Invoice = (int?)config["Inv_Disc_Amount_to_Invoice"] ?? 0;
                purchaseOrderLine.Qty_to_Receive = (decimal?)config["Qty_to_Receive"] ?? 0;
                purchaseOrderLine.Quantity_Received = (decimal?)config["Quantity_Received"] ?? 0;
                purchaseOrderLine.Qty_to_Invoice = (decimal?)config["Qty_to_Invoice"] ?? 0;
                purchaseOrderLine.Quantity_Invoiced = (int?)config["Quantity_Invoiced"] ?? 0;
                purchaseOrderLine.Qty_Rcd_Not_Invoiced = (int?)config["Qty_Rcd_Not_Invoiced"] ?? 0;
                purchaseOrderLine.Prepmt_Amt_to_Deduct = (int?)config["Prepmt_Amt_to_Deduct"] ?? 0;
                purchaseOrderLine.Prepmt_Amt_Deducted = (int?)config["Prepmt_Amt_Deducted"] ?? 0;
                purchaseOrderLine.Allow_Item_Charge_Assignment =
                    (bool?)config["Allow_Item_Charge_Assignment"] ?? false;
                purchaseOrderLine.Qty_to_Assign = (int?)config["Qty_to_Assign"] ?? 0;
                purchaseOrderLine.Item_Charge_Qty_to_Handle = (int?)config["Item_Charge_Qty_to_Handle"] ?? 0;
                purchaseOrderLine.Qty_Assigned = (int?)config["Qty_Assigned"] ?? 0;
                purchaseOrderLine.Job_No = (string)config["Job_No"];
                purchaseOrderLine.Job_Task_No = (string)config["Job_Task_No"];
                purchaseOrderLine.Job_Planning_Line_No = (int?)config["Job_Planning_Line_No"] ?? 0;
                purchaseOrderLine.Job_Line_Type = (string)config["Job_Line_Type"];
                purchaseOrderLine.Job_Unit_Price = (int?)config["Job_Unit_Price"] ?? 0;
                purchaseOrderLine.Job_Line_Amount = (int?)config["Job_Line_Amount"] ?? 0;
                purchaseOrderLine.Job_Line_Discount_Amount = (int?)config["Job_Line_Discount_Amount"] ?? 0;
                purchaseOrderLine.Job_Line_Discount_Percent = (int?)config["Job_Line_Discount_Percent"] ?? 0;
                purchaseOrderLine.Job_Total_Price = (int?)config["Job_Total_Price"] ?? 0;
                purchaseOrderLine.Job_Unit_Price_LCY = (int?)config["Job_Unit_Price_LCY"] ?? 0;
                purchaseOrderLine.Job_Total_Price_LCY = (int?)config["Job_Total_Price_LCY"] ?? 0;
                purchaseOrderLine.Job_Line_Amount_LCY = (int?)config["Job_Line_Amount_LCY"] ?? 0;
                purchaseOrderLine.Job_Line_Disc_Amount_LCY = (int?)config["Job_Line_Disc_Amount_LCY"] ?? 0;
                purchaseOrderLine.Requested_Receipt_Date = (string)config["Requested_Receipt_Date"];
                purchaseOrderLine.Promised_Receipt_Date = (string)config["Promised_Receipt_Date"];
                purchaseOrderLine.Planned_Receipt_Date = (string)config["Planned_Receipt_Date"];
                purchaseOrderLine.Expected_Receipt_Date = (string)config["Expected_Receipt_Date"];
                purchaseOrderLine.Order_Date = (string)config["Order_Date"];
                purchaseOrderLine.Lead_Time_Calculation = (string)config["Lead_Time_Calculation"];
                purchaseOrderLine.Planning_Flexibility = (string)config["Planning_Flexibility"];
                purchaseOrderLine.Prod_Order_No = (string)config["Prod_Order_No"];
                purchaseOrderLine.Prod_Order_Line_No = (int?)config["Prod_Order_Line_No"] ?? 0;
                purchaseOrderLine.Operation_No = (string)config["Operation_No"];
                purchaseOrderLine.Work_Center_No = (string)config["Work_Center_No"];
                purchaseOrderLine.Finished = (bool?)config["Finished"] ?? false;
                purchaseOrderLine.Whse_Outstanding_Qty_Base = (int?)config["Whse_Outstanding_Qty_Base"] ?? 0;
                purchaseOrderLine.Inbound_Whse_Handling_Time = (string)config["Inbound_Whse_Handling_Time"];
                purchaseOrderLine.Blanket_Order_No = (string)config["Blanket_Order_No"];
                purchaseOrderLine.Blanket_Order_Line_No = (int?)config["Blanket_Order_Line_No"] ?? 0;
                purchaseOrderLine.Appl_to_Item_Entry = (int?)config["Appl_to_Item_Entry"] ?? 0;
                purchaseOrderLine.Deferral_Code = (string)config["Deferral_Code"];
                purchaseOrderLine.Depreciation_Book_Code = (string)config["Depreciation_Book_Code"];
                purchaseOrderLine.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                purchaseOrderLine.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                purchaseOrderLine.ShortcutDimCode3 = (string)config["ShortcutDimCode3"];
                purchaseOrderLine.ShortcutDimCode4 = (string)config["ShortcutDimCode4"];
                purchaseOrderLine.ShortcutDimCode5 = (string)config["ShortcutDimCode5"];
                purchaseOrderLine.ShortcutDimCode6 = (string)config["ShortcutDimCode6"];
                purchaseOrderLine.ShortcutDimCode7 = (string)config["ShortcutDimCode7"];
                purchaseOrderLine.ShortcutDimCode8 = (string)config["ShortcutDimCode8"];
                purchaseOrderLine.Gen_Bus_Posting_Group = (string)config["Gen_Bus_Posting_Group"];
                purchaseOrderLine.Gen_Prod_Posting_Group = (string)config["Gen_Prod_Posting_Group"];
                purchaseOrderLine.Over_Receipt_Quantity = (int?)config["Over_Receipt_Quantity"] ?? 0;
                purchaseOrderLine.Over_Receipt_Code = (string)config["Over_Receipt_Code"];
                purchaseOrderLine.Gross_Weight = (int?)config["Gross_Weight"] ?? 0;
                purchaseOrderLine.Net_Weight = (int?)config["Net_Weight"] ?? 0;
                purchaseOrderLine.Unit_Volume = (int?)config["Unit_Volume"] ?? 0;
                purchaseOrderLine.Units_per_Parcel = (int?)config["Units_per_Parcel"] ?? 0;
                purchaseOrderLine.FA_Posting_Date = (string)config["FA_Posting_Date"];
                purchaseOrderLine.AmountBeforeDiscount = (int?)config["AmountBeforeDiscount"] ?? 0;
                purchaseOrderLine.Invoice_Discount_Amount = (int?)config["Invoice_Discount_Amount"] ?? 0;
                purchaseOrderLine.Invoice_Disc_Pct = (int?)config["Invoice_Disc_Pct"] ?? 0;
                purchaseOrderLine.Total_Amount_Excl_VAT = (int?)config["Total_Amount_Excl_VAT"] ?? 0;
                purchaseOrderLine.Total_VAT_Amount = (int?)config["Total_VAT_Amount"] ?? 0;
                purchaseOrderLine.Total_Amount_Incl_VAT = (int?)config["Total_Amount_Incl_VAT"] ?? 0;
                purchaseOrderLine.Specification = (string)config["Specification"];
                purchaseOrderLine.DocStatus = Status;



                purchaseOrderLines.Add(purchaseOrderLine);
            }
        }


        ViewBag.DocumentStatus = Status;
        var purchaseOrderLineList = new PurchaseOrderLineList();
        purchaseOrderLineList.ListOfPurchaseOrderLine = purchaseOrderLines;
        return PartialView("~/Views/PurchaseOrder/PartialViews/PurchaseOrderLines.cshtml", purchaseOrderLines);
    }

    public bool CheckDocPostingGroup(string DocNo)
    {
        var PurchaseOrderLinePage = "PurchaseOrderLines?$filter=Document_No eq '" + DocNo + "'&$format=json";

        var httpResponse = Credentials.GetOdataData(PurchaseOrderLinePage);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            if (details["value"] != null && details["value"].HasValues)
            {
                foreach (JObject config in details["value"])
                {
                    var genBusPostingGroup = (string)config["Gen_Bus_Posting_Group"];
                    var genProdPostingGroup = (string)config["Gen_Prod_Posting_Group"];
                    if (!string.IsNullOrEmpty(genBusPostingGroup) && !string.IsNullOrEmpty(genProdPostingGroup))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    [HttpPost]
    public PartialViewResult UpdatePOLines(UpdatePurchaseOrderLineViewModel data)
    {
        try
        {
            var lineContent = new UpdatePurchaseOrderLineViewModel
            {
                Document_No = data.Document_No,
                Line_No = data.Line_No,
                Description = data.Description,
                Direct_Unit_Cost = data.Direct_Unit_Cost,
                Unit_Cost_LCY = data.Unit_Cost_LCY,
                Line_Amount = data.Line_Amount,
                Quantity = data.Quantity,
                Gen_Prod_Posting_Group = data.Gen_Prod_Posting_Group,
                Gen_Bus_Posting_Group = data.Gen_Bus_Posting_Group,
                Specifications = data.Specifications


            };
            ViewBag.BsPostingGroup = GenBusinessPostingGroupsDownList();
            ViewBag.ProductPostingGroup = GenProductPostingGroupsDownList();
            return PartialView("~/Views/Purchase/PartialViews/UpdatePOLines.cshtml", lineContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return PartialView("~/Views/Shared/Error.cshtml");
        }
    }



    public JsonResult DeletePOLine(string DocNo, int LineNo)
    {
        bool success;
        string message = "";
        try
        {
            var res = false;
            /*res = Credentials.ObjNav.FnDeleteLineswithZeroQuantities(LineNo, DocNo);*/
            if (res)
            {
                success = true;
                message = "Item Deleted Successfully";
            }
            else
            {
                success = false;
                message = "Failed to delete item.";
            }

            return Json(new { message = message, success = success }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult GenerateLpo(string DocNo)
    {
        try
        {
            string message = "";
            bool success = false, view = false;
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;


            //string LPO_Doc = Credentials.ObjNav.CreatePurchaseContract(DocNo, employee.UserID, 2);

            message = Credentials.ObjNav.GenerateLPOReport(DocNo);
            if (message == "")
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }

            return Json(new { message = message, success, view }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult CreateLPOFromContract(string DocNo, string ContractNo)
    {
        string userId = Session["UserId"]?.ToString();
        try
        {
            string LpoNo = Credentials.ObjNav.CreateLPOFromContract(DocNo, ContractNo, userId);
            return Json(new { message = "Success.", success = true, docNo = LpoNo }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult SendDocForApproval(string DocNo)
    {
        string userId = Session["UserId"]?.ToString();

        if (CheckDocPostingGroup(DocNo))
        {
            try
            {
                Credentials.ObjNav.sendLPOApproval(DocNo);
                Credentials.ObjNav.UpdateApprovalEntrySenderID(38, DocNo, userId);

                return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = $"Error: {ex.Message.Replace("'", "")}", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        else
        {
            return Json(new { message = "Update posting groups before sending PO for approval.", success = false },
                JsonRequestBehavior.AllowGet);
        }


    }

    public JsonResult CancelDocForApproval(string DocNo)
    {
        string userId = Session["UserId"]?.ToString();
        try
        {
            Credentials.ObjNav.fnCancelLPOApproval(DocNo);
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult CancelPO(string Reason, string DocNo)
    {
        string userId = Session["UserId"]?.ToString();
        try
        {
           /* Credentials.ObjNav.FnCancelLpo(Reason, DocNo);*/
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public List<SelectListItem> ProcurementVendorsDownList()
    {
        List<SelectListItem> ProcurementVendors = new List<SelectListItem>();
        string page = "ProcurementVendors?$filter=Vendor_Type eq 'Trade'";

        try
        {
            using HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            foreach (JObject config in details["value"])
            {
                SelectListItem dropdownItem = new SelectListItem
                {
                    Text = $"{config["No"]} - {config["Name"]}",
                    Value = (string)config["No"]
                };
                ProcurementVendors.Add(dropdownItem);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving Procurement Vendors.", ex);
        }

        return ProcurementVendors;
    }

    public ActionResult UpdatePurchaseOrderDocument(string DocNo, string VendorNo, string VendorInvoiceNo,
        string DueDate, string PostingDate, string status)
    {
        int option;

        switch (status)
        {
            case "Open":
                option = 0;
                break;
            case "Released":
                option = 1;
                break;
            case "Pending Approval":
                option = 2;
                break;
            case "Pending Prepayment":
                option = 3;
                break;
            default:
                option = -1;
                break;
        }

        bool successVal = false;
        try
        {
            if (Session["UserID"] == null || Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            DateTime DueDate1 = DateTime.Parse(DueDate);
            DateTime postingDate1 = DateTime.Parse(PostingDate);

            string invoice;
            if (VendorInvoiceNo == null)
            {
                invoice = "";
            }
            else
            {
                invoice = VendorInvoiceNo;
            }



            Credentials.ObjNav.FnUpdatePurchaseHeader(DocNo, VendorNo, invoice, DueDate1, postingDate1, option);
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            if (!successVal)
            {
                Session["ErrorMsg"] = ex.Message.Replace("'", "");
            }

            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult SendLPOToMail(string docNo)
    {
        try
        {
            bool res = Credentials.ObjNav.FnSendLPOViaMail(docNo);
            if (res)
            {
                return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(
                    new
                    {
                        message = "Mail not sent! Unknown error occurred. Please try again later.",
                        success = false
                    }, JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult PostLpoDoc(string DocNo)
    {
        try
        {
            string code = Credentials.ObjNav.PostPurchaseOrder(DocNo);
            return Json(new { message = "Success.", success = true, code }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult ReopenPO(string DocNo)
    {
        try
        {
            bool code = false;
           /* code=Credentials.ObjNav.FnReopenLPO(DocNo);*/
            return Json(new { message = "Success.", success = true, code }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult SendToAccountsForReview(string DocNo)
    {
        try
        {
            bool code = false; 
           /* code= Credentials.ObjNav.FnSendLPOToAccountsForReview(DocNo);*/
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult ReturnReturnToSCM(string DocNo, string Reason)
    {
        try
        {
            bool code = false;
             /*code=Credentials.ObjNav.FnSendBackToSupplyChain(DocNo, Reason);*/
            return Json(new { message = "Success.", success = code }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult PublishLpo(string DocNo)
    {
        try
        {
            Credentials.ObjNav.FnPublishLPO(DocNo);
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult SubmitUpdatePoLine(string DocNo, int Line_No, string Description, decimal Direct_Unit_Cost,
        int Quantity, string Gen_Bus_Posting_Group, string Gen_Prod_Posting_Group, decimal Qnty_To_Invoice, string specifications)
    {
        try
        {
            decimal qToReceive = Qnty_To_Invoice == 0 ? Quantity : Qnty_To_Invoice;

           /* Credentials.ObjNav.FnEditPurchaseLineDescription(DocNo, Line_No, Description, Direct_Unit_Cost,
                Quantity, Gen_Prod_Posting_Group, Gen_Bus_Posting_Group, qToReceive, specifications);*/

            return Json(new { message = "Purchase order line updated successfully.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {

            return Json(new { message = ex.Message.Replace("'", ""), success = false }, JsonRequestBehavior.AllowGet);
        }
    }



    //-----------------Revision Vouchers----------------//
    public ActionResult RevisionVouchers()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult RevisionVouchersList()
    {
        try
        {
            List<ProcurementPlan> procurementPlans = new List<ProcurementPlan>();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            /* string pageRV = $"DptProcurementPlan?$filter=Global_Dimension_2_Code eq '" +
                           employeeView?.AdministrativeUnit + "' and Global_Dimension_1_Code eq '" +
                           employeeView?.GlobalDimension1Code + "'&$format=json";*/

            string pageRV = $"DptProcurementPlan?$filter=Global_Dimension_1_Code eq '" +
                            employeeView?.GlobalDimension1Code +
                            "' and Document_Type eq 'Revision Voucher' and Plan_Type eq 'Functional'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(pageRV);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    ProcurementPlan procurement = new ProcurementPlan();
                    procurement.Code = (string)config["Code"];
                    procurement.Description = (string)config["Description"];
                    procurement.ConsolidatedProcurementPlan = (string)config["Consolidated_Procurement_Plan"];
                    procurement.DocumentDate = DateTime.Parse((string)config["Document_Date"]);
                    procurement.CorporateStrategicPlanID = (string)config["Corporate_Strategic_Plan_ID"];
                    procurement.FinancialBudgetID = (string)config["Financial_Budget_ID"];
                    procurement.FinancialYearCode = (string)config["Financial_Year_Code"];
                    procurement.StartDate = DateTime.Parse((string)config["Start_Date"]);
                    procurement.EndDate = DateTime.Parse((string)config["End_Date"]);
                    procurement.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                    procurement.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                    procurement.GlobalDimension3Code = (string)config["Global_Dimension_3_Code"];
                    procurement.TotalBudgetGoods = decimal.Parse((string)config?["Total_Budget_Goods"]);
                    procurement.TotalBudgetWorks = decimal.Parse((string)config?["Total_Budget_Works"]);
                    procurement.TotalBudgetServices = decimal.Parse((string)config?["Total_Budget_Services"]);
                    procurement.TotalBudgetedSpend = decimal.Parse((string)config?["Total_Budgeted_Spend"]);
                    procurement.TotalActualGoods = decimal.Parse((string)config["Total_Actual_Goods"]);
                    procurement.TotalActualWorks = decimal.Parse((string)config["Total_Actual_Works"]);
                    procurement.TotalActualServices = decimal.Parse((string)config["Total_Actual_Services"]);
                    procurement.TotalActualSpend = decimal.Parse((string)config["Total_Actual_Spend"]);
                    procurement.SpendPercent = decimal.Parse((string)config["Spend_Percent"]);
                    procurement.ApprovalStatus = (string)config["Approval_Status"];
                    procurement.CreatedBy = (string)config["Created_By"];
                    procurement.DateCreated = DateTime.Parse((string)config["Date_Created"]);
                    procurement.TimeCreated = TimeSpan.Parse((string)config["Time_Created"]);
                    procurementPlans.Add(procurement);

                }
            }

            return PartialView("~/Views/Purchase/PartialViews/RevisionVouchersList.cshtml",
                procurementPlans.OrderByDescending(x => x.Code));
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public ActionResult RevisionVouchersDocument(string docNo)
    {
        try
        {
            string userId = Session["UserId"].ToString();
            ProcurementPlan procurement = new ProcurementPlan();

            string page = $"DptProcurementPlan?$filter=Code eq '{docNo}'&$format=json";
            //  string page = $"DptProcurementPlan?$filter=Code eq'{docNo}' &$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    procurement.Code = (string)config["Code"];
                    procurement.Description = (string)config["Description"];
                    procurement.ConsolidatedProcurementPlan = (string)config["Consolidated_Procurement_Plan"];
                    procurement.DocumentDate = DateTime.Parse((string)config["Document_Date"]);
                    procurement.CorporateStrategicPlanID = (string)config["Corporate_Strategic_Plan_ID"];
                    procurement.FinancialBudgetID = (string)config["Financial_Budget_ID"];
                    procurement.FinancialYearCode = (string)config["Financial_Year_Code"];
                    procurement.StartDate = DateTime.Parse((string)config["Start_Date"]);
                    procurement.EndDate = DateTime.Parse((string)config["End_Date"]);
                    procurement.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                    procurement.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                    procurement.GlobalDimension3Code = (string)config["Global_Dimension_3_Code"];
                    procurement.TotalBudgetGoods = decimal.Parse((string)config["Total_Budget_Goods"]);
                    procurement.TotalBudgetWorks = decimal.Parse((string)config["Total_Budget_Works"]);
                    procurement.TotalBudgetServices = decimal.Parse((string)config["Total_Budget_Services"]);
                    procurement.TotalBudgetedSpend = decimal.Parse((string)config["Total_Budgeted_Spend"]);
                    procurement.TotalActualGoods = decimal.Parse((string)config["Total_Actual_Goods"]);
                    procurement.TotalActualWorks = decimal.Parse((string)config["Total_Actual_Works"]);
                    procurement.TotalActualServices = decimal.Parse((string)config["Total_Actual_Services"]);
                    procurement.TotalActualSpend = decimal.Parse((string)config["Total_Actual_Spend"]);
                    procurement.SpendPercent = decimal.Parse((string)config["Spend_Percent"]);
                    procurement.ApprovalStatus = (string)config["Approval_Status"];
                    procurement.CreatedBy = (string)config["Created_By"];
                    procurement.DateCreated = DateTime.Parse((string)config["Date_Created"]);
                    procurement.TimeCreated = TimeSpan.Parse((string)config["Time_Created"]);


                }
            }

            return View(procurement);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult RevisionVouchersLines(string DocNo)
    {
        try
        {
            //var reqtype = GetRequisitionType(DocNo);
            List<ProcurementPlanLine> procurementPlanLines = new List<ProcurementPlanLine>();
            string pageLine = "ProcurementPlanLines?$filter=Procurement_Plan_ID eq '" + DocNo + "'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ProcurementPlanLine procurementPlanLine = new ProcurementPlanLine();
                    procurementPlanLine.PPLineNo = (int)config["PP_Line_No"];
                    procurementPlanLine.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                    procurementPlanLine.DocumentType = (string)config["Document_Type"];
                    procurementPlanLine.PlanningCategory = (string)config["Planning_Category"];
                    procurementPlanLine.Description = (string)config["Description"];
                    procurementPlanLine.TotalQuantity = (decimal)config["Total_Quantity"];
                    procurementPlanLine.TotalBudgetedCost = (decimal)config["Total_Budgeted_Cost"];

                    procurementPlanLines.Add(procurementPlanLine);
                }
            }



            return PartialView("~/Views/Purchase/PartialViews/RevisionVouchersLines.cshtml", procurementPlanLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public ActionResult ReportType()
    {
        return PartialView("PartialViews/ReportType");
    }

    public ActionResult GetProcurementPlanReportByType(string documentNumber, string reportType)
    {
        try
        {
            string message = "";
            bool success = false;
            bool isExcel = reportType == "1";

           /* message = Credentials.ObjNav.fnProcPlanReport(documentNumber, Convert.ToInt32(reportType));
*/
            if (string.IsNullOrEmpty(message))
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }
            return Json(new { message, success, view = false }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }




    public JsonResult GenerateProcurementPlanReport(string DocNo)
    {
        try
        {
            string message = "";
            bool success = false, view = false;

           /* message = Credentials.ObjNav.fnProcPlanReport(DocNo, 0);*/
            if (message == "")
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }

            return Json(new { message = message, success, view }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult sendProcurementPlanForApproval(string DocNo)
    {
        try
        {
            var userId = Session["UserID"].ToString();
            Credentials.ObjNav.SendProcurementPlanApproval(DocNo);
            Credentials.ObjNav.UpdateApprovalEntrySenderID(70098, DocNo, userId);


            var Redirect = "/Purchase/FunctionalProcurementPlanDocument?docNo=" + DocNo;
            return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
            /* return Json(new { message = "Failed. Try Again.", success = false },
                 JsonRequestBehavior.AllowGet);*/
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult postProcurementPlan(string DocNo)
    {
        try
        {
            var userId = Session["UserID"].ToString();
            /*Credentials.ObjNav.PostProcurementPlan(DocNo);*/


            var Redirect = "/Purchase/FunctionalProcurementPlanDocument?docNo=" + DocNo;
            return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult CancelProcurementPlanApproval(string DocNo)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            string staffNo = Session["Username"].ToString();
            string msg = "";

            bool valSucc = false;
            string userId = employeeView.UserID;

            /*Credentials.ObjNav.cancelPurchaseRequisitionApproval(DocNo);*/
            Credentials.ObjNav.CancelProcurementPlanApproval(DocNo);

            msg = "Approval Request Successfully Cancelled";

            valSucc = true;

            return Json(new { message = DocNo, success = valSucc }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public List<SelectListItem> GenBusinessPostingGroupsDownList()
    {
        List<SelectListItem> BsPostingGroup = new List<SelectListItem>();
        string page = "GenBusinessPostingGroups";

        try
        {
            using HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            foreach (JObject config in details["value"])
            {
                SelectListItem dropdownItem = new SelectListItem
                {
                    Text = $"{config["Code"]} - {config["Description"]}",
                    Value = (string)config["Code"]
                };
                BsPostingGroup.Add(dropdownItem);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving Business Posting Group.", ex);
        }

        return BsPostingGroup;
    }


    public List<SelectListItem> GenProductPostingGroupsDownList()
    {
        List<SelectListItem> ProductPostingGroup = new List<SelectListItem>();
        string page = "GeneralProductPostingGroups";

        try
        {
            using HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            foreach (JObject config in details["value"])
            {
                SelectListItem dropdownItem = new SelectListItem
                {
                    Text = $"{config["Code"]} - {config["Description"]}",
                    Value = (string)config["Code"]
                };
                ProductPostingGroup.Add(dropdownItem);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving Product Posting Group.", ex);
        }

        return ProductPostingGroup;
    }


    public ActionResult PurchaseRequisitionsPendingApproval()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult PurchaseRequisitionsPendingApprovalList()
    {
        try
        {
            string userId = Session["UserId"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            List<PurchaseRequisition> purchaseRequisitions = new List<PurchaseRequisition>();
            string empNo = employeeView.No;
            string page = "PurchaseRequisition?$filter=Request_By_No eq '" + empNo +
                          "' and Status eq 'Pending Approval'&$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PurchaseRequisition purchase = new PurchaseRequisition();
                    purchase.No = (string)config["No_"];
                    purchase.Description = (string)config["Request_Description"];
                    purchase.Document_Type = (string)config["Document_Type"];
                    purchase.No = (string)config["No"];
                    purchase.PRN_Type = (string)config["PRN_Type"];
                    purchase.Document_Date = ((DateTime)config["Document_Date"]).ToString("dd/MM/yyyy");
                    purchase.Location_Code = (string)config["Location_Code"];
                    purchase.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                    purchase.Description = (string)config["Description"];
                    purchase.Priority_Level = (string)config["Priority_Level"];
                    purchase.Requester_ID = (string)config["Requester_ID"];
                    purchase.Request_By_No = (string)config["Request_By_No"];
                    purchase.Request_By_Name = (string)config["Request_By_Name"];
                    purchase.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                    purchase.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                    purchase.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                    purchase.PP_Planning_Category = (string)config["PP_Planning_Category"];
                    purchase.Job = (string)config["Job"];
                    purchase.Job_Task_No = (string)config["Job_Task_No"];
                    purchase.PP_Total_Budget = (decimal)config["PP_Total_Budget"];
                    //purchase.PP_Total_Commitments = (decimal)config["PP_Total_Commitments"];
                    //purchase.PP_Total_Available_Budget = (decimal)config["PP_Total_Available_Budget"];
                    purchase.Requisition_Amount = (decimal)config["Requisition_Amount"];
                    purchase.Status = (string)config["Status"] == "Released" || (string)config["Status"] == "Posted"
                        ? "Approved"
                        : (string)config["Status"];



                    if ((string)config["Status"] == "Released" || (string)config["Status"] == "Posted")
                    {
                        purchase.Status = "Approved";
                    }
                    else
                    {
                        purchase.Status = (string)config["Status"];
                    }

                    purchaseRequisitions.Add(purchase);
                }
            }

            return PartialView("~/Views/Purchase/PRVReqListView.cshtml",
                purchaseRequisitions.OrderByDescending(x => x.No));
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public ActionResult PurchaseRequisitionsPendingApprovalDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string StaffNo = Session["Username"].ToString();
                PurchaseRequisition purchase = new PurchaseRequisition();

                string page = "PurchaseRequisition?$filter=No eq '" + DocNo + "'&format=json";
                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {

                        purchase.No = (string)config["No"];
                        purchase.PRN_Type = (string)config["PRN_Type"];
                        purchase.Document_Date = ((DateTime)config["Document_Date"]).ToString("dd/MM/yyyy");
                        purchase.Location_Code = (string)config["Location_Code"];
                        purchase.Requisition_Product_Group = (string)config["Requisition_Product_Group"];
                        purchase.Description = (string)config["Description"];
                        purchase.Priority_Level = (string)config["Priority_Level"];
                        purchase.Requester_ID = (string)config["Requester_ID"];
                        purchase.Request_By_No = (string)config["Request_By_No"];
                        purchase.Request_By_Name = (string)config["Request_By_Name"];
                        purchase.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                        purchase.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                        purchase.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                        purchase.PP_Planning_Category = (string)config["PP_Planning_Category"];
                        purchase.Job = (string)config["Job"];
                        purchase.Job_Task_No = (string)config["Job_Task_No"];
                        purchase.PP_Total_Budget = (decimal)config["PP_Total_Budget"];
                        //purchase.PP_Total_Commitments = (decimal)config["PP_Total_Commitments"];
                        //purchase.PP_Total_Available_Budget = (decimal)config["PP_Total_Available_Budget"];
                        purchase.Requisition_Amount = (decimal)config["Requisition_Amount"];
                        purchase.Status = (string)config["Status"];
                        if ((string)config["Status"] == "Released" || (string)config["Status"] == "Posted")
                        {
                            purchase.Status = "Approved";
                        }
                        else
                        {
                            purchase.Status = (string)config["Status"];
                        }
                    }
                }

                purchase.LocationName =
                    DimensinValuesList.GetDimensionValueName(purchase.Shortcut_Dimension_1_Code);
                purchase.AdminUnitName =
                    DimensinValuesList.GetDimensionValueName(purchase.Shortcut_Dimension_2_Code);


                return View(purchase);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult PurchaseRequisitionsPendingApprovalLines(string DocNo)
    {
        try
        {
            #region Purchase Lines

            //var reqtype = GetRequisitionType(DocNo);
            List<PurchaseRequisitionLine> purchaseRequisitionLines = new List<PurchaseRequisitionLine>();
            string pageLine = "PurchaseRequisitionLines?$filter=Document_No eq '" + DocNo + "'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    PurchaseRequisitionLine requisitionLine = new PurchaseRequisitionLine();
                    requisitionLine.Document_Type = (string)config["Document_Type"];
                    requisitionLine.Document_No = (string)config["Document_No"];
                    requisitionLine.Line_No = (int)config["Line_No"];
                    requisitionLine.ProcurementPlanID = (string)config["ProcurementPlanID"];
                    requisitionLine.Type = (string)config["Type"];
                    requisitionLine.ProcurementPlanItemNo = (int)config["ProcurementPlanItemNo"];
                    requisitionLine.Control6 = (string)config["Control6"];
                    requisitionLine.No = (string)config["No"];
                    requisitionLine.Description = (string)config["Description"];
                    requisitionLine.TechnicalSpecifications = (string)config["TechnicalSpecifications"];
                    requisitionLine.UnitofMeasureCode = (string)config["UnitofMeasureCode"];
                    requisitionLine.QuantityInStore = (int)config["QuantityInStore"];
                    requisitionLine.Quantity = (int)config["Quantity"];
                    requisitionLine.DirectUnitCostInclVAT = (string)config["DirectUnitCostInclVAT"];
                    requisitionLine.Amount = (decimal)config["Amount"];
                    requisitionLine.AmountIncludingVAT = (decimal)config["AmountIncludingVAT"];
                    requisitionLine.CurrencyCode = (string)config["CurrencyCode"];
                    requisitionLine.LocationCode = (string)config["LocationCode"];
                    requisitionLine.UnitCostLCY = (decimal)config["UnitCostLCY"];
                    requisitionLine.VATProdPostingGroup = (string)config["VATProdPostingGroup"];
                    requisitionLine.ItemCategoryCode = (string)config["ItemCategoryCode"];
                    requisitionLine.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                    requisitionLine.Budget = (string)config["Budget"];
                    requisitionLine.Budget_Line = (string)config["Budget_Line"];
                    requisitionLine.PPTotalAvailableBudget = (decimal)config["PPTotalAvailableBudget"];
                    requisitionLine.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                    requisitionLine.PPFundingSourceID = (string)config["PPFundingSourceID"];
                    requisitionLine.PPTotalBudget = (decimal)config["PPTotalBudget"];
                    requisitionLine.PPTotalActualCosts = (decimal)config["PPTotalActualCosts"];
                    requisitionLine.PPTotalCommitments = (decimal)config["PPTotalCommitments"];
                    requisitionLine.PPSolicitationType = (string)config["PPSolicitationType"];
                    requisitionLine.PPProcurementMethod = (string)config["PPProcurementMethod"];
                    requisitionLine.PPPreferenceReservationCode = (string)config["PPPreferenceReservationCode"];
                    requisitionLine.PRNConversionProcedure = (string)config["PRNConversionProcedure"];
                    purchaseRequisitionLines.Add(requisitionLine);

                }
            }

            #endregion



            return PartialView("~/Views/Purchase/PurchaseDocumentLineView.cshtml", purchaseRequisitionLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    //Procurement plan ammendments
    public ActionResult ProcurementPlanAmmendments()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    public PartialViewResult ProcurementPlanAmmendmentsList()
    {
        try
        {
            List<ProcurementPlanAmmendments> ammendments = new List<ProcurementPlanAmmendments>();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;

            string pageAmmendments = $"ProcurementPlanAmmendments?$filter=Document_Type eq 'Revision Voucher' and CreatedBy eq '{employeeView.No}' &$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(pageAmmendments);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    ProcurementPlanAmmendments ammendment = new ProcurementPlanAmmendments
                    {
                        Code = (string)config["Code"],
                        Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"],
                        Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"],
                        Type = (string)config["Type"],
                        PlanNo = (string)config["PlanNo"],
                        Description = (string)config["Description"],
                        ApprovalStatus = (string)config["ApprovalStatus"],
                        Ammendment_Reason = (string)config["Ammendment_Reason"],
                        CreatedBy = (string)config["CreatedBy"],
                        DateCreated = (string)config["DateCreated"],
                        TimeCreated = (string)config["TimeCreated"]
                    };
                    ammendments.Add(ammendment);
                }
            }

            return PartialView(
                "~/Views/Purchase/PartialViews/ProcurementPlanAmmendmentsList.cshtml",
                ammendments.OrderByDescending(x => x.Code)
            );
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error
            {
                Message = ex.Message
            };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    [HttpPost]
    public ActionResult ProcurementPlanAmmendmentDocument(string Code)
    {
        try
        {
            /*string userId = Session["UserId"].ToString();*/
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");
            ProcurementPlanAmmendments ammendment = new ProcurementPlanAmmendments();

            string page = $"ProcurementPlanAmmendments?$filter=Code eq '{Code}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ammendment = new ProcurementPlanAmmendments
                    {
                        Code = (string)config["Code"],
                        Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"],
                        Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"],
                        /* Type = (string)config["Type"],*/
                        Type = "Consolidated",
                        PlanNo = (string)config["PlanNo"],
                        Description = (string)config["Description"],
                        ApprovalStatus = (string)config["ApprovalStatus"],
                        Ammendment_Reason = (string)config["Ammendment_Reason"],
                        CreatedBy = (string)config["CreatedBy"],
                        DateCreated = (string)config["DateCreated"],
                        TimeCreated = (string)config["TimeCreated"]
                    };
                }
            }

            return View(ammendment);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error
            {
                Message = ex.Message
            };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult ProcurementPlanAmmendmentLines(string Code)
    {
        try
        {
            //var reqtype = GetRequisitionType(DocNo);
            List<ProcurementPlanAmmendmentsLines> ammendmentsLines = new List<ProcurementPlanAmmendmentsLines>();
            string pageLine = "PPRevisionVoucherLines?$filter=Procurement_Plan_ID eq '" + Code + "'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ProcurementPlanAmmendmentsLines ammendmentLine = new ProcurementPlanAmmendmentsLines();
                    ammendmentLine.PP_Line_No = (int)config["PP_Line_No"];
                    ammendmentLine.Procurement_Plan_ID = (string)config["Procurement_Plan_ID"];
                    ammendmentLine.Document_Type = (string)config["Document_Type"];
                    ammendmentLine.PlanningCategory = (string)config["PlanningCategory"];
                    ammendmentLine.Description = (string)config["Description"];

                    ammendmentLine.TotalQuantity = (int)config["TotalQuantity"];
                    ammendmentLine.Total_Budgeted_Cost = ((decimal)config["Total_Budgeted_Cost"]).ToString("C", new CultureInfo("sw-KE"));

                    ammendmentsLines.Add(ammendmentLine);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanAmmendmentLines.cshtml",
                ammendmentsLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    public PartialViewResult ApprovalComments(string Code)
    {
        try
        {
            //var reqtype = GetRequisitionType(DocNo);
            List<CommentSheet> commentsList = new List<CommentSheet>();
            string pageLine = "CommentSheet?$filter=No eq '" + Code + "'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    CommentSheet comments = new CommentSheet();
                    comments.Document_Type = (string)config["Document_Type"];
                    comments.No = (string)config["No"];
                    comments.Document_Line_No = (int)config["Document_Line_No"];
                    comments.Line_No = (int)config["Line_No"];
                    comments.Date = (string)config["Description"];
                    comments.Comment = (string)config["Comment"];
                    comments.Code = (string)config["Code"];
                    commentsList.Add(comments);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/ApprovalComments.cshtml", commentsList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }




    /*public PartialViewResult ProcurementPlanRevisionEntriesLines(string ProcurementPlanID, string PlanningCategory)
    {
        try
        {
            //var reqtype = GetRequisitionType(DocNo);
            List<ProcurementPlanRevisionEntries> revisionEntries = new List<ProcurementPlanRevisionEntries>();

            string pageLine = "ProcurementPlanRevisionEntries?$filter=ProcurementPlanID eq '" + ProcurementPlanID + "' and Planning_Category eq '" + PlanningCategory + "'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    ProcurementPlanRevisionEntries entry = new ProcurementPlanRevisionEntries
                    {
                        ProcurementPlanID = (string)config["ProcurementPlanID"],
                        EntryNo = (int)config["EntryNo"],
                        VoteItem = (int)config["VoteItem"],
                        PlanItemType = (string)config["PlanItemType"],
                        PlanItemNo = (string)config["PlanItemNo"],
                        Description = (string)config["Description"],
                        Procurement_Type = (string)config["Procurement_Type"],
                        Solicitation_Type = (string)config["Solicitation_Type"],
                        Procurement_Method = (string)config["Procurement_Method"],
                        Alternative_Procurement_Methods = (string)config["Alternative_Procurement_Methods"],
                        Preference_Reservation_Code = (string)config["Preference_Reservation_Code"],
                        Funding_Source_ID = (string)config["Funding_Source_ID"],
                        Requisition_Product_Group = (string)config["Requisition_Product_Group"],
                        Procurement_Use = (string)config["Procurement_Use"],
                        Invitation_Notice_Type = (string)config["Invitation_Notice_Type"],
                        Planning_Date = (string)config["Planning_Date"],
                        Quantity = (int)config["Quantity"],
                        Unit_Cost = (int)config["Unit_Cost"],
                        Line_Budget_Cost = (int)config["Line_Budget_Cost"],
                        Total_Actual_Costs = (int)config["Total_Actual_Costs"],
                        Total_PRN_Commitments = (int)config["Total_PRN_Commitments"],
                        Available_Procurement_Budget = (int)config["Available_Procurement_Budget"],
                        Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"],
                        Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"],
                        Q1_Quantity = (int)config["Q1_Quantity"],
                        Q1_Amount = (int)config["Q1_Amount"],
                        Q1_Budget = (int)config["Q1_Budget"],
                        Q2_Quantity = (int)config["Q2_Quantity"],
                        Q2_Amount = (int)config["Q2_Amount"],
                        Q2_Budget = (int)config["Q2_Budget"],
                        Q3_Quantity = (int)config["Q3_Quantity"],
                        Q3_Amount = (int)config["Q3_Amount"],
                        Q3_Budget = (int)config["Q3_Budget"],
                        Q4_Quantity = (int)config["Q4_Quantity"],
                        Q4_Amount = (int)config["Q4_Amount"],
                        Q4_Budget = (int)config["Q4_Budget"],
                        Work_Plan_Project_ID = (string)config["Work_Plan_Project_ID"],
                        Workplan_Task_No = (string)config["Workplan_Task_No"],
                        Activity_No = (string)config["Activity_No"],
                        Sub_Activity_No = (string)config["Sub_Activity_No"],
                        AGPO_Percent = (int)config["AGPO_Percent"],
                        AGPO_Amount = (int)config["AGPO_Amount"],
                        PWD_Percent = (int)config["PWD_Percent"],
                        PWD_Amount = (int)config["PWD_Amount"],
                        Women_Percent = (int)config["Women_Percent"],
                        Women_Amount = (int)config["Women_Amount"],
                        Youth_Percent = (int)config["Youth_Percent"],
                        Youth_Amount = (int)config["Youth_Amount"],
                        Margin_of_Preference = (int)config["Margin_of_Preference"],
                        Citizen_Contractors_Percent = (int)config["Citizen_Contractors_Percent"],
                        Action_Taken = (string)config["Action_Taken"],
                        Status = (string)config["Status"],
                    };

                    revisionEntries.Add(entry);
                }
            }
            return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanRevisionEntriesLines.cshtml", revisionEntries);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
*/

    public PartialViewResult ProcurementPlanRevisionEntriesLines(string ProcurementPlanID, string Plan_No, string PlanningCategory, string Approval_Status)
    {
        if (string.IsNullOrEmpty(ProcurementPlanID) || string.IsNullOrEmpty(PlanningCategory))
        {
            var errorMsg = new Error { Message = "Invalid Procurement Plan ID or Planning Category." };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", errorMsg);
        }

        EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
        var GlobalDim1Code = employeeView.GlobalDimension1Code;
        ViewBag.AdminUnit = GlobalDim1Code;
        try
        {
            var viewModel = new ProcurementPlanRevisionEntriesViewModel
            {
                ListOfVoteItems = VoteItemsLookup(ProcurementPlanID, Plan_No, PlanningCategory),
                ListOfItems = ItemsLookup(PlanningCategory),
                ListOfProcTypes = ProcTypesLookup(),
                ListOfSolicitationTypes = SolicitationTypesLookup(),
                ListOfSpecialVendor = SpecialVendorLookup(),
                ListOfFundingSourceID = FundingSourceIDLookup(),
                ListOfDim1 = GeolocationLookup(),
                ListOfDim2 = AdminUnitLookup(),
                ListOfVote = VoteLookup(),
                ListOfSubProgram = SubProgramLookup(),
                ListOfClass = ClassLookup(),
                ListOfFundingSource = FundingSourceLookup(),
                RevisionEntries = GetRevisionEntries(ProcurementPlanID, Plan_No, PlanningCategory, Approval_Status)
            };

            return PartialView("~/Views/Purchase/PartialViews/ProcurementPlanRevisionEntriesLines.cshtml",
                viewModel);
        }
        catch (Exception ex)
        {
            var errorMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", errorMsg);
        }
    }


    private List<SelectListItem> VoteItemsLookup(string ProcurementPlanID, string RevisedPlanNo,
        string PlanningCategory)
    {
        var voteItem = new List<SelectListItem>();
        var employeeView = Session["EmployeeData"] as EmployeeView;
        var geographical = employeeView.GlobalDimension1Code;
        var adminUnit = employeeView.AdministrativeUnit;
        string endpoint = $"ProcurementPlanEntry?$filter= ProcurementPlanID eq '{RevisedPlanNo}' and PlanningCategory eq '{PlanningCategory}' " +
                          $"and GlobalDimension1Code eq '{geographical}' and GlobalDimension2Code eq '{adminUnit}'&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(endpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            voteItem = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Description"]}",
                Value = config["EntryNo"].ToString()
            }).ToList();
        }

        return voteItem;
    }

    private List<SelectListItem> ItemsLookup(string PlanningCategory)
    {
        var items = new List<SelectListItem>();
        string endpoint = $"Items?$filter=Gen_Prod_Posting_Group eq '{PlanningCategory}'&$format=json";

        using (var httpResponse = Credentials.GetOdataData(endpoint))
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var details = JObject.Parse(streamReader.ReadToEnd());
            items = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Description"]}-{PlanningCategory}",
                Value = config["No"].ToString()
            }).ToList();
        }
        return items;
    }


    private List<SelectListItem> ProcTypesLookup()
    {
        var ProcTypes = new List<SelectListItem>();

        string ProcTypesEndpoint = "ProcurementTypes?$format=json";

        HttpWebResponse httpResponseProcTypes = Credentials.GetOdataData(ProcTypesEndpoint);
        using (var streamReader = new StreamReader(httpResponseProcTypes.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            ProcTypes = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Description"]}",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return ProcTypes;
    }

    private List<SelectListItem> SolicitationTypesLookup()
    {
        var SolTypes = new List<SelectListItem>();

        string SolTypesEndpoint = "SolicitationType?$format=json";

        HttpWebResponse httpResponseSolTypes = Credentials.GetOdataData(SolTypesEndpoint);
        using (var streamReader = new StreamReader(httpResponseSolTypes.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            SolTypes = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Description"]}",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return SolTypes;
    }

    private List<SelectListItem> SpecialVendorLookup()
    {
        var SpecialVendor = new List<SelectListItem>();

        string SpecialVendorEndpoint = "SpecialVendor?$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(SpecialVendorEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            SpecialVendor = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Description"]}",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return SpecialVendor;
    }

    private List<SelectListItem> FundingSourceIDLookup()
    {
        var FundingSourceID = new List<SelectListItem>();

        string FundingSourceIDEndpoint = "FundingSource?$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(FundingSourceIDEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            FundingSourceID = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Description"]}",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return FundingSourceID;
    }


    private List<SelectListItem> GeolocationLookup()
    {
        var Dim1 = new List<SelectListItem>();

        string Dim1Page = "DimensionValues?$filter=Global_Dimension_No eq 1 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(Dim1Page);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            Dim1 = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " (" + config["Code"].ToString() + ")",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return Dim1;
    }

    private List<SelectListItem> AdminUnitLookup()
    {
        var Dim2 = new List<SelectListItem>();

        string Dim2Page = "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(Dim2Page);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            Dim2 = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " (" + config["Code"].ToString() + ")",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return Dim2;
    }

    private List<SelectListItem> VoteLookup()
    {
        var vote = new List<SelectListItem>();

        /* string SpecialVendorEndpoint = "FundingSource?$format=json";*/

        string voteEndpoint = "DimensionValues?$filter=Global_Dimension_No eq 3 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(voteEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            vote = details["value"].Select(config => new SelectListItem
            {
                Text = $"{config["Name"]} ({config["Code"]})",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return vote;
    }

    private List<SelectListItem> SubProgramLookup()
    {
        var SubProgram = new List<SelectListItem>();

        string SubProgramEndpoint =
            "DimensionValues?$filter=Global_Dimension_No eq 4 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(SubProgramEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            SubProgram = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " (" + config["Code"].ToString() + ")",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return SubProgram;
    }

    private List<SelectListItem> ClassLookup()
    {
        var _class = new List<SelectListItem>();

        string ClassEndpoint = "DimensionValues?$filter=Global_Dimension_No eq 5 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(ClassEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            _class = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " (" + config["Code"].ToString() + ")",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return _class;
    }

    private List<SelectListItem> FundingSourceLookup()
    {
        var FundingSource = new List<SelectListItem>();

        string FundingSourceEndpoint =
            "DimensionValues?$filter=Global_Dimension_No eq 6 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(FundingSourceEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            FundingSource = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " (" + config["Code"].ToString() + ")",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return FundingSource;
    }


    private List<SelectListItem> SubActivityLookup()
    {
        var SubActivity = new List<SelectListItem>();

        string SubActivityEndpoint =
            "DimensionValues?$filter=Global_Dimension_No eq 4 and Blocked eq false&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(SubActivityEndpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            SubActivity = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " (" + config["Code"].ToString() + ")",
                Value = config["Code"].ToString()
            }).ToList();
        }

        return SubActivity;
    }

    private List<SelectListItem> BudgetAcountLookup()
    {
        var BudgetAcc = new List<SelectListItem>();

        string BudgetAccPage = "glAccount?$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(BudgetAccPage);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            BudgetAcc = details["value"].Select(config => new SelectListItem
            {
                Text = config["Name"].ToString() + " - " + config["No"].ToString(),
                Value = config["No"].ToString()
            }).ToList();
        }

        return BudgetAcc;
    }

    private List<SelectListItem> ActivityLookup()
    {
        var activity = new List<SelectListItem>();

        /* string activityPage = "StrategyWorkplanLines?$format=json";*/
        string activityPage = "StrategyWorkplanLines?$filter=KRA_ID eq 'OUTCOME 1'&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(activityPage);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            activity = details["value"].Select(config => new SelectListItem
            {
                Text = config["Description"].ToString() + "-" + config["Activity_ID"].ToString(),
                Value = config["Activity_ID"].ToString()
            }).ToList();
        }

        return activity;
    }

    private List<ProcurementPlanRevisionEntries> GetRevisionEntries(string procurementPlanID, string Plan_No,
        string planningCategory, string Approval_Status)
    {

        var revisionEntries = new List<ProcurementPlanRevisionEntries>();
        string endpoint =
            $"ProcurementPlanRevisionEntries?$filter=ProcurementPlanID eq '{procurementPlanID}' and Planning_Category eq '{planningCategory}'&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(endpoint);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);

            foreach (var config in details["value"])
            {
                var entry = new ProcurementPlanRevisionEntries
                {
                    ProcurementPlanID = (string)config["ProcurementPlanID"],
                    EntryNo = (int)config["EntryNo"],
                    VoteItem = (int)config["VoteItem"],
                    PlanItemType = (string)config["PlanItemType"],
                    PlanItemNo = (string)config["PlanItemNo"],
                    Description = (string)config["Description"],
                    Procurement_Type = (string)config["Procurement_Type"],
                    Solicitation_Type = (string)config["Solicitation_Type"],
                    Procurement_Method = (string)config["Procurement_Method"],
                    Alternative_Procurement_Methods = (string)config["Alternative_Procurement_Methods"],
                    Preference_Reservation_Code = (string)config["Preference_Reservation_Code"],
                    Funding_Source_ID = (string)config["Funding_Source_ID"],
                    Requisition_Product_Group = (string)config["Requisition_Product_Group"],
                    Procurement_Use = (string)config["Procurement_Use"],
                    Invitation_Notice_Type = (string)config["Invitation_Notice_Type"],
                    Planning_Date = (string)config["Planning_Date"],
                    Quantity = (int)config["Quantity"],
                    Unit_Cost = (int)config["Unit_Cost"],
                    Line_Budget_Cost = (int)config["Line_Budget_Cost"],
                    Total_Actual_Costs = (int)config["Total_Actual_Costs"],
                    Total_PRN_Commitments = (int)config["Total_PRN_Commitments"],
                    Available_Procurement_Budget = (int)config["Available_Procurement_Budget"],
                    Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"],
                    Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"],
                    Shortcut_Dimension_3_Code = (string)config["Shortcut_Dimension_3_Code"],
                    Shortcut_Dimension_4_Code = (string)config["Shortcut_Dimension_4_Code"],
                    Shortcut_Dimension_5_Code = (string)config["Shortcut_Dimension_5_Code"],
                    Shortcut_Dimension_6_Code = (string)config["Shortcut_Dimension_6_Code"],
                    Budget_Account = (string)config["Budget_Account"],
                    Q1_Quantity = (int)config["Q1_Quantity"],
                    Q1_Amount = (int)config["Q1_Amount"],
                    Q1_Budget = (int)config["Q1_Budget"],
                    Q2_Quantity = (int)config["Q2_Quantity"],
                    Q2_Amount = (int)config["Q2_Amount"],
                    Q2_Budget = (int)config["Q2_Budget"],
                    Q3_Quantity = (int)config["Q3_Quantity"],
                    Q3_Amount = (int)config["Q3_Amount"],
                    Q3_Budget = (int)config["Q3_Budget"],
                    Q4_Quantity = (int)config["Q4_Quantity"],
                    Q4_Amount = (int)config["Q4_Amount"],
                    Q4_Budget = (int)config["Q4_Budget"],
                    Work_Plan_Project_ID = (string)config["Work_Plan_Project_ID"],
                    Workplan_Task_No = (string)config["Workplan_Task_No"],
                    Activity_No = (string)config["Activity_No"],
                    Sub_Activity_No = (string)config["Sub_Activity_No"],
                    AGPO_Percent = (int)config["AGPO_Percent"],
                    AGPO_Amount = (int)config["AGPO_Amount"],
                    PWD_Percent = (int)config["PWD_Percent"],
                    PWD_Amount = (int)config["PWD_Amount"],
                    Women_Percent = (int)config["Women_Percent"],
                    Women_Amount = (int)config["Women_Amount"],
                    Youth_Percent = (int)config["Youth_Percent"],
                    Youth_Amount = (int)config["Youth_Amount"],
                    Margin_of_Preference = (int)config["Margin_of_Preference"],
                    Citizen_Contractors_Percent = (int)config["Citizen_Contractors_Percent"],
                    Action_Taken = (string)config["Action_Taken"],
                    Status = (string)config["Status"],
                };
                revisionEntries.Add(entry);
                ViewBag.ApprovalStatus = Approval_Status;
                ViewBag.procurementPlanID = procurementPlanID;
                ViewBag.Plan_No = Plan_No;
                ViewBag.planningCategory = planningCategory;
            }
        }

        return revisionEntries;
    }


    public PartialViewResult NewProcurementPlanAmmendment()
    {
        try
        {

            string StaffNo = Session["Username"].ToString();
            string Dim1 = "", Dim2 = "";

            ProcurementPlanAmmendments newProcurementPlanAmmendment = new ProcurementPlanAmmendments();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            Dim1 = employeeView.GlobalDimension1Code;
            Dim2 = employeeView.GlobalDimension2Code;
            newProcurementPlanAmmendment.CreatedBy = employeeView.UserID;
            /* newProcurementPlanAmmendment.Name = employeeView.FullName;*/
            newProcurementPlanAmmendment.Global_Dimension_1_Code = Dim1;
            /* newProcurementPlanAmmendment.Global_Dimension_2_Code = Dim2;*/


            #region Dim1 List

            List<DropdownList> dim1 = new List<DropdownList>();
            string pageDim1 = "DimensionValues?$filter=Global_Dimension_No eq 1 and Blocked eq false&$format=json";


            HttpWebResponse httpResponseDepartment = Credentials.GetOdataData(pageDim1);
            using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Code"] + "-" + (string)config["Name"];
                    dropDown.Value = (string)config["Code"];
                    dim1.Add(dropDown);
                }
            }

            #endregion

            #region dim2

            List<DropdownList> dim2 = new List<DropdownList>();
            /*   string pageDim2 = "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$format=json";*/
            string pageDim2 = $"DimensionValues?$filter=Global_Dimension_No eq 2 and Shortcut_Dimension_1_Code eq '{Dim1}' and Blocked eq false&$format=json";




            HttpWebResponse httpResponseDivision = Credentials.GetOdataData(pageDim2);
            using (var streamReader = new StreamReader(httpResponseDivision.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                    dropdownList.Value = (string)config["Code"];
                    dim2.Add(dropdownList);
                }
            }

            #endregion

            #region procurementPlans

            List<DropdownList> procurementPlans = new List<DropdownList>();
            string stPlans = "ApprovedProcurementPlan?$format=json";

            HttpWebResponse httpResponsePlans = Credentials.GetOdataData(stPlans);
            using (var streamReader = new StreamReader(httpResponsePlans.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Code"] + "-" + (string)config["Description"];
                    dropdownList.Value = (string)config["Code"];
                    procurementPlans.Add(dropdownList);
                }
            }

            #endregion


            newProcurementPlanAmmendment.ListOfDim1 = dim1.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newProcurementPlanAmmendment.ListOfDim2 = dim2.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newProcurementPlanAmmendment.ListOfProcurementPlans = procurementPlans.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();


            return PartialView("~/Views/Purchase/PartialViews/NewProcurementPlanAmmendment.cshtml",
                newProcurementPlanAmmendment);

        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public JsonResult Toggle_LPO_Created(string DocNo, int LineNo, bool _Line_Selected)
    {


        try
        {

            Credentials.ObjNav.FnUpdatePRNLines(LineNo, DocNo, _Line_Selected);
            return Json(new { message = "Success.", success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}\n{e.StackTrace}");
            return Json(new { message = "An error occurred while processing your request.", success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult PurchaseInvoice(string status)
    {
        try
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");

            ViewBag.Status = status;

            return View();
        }
        catch (Exception ex)
        {
            var errorMsg = new Error
            {
                Message = ex.Message
            };
            return View("~/Views/Common/ErrorMessange.cshtml", errorMsg);
        }
    }

    public PartialViewResult PurchaseInvoiceList(string status)
    {
        try
        {
            var assignedAssetList = new List<PurchaseInvoice>();
            {
                var employee = Session["EmployeeData"] as EmployeeView;
                var geographicalLocation = employee?.GlobalDimension1Code;
                var page = "";
                switch (status)
                {
                    case "Released":
                        page = "PurchaseInvoice?$filter=Shortcut_Dimension_1_Code eq '" + geographicalLocation +
                               "' and Document_Type eq 'Invoice'&$format=json";
                        break;
                    case "Posted":
                        page = "PostedPurchaseInvoice?$filter=Shortcut_Dimension_1_Code eq '" +
                               geographicalLocation +
                               "'&$format=json";
                        break;
                }

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ViewBag.PortalStatus = status;
                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;
                        var statusCheck = "";
                        if ((string)config["Status"] != null)
                        {
                            statusCheck = (string)config["Status"];

                        }

                        var assignedAsset = new PurchaseInvoice
                        {
                            No = (string)config["No"],
                            BuyFromVendorNo = (string)config["Buy_from_Vendor_No"],
                            BuyFromVendorName = (string)config["Buy_from_Vendor_Name"],
                            VendorInvoiceNo = (string)config["Vendor_Invoice_No"],
                            Amount = ((decimal)config["Amount"]).ToString("C", new CultureInfo("sw-KE")),
                            Status = statusCheck
                        };
                        if (status == "Posted")
                        {
                            assignedAsset.Closed = (string)config["Closed"];
                            assignedAsset.Cancelled = (string)config["Cancelled"];
                            assignedAsset.Corrective = (string)config["Corrective"];
                            assignedAsset.Remaining_Amount = (string)config["Remaining_Amount"];
                        }

                        assignedAssetList.Add(assignedAsset);

                    }
                }

                return PartialView("PartialViews/PurchaseInvoiceList", assignedAssetList);
            }
        }
        catch (Exception ex)
        {
            var erroMsg = new Error
            {
                Message = ex.Message
            };
            return PartialView("Partial Views/ErroMessangeView", erroMsg);
        }
    }

    [HttpPost]
    public ActionResult PurchaseInvoiceDocument(string docNo, string status)
    {
        var assignedAsset = new PurchaseInvoice();
        try
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");
            var page = "";

            switch (status)
            {
                case "Released":
                    page = "PurchaseInvoice?$filter=No eq '" + docNo + "'&$format=json";
                    break;
                case "Posted":
                    page = "PostedPurchaseInvoice?$filter=No eq '" + docNo + "'&$format=json";
                    break;
            }

            ViewBag.Status = status;
            var httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    var amountLcyString = (string)config["Payment_Discount_Percent"];
                    var isValidAmount = int.TryParse(amountLcyString, out var amountLcy);
                    var formattedAmountLcy = isValidAmount ? amountLcy.ToString("N2") : "0.00";
                    string input = (string)config["Source_RecordID"];
                    string prefix = "Expense Requisition: ";
                    string extractedValue = "";
                    if (input != "" || input != null)
                    {
                        if (input.StartsWith(prefix))
                        {
                            extractedValue = input.Substring(prefix.Length).Trim();
                        }
                    }

                    // Initialize assignedAsset with common data
                    assignedAsset = new PurchaseInvoice
                    {
                        DocumentType = (string)config["Document_Type"],
                        No = (string)config["No"],
                        BuyFromVendorName = (string)config["Buy_from_Vendor_Name"],
                        BuyFromVendorNo = (string)config["Buy_from_Vendor_No"],
                        BuyFromAddress = (string)config["Buy_from_Address"],
                        BuyFromAddress2 = (string)config["Buy_from_Address_2"],
                        BuyFromCity = (string)config["Buy_from_City"],
                        BuyFromCounty = (string)config["Buy_from_County"],
                        BuyFromPostCode = (string)config["Buy_from_Post_Code"],
                        BuyFromCountryRegionCode = (string)config["Buy_from_Country_Region_Code"],
                        BuyFromContactNo = (string)config["Buy_from_Contact_No"],
                        BuyFromContactPhoneNo = (string)config["BuyFromContactPhoneNo"],
                        BuyFromContactMobilePhoneNo = (string)config["BuyFromContactMobilePhoneNo"],
                        BuyFromContactEmail = (string)config["BuyFromContactEmail"],
                        BuyFromContact = (string)config["Buy_from_Contact"],
                        PostingDescription = (string)config["Posting_Description"],
                        DimensionSetId = (string)config["Dimension_Set_ID"],
                        PostingDate = DateTime.ParseExact((string)config["Posting_Date"], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                        VATReportingDate = DateTime.ParseExact((string)config["VAT_Reporting_Date"], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                        DocumentDate = (string)config["Document_Date"],
                        DueDate = DateTime
                            .ParseExact((string)config["Due_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy"),
                        VendorInvoiceNo = (string)config["Vendor_Invoice_No"],
                        VendorOrderNo = (string)config["Vendor_Order_No"],
                        PurchaserCode = (string)config["Purchaser_Code"],
                        OrderAddressCode = (string)config["Order_Address_Code"],
                        ResponsibilityCenter = (string)config["Responsibility_Center"],
                        CurrencyCode = (string)config["Currency_Code"],
                        ExpectedReceiptDate = DateTime.ParseExact((string)config["Expected_Receipt_Date"],
                            "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                        PaymentTermsCode = (string)config["Payment_Terms_Code"],
                        PaymentMethodCode = (string)config["Payment_Method_Code"],
                        ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"],
                        ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"],
                        PaymentDiscountPercent = (int)config["Payment_Discount_Percent"],
                        PmtDiscountDate = (string)config["Pmt_Discount_Date"],
                        TaxLiable = (bool)config["Tax_Liable"],
                        TaxAreaCode = (string)config["Tax_Area_Code"],
                        LocationCode = (string)config["Location_Code"],
                        ShipmentMethodCode = (string)config["Shipment_Method_Code"],
                        PaymentReference = (string)config["Payment_Reference"],
                        CreditorNo = (string)config["Creditor_No"],
                        VendorPostingGroup = (string)config["Vendor_Posting_Group"],
                        ShipToName = (string)config["Ship_to_Name"],
                        ShipToAddress = (string)config["Ship_to_Address"],
                        ShipToAddress2 = (string)config["Ship_to_Address_2"],
                        ShipToCity = (string)config["Ship_to_City"],
                        ShipToCounty = (string)config["Ship_to_County"],
                        ShipToPostCode = (string)config["Ship_to_Post_Code"],
                        ShipToCountryRegionCode = (string)config["Ship_to_Country_Region_Code"],
                        ShipToContact = (string)config["Ship_to_Contact"],
                        ApplicableForServDecl = (bool)config["Applicable_For_Serv_Decl"],
                        PayToName = (string)config["Pay_to_Name"],
                        PayToAddress = (string)config["Pay_to_Address"],
                        PayToAddress2 = (string)config["Pay_to_Address_2"],
                        PayToCity = (string)config["Pay_to_City"],
                        PayToCounty = (string)config["Pay_to_County"],
                        PayToPostCode = (string)config["Pay_to_Post_Code"],
                        PayToCountryRegionCode = (string)config["Pay_to_Country_Region_Code"],
                        PayToContactNo = (string)config["Pay_to_Contact_No"],
                        PayToContactPhoneNo = (string)config["PayToContactPhoneNo"],
                        PayToContactMobilePhoneNo = (string)config["PayToContactMobilePhoneNo"],
                        PayToContactEmail = (string)config["PayToContactEmail"],
                        PayToContact = (string)config["Pay_to_Contact"],
                        RemitToCode = (string)config["Remit_to_Code"],
                        RemitToName = (string)config["Remit_to_Name"],
                        RemitToAddress = (string)config["Remit_to_Address"],
                        RemitToAddress2 = (string)config["Remit_to_Address_2"],
                        RemitToCity = (string)config["Remit_to_City"],
                        RemitToCounty = (string)config["Remit_to_County"],
                        RemitToPostCode = (string)config["Remit_to_Post_Code"],
                        RemitToCountryRegionCode = (string)config["Remit_to_Country_Region_Code"],
                        RemitToContact = (string)config["Remit_to_Contact"],
                        Status = (string)config["Status"],
                        OrderNo = (string)config["Posted_Order_No"],
                        PRN_No = GetPoPrn((string)config["Posted_Order_No"]),
                        ExpenseRequisitionNumber = extractedValue,
                        Amount = formattedAmountLcy,

                    };
                    if (status == "Released")
                    {
                        assignedAsset.DirectorateCode1 = (string)config["Directorate_Code1"];
                        assignedAsset.DepartmentCode = (string)config["Department_Code"];
                        assignedAsset.BuyFromVendorNo = (string)config["Buy_from_Vendor_No"];
                        assignedAsset.AssignedUserID = (string)config["Assigned_User_ID"];
                        assignedAsset.PricesIncludingVAT = (bool)config["Prices_Including_VAT"];
                        assignedAsset.VATBusPostingGroup = (string)config["VAT_Bus_Posting_Group"];
                        assignedAsset.JournalTemplName = (string)config["Journal_Templ_Name"];
                        assignedAsset.ReasonCode = (string)config["Reason_Code"];
                        assignedAsset.PayToOptions = (string)config["PayToOptions"];
                        assignedAsset.TransactionSpecification = (string)config["Transaction_Specification"];
                        assignedAsset.TransactionType = (string)config["Transaction_Type"];
                        assignedAsset.TransportMethod = (string)config["Transport_Method"];
                        assignedAsset.EntryPoint = (string)config["Entry_Point"];
                        assignedAsset.Area = (string)config["Area"];

                    }

                }
            }

            return View(assignedAsset);
        }
        catch (Exception ex)
        {
            var errMsg = new Error();
            errMsg.Message = ex.Message.Replace("'", "");
            return View("~/Views/Common/ErrorMessange.cshtml", errMsg);
        }
    }

    public string GetPoPrn(string OrderNo)
    {
        string page = $"PurchaseOrder?$filter=No eq '{OrderNo}'&$format=json";

        HttpWebResponse httpResponse = Credentials.GetOdataData(page);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                var requisitionNo = (string)config["Requisition_No"];
                if (!string.IsNullOrEmpty(requisitionNo))
                {
                    return requisitionNo;
                }
            }
        }

        return null;
    }


    [HttpPost]
    public JsonResult SubmitProcurementPlanAmmmendment(ProcurementPlanAmmendments ammendment)
    {
        try
        {
            string StaffNo = Session["Username"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string DocNo = "";

            /*DocNo = Credentials.ObjNav.FnCreateProcPlanRevision("", ammendment.PlanNo, ammendment.Ammendment_Reason,
                StaffNo, ammendment.Global_Dimension_1_Code, ammendment.Global_Dimension_2_Code,
                ammendment.Description);*/

            string Redirect = "/Purchase/ProcurementPlanAmmendmentDocument?DocNo=" + DocNo;

            return Json(new { message = DocNo, success = true }, JsonRequestBehavior.AllowGet);


            /* return Json(new { message = "Document not created. Please try again later...", success = false },
                 JsonRequestBehavior.AllowGet);*/
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult GetItems(string ProcurementCategory)
    {
        try
        {
            #region ItemsLookup

            List<DropdownList> ItemsLookup = new List<DropdownList>();
            string pageItems = $"Items?$filter=ProcurementCategory eq '{ProcurementCategory}'&$format=json";

            HttpWebResponse httpResponseItems = Credentials.GetOdataData(pageItems);
            using (var streamReader = new StreamReader(httpResponseItems.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                if (details["value"] != null)
                {
                    foreach (JObject config in details["value"])
                    {
                        DropdownList itemsDropdown = new DropdownList
                        {
                            Text = (string)config["Outputs"],
                            Value = (string)config["Outputs"]
                        };
                        ItemsLookup.Add(itemsDropdown); // Add the item to the list
                    }
                }
            }

            #endregion

            var response = new
            {
                ListOfItems = ItemsLookup.Select(x => new
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList(),
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            // Optionally log the error here
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult NewProcurementPlanAmmendmentEntry2()
    {
        try
        {

            string StaffNo = Session["Username"].ToString();
            string Dim1 = "", Dim2 = "";

            ProcurementPlanRevisionEntries newProcurementPlanAmmendmentEntry = new ProcurementPlanRevisionEntries();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            Dim1 = employeeView.GlobalDimension1Code;
            Dim2 = employeeView.GlobalDimension2Code;

            return PartialView("~/Views/Purchase/PartialViews/NewProcurementPlanAmmendmentEntry.cshtml");

        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public PartialViewResult NewProcurementPlanAmmendmentEntry(string ProcurementPlanID, string RevisedPlanNo, string PlanningCategory)
    {

        try
        {
            ProcurementPlanRevisionEntries newPPAmmendmentEntry = new ProcurementPlanRevisionEntries();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            newPPAmmendmentEntry.ProcurementPlanID = ProcurementPlanID;
            newPPAmmendmentEntry.Planning_Category = PlanningCategory;
            newPPAmmendmentEntry.RevisedPlanNo = RevisedPlanNo;

            #region VoteItemsLookup

            List<DropdownList> voteItems = new List<DropdownList>();
            string pagevoteItems =
                $"ProcurementPlanEntry?$filter= ProcurementPlanID eq '{RevisedPlanNo}' and GlobalDimension1Code eq '{employeeView.GlobalDimension1Code}' and PlanningCategory eq '{PlanningCategory}'&$format=json";

            HttpWebResponse httpResponsevoteItems = Credentials.GetOdataData(pagevoteItems);
            using (var streamReader = new StreamReader(httpResponsevoteItems.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Description"] + "-" + (string)config["EntryNo"];
                    dropDown.Value = (string)config["EntryNo"];
                    voteItems.Add(dropDown);
                }
            }

            #endregion

            #region ItemsLookup

            List<DropdownList> items = new List<DropdownList>();
            string pageItems = $"Items?$filter=Gen_Prod_Posting_Group eq '{PlanningCategory}'&$format=json";

            HttpWebResponse httpResponseItems = Credentials.GetOdataData(pageItems);
            using (var streamReader = new StreamReader(httpResponseItems.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Description"] + "-" + (string)config["No"];
                    dropDown.Value = (string)config["No"];
                    items.Add(dropDown);
                }
            }

            #endregion

            #region ProcTypes

            List<DropdownList> ProcTypes = new List<DropdownList>();
            string pageProcTypes = "ProcurementTypes?$format=json";

            HttpWebResponse httpResponseProcTypes = Credentials.GetOdataData(pageProcTypes);
            using (var streamReader = new StreamReader(httpResponseProcTypes.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);


                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Description"];

                    switch (dropDown.Text)
                    {
                        case "GOODS":
                            dropDown.Value = "1";
                            break;

                        case "SERVICES":
                            dropDown.Value = "2";
                            break;

                        case "WORKS":
                            dropDown.Value = "3";
                            break;

                        default:
                            dropDown.Value = "0";
                            break;
                    }
                    ProcTypes.Add(dropDown);
                }
            }

            #endregion

            #region SolicitationTypesLookup

            List<DropdownList> SolTypes = new List<DropdownList>();
            string SolTypesEndpoint = "SolicitationType?$format=json";

            HttpWebResponse httpResponseSolTypes = Credentials.GetOdataData(SolTypesEndpoint);
            using (var streamReader = new StreamReader(httpResponseSolTypes.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Description"] + "-" + (string)config["Code"];
                    dropDown.Value = (string)config["Code"];
                    SolTypes.Add(dropDown);
                }
            }


            #endregion

            #region SpecialVendorLookup

            List<DropdownList> SpecialVendor = new List<DropdownList>();
            string SpecialVendorEndpoint = "SpecialVendor?$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(SpecialVendorEndpoint);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Description"];
                    dropDown.Value = (string)config["Code"];
                    SpecialVendor.Add(dropDown);
                }
            }

            #endregion

            #region FundingSourceIDLookup

            List<DropdownList> FundingSourceID = new List<DropdownList>();
            string PageFundingSourceID = "FundingSource?$format=json";

            HttpWebResponse httpResponseFundingSourceID = Credentials.GetOdataData(PageFundingSourceID);
            using (var streamReader = new StreamReader(httpResponseFundingSourceID.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Description"];
                    dropDown.Value = (string)config["Code"];
                    FundingSourceID.Add(dropDown);
                }
            }

            #endregion


            #region VoteLookup

            List<DropdownList> vote = new List<DropdownList>();
            /*  string pageVote = "FundingSource?$format=json";*/
            string pageVote = "DimensionValues?$filter=Global_Dimension_No eq 3 and Blocked eq false&$format=json";

            HttpWebResponse httpResponseVote = Credentials.GetOdataData(pageVote);
            using (var streamReader = new StreamReader(httpResponseVote.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Name"];
                    dropDown.Value = (string)config["Code"];
                    vote.Add(dropDown);
                }
            }

            #endregion

            #region SubProgramLookup

            List<DropdownList> SubProgram = new List<DropdownList>();
            string SubProgramEndpoint =
                "DimensionValues?$filter=Global_Dimension_No eq 4 and Blocked eq false&$format=json";

            HttpWebResponse httpResponseSubProgram = Credentials.GetOdataData(SubProgramEndpoint);
            using (var streamReader = new StreamReader(httpResponseSubProgram.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Name"] + " (" + (string)config["Code"] + ")";
                    dropDown.Value = (string)config["Code"];
                    SubProgram.Add(dropDown);
                }
            }

            #endregion

            #region ClassLookup

            List<DropdownList> _class = new List<DropdownList>();
            string ClassEndpoint =
                "DimensionValues?$filter=Global_Dimension_No eq 5 and Blocked eq false&$format=json";

            HttpWebResponse httpResponseClass = Credentials.GetOdataData(ClassEndpoint);
            using (var streamReader = new StreamReader(httpResponseClass.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Name"] + " (" + (string)config["Code"] + ")";
                    dropDown.Value = (string)config["Code"];
                    _class.Add(dropDown);
                }
            }

            #endregion

            #region FundingSourceLookup

            List<DropdownList> FundingSource = new List<DropdownList>();
            string PageFundingSource =
                "DimensionValues?$filter=Global_Dimension_No eq 6 and Blocked eq false&$format=json";

            HttpWebResponse httpResponseFundingSource = Credentials.GetOdataData(PageFundingSource);
            using (var streamReader = new StreamReader(httpResponseFundingSource.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    DropdownList dropDown = new DropdownList();
                    dropDown.Text = (string)config["Name"] + " (" + (string)config["Code"] + ")";
                    dropDown.Value = (string)config["Code"];
                    FundingSource.Add(dropDown);
                }
            }

            #endregion



            newPPAmmendmentEntry.ListOfVoteItems = voteItems.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newPPAmmendmentEntry.ListOfItems = items.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newPPAmmendmentEntry.ListOfProcTypes = ProcTypes.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newPPAmmendmentEntry.ListOfSolicitationTypes = SolTypes.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newPPAmmendmentEntry.ListOfSpecialVendor = SpecialVendor.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newPPAmmendmentEntry.ListOfFundingSourceID = FundingSourceID.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();


            newPPAmmendmentEntry.ListOfVote = vote.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();


            newPPAmmendmentEntry.ListOfSubProgram = SubProgram.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            newPPAmmendmentEntry.ListOfClass = _class.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();

            newPPAmmendmentEntry.ListOfFundingSource = FundingSource.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();




            return PartialView("~/Views/Purchase/PartialViews/NewProcurementPlanAmmendmentEntry.cshtml",
                newPPAmmendmentEntry);
        }
        catch (Exception ex)
        {
            var errorMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", errorMsg);
        }
    }

    [HttpPost]
    public JsonResult SubmitProcurementPlanAmmendmentEntry(ProcurementPlanRevisionEntries planEntry,
        string planningCategory)
    {

        string Procurement_Type;

        switch (planEntry.Procurement_Type)
        {

            case "0": // Handles both "Edited" and "0"
                Procurement_Type = "GOODS";
                break;


            case "1": // Handles both "Delete" and "1"
                Procurement_Type = "SERVICES";
                break;

            case "2":
                Procurement_Type = "WORKS";
                break;

            case "3":
                Procurement_Type = "ASSETS";
                break;
            default:
                Procurement_Type = "GOODS";
                break;
        }





        try
        {
            string StaffNo = Session["Username"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            bool res = false;

            /*res =
                Credentials.ObjNav.FnCreateLinesProcplanLines(
                    planEntry.ProcurementPlanID,
                    planEntry.Planning_Category,
                    1, //null- this is for document type int.Parse(planEntry.docType)
                    planEntry.Planning_Category, //this is for GL account
                    planEntry.docNo,
                    int.Parse(planEntry.PlanItemType), //int.Parse(planEntry.PlanItemType),-------- For PlanItemType
                    planEntry.PlanItemNo,//brings error
                    Procurement_Type,  //To be removed
                    int.Parse(planEntry.Procurement_Method),
                    "DONOR", //funding source ID
                    int.Parse(planEntry.Procurement_Type), *//*-------int.Parse(planEntry.Requisition_Product_Group),*//*
                    planEntry.Quantity,
                    planEntry.Unit_Cost,
                    employeeView.GlobalDimension1Code,
                    employeeView.GlobalDimension2Code,
                    planEntry.Shortcut_Dimension_3_Code, // Vote
                    planEntry.Shortcut_Dimension_4_Code, // Sub Program
                    planEntry.Shortcut_Dimension_5_Code, //class
                    planEntry.Shortcut_Dimension_6_Code, //funding source Dim

                    planEntry.Q1_Quantity,
                    planEntry.Q2_Quantity,
                    planEntry.Q3_Quantity,
                    planEntry.Q4_Quantity,

                    planEntry.Q1_Amount,
                    planEntry.Q2_Amount,
                    planEntry.Q3_Amount,
                    planEntry.Q4_Amount,

                    planEntry.AGPO_Percent,
                    planEntry.PWD_Percent,
                    planEntry.Women_Percent,
                    planEntry.Youth_Percent,
                    planEntry.Citizen_Contractors_Percent,
                    int.Parse(planEntry.Action_Taken),
                    employeeView.UserID
                );
*/

            if (res)
            {
                string Redirect = planEntry.docNo;

                return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Record not updated. Please try again later...", success = false },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    [HttpPost]
    public JsonResult SubmitUpdatedEntry(ProcurementPlanRevisionEntries planEntry, string planningCategory)
    {
        try
        {
            string StaffNo = Session["Username"].ToString();
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            bool res = false;

            int actionTaken = 0;  //for edited
            string Requisition_Product_Group = "";  //for Requisition_Product_Group

            /* switch (planEntry.Action_Taken)
             {
                 case "Edited":
                 case "0": // Handles both "Edited" and "0"
                     actionTaken = 0;
                     break;

                 case "Delete":
                 case "1": // Handles both "Delete" and "1"
                     actionTaken = 1;
                     break;

                 case "Addition":
                 case "3": // Handles both "Addition" and "3"
                     actionTaken = 3;
                     break;

                 default:
                     actionTaken = 2;
                     break;
             }*/

            if (planEntry.Procurement_Type == null) { planEntry.Procurement_Type = "GOODS"; }

            switch (planEntry.Procurement_Type)
            {

                case "Goods": // Handles both "Edited" and "0"
                    Requisition_Product_Group = "0";
                    break;


                case "Services": // Handles both "Delete" and "1"
                    Requisition_Product_Group = "1";
                    break;

                case "Works":
                    Requisition_Product_Group = "2";
                    break;

                case "Assets":
                    Requisition_Product_Group = "3";
                    break;
                default:
                    Requisition_Product_Group = "0";
                    break;
            }

            if (planEntry.Solicitation_Type == null)
            {
                planEntry.Solicitation_Type = "OPENTENDER";
            }

            if (planEntry.Alternative_Procurement_Methods == null)
            {
                planEntry.Alternative_Procurement_Methods = "N/a";
            }

           /* res = Credentials.ObjNav.FnUpdateLinesProcplanLines(
                planEntry.ProcurementPlanID,
                planningCategory,
                1, // int.Parse(planEntry.docType)
                planEntry.Solicitation_Type,
                planEntry.docNo,
                planEntry.EntryNo,
                int.Parse(planEntry.PlanItemType),
                planEntry.PlanItemNo,
                planEntry.Procurement_Type, *//*this is For Procurement Type----- planEntry.Procurement_Type, *//*
                int.Parse(planEntry.Procurement_Method),
                "", //funding source ID
                int.Parse(Requisition_Product_Group),*//*-------int.Parse(planEntry.Requisition_Product_Group),*//*
                planEntry.Quantity,
                planEntry.Unit_Cost,
                employeeView.GlobalDimension1Code,
                employeeView.GlobalDimension2Code,
                planEntry.Shortcut_Dimension_3_Code, //vote
                planEntry.Shortcut_Dimension_4_Code, //subprogram
                planEntry.Shortcut_Dimension_5_Code, //class
                planEntry.Shortcut_Dimension_6_Code, //funding source DIM

                planEntry.Q1_Quantity,
                planEntry.Q2_Quantity,
                planEntry.Q3_Quantity,
                planEntry.Q4_Quantity,

                planEntry.Q1_Amount,
                planEntry.Q2_Amount,
                planEntry.Q3_Amount,
                planEntry.Q4_Amount,
                planEntry.AGPO_Percent,
                planEntry.PWD_Percent,
                planEntry.Women_Percent,
                planEntry.Youth_Percent,
                planEntry.Citizen_Contractors_Percent,
                actionTaken,
                StaffNo,
                planEntry.Alternative_Procurement_Methods
            );
*/

            if (res)
            {
                string Redirect = planEntry.docNo;

                return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Record not updated. Please try again later...", success = false },
                JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult SendProcPlanRevisionApproval(string DocNo)
    {
        try
        {
            var userId = Session["UserID"].ToString();
            Credentials.ObjNav.SendProcurementPlanApproval(DocNo);
            Credentials.ObjNav.UpdateApprovalEntrySenderID(70098, DocNo, userId);

            var Redirect = DocNo;
            return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult CancelProcPlaRevApproval(string DocNo)
    {
        try
        {
            var userId = Session["UserID"].ToString();
           /* Credentials.ObjNav.CancelProcPlaRevApproval(DocNo);*/
            var Redirect = DocNo;
            return Json(new { message = Redirect, success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    [CustomAuthorization(Role = "PROCUREMENT")]
    public ActionResult ArchivedPurchaseOrder()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("Error", erroMsg);
        }
    }

    public PartialViewResult ArchivedPOList()
    {
        var userId = Session["UserID"].ToString();
        try
        {
            List<PurchaseOrderArchive> archivePosList = new List<PurchaseOrderArchive>();
            var page = "PurchaseOrderArchive?$filter=Archived_By eq '" + userId + "'&$format=json";

            using (var httpResponse = Credentials.GetOdataData(page))
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    PurchaseOrderArchive archivedPo = new PurchaseOrderArchive
                    {
                        No = (string)config["No"],
                        Document_Type = (string)config["Document_Type"],
                        Version_No = (int)config["Version_No"],
                        Date_Archived = (string)config["Date_Archived"],
                        Time_Archived = (string)config["Time_Archived"],
                        Archived_By = (string)config["Archived_By"],
                        Buy_from_Vendor_No = (string)config["Buy_from_Vendor_No"],
                        Buy_from_Vendor_Name = (string)config["Buy_from_Vendor_Name"]
                    };
                    archivePosList.Add(archivedPo);
                }
            }


            return PartialView("~/Views/Purchase/PartialViews/ArchivedPOList.cshtml", archivePosList);

        }
        catch (Exception ex)
        {
            Error erroMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    [HttpPost]
    [CustomAuthorization(Role = "PROCUREMENT")]
    public ActionResult ArchivedPODocumentView(string No)
    {
        if (Session["Username"] == null)
        {
            return RedirectToAction("Login", "Login");
        }

        try
        {
            string userId = Session["UserId"].ToString();
            PurchaseOrderArchive archivedPOItem = new PurchaseOrderArchive();

            string page = $"PurchaseOrderArchive?$filter=No eq '{No}'&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    archivedPOItem.Document_Type = (string)config["Document_Type"];
                    archivedPOItem.No = (string)config["No"];
                    archivedPOItem.Doc_No_Occurrence = (int?)config["Doc_No_Occurrence"] ?? 0;
                    archivedPOItem.Version_No = (int?)config["Version_No"] ?? 0;
                    archivedPOItem.Buy_from_Vendor_No = (string)config["Buy_from_Vendor_No"];
                    archivedPOItem.Buy_from_Contact_No = (string)config["Buy_from_Contact_No"];
                    archivedPOItem.Buy_from_Vendor_Name = (string)config["Buy_from_Vendor_Name"];
                    archivedPOItem.Buy_from_Address = (string)config["Buy_from_Address"];
                    archivedPOItem.Buy_from_Address_2 = (string)config["Buy_from_Address_2"];
                    archivedPOItem.Buy_from_City = (string)config["Buy_from_City"];
                    archivedPOItem.Buy_from_County = (string)config["Buy_from_County"];
                    archivedPOItem.Buy_from_Post_Code = (string)config["Buy_from_Post_Code"];
                    archivedPOItem.Buy_from_Country_Region_Code = (string)config["Buy_from_Country_Region_Code"];
                    archivedPOItem.Buy_from_Contact = (string)config["Buy_from_Contact"];
                    archivedPOItem.BuyFromContactPhoneNo = (string)config["BuyFromContactPhoneNo"];
                    archivedPOItem.BuyFromContactMobilePhoneNo = (string)config["BuyFromContactMobilePhoneNo"];
                    archivedPOItem.BuyFromContactEmail = (string)config["BuyFromContactEmail"];
                    archivedPOItem.Posting_Date = (string)config["Posting_Date"];
                    archivedPOItem.VAT_Reporting_Date = (string)config["VAT_Reporting_Date"];
                    archivedPOItem.Order_Date = (string)config["Order_Date"];
                    archivedPOItem.Document_Date = (string)config["Document_Date"];
                    archivedPOItem.Vendor_Order_No = (string)config["Vendor_Order_No"];
                    archivedPOItem.Vendor_Shipment_No = (string)config["Vendor_Shipment_No"];
                    archivedPOItem.Vendor_Invoice_No = (string)config["Vendor_Invoice_No"];
                    archivedPOItem.Order_Address_Code = (string)config["Order_Address_Code"];
                    archivedPOItem.Purchaser_Code = (string)config["Purchaser_Code"];
                    archivedPOItem.Responsibility_Center = (string)config["Responsibility_Center"];
                    archivedPOItem.Status = (string)config["Status"];
                    archivedPOItem.Pay_to_Vendor_No = (string)config["Pay_to_Vendor_No"];
                    archivedPOItem.Pay_to_Contact_No = (string)config["Pay_to_Contact_No"];
                    archivedPOItem.Pay_to_Name = (string)config["Pay_to_Name"];
                    archivedPOItem.Pay_to_Address = (string)config["Pay_to_Address"];
                    archivedPOItem.Pay_to_Address_2 = (string)config["Pay_to_Address_2"];
                    archivedPOItem.Pay_to_City = (string)config["Pay_to_City"];
                    archivedPOItem.Pay_to_County = (string)config["Pay_to_County"];
                    archivedPOItem.Pay_to_Post_Code = (string)config["Pay_to_Post_Code"];
                    archivedPOItem.Pay_to_Country_Region_Code = (string)config["Pay_to_Country_Region_Code"];
                    archivedPOItem.Pay_to_Contact = (string)config["Pay_to_Contact"];
                    archivedPOItem.PayToContactPhoneNo = (string)config["PayToContactPhoneNo"];
                    archivedPOItem.PayToContactMobilePhoneNo = (string)config["PayToContactMobilePhoneNo"];
                    archivedPOItem.PayToContactEmail = (string)config["PayToContactEmail"];
                    archivedPOItem.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                    archivedPOItem.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                    archivedPOItem.Payment_Terms_Code = (string)config["Payment_Terms_Code"];
                    archivedPOItem.Due_Date = (string)config["Due_Date"];
                    archivedPOItem.Payment_Discount_Percent = (int?)config["Payment_Discount_Percent"] ?? 0;
                    archivedPOItem.Payment_Method_Code = (string)config["Payment_Method_Code"];
                    archivedPOItem.On_Hold = (string)config["On_Hold"];
                    archivedPOItem.Prices_Including_VAT = (bool?)config["Prices_Including_VAT"] ?? false;
                    archivedPOItem.Tax_Liable = (bool?)config["Tax_Liable"] ?? false;
                    archivedPOItem.Tax_Area_Code = (string)config["Tax_Area_Code"];
                    archivedPOItem.Archived_By = (string)config["Archived_By"];
                    archivedPOItem.Date_Archived = (string)config["Date_Archived"];
                    archivedPOItem.Time_Archived = (string)config["Time_Archived"];
                    archivedPOItem.Interaction_Exist = (bool?)config["Interaction_Exist"] ?? false;
                }
            }

            return View(archivedPOItem);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }

    public ActionResult ArchivedPurchaseOrderLines(string DocNo, string Status)
    {
        var StaffNo = Session["Username"].ToString();

        var purchaseOrderLines = new List<PurchaseOrderLines>();

        var PurchaseOrderLinePage = "ArchivedPurchaseOrderLines?$filter=Document_No eq '" + DocNo + "'&$format=json";

        var httpResponse = Credentials.GetOdataData(PurchaseOrderLinePage);
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            var details = JObject.Parse(result);
            foreach (JObject config in details["value"])
            {
                var purchaseOrderLine = new PurchaseOrderLines();

                purchaseOrderLine.Document_Type = (string)config["Document_Type"];
                purchaseOrderLine.Document_No = (string)config["Document_No"];
                purchaseOrderLine.Line_No = (int?)config["Line_No"] ?? 0;
                purchaseOrderLine.Type = (string)config["Type"];
                purchaseOrderLine.No = (string)config["No"];
                purchaseOrderLine.Item_Reference_No = (string)config["Item_Reference_No"];
                purchaseOrderLine.IC_Partner_Code = (string)config["IC_Partner_Code"];
                purchaseOrderLine.IC_Partner_Ref_Type = (string)config["IC_Partner_Ref_Type"];
                purchaseOrderLine.IC_Partner_Reference = (string)config["IC_Partner_Reference"];
                purchaseOrderLine.Variant_Code = (string)config["Variant_Code"];
                purchaseOrderLine.Nonstock = (bool?)config["Nonstock"] ?? false;
                purchaseOrderLine.VAT_Percent = (int?)config["VAT_Percent"] ?? 0;
                purchaseOrderLine.VAT_Prod_Posting_Group = (string)config["VAT_Prod_Posting_Group"];
                purchaseOrderLine.Description = (string)config["Description"];
                purchaseOrderLine.Description_2 = (string)config["Description_2"];
                purchaseOrderLine.Drop_Shipment = (bool?)config["Drop_Shipment"] ?? false;
                purchaseOrderLine.Return_Reason_Code = (string)config["Return_Reason_Code"];
                purchaseOrderLine.Location_Code = (string)config["Location_Code"];
                purchaseOrderLine.Bin_Code = (string)config["Bin_Code"];
                purchaseOrderLine.Quantity = (int?)config["Quantity"] ?? 0;
                purchaseOrderLine.Unit_of_Measure_Code = (string)config["Unit_of_Measure_Code"];
                purchaseOrderLine.Reserved_Quantity = (int?)config["Reserved_Quantity"] ?? 0;
                purchaseOrderLine.Job_Remaining_Qty = (int?)config["Job_Remaining_Qty"] ?? 0;
                purchaseOrderLine.Unit_of_Measure = (string)config["Unit_of_Measure"];
                purchaseOrderLine.Approved_Requisition_Amount = (int?)config["Approved_Requisition_Amount"] ?? 0;
                purchaseOrderLine.Approved_Unit_Cost = (int?)config["Approved_Unit_Cost"] ?? 0;
                purchaseOrderLine.Direct_Unit_Cost = (int?)config["Direct_Unit_Cost"] ?? 0;
                purchaseOrderLine.Indirect_Cost_Percent = (int?)config["Indirect_Cost_Percent"] ?? 0;
                purchaseOrderLine.Unit_Cost_LCY = (int)config["Unit_Cost_LCY"];
                purchaseOrderLine.Unit_Price_LCY = (int?)config["Unit_Price_LCY"] ?? 0;
                purchaseOrderLine.Line_Amount = (int?)config["Line_Amount"] ?? 0;
                purchaseOrderLine.Tax_Liable = (bool?)config["Tax_Liable"] ?? false;
                purchaseOrderLine.Tax_Area_Code = (string)config["Tax_Area_Code"];
                purchaseOrderLine.Tax_Group_Code = (string)config["Tax_Group_Code"];
                purchaseOrderLine.Use_Tax = (bool?)config["Use_Tax"] ?? false;
                purchaseOrderLine.Line_Discount_Percent = (int?)config["Line_Discount_Percent"] ?? 0;
                purchaseOrderLine.Line_Discount_Amount = (int?)config["Line_Discount_Amount"] ?? 0;
                purchaseOrderLine.Procurement_Plan = (string)config["Procurement_Plan"];
                purchaseOrderLine.Procurement_Plan_Item = (string)config["Procurement_Plan_Item"];
                purchaseOrderLine.Prepayment_Percent = (int?)config["Prepayment_Percent"] ?? 0;
                purchaseOrderLine.Prepmt_Line_Amount = (int?)config["Prepmt_Line_Amount"] ?? 0;
                purchaseOrderLine.Prepmt_Amt_Inv = (int?)config["Prepmt_Amt_Inv"] ?? 0;
                purchaseOrderLine.Allow_Invoice_Disc = (bool?)config["Allow_Invoice_Disc"] ?? false;
                purchaseOrderLine.Inv_Discount_Amount = (int?)config["Inv_Discount_Amount"] ?? 0;
                purchaseOrderLine.Inv_Disc_Amount_to_Invoice = (int?)config["Inv_Disc_Amount_to_Invoice"] ?? 0;
                purchaseOrderLine.Qty_to_Receive = (int?)config["Qty_to_Receive"] ?? 0;
                purchaseOrderLine.Quantity_Received = (decimal?)config["Quantity_Received"] ?? 0;
                purchaseOrderLine.Qty_to_Invoice = (decimal?)config["Qty_to_Invoice"] ?? 0;
                purchaseOrderLine.Quantity_Invoiced = (int?)config["Quantity_Invoiced"] ?? 0;
                purchaseOrderLine.Qty_Rcd_Not_Invoiced = (int?)config["Qty_Rcd_Not_Invoiced"] ?? 0;
                purchaseOrderLine.Prepmt_Amt_to_Deduct = (int?)config["Prepmt_Amt_to_Deduct"] ?? 0;
                purchaseOrderLine.Prepmt_Amt_Deducted = (int?)config["Prepmt_Amt_Deducted"] ?? 0;
                purchaseOrderLine.Allow_Item_Charge_Assignment =
                    (bool?)config["Allow_Item_Charge_Assignment"] ?? false;
                purchaseOrderLine.Qty_to_Assign = (int?)config["Qty_to_Assign"] ?? 0;
                purchaseOrderLine.Item_Charge_Qty_to_Handle = (int?)config["Item_Charge_Qty_to_Handle"] ?? 0;
                purchaseOrderLine.Qty_Assigned = (int?)config["Qty_Assigned"] ?? 0;
                purchaseOrderLine.Job_No = (string)config["Job_No"];
                purchaseOrderLine.Job_Task_No = (string)config["Job_Task_No"];
                purchaseOrderLine.Job_Planning_Line_No = (int?)config["Job_Planning_Line_No"] ?? 0;
                purchaseOrderLine.Job_Line_Type = (string)config["Job_Line_Type"];
                purchaseOrderLine.Job_Unit_Price = (int?)config["Job_Unit_Price"] ?? 0;
                purchaseOrderLine.Job_Line_Amount = (int?)config["Job_Line_Amount"] ?? 0;
                purchaseOrderLine.Job_Line_Discount_Amount = (int?)config["Job_Line_Discount_Amount"] ?? 0;
                purchaseOrderLine.Job_Line_Discount_Percent = (int?)config["Job_Line_Discount_Percent"] ?? 0;
                purchaseOrderLine.Job_Total_Price = (int?)config["Job_Total_Price"] ?? 0;
                purchaseOrderLine.Job_Unit_Price_LCY = (int?)config["Job_Unit_Price_LCY"] ?? 0;
                purchaseOrderLine.Job_Total_Price_LCY = (int?)config["Job_Total_Price_LCY"] ?? 0;
                purchaseOrderLine.Job_Line_Amount_LCY = (int?)config["Job_Line_Amount_LCY"] ?? 0;
                purchaseOrderLine.Job_Line_Disc_Amount_LCY = (int?)config["Job_Line_Disc_Amount_LCY"] ?? 0;
                purchaseOrderLine.Requested_Receipt_Date = (string)config["Requested_Receipt_Date"];
                purchaseOrderLine.Promised_Receipt_Date = (string)config["Promised_Receipt_Date"];
                purchaseOrderLine.Planned_Receipt_Date = (string)config["Planned_Receipt_Date"];
                purchaseOrderLine.Expected_Receipt_Date = (string)config["Expected_Receipt_Date"];
                purchaseOrderLine.Order_Date = (string)config["Order_Date"];
                purchaseOrderLine.Lead_Time_Calculation = (string)config["Lead_Time_Calculation"];
                purchaseOrderLine.Planning_Flexibility = (string)config["Planning_Flexibility"];
                purchaseOrderLine.Prod_Order_No = (string)config["Prod_Order_No"];
                purchaseOrderLine.Prod_Order_Line_No = (int?)config["Prod_Order_Line_No"] ?? 0;
                purchaseOrderLine.Operation_No = (string)config["Operation_No"];
                purchaseOrderLine.Work_Center_No = (string)config["Work_Center_No"];
                purchaseOrderLine.Finished = (bool?)config["Finished"] ?? false;
                purchaseOrderLine.Whse_Outstanding_Qty_Base = (int?)config["Whse_Outstanding_Qty_Base"] ?? 0;
                purchaseOrderLine.Inbound_Whse_Handling_Time = (string)config["Inbound_Whse_Handling_Time"];
                purchaseOrderLine.Blanket_Order_No = (string)config["Blanket_Order_No"];
                purchaseOrderLine.Blanket_Order_Line_No = (int?)config["Blanket_Order_Line_No"] ?? 0;
                purchaseOrderLine.Appl_to_Item_Entry = (int?)config["Appl_to_Item_Entry"] ?? 0;
                purchaseOrderLine.Deferral_Code = (string)config["Deferral_Code"];
                purchaseOrderLine.Depreciation_Book_Code = (string)config["Depreciation_Book_Code"];
                purchaseOrderLine.Shortcut_Dimension_1_Code = (string)config["Shortcut_Dimension_1_Code"];
                purchaseOrderLine.Shortcut_Dimension_2_Code = (string)config["Shortcut_Dimension_2_Code"];
                purchaseOrderLine.ShortcutDimCode3 = (string)config["ShortcutDimCode3"];
                purchaseOrderLine.ShortcutDimCode4 = (string)config["ShortcutDimCode4"];
                purchaseOrderLine.ShortcutDimCode5 = (string)config["ShortcutDimCode5"];
                purchaseOrderLine.ShortcutDimCode6 = (string)config["ShortcutDimCode6"];
                purchaseOrderLine.ShortcutDimCode7 = (string)config["ShortcutDimCode7"];
                purchaseOrderLine.ShortcutDimCode8 = (string)config["ShortcutDimCode8"];
                purchaseOrderLine.Gen_Bus_Posting_Group = (string)config["Gen_Bus_Posting_Group"];
                purchaseOrderLine.Gen_Prod_Posting_Group = (string)config["Gen_Prod_Posting_Group"];
                purchaseOrderLine.Over_Receipt_Quantity = (int?)config["Over_Receipt_Quantity"] ?? 0;
                purchaseOrderLine.Over_Receipt_Code = (string)config["Over_Receipt_Code"];
                purchaseOrderLine.Gross_Weight = (int?)config["Gross_Weight"] ?? 0;
                purchaseOrderLine.Net_Weight = (int?)config["Net_Weight"] ?? 0;
                purchaseOrderLine.Unit_Volume = (int?)config["Unit_Volume"] ?? 0;
                purchaseOrderLine.Units_per_Parcel = (int?)config["Units_per_Parcel"] ?? 0;
                purchaseOrderLine.FA_Posting_Date = (string)config["FA_Posting_Date"];
                purchaseOrderLine.AmountBeforeDiscount = (int?)config["AmountBeforeDiscount"] ?? 0;
                purchaseOrderLine.Invoice_Discount_Amount = (int?)config["Invoice_Discount_Amount"] ?? 0;
                purchaseOrderLine.Invoice_Disc_Pct = (int?)config["Invoice_Disc_Pct"] ?? 0;
                purchaseOrderLine.Total_Amount_Excl_VAT = (int?)config["Total_Amount_Excl_VAT"] ?? 0;
                purchaseOrderLine.Total_VAT_Amount = (int?)config["Total_VAT_Amount"] ?? 0;
                purchaseOrderLine.Total_Amount_Incl_VAT = (int?)config["Total_Amount_Incl_VAT"] ?? 0;
                purchaseOrderLine.DocStatus = Status;

                purchaseOrderLines.Add(purchaseOrderLine);
            }
        }


        ViewBag.DocumentStatus = Status;
        var purchaseOrderLineList = new PurchaseOrderLineList();
        purchaseOrderLineList.ListOfPurchaseOrderLine = purchaseOrderLines;
        return PartialView("~/Views/PurchaseOrder/PartialViews/PurchaseOrderLines.cshtml", purchaseOrderLines);
    }

    //Generate PlanRevision report

    public ActionResult GetAmmendmentReportByType(string documentNumber, string reportType)
    {
        try
        {
            string message = "";
            bool success = false;
            bool isExcel = reportType == "1";

            /*message = Credentials.ObjNav.GeneratePlanRevisionReport(documentNumber);*/
            /* message = Credentials.ObjNav.GenerateAnnualFunctionalPlan(documentNumber, Convert.ToInt32(reportType));*/

            if (string.IsNullOrEmpty(message))
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }
            return Json(new { message, success, view = false }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }


    public JsonResult GeneratePlanRevisionReport(string DocNo)
    {
        try
        {
            string message = "";
            bool success = false, view = false;
           /* message = Credentials.ObjNav.GeneratePlanRevisionReport(DocNo);*/
            if (message == "")
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }
            return Json(new { message = message, success, view }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public JsonResult GenerateArchivedReport(string DocNo, int Version)
    {
        try
        {
            string message = "";
            bool success = false, view = false;
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;

           /* message = Credentials.ObjNav.GenerateArchivedLPOReport(DocNo, Version);*/
            if (message == "")
            {
                success = false;
                message = "File Not Found";
            }
            else
            {
                success = true;
            }

            return Json(new { message = message, success, view }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    public PartialViewResult POAttachmentLines(string DocNo, string Status)
    {
        try
        {
            List<POAttachmentLine> poAttachmentLines = new List<POAttachmentLine>();
            string pageLine = $"Attachments?$filter=Document_No eq '{DocNo}'&$format=json";
            HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);

            using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);

                foreach (JObject config in details["value"])
                {
                    POAttachmentLine poAttachmentsItem = new POAttachmentLine
                    {
                        LineNo = (int)config["LineNo"],
                        Document_No = (string)config["Document_No"],
                        Link = (string)config["Link"],
                        LPO_Type = (string)config["LPO_Type"],
                        Module = (string)config["Module"],
                        FileType = (string)config["FileType"],
                        User = (string)config["User"],
                        FileName = (string)config["FileName"],
                        DocumentID = (string)config["DocumentID"],
                        Order_No = (string)config["Order_No"]
                    };

                    poAttachmentLines.Add(poAttachmentsItem);
                }
            }

            ViewBag.Status = Status;

            return PartialView("~/Views/Purchase/PartialViews/POAttachmentLines.cshtml", poAttachmentLines);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error { Message = ex.Message };
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


    //Inspection Committee
    public ActionResult InspectionCommittee()
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }

    }
    public PartialViewResult InspectionCommitteeList()
    {

        try
        {
            List<IFSTenderCommitteeList> IFSTenderCommitteeList = new List<IFSTenderCommitteeList>();
            EmployeeView employee = Session["EmployeeData"] as EmployeeView;
            //string page = "IFSTenderCommittee?$filter=Created_By eq '" + employee.UserID + "'&$format=json";
            string page = "InspectionCommitteeCard?$filter=Committee_Type eq 'INSPECTION'&$orderby=Document_No desc&$format=json";

            HttpWebResponse httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSTenderCommitteeList IFSTender = new IFSTenderCommitteeList
                    {
                        Document_No = (string)config["Document_No"],
                        Committee_Type = (string)config["Committee_Type"],
                        Description = (string)config["Description"],
                        Appointment_Effective_Date = (string)config["Appointment_Effective_Date"],
                        Appointing_Authority = (string)config["Appointing_Authority"],
                        Tender_Name = (string)config["Tender_Name"],
                        Approval_Status = (string)config["Approval_Status"],
                        Primary_Region = (string)config["Primary_Region"],
                        Primary_Directorate = (string)config["Primary_Directorate"],
                        Primary_Department = (string)config["Primary_Department"],
                        /*Blocked = (bool)config["Blocked"],*/
                        Created_By = (string)config["Created_By"],
                        Created_Date = (string)config["Created_Date"],
                        Created_Time = (string)config["Created_Time"],
                        IFS_Code = (string)config["IFS_Code"],
                        Document_Date = (string)config["Document_Date"],











                    };
                    IFSTenderCommitteeList.Add(IFSTender);
                }
            }

            return PartialView("~/Views/Purchase/PartialViews/InspectionCommitteeList.cshtml",
                IFSTenderCommitteeList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public PartialViewResult InspectionCommitteeLines(string DocNo, string status)
    {

        try
        {
            List<IFSTenderCommitteeLines> IFSTenderCommitteeLines = new List<IFSTenderCommitteeLines>();
            string pageLine = "IFSTenderCommitteeLines?$filter=Document_No eq '" + DocNo + "'&$format=json";
            HttpWebResponse httpResponse = Credentials.GetOdataData(pageLine);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    IFSTenderCommitteeLines IFSTenderLine = new IFSTenderCommitteeLines
                    {
                        Document_No = (string)config["Document_No"],
                        Committee_Type = (string)config["Committee_Type"],
                        Line_No = (int)config["Line_No"],
                        Role = (string)config["Role"],
                        Member_No = (string)config["Member_No"],
                        Member_Name = (string)config["Member_Name"],
                        Designation = (string)config["Designation"],
                        Member_Email = (string)config["Member_Email"],
                    };
                    IFSTenderCommitteeLines.Add(IFSTenderLine);
                }
            }

            IFSTenderCommitteeLineList tenderCommitteeLineList = new IFSTenderCommitteeLineList();
            tenderCommitteeLineList.Status = status;
            tenderCommitteeLineList.ListOfTenderCommitteeLine = IFSTenderCommitteeLines;

            ViewBag.Document_Status = status;
            return PartialView("~/Views/Purchase/PartialViews/IFSTenderCommitteeLines.cshtml",
                tenderCommitteeLineList);
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }
    public ActionResult InspectionCommitteeDocumentView(string DocNo)
    {
        try
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                //string doc = EncryptionHelper.DecryptDocumentNo(DocNo);
                IFSTenderCommitteeCard IFSTender = new IFSTenderCommitteeCard();
                string page = "InspectionCommitteeCard?$filter=Document_No eq '" + DocNo + "'&format=json";

                HttpWebResponse httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        IFSTender.Document_No = (string)config["Document_No"];
                        IFSTender.Committee_Type = (string)config["Committee_Type"];
                        IFSTender.IFS_Code = (string)config["IFS_Code"];
                        IFSTender.Description = (string)config["Description"];
                        IFSTender.Document_Date = (string)config["Document_Date"];
                        IFSTender.Appointment_Effective_Date = (string)config["Appointment_Effective_Date"];
                        IFSTender.Appointing_Authority = (string)config["Appointing_Authority"];
                        IFSTender.Tender_Name = (string)config["Tender_Name"];
                        IFSTender.Duration = (int)config["Duration"];
                        IFSTender.Raised_By = (string)config["Raised_By"];
                        IFSTender.Name = (string)config["Name"];
                        IFSTender.Staff_Id = (string)config["Staff_Id"];
                        IFSTender.Designation = (string)config["Designation"];
                        IFSTender.Global_Dimension_1_Code = (string)config["Global_Dimension_1_Code"];
                        IFSTender.Global_Dimension_2_Code = (string)config["Global_Dimension_2_Code"];
                        IFSTender.Min_Required_No_of_Members = (int)config["Min_Required_No_of_Members"];
                        IFSTender.Actual_No_of_Committee_Memb = (int)config["Actual_No_of_Committee_Memb"];
                        IFSTender.Approval_Status = (string)config["Approval_Status"];
                        IFSTender.Created_By = (string)config["Raised_By"];
                        IFSTender.Created_Date = (string)config["Created_Date"];
                        IFSTender.Created_Time = (string)config["Created_Time"];
                    }
                }
                #region ProcurementCommitteeTypes    
                List<DropdownList> ProcurementCommitteeTypesList = new List<DropdownList>();
                string pageCommitteeTypes = "ProcurementCommitteeTypes?&$format=json";
                HttpWebResponse httpResponseCommitteeTypes = Credentials.GetOdataData(pageCommitteeTypes);
                using (var streamReader = new StreamReader(httpResponseCommitteeTypes.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Committee_Type"];
                        dropdownList.Value = (string)config["Committee_Type"];
                        ProcurementCommitteeTypesList.Add(dropdownList);
                    }
                }
                #endregion

                #region IfsCode    
                List<DropdownList> IfsCodeList = new List<DropdownList>();
                string pageIfsCode = "SourcingProcessList?$format=json";
                HttpWebResponse httpResponseIfsCode = Credentials.GetOdataData(pageIfsCode);
                using (var streamReader = new StreamReader(httpResponseIfsCode.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"];
                        dropdownList.Value = (string)config["Code"];
                        IfsCodeList.Add(dropdownList);
                    }
                }
                #endregion

                #region Dim1    
                List<DropdownList> Dim1List = new List<DropdownList>();
                string pageDim1 = "DimensionValueList?$filter= Dimension_Code eq 'GEOGRAPHICAL'&$format=json";
                HttpWebResponse httpResponseDim1 = Credentials.GetOdataData(pageDim1);
                using (var streamReader = new StreamReader(httpResponseDim1.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        Dim1List.Add(dropdownList);
                    }
                }
                #endregion

                #region Dim2    
                List<DropdownList> Dim2List = new List<DropdownList>();
                string pageDim2 = "DimensionValueList?$filter= Dimension_Code eq 'ADMINISTRATIVE'&$format=json";
                HttpWebResponse httpResponseDim2 = Credentials.GetOdataData(pageDim2);
                using (var streamReader = new StreamReader(httpResponseDim2.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        DropdownList dropdownList = new DropdownList();
                        dropdownList.Text = (string)config["Code"] + "-" + (string)config["Name"];
                        dropdownList.Value = (string)config["Code"];
                        Dim2List.Add(dropdownList);
                    }
                }
                #endregion

                IFSTender.ListOfCommitteeTypes = ProcurementCommitteeTypesList.Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Text,
                              Value = x.Value
                          }).ToList();

                IFSTender.ListOfIFSCodes = IfsCodeList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.Text,
                            Value = x.Value
                        }).ToList();


                IFSTender.ListOfDim1 = Dim1List.Select(x =>
                       new SelectListItem()
                       {
                           Text = x.Text,
                           Value = x.Value
                       }).ToList();

                IFSTender.ListOfDim2 = Dim2List.Select(x =>
                       new SelectListItem()
                       {
                           Text = x.Text,
                           Value = x.Value
                       }).ToList();

                return View(IFSTender);
            }
        }
        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
        }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult updateInspectionCommitteeHeader(IFSTenderCommitteeCard updatedHeader)
    {
        try
        {
            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            string responsibleEmployeeNo = employeeView.No;
            string userID = employeeView.UserID;
            bool result = false;
            var Document_No = updatedHeader.Document_No;
            var IFS_Code = updatedHeader.IFS_Code;
            var Committee_Type = updatedHeader.Committee_Type;
            var Description = updatedHeader.Description;
            var Tender_Name = updatedHeader.Tender_Name;
            var appointmentEffective_Date = DateTime.ParseExact(updatedHeader.Appointment_Effective_Date.Replace("-", "/"), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
            var Appointing_Authority = updatedHeader.Appointing_Authority;
            var Duration = updatedHeader.Duration;

            // result = Credentials.ObjNav.UpdateTenderCommitees(
            //     Document_No,
            //     IFS_Code,
            //     Committee_Type,
            //     Description,
            //     Tender_Name,
            //     appointmentEffective_Date,
            //     Appointing_Authority,
            //     Duration,
            // );



            if (result)
            {
                string redirectUrl = "/Purchase/IFSTenderCommittee?DocNo=" + updatedHeader.Document_No;
                return Json(new { message = redirectUrl, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error submitting record. Try again.", success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }
        catch (Exception ex)
        {
            return Json(new { message = ex.Message.Replace("'", ""), success = false },
                JsonRequestBehavior.AllowGet);
        }
    }


    public ActionResult NewInspectionCommittee()
    {
        try
        {

            IFSTenderCommitteeCard tenderCommittee = new IFSTenderCommitteeCard();
            Session["httpResponse"] = null;


            EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
            tenderCommittee.Created_By = employeeView.UserID;
            tenderCommittee.Global_Dimension_1_Code = employeeView.GlobalDimension1Code;
            List<DropdownList> committeeTypesList = new List<DropdownList>();
            string pageCommitteeTypes = "ProcurementCommitteeTypes?&$format=json";
            HttpWebResponse httpResponseCT = Credentials.GetOdataData(pageCommitteeTypes);
            using (var streamReader = new StreamReader(httpResponseCT.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["Committee_Type"];
                    dropdownList.Value = (string)config["Committee_Type"];
                    committeeTypesList.Add(dropdownList);
                }
            }



            IFSTenderCommitteeCard IFSCodes = new IFSTenderCommitteeCard();
            Session["httpResponse"] = null;
            tenderCommittee.Raised_By = employeeView.UserID;
            tenderCommittee.Location = DimensinValuesList.GetDimensionValueName(employeeView.GlobalDimension1Code);
            tenderCommittee.AdminUnit = DimensinValuesList.GetDimensionValueName(employeeView.AdministrativeUnit);
            tenderCommittee.Committee_Type = "INSPECTION";

            tenderCommittee.Name = employeeView.FirstName+" " + employeeView.LastName;
            tenderCommittee.Staff_Id = employeeView.No;
            List<DropdownList> IFSCodesList = new List<DropdownList>();
            string pageIFSCodes = $"PurchaseList?$filter=Document_Type eq 'Order' and Shortcut_Dimension_1_Code eq '{employeeView.GlobalDimension1Code}'&$format=json";
            HttpWebResponse httpResponseIFSCodes = Credentials.GetOdataData(pageIFSCodes);
            using (var streamReader = new StreamReader(httpResponseIFSCodes.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    DropdownList dropdownList = new DropdownList();
                    dropdownList.Text = (string)config["No"];
                    dropdownList.Value = (string)config["No"];
                    IFSCodesList.Add(dropdownList);
                }
            }


            tenderCommittee.ListOfCommitteeTypes = committeeTypesList.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();


            tenderCommittee.ListOfIFSCodes = IFSCodesList.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();



            return PartialView("~/Views/Purchase/PartialViews/NewIFSTenderCommittee.cshtml", tenderCommittee);

        }

        catch (Exception ex)
        {
            Error erroMsg = new Error();
            erroMsg.Message = ex.Message;
            return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
        }
    }


}