using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Latest_Staff_Portal.CustomSecurity;
using Latest_Staff_Portal.Models;
using Latest_Staff_Portal.ViewModel;
using Newtonsoft.Json.Linq;
namespace Latest_Staff_Portal.Controllers
{
    [CustomeAuthentication]
    [CustomAuthorization(Role = "ALLUSERS")]
    public class DocumentApprovalController : Controller
    {
        public ActionResult DocumentForApprovalSummery(string rn)
        {
            try
            {
                if (Session["UserID"] == null || Session["Username"] == null) return RedirectToAction("Login", "Login");

                if (string.IsNullOrEmpty(rn)) return RedirectToAction("Dashboard", "Dashboard");

                var userID = Session["UserID"].ToString();
                var DocCount = new DocumentCount();
                DocCount.LeaveCount = 0;
                DocCount.PRNCount = 0;
                DocCount.LPOCount = 0;
                DocCount.SRNCount = 0;
                DocCount.ImpCount = 0;
                DocCount.SurrCount = 0;
                DocCount.ClaimCount = 0;
                DocCount.Transport = 0;
                DocCount.Training = 0;
                DocCount.PVCount = 0;
                DocCount.TNA = 0;
                DocCount.FuelCount = 0;
                DocCount.MaintenanceCount = 0;
                DocCount.impMemoCount = 0;
                DocCount.LVReimbursementCount = 0;
                DocCount.LVTimeoffCount = 0;
                DocCount.LVCarryForwardCount = 0;
                DocCount.TimeSheet = 0;
                DocCount.Workshop = 0;
                DocCount.RFQCount = 0;
                DocCount.MobilityCount = 0;
                DocCount.PCACount = 0;
                DocCount.TrainingExtention = 0;
                DocCount.WelfareCount = 0;
                DocCount.Leaveplanner = 0;
                DocCount.ExpenseRequisition = 0;

                var page = "ApprovalEntries?$filter=Approver_ID eq '" + userID + "' and Status eq '" + rn +
                           "'&$format=json";
                var httpResponse = Credentials.GetOdataData(page);
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    if ((string)config["Table_ID"] == "69205") DocCount.LeaveCount += 1;
                    if ((string)config["Table_ID"] == "38" && (string)config["Document_Type"] == "Quote")
                        DocCount.PRNCount += 1;
                    if ((string)config["Table_ID"] == "38" && (string)config["Document_Type"] == "Order")
                        DocCount.LPOCount += 1;
                    if ((string)config["Table_ID"] == "70134954") DocCount.SRNCount += 1;
                    if ((string)config["Table_ID"] == "70135176") DocCount.ImpCount += 1;
                    if ((string)config["Table_ID"] == "70135168") DocCount.SurrCount += 1;
                    if ((string)config["Table_ID"] == "70135169") DocCount.ClaimCount += 1;
                    if ((string)config["Table_ID"] == "59003") DocCount.Transport += 1;
                    if ((string)config["Table_ID"] == "70135040") DocCount.Training += 1;
                    if ((string)config["Table_ID"] == "5740") DocCount.TransferOrder += 1;
                    if ((string)config["Table_ID"] == "70135171") DocCount.PVCount += 1;
                    if ((string)config["Table_ID"] == "70135057") DocCount.TNA += 1;
                    if ((string)config["Table_ID"] == "55277") DocCount.impMemoCount += 1;
                    var ddocno = (string)config["Document_No"];
                    if ((string)config["Table_ID"] == "50029")
                    {
                        var DocType = GetLeaveDocType(ddocno);
                        if (DocType == "Time Off Lie")
                            DocCount.LVTimeoffCount += 1;
                        else if (DocType == "Carry Forward")
                            DocCount.LVCarryForwardCount += 1;
                        else if (DocType == "Re-Imbursement") DocCount.LVReimbursementCount += 1;
                    }

                    if ((string)config["Table_ID"] == "70135250") DocCount.TimeSheet += 1;
                    if ((string)config["Table_ID"] == "70135164") DocCount.Workshop += 1;
                    if ((string)config["Table_ID"] == "173") DocCount.RFQCount += 1;
                    if ((string)config["Table_ID"] == "69055") DocCount.MobilityCount += 1;
                    if ((string)config["Table_ID"] == "50763") DocCount.PCACount += 1;
                    if ((string)config["Table_ID"] == "69011") DocCount.WelfareCount += 1;
                    if ((string)config["Table_ID"] == "50064") DocCount.TrainingExtention += 1;
                    if ((string)config["Table_ID"] == "69217") DocCount.Leaveplanner += 1;
                    if ((string)config["Table_ID"] == "50110") DocCount.ExpenseRequisition += 1;
                    if ((string)config["Table_ID"] == "69520") DocCount.LeaveRecall += 1;
                }

                DocCount.Status = rn;
                return View(DocCount);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }

        public PartialViewResult DocumentForApproval(string rn)
        {
            var userID = Session["UserID"].ToString();
            var DocCount = new DocumentCount();

            DocCount.LeaveCount = 0;
            DocCount.PRNCount = 0;
            DocCount.LPOCount = 0;
            DocCount.SRNCount = 0;
            DocCount.ImpCount = 0;
            DocCount.SurrCount = 0;
            DocCount.ClaimCount = 0;
            DocCount.Transport = 0;
            DocCount.Training = 0;
            DocCount.PVCount = 0;
            DocCount.TNA = 0;
            DocCount.FuelCount = 0;
            DocCount.MaintenanceCount = 0;
            DocCount.impMemoCount = 0;
            DocCount.LVReimbursementCount = 0;
            DocCount.LVTimeoffCount = 0;
            DocCount.LVCarryForwardCount = 0;
            DocCount.TimeSheet = 0;
            DocCount.Workshop = 0;
            DocCount.RFQCount = 0;
            DocCount.MobilityCount = 0;
            DocCount.PCACount = 0;
            DocCount.TrainingExtention = 0;
            DocCount.WelfareCount = 0;
            DocCount.Leaveplanner = 0;
            DocCount.ExpenseRequisition = 0;
            var page = "ApprovalEntries?$filter=Approver_ID eq '" + userID + "' and Status eq '" + rn +
                       "'&$format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    if ((string)config["Table_ID"] == "69205") DocCount.LeaveCount += 1;
                    if ((string)config["Table_ID"] == "38" && (string)config["Document_Type"] == "Quote")
                        DocCount.PRNCount += 1;
                    if ((string)config["Table_ID"] == "38" && (string)config["Document_Type"] == "Order")
                        DocCount.LPOCount += 1;
                    if ((string)config["Table_ID"] == "70134954") DocCount.SRNCount += 1;
                    if ((string)config["Table_ID"] == "70135176") DocCount.ImpCount += 1;
                    if ((string)config["Table_ID"] == "70135168") DocCount.SurrCount += 1;
                    if ((string)config["Table_ID"] == "70135169") DocCount.ClaimCount += 1;
                    if ((string)config["Table_ID"] == "59003") DocCount.Transport += 1;
                    if ((string)config["Table_ID"] == "70135040") DocCount.Training += 1;
                    if ((string)config["Table_ID"] == "5740") DocCount.TransferOrder += 1;
                    if ((string)config["Table_ID"] == "70135171") DocCount.PVCount += 1;
                    if ((string)config["Table_ID"] == "70135057") DocCount.TNA += 1;
                    if ((string)config["Table_ID"] == "55277") DocCount.impMemoCount += 1;
                    var ddocno = (string)config["Document_No"];
                    if ((string)config["Table_ID"] == "70135140" && ddocno.Contains("FUEL")) DocCount.FuelCount += 1;
                    if ((string)config["Table_ID"] == "70135140" && ddocno.Contains("MENT"))
                        DocCount.MaintenanceCount += 1;
                    if ((string)config["Table_ID"] == "50029")
                    {
                        var DocType = GetLeaveDocType(ddocno);
                        if (DocType == "Time Off Lie")
                            DocCount.LVTimeoffCount += 1;
                        else if (DocType == "Carry Forward")
                            DocCount.LVCarryForwardCount += 1;
                        else if (DocType == "Re-Imbursement") DocCount.LVReimbursementCount += 1;
                    }
                    if ((string)config["Table_ID"] == "70135250") DocCount.TimeSheet += 1;
                    if ((string)config["Table_ID"] == "70135164") DocCount.Workshop += 1;
                    if ((string)config["Table_ID"] == "173") DocCount.RFQCount += 1;
                    if ((string)config["Table_ID"] == "69055") DocCount.MobilityCount += 1;
                    if ((string)config["Table_ID"] == "50763") DocCount.PCACount += 1;
                    if ((string)config["Table_ID"] == "69011") DocCount.WelfareCount += 1;
                    if ((string)config["Table_ID"] == "50064") DocCount.TrainingExtention += 1;
                    if ((string)config["Table_ID"] == "69217") DocCount.Leaveplanner += 1;
                    if ((string)config["Table_ID"] == "50110") DocCount.ExpenseRequisition += 1;

                }


                DocCount.Status = rn;
            }

            return PartialView("~/Views/DocumentApproval/PartialViews/ApprovalRequestsView.cshtml", DocCount);
        }

        public ActionResult GetDocumentForApprovalList(string TbID, string Title, string Status, string DocType)
        {
            try
            {
                if (TbID != "" && Title != "" && Status != "")
                {
                    var newList = new DocumentsForApprovalList
                    {
                        TableID = TbID,
                        Title = Title,
                        Status = Status,
                        DocType = DocType
                    };
                    Session["TableId"] = TbID;
                    return View(newList);
                }
                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }

        protected string GetLeaveDocType(string DocNo)
        {
            var DocType = "";
            var page = "LeaveReimbursements?$filter=No eq '" + DocNo + "'&format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();

            var details = JObject.Parse(result);
            if (details["value"].Any())
                foreach (JObject config in details["value"])
                    DocType = (string)config["ApplicationType"];

            return DocType;
        }

        protected string GetStrategyWorkplanDocType(string DocNo)
        {

            var DocType = "";
            var page = "FunctionalAWPCard?$filter=No eq '" + DocNo + "'&format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();

            var details = JObject.Parse(result);
            if (details["value"].Any())
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    DocType = (string)config["Planning_Budget_Type"];
                }

            return DocType;
        }
        protected string GetPaymentType(string DocNo)
        {
            var DocType = "";
            var page = "PaymentsQuery?$filter=No eq '" + DocNo + "'&format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();

            var details = JObject.Parse(result);
            if (details["value"].Any())
                foreach (var jToken in details["value"])
                {
                    var config = (JObject)jToken;
                    DocType = (string)config["DocumentType"];
                }

            return DocType;
        }
        public ActionResult LoadDocumentForApprovalList(string TbID, string Status, string DocType, string Title)
        {
            try
            {
                if (TbID != "")
                {
                    var userID = Session["UserID"].ToString();
                    var approvalDocList = new List<DocumentsForApproval>();
                    var page = "";
                    if (DocType != "N")
                        page = $"ApprovalEntries?$filter=Approver_ID eq '{userID}' and Table_ID eq {TbID} and Status eq '{Status}' and Document_Type eq '{DocType}'&$format=json";
                    else
                        page = "ApprovalEntries?$filter=Table_ID eq " + Convert.ToInt32(TbID) +
                               " and Approver_ID eq '" + userID + "' and Status eq '" + Status + "'&$format=json";
                    if (TbID == "50029")
                        page = "Approval_Entries?$filter=Table_ID eq " + Convert.ToInt32(TbID) + " and Application_Type eq '" + Title + "' and Approver_ID eq '" + userID +
                               "' and Status eq '" + Status + "'&$format=json";
                    var httpResponse = Credentials.GetOdataData(page);
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        var details = JObject.Parse(result);
                        if (details["value"].Any())
                            foreach (var jToken in details["value"])
                            {
                                var config = (JObject)jToken;
                                var DocList = new DocumentsForApproval();
                                DocList.TabelID = (string)config["Table_ID"];
                                DocList.Entry_No = (string)config["Entry_No"];
                                if ((string)config["Document_No"] == "")
                                {
                                    var s = (string)config["Record_ID_to_Approve"];
                                    var t = s.Split(':');
                                    DocList.Document_No = t[1].Trim();
                                }
                                else
                                {
                                    DocList.Document_No = (string)config["Document_No"];
                                }

                                if (TbID == "69205")
                                {
                                    var pageLv =
                                        "HRLeaveRequisition?$select=Employee_No&$filter=Application_Code eq '" +
                                        DocList.Document_No + "'&$format=json";

                                    var httpResponseLV = Credentials.GetOdataData(pageLv);
                                    using var streamReaderLV = new StreamReader(httpResponseLV.GetResponseStream());
                                    var resultLV = streamReaderLV.ReadToEnd();

                                    var detailsLV = JObject.Parse(resultLV);
                                    foreach (JObject configLV in detailsLV["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeName((string)configLV["Employee_No"]);
                                }

                                if (TbID == "50029")
                                {
                                    var pageLv = "LeaveReimbursements?$select=EmployeeName&$filter=No eq '" +
                                                 DocList.Document_No + "'&$format=json";

                                    var httpResponseLV = Credentials.GetOdataData(pageLv);
                                    using var streamReaderLV = new StreamReader(httpResponseLV.GetResponseStream());
                                    var resultLV = streamReaderLV.ReadToEnd();

                                    var detailsLV = JObject.Parse(resultLV);
                                    foreach (JObject configLV in detailsLV["value"])
                                        DocList.Sender_Name = (string)configLV["EmployeeName"];
                                }
                                else if (TbID == "38")
                                {
                                    //var pagePRV = "PurchaseRequisition?$select=Employee_No_&$filter=No_ eq '" +
                                    //              DocList.Document_No + "'&$format=json";
                                    var pagePRV = "PurchaseRequisition?$filter=No eq '" + DocList.Document_No + "'&format=json";


                                    var httpResponseP = Credentials.GetOdataData(pagePRV);
                                    using var streamReaderP = new StreamReader(httpResponseP.GetResponseStream());
                                    var resultP = streamReaderP.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeName((string)configP["Employee_No_"]);
                                }
                                else if (TbID == "70134954")
                                {
                                    var pageStore = "StoreRequisition?$select=Employee_No&$filter=No eq '" +
                                                    DocList.Document_No + "'&$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageStore);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeName((string)configP["Employee_No"]);
                                }
                                else if (TbID == "70135176")
                                {
                                    var pageImp = "ImprestReq?$select=AccountNo&$filter=No eq '" + DocList.Document_No +
                                                  "'&$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageImp);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeName((string)configP["AccountNo"]);
                                }
                                else if (TbID == "70135168")
                                {
                                    var pageImp = "ImprestSurrenderList?$select=Account_No&$filter=No eq '" +
                                                  DocList.Document_No + "'&$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageImp);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeName((string)configP["Account_No"]);
                                }
                                else if (TbID == "59003")
                                {
                                    var pageTransport = "TransportReqList?&$filter=Transport_Requisition_No eq '" +
                                                        DocList.Document_No + "'&$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageTransport);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeName((string)configP["Employee_No"]);
                                }
                                else if (TbID == "69217")
                                {
                                    var pagePlanner = "HRLeavePlanner?$filter=Application_Code eq '" +
                                                      DocList.Document_No + "'&$format=json";

                                    var plannerLines = new List<LeavePlannerLines>();

                                    var httpResponsePlanner = Credentials.GetOdataData(pagePlanner);
                                    using var streamReaderPlanner =
                                        new StreamReader(httpResponsePlanner.GetResponseStream());
                                    var resultPlanner = streamReaderPlanner.ReadToEnd();

                                    var detailsPlanner = JObject.Parse(resultPlanner);
                                    foreach (JObject configPlanner in detailsPlanner["value"])
                                        DocList.Sender_Name = (string)configPlanner["Implementing_Unit_Name"];
                                }
                                else if (TbID == "50110")
                                {
                                    var pageStaffC = "ExpenseRequisitionCard?$filter=No eq '" + DocList.Document_No +
                                                     "' &$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeNameByUserID((string)configP["Created_By"]);
                                }
                                else if (TbID == "57000")
                                {
                                    var pageStaffC = "PaymentsQuery?$filter=No eq '" + DocList.Document_No +
                                                     "' &$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeNameByUserID((string)configP["CreatedBy"]);
                                }
                                else if (TbID == "80019")
                                {
                                    var pageStaffC = "FunctionalAWPCard?$filter=No eq '" + DocList.Document_No +
                                                     "' &$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (var jToken1 in detailsP["value"])
                                    {
                                        var configP = (JObject)jToken1;
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeNameByUserID((string)configP["Created_By"]);
                                    }
                                }
                                else if (TbID == "69520")
                                {
                                    var pageStaffC = "LaeveApplicationRecall?$filter=Recall_No eq '" +
                                                     DocList.Document_No + "' &$format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (var jToken1 in detailsP["value"])
                                    {
                                        var configP = (JObject)jToken1;
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeNameByUserID((string)configP["Employee_No"]);
                                    }
                                }
                                else if (TbID == "69055")
                                {
                                    var pageStaffC = "EmployeeTransfer?$filter=Document_No eq '" + DocList.Document_No +
                                                     "'&format=json";
                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (var jToken1 in detailsP["value"])
                                    {
                                        var configP = (JObject)jToken1;
                                        DocList.Sender_Name = (string)configP["Employee_Name"];
                                    }
                                }
                                else if (TbID == "70141")
                                {
                                    string pageStaffC = "IFSTenderCommitteeLines?$filter=Document_No eq '" + DocList.Document_No + "'&$format=json";

                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeNameByUserID((string)configP["Created_By"]);
                                }
                                else if (TbID == "56007")
                                {
                                    string pageStaffC = "DepositReceipt?$filter=No eq '" + DocList.Document_No + "'&$format=json";

                                    var httpResponseStore = Credentials.GetOdataData(pageStaffC);
                                    using var streamReaderStore =
                                        new StreamReader(httpResponseStore.GetResponseStream());
                                    var resultP = streamReaderStore.ReadToEnd();

                                    var detailsP = JObject.Parse(resultP);
                                    foreach (JObject configP in detailsP["value"])
                                        DocList.Sender_Name =
                                            CommonClass.GetEmployeeNameByUserID((string)configP["Created_By"]);
                                }

                                else
                                {
                                    var Names = CommonClass.GetEmployeeNameByUserID((string)config["Sender_ID"]);
                                    if (Names != "")
                                        DocList.Sender_Name = Names;
                                    else
                                        DocList.Sender_Name = (string)config["Sender_ID"];
                                }

                                DocList.DateSend =
                                    ((DateTime)config["Date_Time_Sent_for_Approval"]).ToString("dd/MM/yyyy");
                                if (Status == "Open")
                                    DocList.Status = "Pending Approval";
                                else
                                    DocList.Status = Status;
                                DocList.Sequence = (string)config["Sequence_No"];
                                approvalDocList.Add(DocList);
                            }
                    }

                    return PartialView(
                        "~/Views/DocumentApproval/Document Approval Views/DocumentForApprovalList.cshtml",
                        approvalDocList);
                }

                return PartialView("~/Views/DocumentApproval/Document Approval Views/DocumentForApprovalList.cshtml");
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult LeaveReqDocApprovalDetails(string DocNo)
        {
            try
            {
                var LeaveDoc = new LeaveReqList();
                var Reli = "";

                #region Implementing Unit List
                List<ImplementingUnitsList> Dim1List = new List<ImplementingUnitsList>();
                string pageDepartment = "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$orderby=Name asc&format=json";

                var httpResponseDepartment = Credentials.GetOdataData(pageDepartment);
                using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var Impunit = new ImplementingUnitsList();
                        Impunit.Code = (string)config["Code"];
                        Impunit.Name = (string)config["Name"];//(string)config["Code"] + "  " +
                        Dim1List.Add(Impunit);
                    }
                }

                #endregion


                var page = "HRLeaveRequisition?$filter=Application_Code eq '" + DocNo + "'&format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        LeaveDoc.No = (string)config["Application_Code"];
                        LeaveDoc.EmpNo = (string)config["Employee_No"];
                        LeaveDoc.EmpName = CommonClass.GetEmployeeName((string)config["Employee_No"]);
                        LeaveDoc.Leave_Type = (string)config["Leave_Type"];
                        LeaveDoc.Applied_Days = (string)config["Days_Applied"];
                        LeaveDoc.Date = Convert.ToDateTime((string)config["Application_Date"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Starting_Date =
                            Convert.ToDateTime((string)config["Start_Date"]).ToString("dd/MM/yyyy");
                        LeaveDoc.End_Date = Convert.ToDateTime((string)config["End_Date"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Return_Date = Convert.ToDateTime((string)config["Return_Date"]).ToString("dd/MM/yyyy");
                        LeaveDoc.RelieverName = (string)config["Reliever"] + " - " +
                                            CommonClass.GetEmployeeName((string)config["Reliever"]);

                        LeaveDoc.Reliever = (string)config["Reliever"];

                        LeaveDoc.Department = (string)config["Department"];
                        LeaveDoc.Remarks = (string)config["Applicant_Comments"];
                        LeaveDoc.Status = (string)config["Status"];
                        LeaveDoc.GeographicalName = CommonClass.GetDimensionValue((string)config["ShortcutDimension1Code"]);
                        LeaveDoc.AdminUnitName = CommonClass.GetDimensionValue((string)config["ShortcutDimension2Code"]);
                        LeaveDoc.RelieverImplementingUnit = (string)config["RelieverAdminUnit"];
                        LeaveDoc.RelieverAdminUnitName = (string)config["RelieverGeographicalName"];
                    }
                }

                #region ReliverList

                var relieverList = new List<RelieverList>();

                var Department = CommonClass.EmployeeDepartment(Reli);
                var pageReliever = "EmployeeList?$filter=No ne '" + Reli +
                                   "' and Status eq 'Active' and Global_Dimension_2_Code eq '" + Department +
                                   "'&format=json";


                var httpResponseReliever = Credentials.GetOdataData(pageReliever);
                using (var streamReader = new StreamReader(httpResponseReliever.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                        if ((string)config["First_Name"] != "")
                        {
                            var Rlist = new RelieverList();
                            Rlist.No = (string)config["No"];
                            Rlist.Name = (string)config["First_Name"] + " " + (string)config["Middle_Name"] + " " +
                                         (string)config["Last_Name"];
                            relieverList.Add(Rlist);
                        }
                }

                #endregion


                #region Handover Tasks

                var HandoverTaskList = new List<HandoverTask>();

                var pageHandoverTask = "HandoverNotes?$filter=Type eq 'Handover Taks' and LeaveNo eq '" + DocNo +
                                       "'&format=json";
                var i = 0;
                var httpResponseHandoverTask = Credentials.GetOdataData(pageHandoverTask);
                using (var streamReader = new StreamReader(httpResponseHandoverTask.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        i++;
                        if ((string)config["First_Name"] != "")
                        {
                            var Rlist = new HandoverTask();
                            Rlist.No = i.ToString(); // (string)config["EntryNo"];
                            Rlist.Activity = (string)config["Activity"];
                            Rlist.Status = (string)config["Status"];
                            Rlist.Reason = (string)config["Reason"];
                            HandoverTaskList.Add(Rlist);
                        }
                    }
                }

                #endregion

                #region Handover Files

                var HandoverFileList = new List<HandoverFile>();

                var j = 0;

                var pageHandoverFile = "HandoverNotes?$filter=Type eq 'Handover Files' and LeaveNo eq '" + DocNo +
                                       "'&format=json";

                var httpResponseHandoverFile = Credentials.GetOdataData(pageHandoverFile);
                using (var streamReader = new StreamReader(httpResponseHandoverFile.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        j++;
                        if ((string)config["First_Name"] != "")
                        {
                            var Rlist = new HandoverFile();
                            Rlist.No = j.ToString();
                            Rlist.Activity = (string)config["Activity"];
                            Rlist.Status = (string)config["Status"];

                            HandoverFileList.Add(Rlist);
                        }
                    }
                }

                #endregion

                LeaveDoc.ListOfHandoverFiles = HandoverFileList;
                LeaveDoc.ListOfHandoverTasks = HandoverTaskList;
                LeaveDoc.ListOfRelievers = relieverList.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.No
                    }).ToList();
                LeaveDoc.ListOfImplementingUnits = Dim1List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                return PartialView("~/Views/DocumentApproval/Document Approval Views/LeaveDocumentView.cshtml",
                    LeaveDoc);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public PartialViewResult LeaveReimbursementDocApprovalDetails(string DocNo)
        {
            try
            {
                var LeaveDoc = new LeaveReimbursementList();

                var page = "LeaveReimbursements?$filter=No eq '" + DocNo + "'&format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);

                    foreach (JObject config in details["value"])
                    {
                        LeaveDoc.No = (string)config["No"];
                        LeaveDoc.EmpNo = (string)config["EmployeeNo"];
                        LeaveDoc.EmpName = (string)config["EmployeeName"];
                        LeaveDoc.Leave_Type = (string)config["LeaveType"];
                        LeaveDoc.LeaveNo = (string)config["Leave_Number"];
                        LeaveDoc.Days_Applied = (string)config["AppliedDays"];
                        LeaveDoc.Date = Convert.ToDateTime((string)config["Date"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Starting_Date =
                            Convert.ToDateTime((string)config["StartingDate"]).ToString("dd/MM/yyyy");
                        LeaveDoc.End_Date = Convert.ToDateTime((string)config["EndDate"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Return_Date = Convert.ToDateTime((string)config["ReturnDate"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Reliever = (string)config["RelieverNo"];
                        LeaveDoc.Department = (string)config["DepartmentCode"];
                        LeaveDoc.Remarks = (string)config["Purpose"];
                        LeaveDoc.Status = (string)config["Status"];
                        var approveddays = Convert.ToDecimal((string)config["DaysApproved"]).ToString("#,##0");
                        if (approveddays == "0") approveddays = "";
                        LeaveDoc.DaystoReimburse = approveddays;
                    }
                }

                return PartialView("~/Views/DocumentApproval/Document Approval Views/LeaveReimbursementView.cshtml",
                    LeaveDoc);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult ExpenseRequisitionDocument(string DocNo)
        {
            try
            {
                var employee = Session["EmployeeData"] as EmployeeView;
                var userId = employee.UserID;
                var StaffNo = Session["Username"].ToString();

                var expense = new ExpenseRequisition();
                var docDetails = new ExpenseRequisitionDocument();

                var rsrceReq = "ExpenseRequisitionCard?$filter=No eq '" + DocNo + "' &$format=json";
                var httpResponse = Credentials.GetOdataData(rsrceReq);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        expense.No = (string)config["No"];
                        expense.Description = (string)config["Description"];
                        expense.CorporateStrategy = (string)config["Corporate_Strategy"];
                        expense.ReportingPeriod = (string)config["Reporting_Period"];
                        expense.BudgetCode = (string)config["Budget_Code"];
                        expense.Date = DateTime
                            .ParseExact((string)config["Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                        expense.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                        expense.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                        expense.Workplan = (string)config["Workplan"];
                        expense.ActivityCode = (string)config["Activity_Code"];
                        expense.ActivityDescription = (string)config["Activity_Description"];
                        expense.RequiresImprest = bool.Parse((string)config["Requires_Imprest"]);
                        expense.ImprestType = (string)config["Imprest_Type"];
                        expense.StartDate = DateTime
                            .ParseExact((string)config["Start_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                        expense.NoOfDays = int.Parse((string)config["No_Of_Days"]);
                        expense.EndDate = DateTime
                            .ParseExact((string)config["End_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                        expense.Subject = (string)config["Subject"];
                        expense.Objective = (string)config["Objective"];
                        expense.RequiresDirectPayment = bool.Parse((string)config["Requires_Direct_Payment"]);
                        expense.RequiresPRN = bool.Parse((string)config["Requires_PRN"]);
                        expense.ProcurementPlanID = (string)config["Procurement_Plan_ID"];
                        expense.PRNType = (string)config["PRN_Type"];
                        expense.LocationCode = (string)config["Location_Code"];
                        expense.RequisitionProductGroup = (string)config["Requisition_Product_Group"];
                        expense.PPPlanCategory = (string)config["PP_Plan_Category"];
                        expense.ApprovalStatus = (string)config["Approval_Status"];
                        expense.Status = (string)config["Status"];
                        expense.CreatedBy = (string)config["Created_By"];
                        string dateString = (string)config["Date_Created"];
                        string format = "MM/dd/yyyy HH:mm:ss";

                        if (DateTime.TryParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                        {
                            expense.DateCreated = parsedDate;
                        }
                        else
                        {
                            expense.DateCreated = DateTime.MinValue;
                        }

                        expense.CommittedBy = (string)config["Committed_By"];

                        string dateString2 = (string)config["Date_Committed"];
                        string format2 = "MM/dd/yyyy HH:mm:ss";
                        if (DateTime.TryParseExact(dateString2, format2, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate2))
                        {
                            expense.DateCommitted = parsedDate2;
                        }
                        else
                        {
                            expense.DateCommitted = DateTime.MinValue; // Set a default value
                        }

                        expense.RecalledBy = (string)config["Recalled_By"];
                        expense.DateRecalled = DateTime.Parse((string)config["Date_Recalled"]);

                        var expenseRequisitionLines = new List<ExpenseRequisitionLine>();

                        var rsrceReqLines = "ExpenseRequisitionLine?$filter=Document_No eq '" + DocNo +
                                            "' &$format=json";


                        var httpResponseLines = Credentials.GetOdataData(rsrceReqLines);
                        using var streamReaderLines = new StreamReader(httpResponseLines.GetResponseStream());
                        var resultLines = streamReaderLines.ReadToEnd();

                        var detailsLines = JObject.Parse(resultLines);


                        foreach (JObject configLines in detailsLines["value"])
                        {
                            var expenseLine = new ExpenseRequisitionLine();

                            expenseLine.DocumentNo = (string)configLines["Document_No"];
                            var lin = (string)configLines["Line_No"];
                            expenseLine.LineNo = Convert.ToInt32(lin);
                            expenseLine.GLAccount = (string)configLines["G_L_Account"];
                            expenseLine.GLAccountName = (string)configLines["G_L_Account_Name"];
                            expenseLine.ShortcutDimension3Code = (string)configLines["Shortcut_Dimension_3_Code"];
                            expenseLine.ShortcutDimension4Code = (string)configLines["Shortcut_Dimension_4_Code"];
                            expenseLine.ShortcutDimension5Code = (string)configLines["Shortcut_Dimension_5_Code"];
                            expenseLine.ShortcutDimension6Code = (string)configLines["Shortcut_Dimension_6_Code"];
                            expenseLine.ShortcutDimension7Code = (string)configLines["Shortcut_Dimension_7_Code"];
                            expenseLine.ShortcutDimension8Code = (string)configLines["Shortcut_Dimension_8_Code"];
                            expenseLine.BudgetAllocation = (int)configLines["Budget_Allocation"];
                            expenseLine.BudgetBalance = (int)configLines["Budget_Balance"];
                            expenseLine.TotalCommitments = (int)configLines["Total_Committments"];
                            expenseLine.TotalAmount = (int)configLines["Total_Amount"];
                            expenseLine.TotalAllocation = (int)configLines["Total_Allocation"];
                            expenseLine.Status = (string)configLines["Status"];
                            expenseLine.Error = (bool)configLines["Error"];
                            expenseLine.ErrorMessage = (string)configLines["Error_Message"];
                            expenseLine.RecalledBy = (string)configLines["Recalled_By"];
                            expenseLine.RecalledOn = DateTime.Parse((string)configLines["Recalled_On"]);
                            expenseRequisitionLines.Add(expenseLine);
                        }

                        docDetails = new ExpenseRequisitionDocument
                        {
                            DocHeader = expense,
                            ListOfExpenseRequisitionLine = expenseRequisitionLines
                        };
                    }
                }

                return PartialView("~/Views/DocumentApproval/Document Approval Views/ExpenseRequisition.cshtml",
                    expense);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult LeaveCarryForward(string DocNo)
        {
            try
            {
                var LeaveDoc = new LeaveReimbursementList();

                var page = "LeaveReimbursements?$filter=No eq '" + DocNo + "'&format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        LeaveDoc.No = (string)config["No"];
                        LeaveDoc.EmpNo = (string)config["EmployeeNo"];
                        LeaveDoc.EmpName = (string)config["EmployeeName"];
                        LeaveDoc.Leave_Type = (string)config["LeaveType"];
                        LeaveDoc.Days_Applied = (string)config["AppliedDays"];
                        LeaveDoc.Date = Convert.ToDateTime((string)config["Date"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Starting_Date =
                            Convert.ToDateTime((string)config["StartingDate"]).ToString("dd/MM/yyyy");
                        LeaveDoc.End_Date = Convert.ToDateTime((string)config["EndDate"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Return_Date = Convert.ToDateTime((string)config["ReturnDate"]).ToString("dd/MM/yyyy");
                        LeaveDoc.Reliever = (string)config["RelieverNo"];
                        LeaveDoc.Department = (string)config["DepartmentCode"];
                        var approveddays = Convert.ToDecimal((string)config["DaysApproved"]).ToString("#,##0");
                        if (approveddays == "0") approveddays = "";
                        LeaveDoc.DaystoReimburse = approveddays;
                        LeaveDoc.Remarks = (string)config["Purpose"];
                        LeaveDoc.Status = (string)config["Status"];
                    }
                }

                return PartialView("~/Views/DocumentApproval/Document Approval Views/LeaveCarryForwardView.cshtml",
                    LeaveDoc);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        protected string GetRequisitionType(string DocNo)
        {
            var ReqType = "";
            var page = "PurchaseRequisition?$filter=No_ eq '" + DocNo + "'&format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();

            var details = JObject.Parse(result);
            foreach (JObject config in details["value"]) ReqType = (string)config["Requisition_Type"];

            return ReqType;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdatePurchaseItemGL(string LnNo, string DocNo, string GLAccount)
        {
            try
            {
                //  Credentials.ObjNav.UpdateItemGlPortal(DocNo, LnNo, GLAccount);
                return Json(new { message = "Purchase Line Updated successfully", success = true },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        private string GetEmployeeName(string userID)
        {
            string names = null;
            try
            {
                var page = "EmployeeList?$filter=EmployeeUserID eq '" + userID + "'&format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);

                if (details["value"].Count() > 0)
                    foreach (JObject config in details["value"])
                        //  string Role = "";
                        names = (string)config["No"];
                //names[2] = (string)config["FirstName"] + " "+ (string)config["LastName"];
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
            }

            return names;
        }
        public PartialViewResult ImprestMemoDocumentDetails(string DocNo)
        {
            try
            {
                #region Imp Header

                var ImpDoc = new ImprestMemoHeader();

                #region Dim1 List

                var Dim1List = new List<DimensionValues>();
                var pageDepartment =
                    "DimensionValues?$filter=Global_Dimension_No eq 1 and Blocked eq false&$format=json";

                var httpResponseDepartment = Credentials.GetOdataData(pageDepartment);
                using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var Department = new DimensionValues();
                        Department.Code = (string)config["Code"];
                        Department.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim1List.Add(Department);
                    }
                }

                #endregion

                #region dim2

                var Dim2List = new List<DimensionValues>();
                var pageDivision = "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$format=json";

                var httpResponseDivision = Credentials.GetOdataData(pageDivision);
                using (var streamReader = new StreamReader(httpResponseDivision.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim2List.Add(DList);
                    }
                }

                #endregion

                #region dim3

                var Dim3List = new List<DimensionValues>();
                var pageDim3 = "DimensionValues?$filter=Global_Dimension_No eq 3 and Blocked eq false&$format=json";

                var httpResponseDim3 = Credentials.GetOdataData(pageDim3);
                using (var streamReader = new StreamReader(httpResponseDim3.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim3List.Add(DList);
                    }
                }

                #endregion

                #region dim4

                var Dim4List = new List<DimensionValues>();
                var pageDim4 = "DimensionValues?$filter=Global_Dimension_No eq 4 and Blocked eq false&$format=json";

                var httpResponseDim4 = Credentials.GetOdataData(pageDim4);
                using (var streamReader = new StreamReader(httpResponseDim4.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim4List.Add(DList);
                    }
                }

                #endregion

                #region dim5

                var Dim5List = new List<DimensionValues>();
                var pageDim5 = "DimensionValues?$filter=Global_Dimension_No eq 5 and Blocked eq false&$format=json";

                var httpResponseDim5 = Credentials.GetOdataData(pageDim5);
                using (var streamReader = new StreamReader(httpResponseDim5.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim5List.Add(DList);
                    }
                }

                #endregion

                #region Responsibility

                var RespCList = new List<RespCenter>();
                var pageResC = "ResponsibilityCenters?$format=json";

                var httpResponseResC = Credentials.GetOdataData(pageResC);
                using (var streamReader = new StreamReader(httpResponseResC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var RCList = new RespCenter();
                        RCList.Code = (string)config["Code"];
                        RCList.Name = (string)config["Name"];
                        RespCList.Add(RCList);
                    }
                }

                #endregion

                #region EmployeeList

                var employeeList = new List<EmployeeList>();
                // string Department2 = CommonClass.EmployeeDepartment(StaffNo);
                var StaffNo = Session["Username"].ToString();
                var Departments = CommonClass.EmployeeDepartment(StaffNo);

                var pageReliever = "EmployeeList?$filter=Status eq 'Active' and Directorate_Code eq '" + Departments +
                                   "'&format=json";


                var httpResponseReliever = Credentials.GetOdataData(pageReliever);
                using (var streamReader = new StreamReader(httpResponseReliever.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                        if ((string)config["FirstName"] != "")
                        {
                            var Rlist = new EmployeeList();
                            Rlist.No = (string)config["No"];
                            Rlist.Name = (string)config["FirstName"] + " " + (string)config["MiddleName"] + " " +
                                         (string)config["LastName"];
                            employeeList.Add(Rlist);
                        }
                }

                #endregion

                #region ImprestDue

                // List<ImprestDuetype> ImprestDue = new List<ImprestDuetype>();
                var ImmpsDue = new List<ImpDuetyp>();
                var pageImprestDueType = "ImprestDueType?$format=json";

                var httpResponseImpDue = Credentials.GetOdataData(pageImprestDueType);
                using (var streamReader = new StreamReader(httpResponseImpDue.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var impD = new ImpDuetyp();
                        impD.Code = (string)config["Code"];
                        impD.Name = (string)config["Code"];
                        ImmpsDue.Add(impD);
                    }
                }

                #endregion

                var page = "ImprestMemo?$filter=No eq '" + DocNo + "'&$format=json";
                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        ImpDoc.No = (string)config["No"];
                        ImpDoc.DocumentDate = Convert.ToDateTime((string)config["Date"]).ToString("dd/MM/yyyy");
                        ImpDoc.DateNeeded = Convert.ToDateTime((string)config["DateRequired"]).ToString("dd/MM/yyyy");
                        ImpDoc.Remarks = (string)config["Purpose"];
                        ImpDoc.DocD = new NewImprestMemoRequisition
                        {
                            Dim1 = (string)config["GlobalDimension1Code"],
                            Dim2 = (string)config["ShortcutDimension2Code"],
                            Dim3 = (string)config["ShortcutDimension3Code"],
                            Dim4 = (string)config["ShortcutDimension4Code"],
                            Dim5 = (string)config["ShortcutDimension5Code"]
                        };
                        ImpDoc.Dim1 = (string)config["GlobalDimension1Code"];
                        ImpDoc.Dim2 = (string)config["ShortcutDimension2Code"];
                        ImpDoc.Dim3 = (string)config["ShortcutDimension3Code"];
                        ImpDoc.Dim4 = (string)config["ShortcutDimension4Code"];
                        ImpDoc.Dim5 = (string)config["ShortcutDimension5Code"];


                        ImpDoc.TotalAmount = Convert.ToDecimal((string)config["TotalNetAmount"]).ToString("#,##0.00");
                        ImpDoc.Status = (string)config["Status"];
                        ImpDoc.Subject = (string)config["Subject"];
                        ImpDoc.Body = (string)config["Body1"] + (string)config["Body2"] + (string)config["Body3"] +
                                      (string)config["Body4"] +
                                      (string)config["Body5"] + (string)config["Body6"] + (string)config["Body7"] +
                                      (string)config["Body8"] + (string)config["Body9"] +
                                      (string)config["Body10"];
                        ImpDoc.To = (string)config["MemoTo"];
                        ImpDoc.From = (string)config["MemoFrom"];
                        ImpDoc.ImprestDueType = (string)config["ImprestDueType"];
                    }
                }

                ImpDoc.DocD.ListOfDim1 = Dim1List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim2 = Dim2List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim3 = Dim3List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim4 = Dim4List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim5 = Dim5List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfResponsibility = RespCList.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();

                ImpDoc.ListOfEmployeeList = employeeList.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.No
                    }).ToList();
                ImpDoc.ListOfImprestDue = ImmpsDue.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();

                #endregion

                #region Imp Lines

                var ImpLines = new List<ImprestMemoLines>();
                var pageLine = "ImprestMemoLines?$filter=No eq '" + DocNo + "'&$format=json";
                var httpResponseLine = Credentials.GetOdataData(pageLine);
                using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var ImLine = new ImprestMemoLines();
                        ImLine.DocNo = (string)config["No"];
                        ImLine.AdvanceType = (string)config["Advance_Type"];
                        ImLine.Item = (string)config["Account_No"];
                        ImLine.ItemDesc = (string)config["Account_Name"];
                        ImLine.ItemDesc2 = (string)config["Purpose"];
                        ImLine.LnNo = (string)config["Line_No"];
                        ImLine.UoN = (string)config["Unit_of_Measure"];
                        ImLine.Quantity = (string)config["Quantity"];
                        ImLine.UnitAmount = Convert.ToDecimal((string)config["Daily_Rate_Amount"]).ToString("#,##0.00");
                        ImLine.Amount = Convert.ToDecimal((string)config["Amount"]).ToString("#,##0.00");
                        ImLine.EmployeeNo = (string)config["Employee_Name"];
                        ImpLines.Add(ImLine);
                    }
                }

                #endregion

                var ImpmemoItemLines = new List<ImprestMemoItems>();
                var pageMemoLine = "ImprestMemoLine?$filter=No eq '" + DocNo +
                                   "' and ImprestType eq 'ItemCash' &$format=json";
                var httpResponseMemoLine = Credentials.GetOdataData(pageMemoLine);
                using (var streamReader = new StreamReader(httpResponseMemoLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var ImLine1 = new ImprestMemoItems();
                        ImLine1.DocNo = (string)config["No"];
                        ImLine1.AdvanceType = (string)config["AdvanceType"];
                        ImLine1.Item = (string)config["AccountNo"];
                        ImLine1.ItemDesc = (string)config["AccountName"];
                        ImLine1.ItemDesc2 = (string)config["Purpose"];
                        ImLine1.LnNo = (string)config["LineNo"];
                        ImLine1.UoN = (string)config["UnitofMeasure"];
                        ImLine1.Quantity = (string)config["Quantity"];
                        ImLine1.UnitAmount = Convert.ToDecimal((string)config["UnitCostLCY"]).ToString("#,##0.00");
                        ImLine1.Amount = Convert.ToDecimal((string)config["Amount"]).ToString("#,##0.00");
                        ImLine1.EmployeeNo = (string)config["EmployeeName"];
                        ImpmemoItemLines.Add(ImLine1);
                    }
                }

                #region Imp Lines

                var ImpLines2 = new List<ImprestMemoNonStaff>();
                var pageLine2 = "ImprestMemoNonStaff?$filter=ImprestMemoNo eq '" + DocNo + "'&$format=json";
                var httpResponseLine2 = Credentials.GetOdataData(pageLine2);
                using (var streamReader = new StreamReader(httpResponseLine2.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var ImLine2 = new ImprestMemoNonStaff();
                        ImLine2.ImprestMemoNo = (string)config["ImprestMemoNo"];
                        ImLine2.Names = (string)config["Names"];
                        ImLine2.Designation = (string)config["Designation"];
                        ImLine2.Organization = (string)config["OrganizationInstitution"];
                        ImLine2.LineNo = (string)config["LineNo"];
                        ImpLines2.Add(ImLine2);
                    }
                }

                #endregion

                Session["Location"] = "4";

                var docDetails = new ImprestMemoDocument
                {
                    DocHeader = ImpDoc,
                    ListOfImprestMemoLines = ImpLines,
                    ListOfImprestMemoNonStaff = ImpLines2,
                    ListOfImprestMemoItemsList = ImpmemoItemLines
                };
                return PartialView("~/Views/DocumentApproval/Document Approval Views/ImprestMemoApproval.cshtml",
                    docDetails);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult TransportReqDocApprovalDetails(string DocNo, string Sequence)
        {
            try
            {
                #region Header

                // TransportRequisitionList transDoc = new TransportRequisitionList();
                var transDoc = new TransportReqList();
                var page = "TransportReqList?$filter=Transport_Requisition_No eq '" + DocNo + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        transDoc.TransportRequisitionNo = (string)config["Transport_Requisition_No"];
                        transDoc.RequestedBy = (string)config["Requested_By"];
                        transDoc.RequestedByName = (string)config["Requested_By_Name"];
                        transDoc.Position = (string)config["Position"];
                        transDoc.ApprovedImprestMemo = (string)config["Approved_Imprest_Memo"];
                        transDoc.From = (string)config["From"];
                        transDoc.Destination = (string)config["Destination"];
                        transDoc.RouteDescription = (string)config["Route_Description"];
                        transDoc.DateofTrip = (DateTime)config["Date_of_Trip"];
                        transDoc.TimeRequested = (TimeSpan)config["Time_Requested"];
                        transDoc.Timeout = (DateTime)config["Time_out"];
                        transDoc.TimeIn = (TimeSpan)config["Time_In"];
                        transDoc.RouteCOde = (string)config["Journey_Route"];
                        transDoc.OpeningOdometerReading = (int)config["Opening_Odometer_Reading"];
                        transDoc.ClosingOdometerReading = (int)config["Closing_Odometer_Reading"];
                        transDoc.NoofDaysRequested = (int)config["No_of_Days_Requested"];
                        transDoc.NumberofHoursRequested = (int)config["Number_of_Hours_Requested"];
                        transDoc.TripEndDate = (DateTime)config["Trip_End_Date"];
                        transDoc.TripEndTime = (TimeSpan)config["Trip_End_Time"];
                        transDoc.Status = (string)config["Status"];
                        transDoc.ResponsibilityCenter = (string)config["Responsibility_Center"];
                        transDoc.WorkTicketNo = (string)config["Work_Ticket_No"];
                        transDoc.UserId = (string)config["User_Id"];
                        transDoc.HOD = (bool)config["HOD"];
                        transDoc.Driver = (string)config["Vehicle_Allocated"];
                        transDoc.Vehicle = (string)config["Driver_Allocated"];
                        transDoc.AddedOn = (DateTime)config["Added_On"];
                        transDoc.DateofRequest = (DateTime)config["Date_of_Request"];
                        transDoc.Subject = (string)config["Subject"];
                        transDoc.Comments = (string)config["Comments"];
                        transDoc.FuelRequestCode = (string)config["Fuel_Request_Code"];
                        transDoc.ReasonforReopening = (string)config["Reason_for_Reopening"];
                    }
                }

                #endregion

                #region Passenger Lines

                var PassengerList = new List<Passengers>();
                var pageLine = "TransportPassengers?$filter=Req_No eq '" + DocNo + "'&$format=json";
                var httpResponseLine = Credentials.GetOdataData(pageLine);
                using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var passenger = new Passengers();
                        passenger.Type = (string)config["Passenger_Type"];
                        passenger.No = (string)config["No"];
                        passenger.Name = (string)config["Name"];
                        passenger.Position = (string)config["Position"];
                        PassengerList.Add(passenger);
                    }
                }

                #endregion

                #region Driver List

                var driverList = new List<DropdownList>();
                var pageDriver = "FltDriverList?$format=json";
                var httpResponseDriver = Credentials.GetOdataData(pageDriver);
                using (var streamReader = new StreamReader(httpResponseDriver.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var d = new DropdownList();
                        d.Value = (string)config["Driver"];
                        d.Text = (string)config["Driver_Name"];
                        driverList.Add(d);
                    }
                }

                #endregion

                #region Vehicle List

                var vehiclerList = new List<DropdownList>();
                var pageVehicle = "FltVehicleList?$format=json";
                //if (transDoc. == "0")
                //{
                //    pageVehicle = "FltVehicleList?$format=json";
                //}
                //else if (transDoc.VehicleType == "Vehicle")
                //{
                //    pageVehicle = "FltVehicleList?$filter=Vehicle_Type eq 'Vehicle'&$format=json";
                //}
                //else if (TransDoc.VehicleType == "Boat")
                //{
                //    pageVehicle = "FltVehicleList?$filter=Vehicle_Type eq 'Vessel'&$format=json";
                //}
                //else if (TransDoc.VehicleType == "Taxi")
                //{
                //    pageVehicle = "FltVehicleList?$format=json";
                //}
                var httpResponseVehicle = Credentials.GetOdataData(pageVehicle);
                using (var streamReader = new StreamReader(httpResponseVehicle.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var v = new DropdownList();
                        v.Value = (string)config["Registration_No"];
                        v.Text = (string)config["No"] + "-" + (string)config["Serial_No"] + " (" +
                                 (string)config["Current_Reading"] + (string)config["Next_Service_Kilometers"] + ")";
                        vehiclerList.Add(v);
                    }
                }

                #endregion

                var docDetails = new TransDocument
                {
                    DocHeader = transDoc,
                    ListOfPassengers = PassengerList,
                    ListOfDrivers = driverList.Select(x =>
                        new SelectListItem
                        {
                            Text = x.Text,
                            Value = x.Value
                        }).ToList(),
                    ListOfVehicles = vehiclerList.Select(x =>
                        new SelectListItem
                        {
                            Text = x.Text,
                            Value = x.Value
                        }).ToList()
                };
                return PartialView(
                    "~/Views/DocumentApproval/Document Approval Views/TransportApprovalDocDetails.cshtml", docDetails);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult TrainingReqDocApprovalDetails(string DocNo, string Sequence)
        {
            try
            {
                #region Header

                var training = new TrainingApplications();
                var page = "HRTrainingApplication?$filter=Application_No eq '" + DocNo + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        training.Code = (string)config["Code"];
                        training.CourseTitle = (string)config["Course_Title"];
                        training.StartDateTime = (string)config["Start_DateTime"];
                        training.EndDateTime = (string)config["End_DateTime"];
                        training.Status = (string)config["Status"];
                        training.Duration = (int)config["Duration"];
                        training.Cost = (decimal)config["Cost"];
                        training.Location = (string)config["Location"];
                        training.Description = (string)config["Description"];
                        training.Year = (string)config["Year"];
                        training.Provider = (string)config["Provider"];
                        training.EmployeeNo = (string)config["Employee_No"];
                        training.ApplicationDate = (string)config["Application_Date"];
                        training.NoSeries = (string)config["No_Series"];
                        training.EmployeeDepartment = (string)config["Employee_Department"];
                        training.EmployeeName = (string)config["Employee_Name"];
                        training.ProviderName = (string)config["Provider_Name"];
                        training.NoOfParticipants = (int)config["No_of_Participants"];
                        training.ApprovedCost = (decimal)config["Approved_Cost"];
                        training.GlobalDimension1Code = (string)config["Global_Dimension_1_Code"];
                        training.GlobalDimension2Code = (string)config["Global_Dimension_2_Code"];
                        training.ActualStartDate = (string)config["Actual_Start_Date"];
                        training.ActualEndDate = (string)config["Actual_End_Date"];
                        training.EstimatedCost = (decimal)config["Estimated_Cost"];
                        training.ImprestCreated = (bool)config["Imprest_Created"];
                        training.TrainingPlanCost = (decimal)config["Training_Plan_Cost"];
                        training.Budget = (decimal)config["Budget"];
                        training.Actual = (decimal)config["Actual"];
                        training.Commitment = (decimal)config["Commitment"];
                        training.GLAccount = (string)config["GL_Account"];
                        training.BudgetName = (string)config["Budget_Name"];
                        training.AvailableFunds = (decimal)config["Available_Funds"];
                        training.Local = (string)config["Local"];
                        training.RequiresFlight = (bool)config["Requires_Flight"];
                        training.SupervisorNo = (string)config["Supervisor_No"];
                        training.SupervisorName = (string)config["Supervisor_Name"];
                        training.TrainingPlanNo = (string)config["Training_Plan_No"];
                        training.TrainingVenueRegionCode = (string)config["Training_Venue_Region_Code"];
                    }
                }

                #endregion

                #region Training Lines

                var participantList = new List<Trainees>();
                var pageLine = "HRTrainingPartcipants?$filter=TrainingCode eq '" + DocNo + "'&$format=json";
                var httpResponseLine = Credentials.GetOdataData(pageLine);
                using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var participants = new Trainees();
                        participants.No = (string)config["EmployeeCode"];
                        participants.Name = (string)config["Employeename"];
                        participantList.Add(participants);
                    }
                }

                #endregion

                #region Training Lines

                var TrainingCostList = new List<TrainingCost>();
                var pageTLine = "HRTrainingCost?$filter=TrainingId eq '" + DocNo + "'&$format=json";
                var httpResponseTLine = Credentials.GetOdataData(pageTLine);
                using (var streamReader = new StreamReader(httpResponseTLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var TrCost = new TrainingCost();
                        TrCost.No = (string)config["TrainingId"];
                        TrCost.Item = (string)config["TrainingCostItem"];
                        TrCost.Cost = ((decimal)config["Cost"]).ToString("#,##0.00");
                        TrainingCostList.Add(TrCost);
                    }
                }

                #endregion

                var docDetails = new TrainingDocument
                {
                    DocHeader = training,
                    ListOfTrainees = participantList,
                    ListOfTraininingCost = TrainingCostList
                };
                return PartialView("~/Views/DocumentApproval/Document Approval Views/TrainingApprovalDocDetails.cshtml",
                    docDetails);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }


        public PartialViewResult TimeSheetDocumentDetails(string DocNo)
        {
            try
            {
                var StaffNo = Session["Username"].ToString();
                var TransDoc = new TimesheetHeader();

                #region Dim1 List

                var Dim1List = new List<DimensionValues>();
                var pageDepartment = "DimensionValues?$filter=Global_Dimension_No eq 1&format=json";

                var httpResponseDepartment = Credentials.GetOdataData(pageDepartment);
                using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var Department = new DimensionValues();
                        Department.Code = (string)config["Code"];
                        Department.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim1List.Add(Department);
                    }
                }

                #endregion

                #region dim2

                var Dim2List = new List<DimensionValues>();
                var pageDivision = "DimensionValues?$filter=Global_Dimension_No eq 2&format=json";

                var httpResponseDivision = Credentials.GetOdataData(pageDivision);
                using (var streamReader = new StreamReader(httpResponseDivision.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim2List.Add(DList);
                    }
                }

                #endregion

                var page = "TimesheetHeader?$filter=Timesheet_No eq '" + DocNo + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        TransDoc.No = (string)config["Timesheet_No"];
                        TransDoc.Period = (string)config["Period"];
                        TransDoc.Month = (string)config["Month"];
                        TransDoc.Year = (string)config["Year"];
                        TransDoc.EmployeeNo = (string)config["Employee_No"] + "(" +
                                              CommonClass.GetEmployeeName((string)config["Employee_No"]) + ")";
                        TransDoc.StartDate = Convert.ToDateTime((string)config["StartDate"]).ToString("dd/MM/yyyy");
                        TransDoc.EndDate = Convert.ToDateTime((string)config["EndDate"]).ToString("dd/MM/yyyy");
                        TransDoc.Doc_Date = Convert.ToDateTime((string)config["Date_Created"]).ToString("dd/MM/yyyy");
                        TransDoc.Hours_Worked = (string)config["Hours_Worked"];
                        TransDoc.Total_Days_Worked = (string)config["Total_Days_Worked"];
                        TransDoc.Status = (string)config["Status"];
                    }
                }

                TransDoc.ListOfDim1 = Dim1List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                TransDoc.ListOfDim2 = Dim2List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();

                #region TimesheetLines

                var TimesheetList = new List<TimesheetLines>();
                var pageLine = "TimesheetEntries?$filter=Timesheet_No eq '" + DocNo + "'&$format=json";
                var httpResponseLine = Credentials.GetOdataData(pageLine);
                using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var TSLines = new TimesheetLines();
                        TSLines.Ledger_No = (string)config["Ledger_No"];
                        TSLines.Line_No = (string)config["Line_No"];
                        TSLines.Description = (string)config["Description"];
                        TSLines.Day_Type = (string)config["Day_Type"];
                        TSLines.ProjectCode = (string)config["Project_Code"];
                        TSLines.Date = (string)config["Date"];
                        TSLines.Hours = (string)config["Hours"];
                        TSLines.Non_Working_Day = (string)config["Non_Working_Day"];
                        TSLines.PercentageLoe = (string)config["LOE"];
                        TimesheetList.Add(TSLines);
                    }
                }

                #endregion

                TransDoc.ListOfTimesheetLines = TimesheetList;
                return PartialView("~/Views/DocumentApproval/Document Approval Views/TimeSheetDocumentDetail.cshtml",
                    TransDoc);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult WorkshopDocumentDetails(string DocNo)
        {
            try
            {
                var StaffNo = Session["Username"].ToString();
                var ImpDoc = new ImprestHeader();

                #region Dim1 List

                var Dim1List = new List<DimensionValues>();
                var pageDepartment =
                    "DimensionValues?$filter=Global_Dimension_No eq 1 and Blocked eq false&$format=json";

                var httpResponseDepartment = Credentials.GetOdataData(pageDepartment);
                using (var streamReader = new StreamReader(httpResponseDepartment.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var Department = new DimensionValues();
                        Department.Code = (string)config["Code"];
                        Department.Name = (string)config["Code"];
                        Dim1List.Add(Department);
                    }
                }

                #endregion

                #region dim2

                var Dim2List = new List<DimensionValues>();
                var pageDivision = "DimensionValues?$filter=Global_Dimension_No eq 2 and Blocked eq false&$format=json";

                var httpResponseDivision = Credentials.GetOdataData(pageDivision);
                using (var streamReader = new StreamReader(httpResponseDivision.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"];
                        Dim2List.Add(DList);
                    }
                }

                #endregion

                #region dim3

                var Dim3List = new List<DimensionValues>();
                var pageDim3 = "DimensionValues?$filter=Global_Dimension_No eq 3 and Blocked eq false&$format=json";

                var httpResponseDim3 = Credentials.GetOdataData(pageDim3);
                using (var streamReader = new StreamReader(httpResponseDim3.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"];
                        Dim3List.Add(DList);
                    }
                }

                #endregion

                #region dim4

                var Dim4List = new List<DimensionValues>();
                var pageDim4 = "DimensionValues?$filter=Global_Dimension_No eq 4 and Blocked eq false&$format=json";

                var httpResponseDim4 = Credentials.GetOdataData(pageDim4);
                using (var streamReader = new StreamReader(httpResponseDim4.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"];
                        Dim4List.Add(DList);
                    }
                }

                #endregion

                #region dim5

                var Dim5List = new List<DimensionValues>();
                var pageDim5 = "DimensionValues?$filter=Global_Dimension_No eq 5 and Blocked eq false&$format=json";

                var httpResponseDim5 = Credentials.GetOdataData(pageDim5);
                using (var streamReader = new StreamReader(httpResponseDim5.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"];
                        Dim5List.Add(DList);
                    }
                }

                #endregion

                #region dim6

                var Dim6List = new List<DimensionValues>();
                var pageDim6 = "DimensionValues?$filter=Global_Dimension_No eq 6 and Blocked eq false&$format=json";

                var httpResponseDim6 = Credentials.GetOdataData(pageDim6);
                using (var streamReader = new StreamReader(httpResponseDim6.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"];
                        Dim6List.Add(DList);
                    }
                }

                #endregion

                #region dim7

                var Dim7List = new List<DimensionValues>();
                var pageDim7 = "DimensionValues?$filter=Global_Dimension_No eq 7&$format=json";

                var httpResponseDim7 = Credentials.GetOdataData(pageDim7);
                using (var streamReader = new StreamReader(httpResponseDim7.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var DList = new DimensionValues();
                        DList.Code = (string)config["Code"];
                        DList.Name = (string)config["Code"] + "  " + (string)config["Name"];
                        Dim7List.Add(DList);
                    }
                }

                #endregion

                #region Responsibility

                var RespCList = new List<RespCenter>();
                var pageResC = "ResponsibilityCenters?$format=json";

                var httpResponseResC = Credentials.GetOdataData(pageResC);
                using (var streamReader = new StreamReader(httpResponseResC.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);


                    foreach (JObject config in details["value"])
                    {
                        var RCList = new RespCenter();
                        RCList.Code = (string)config["Code"];
                        RCList.Name = (string)config["Name"];
                        RespCList.Add(RCList);
                    }
                }

                #endregion

                var page = "WorkShopAdvance?$filter=No eq '" + DocNo + "'&$format=json";
                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        ImpDoc.No = (string)config["No"];
                        ImpDoc.DateNeeded = Convert.ToDateTime((string)config["Date"]).ToString("dd/MM/yyyy");
                        ImpDoc.Remarks = (string)config["Purpose"];
                        ImpDoc.DocD = new NewImprestRequisition
                        {
                            Dim1 = (string)config["GlobalDimension1Code"],
                            Dim2 = (string)config["ShortcutDimension2Code"],
                            Dim3 = (string)config["ShortcutDimension3Code"],
                            Dim4 = (string)config["ShortcutDimension4Code"],
                            Dim5 = (string)config["ShortcutDimension5Code"],
                            Dim6 = (string)config["ShortcutDimension6Code"],
                            Dim7 = (string)config["ShortcutDimension7Code"],
                            RespC = (string)config["Responsibility_Center"]
                        };
                        ImpDoc.Dim1 = (string)config["GlobalDimension1Code"];
                        ImpDoc.Dim2 = (string)config["ShortcutDimension2Code"];
                        ImpDoc.Dim3 = (string)config["ShortcutDimension3Code"];
                        ImpDoc.Dim4 = (string)config["ShortcutDimension4Code"];
                        ImpDoc.Dim5 = (string)config["ShortcutDimension5Code"];
                        ImpDoc.Dim6 = (string)config["ShortcutDimension6Code"];
                        ImpDoc.Dim7 = (string)config["ShortcutDimension7Code"];
                        ImpDoc.RespC = (string)config["Responsibility_Center"];
                        ImpDoc.Dim1Name = (string)config["FunctionName"];
                        ImpDoc.Dim2Name = (string)config["BudgetCenterName"];
                        ImpDoc.Dim3Name = (string)config["Dim3"];
                        ImpDoc.Dim4Name = (string)config["Dim4"];
                        ImpDoc.Dim5Name = (string)config["Dim5"];
                        ImpDoc.Dim6Name = (string)config["Dim6"];
                        ImpDoc.Dim7Name = (string)config["Dim7"];
                        ImpDoc.AccountNo = (string)config["AccountNo"] + " - " + (string)config["Payee"];
                        ImpDoc.TotalAmount = Convert.ToDecimal((string)config["TotalNetAmount"]).ToString("#,##0.00");
                        ImpDoc.Status = (string)config["Status"];
                    }
                }

                ImpDoc.DocD.ListOfDim1 = Dim1List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim2 = Dim2List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim3 = Dim3List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim4 = Dim4List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim5 = Dim5List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim6 = Dim6List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfDim7 = Dim7List.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                ImpDoc.DocD.ListOfResponsibility = RespCList.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Code
                    }).ToList();
                //#region TimesheetLines
                //List<ImprestLines> ImpLines = new List<ImprestLines>();
                //string pageLine = "WorkShopAdvLines?$filter=No eq '" + DocNo + "'&$format=json";
                //HttpWebResponse httpResponseLine = Credentials.GetOdataData(pageLine);
                //using (var streamReader = new StreamReader(httpResponseLine.GetResponseStream()))
                //{
                //    var result = streamReader.ReadToEnd();

                //    var details = JObject.Parse(result);
                //    foreach (JObject config in details["value"])
                //    {
                //        ImprestLines ImLine = new ImprestLines();
                //        ImLine.DocNo = (string)config["No"];
                //        ImLine.AdvanceType = (string)config["AdvanceType"];
                //        ImLine.Item = (string)config["AccountNo"];
                //        ImLine.ItemDesc = (string)config["AccountName"];
                //        ImLine.ItemDesc2 = (string)config["Purpose"];
                //        ImLine.LnNo = (string)config["Line_No"];
                //        ImLine.Amount = Convert.ToDecimal((string)config["Amount"]).ToString("#,##0.00");
                //        ImpLines.Add(ImLine);
                //    }
                //}
                //#endregion

                return PartialView("~/Views/DocumentApproval/Document Approval Views/WorkshopDocumentDetail.cshtml",
                    ImpDoc);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        public PartialViewResult PCADocApprovalDetails(string DocNo)
        {
            try
            {
                var pca = new PayChangeAdvice();

                var page = "PayChangeAdvice?$filter=ChangeAdviceSerialNo eq '" + DocNo + "'&$format=json";
                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        pca.ChangeAdviceSerialNo = (string)config["ChangeAdviceSerialNo"];
                        pca.Document_Type = (string)config["Document_Type"];
                        pca.Change_Bank_Details = (bool)config["Change_Bank_Details"];
                        pca.EmployeeCode = (string)config["EmployeeCode"];
                        pca.EmployeeName = (string)config["EmployeeName"];
                        pca.Current_Job_Grade = (string)config["Current_Job_Grade"];
                        pca.Salary_Scale = (string)config["Salary_Scale"];
                        pca.Present = (string)config["Present"];
                        pca.Designation = (string)config["Designation"];
                        pca.BasicPay = (string)config["BasicPay"];
                        pca.PaysNSSF = (bool)config["PaysNSSF"];
                        pca.PaysNHIF = (bool)config["PaysNHIF"];
                        pca.PaysPAYE = (bool)config["PaysPAYE"];
                        pca.Pays_NITA = (bool)config["Pays_NITA"];
                        pca.Effective_Date = (string)config["Effective_Date"];
                        pca.Comments = (string)config["Comments"];
                        pca.Description = (string)config["Description"];
                        pca.Status = (string)config["Status"];
                        pca.PayrollPeriod = DateTime.Parse((string)config["PayrollPeriod"]);
                        pca.Implementing_Unit = (string)config["Implementing_Unit"];
                        pca.Duty_Station = (string)config["Duty_Station"];
                        pca.Source_Document = (string)config["Source_Document"];
                        pca.Document_No = (string)config["Document_No"];
                        pca.Employee_Status = (string)config["Employee_Status"];
                        pca.Employee_Status_II = (string)config["Employee_Status_II"];
                        pca.Bank_Account_Number = (string)config["Bank_Account_Number"];
                        pca.Employee_Bank = (string)config["Employee_x0027_s_Bank"];
                        pca.Bank_Name = (string)config["Bank_Name"];
                        pca.Bank_Branch = (string)config["Bank_Branch"];
                        pca.Bank_Branch_Name = (string)config["Bank_Branch_Name"];
                    }
                }


                return PartialView("~/Views/DocumentApproval/Document Approval Views/PcaDocDetails.cshtml", pca);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        [HttpPost]
        public JsonResult ApproveDocument(string DocNo, string EntryNo)
        {
            try
            {
                var rtVal = false;
                var msg = "";
                var Redirect = "";


                if (Session["UserID"] != null)
                {
                    var userID = Session["UserID"].ToString();
                    Credentials.ObjNav.fnApproveRequest(DocNo, userID);
                    rtVal = true;
                    msg = "Request approved Successfully";
                }
                else
                {
                    rtVal = false;
                    msg = "/Login/Login";
                }

                if (rtVal)
                {
                    var tableId = int.Parse(Session["TableId"].ToString());
                    if (tableId == 70141)
                    {
                        Credentials.ObjNav.FnSendTenderCommitteNotification(DocNo);
                    }
                    Redirect =
                        $"/DocumentApproval/GetDocumentForApprovalList?TbID={tableId}&Status=Open&Title=%20&DocType=N";
                }

                return Json(new { message = Redirect, success = true, Redirect = rtVal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RejectDocument(string TBID, string DocNo, string Comments, string SeqNo, string EntryNo)
        {
            try
            {
                var userID = Session["UserID"].ToString();

                Credentials.ObjNav.DocumentRejections(Convert.ToInt32(EntryNo), DocNo, userID, Comments,
                    Convert.ToInt32(TBID), Convert.ToInt32(SeqNo));
                var msg = "Approval Request Rejected";
                if (TBID == "69205")
                {
                    var page = "HRLeaveRequisition?$filter=Application_Code eq '" + DocNo + "'&format=json";

                    var httpResponse = Credentials.GetOdataData(page);
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        var details = JObject.Parse(result);
                        foreach (JObject config in details["value"])
                        {
                            var sender = (string)config["Employee_No"];
                            var senderName = CommonClass.GetEmployeeFirstName((string)config["Employee_No"]);
                            var Senderemail = CommonClass.GetEmployeeEmail(sender);
                            var Designation = CommonClass.GetEmployeeSalutationDetails(sender);

                            var msgs = "";
                            msgs = "Dear " + Designation[1] + " " + senderName + ",<br /><br />"
                                   + "Your leave application has not been approved.<br /><br />"
                                   + "<b />Details:<br /><br />"
                                   + "<b>Reason :</b> <i>" + Comments + "</i><br /><br />"
                                ;
                            Credentials.ObjNav.UpdateLeaveRejection(DocNo, Comments);
                            CommonClass.SendEmailAlert(msgs, Senderemail, "Leave Approval Rejected: " + DocNo);
                        }
                    }
                }

                return Json(new { message = msg, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PreviousApprovalTrail(string TableID, string DocNo, string DocType)
        {
            try
            {
                var approvalDocList = new List<DocumentsForApproval>();
                var userID = Session["UserID"].ToString();

                var page =
                    "ApprovalEntries?$select=Sequence_No,Approver_ID,Last_Modified_By_User_ID,Status&$filter=Document_No eq '" +
                    DocNo + "' and Table_ID eq " + TableID + " and Status eq 'Approved'&$format=json";
                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (JObject config in details["value"])
                    {
                        var DocList = new DocumentsForApproval();
                        DocList.Sequence = (string)config["Sequence_No"];
                        var appr = (string)config["Approver_ID"];
                        var AppNo = appr.Replace(@"COURT\", "");
                        var EmplName = CommonClass.GetEmployeeName(AppNo);
                        if (EmplName != null)
                            DocList.Approver = (string)config["Approver_ID"] + " - " + EmplName;
                        else
                            DocList.Approver = (string)config["Approver_ID"];
                        if (TableID == "69205")
                        {
                            if ((string)config["Sequence_No"] == "1")
                                DocList.Status = "Forwaded";
                            else if ((string)config["Sequence_No"] == "2")
                                DocList.Status = "Forwaded";
                            else
                                DocList.Status = (string)config["Status"];
                        }
                        else if (TableID == "69011")
                        {
                            if ((string)config["Sequence_No"] == "1")
                                DocList.Status = "Verified";
                            else if ((string)config["Sequence_No"] == "1")
                                DocList.Status = "Recommended";
                            else
                                DocList.Status = (string)config["Status"];
                        }
                        else
                        {
                            DocList.Status = (string)config["Status"];
                        }

                        approvalDocList.Add(DocList);
                    }
                }

                return PartialView("~/Views/DocumentApproval/Document Approval Views/DocApprovalTrail.cshtml",
                    approvalDocList.OrderByDescending(x => x.Sequence));
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message;
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }

        [HttpPost]
        public JsonResult AssignDriverVehicle(string DocNo, string Driver, string Vehicle)
        {
            try
            {
                var rtVal = false;
                var msg = "";

                if (Session["Username"] != null)
                {
                    var userID = Session["Username"].ToString();
                    Credentials.ObjNav.AssignTransportRequisitionDriver(DocNo, Driver, Vehicle, userID);
                    rtVal = true;
                    msg = "Request approved Successfully";
                }
                else
                {
                    rtVal = false;
                    msg = "/Login/Login";
                }

                return Json(new { message = msg, success = true, Redirect = rtVal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ApproveLeaveAllocationDays(string DocNo, string Days, string Comments)
        {
            try
            {
                var rtVal = false;
                var msg = "";

                if (Session["Username"] != null)
                {
                    var userID = Session["Username"].ToString();
                    //  Credentials.ObjNav.ApproveLeaveReimbursement(DocNo, Convert.ToInt32(Days), Comments, userID);
                    rtVal = true;
                    msg = "Request approved Successfully";
                }
                else
                {
                    rtVal = false;
                    msg = "/Login/Login";
                }

                return Json(new { message = msg, success = true, Redirect = rtVal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult LeavePlannerDocument(string DocNo)
        {
            var leavePlanner = new LeavePlannerList();
            var page = "HRLeavePlanner?$filter=Application_Code eq '" + DocNo + "'&$format=json";
            var httpResponse = Credentials.GetOdataData(page);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    leavePlanner.Application_Code = (string)config["Application_Code"];
                    leavePlanner.Leave_Calendar = (string)config["Leave_Calendar"];
                    leavePlanner.Status = (string)config["Status"];
                    leavePlanner.Job_Tittle = (string)config["Implementing_Unit_Name"];
                    leavePlanner.Names = (string)config["Responsibility_Center"];
                }
            }


            var pageLines = "HRLeavePlannerLines?$filter=Application_Code eq '" + DocNo +
                            "'&$format=json&$orderby=Start_Date";

            var plannerLines = new List<LeavePlannerLines>();

            var httpResponseLines = Credentials.GetOdataData(pageLines);
            using (var streamReaderLines = new StreamReader(httpResponseLines.GetResponseStream()))
            {
                var result = streamReaderLines.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    var leavePlannerLines = new LeavePlannerLines();
                    leavePlannerLines.Line_No = int.Parse((string)config["Line_No"]);
                    leavePlannerLines.Application_Code = (string)config["Application_Code"];
                    leavePlannerLines.Leave_Type = (string)config["Leave_Type"];
                    leavePlannerLines.Days_Applied = int.Parse((string)config["Days_Applied"]);
                    leavePlannerLines.Start_Date = Convert.ToDateTime(config["Start_Date"]).ToString("dd/MM/yyyy");
                    leavePlannerLines.End_Date = Convert.ToDateTime(config["End_Date"]).ToString("dd/MM/yyyy");
                    leavePlannerLines.Return_Date = Convert.ToDateTime(config["End_Date"]).ToString("dd/MM/yyyy");
                    leavePlannerLines.Applicant_Comments = (string)config["Applicant_Comments"];
                    leavePlannerLines.Request_Leave_Allowance = bool.Parse((string)config["Request_Leave_Allowance"]);
                    leavePlannerLines.Reliever = (string)config["Employee_No"];
                    leavePlannerLines.Reliever_Name = CommonClass.GetEmployeeName((string)config["Employee_No"]);
                    leavePlannerLines.Approved_days = int.Parse((string)config["Approved_days"]);
                    leavePlannerLines.Date_of_Exam = DateTime.Parse(config["Date_of_Exam"]?.ToString());
                    leavePlannerLines.Details_of_Examination = (string)config["Details_of_Examination"];
                    plannerLines.Add(leavePlannerLines);
                }

                var docDetails = new LeavePlannerDocument
                {
                    DocHeader = leavePlanner,
                    ListOfLeavePlannerLines = plannerLines
                };

                return PartialView("~/Views/DocumentApproval/Document Approval Views/LeavePlannerDetails.cshtml",
                    docDetails);
            }
        }

        public PartialViewResult LoanApprovalDocument(string DocNo)
        {
            var loanApplicationCard = new LoanApplicationCard();
            var LeaveDoc = new LeaveReqList();


            //string loanApplicationsCard = "LoanApplicationsCard?$filter=Loan_No eq '" + loanNo + "'&$format=json";
            //HttpWebResponse httpResponse = Credentials.GetOdataData(loanApplicationsCard);
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();

            //    var details = JObject.Parse(result);
            //    foreach (JObject config in details["value"])
            //    {
            var loanApplicationsCard = "CarLoanApplicationsCard?$filter=Loan_No eq '" + DocNo + "'&$format=json";
            var httpResponse = Credentials.GetOdataData(loanApplicationsCard);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var details = JObject.Parse(result);
                foreach (JObject config in details["value"])
                {
                    loanApplicationCard.LoanNo = (string)config["Loan_No"];
                    loanApplicationCard.LoanProductType = (string)config["Loan_Product_Type"];
                    loanApplicationCard.LoanCategory = (string)config["Loan_Category"];
                    loanApplicationCard.Description = (string)config["Description"];
                    loanApplicationCard.EmployeeNo = (string)config["Employee_No"];
                    loanApplicationCard.EmployeeName = (string)config["Employee_Name"];
                    loanApplicationCard.ApplicationDate = (string)config["Application_Date"];
                    loanApplicationCard.AmountRequested = (decimal)config["Amount_Requested"];
                    loanApplicationCard.Reason = (string)config["Reason"];
                    loanApplicationCard.ApprovalCommittee = (string)config["Approval_Committee"];
                    loanApplicationCard.LoanStatus = (string)config["Loan_Status"];
                    loanApplicationCard.IssuedDate = DateTime.Parse((string)config["Issued_Date"]);
                    loanApplicationCard.ApprovedAmount = (decimal)config["Approved_Amount"];
                    //loanApplicationCard.Instalment = (decimal)config["Instalment"];
                    loanApplicationCard.Repayment = (decimal)config["Repayment"];
                    loanApplicationCard.FlatRatePrincipal = (decimal)config["Flat_Rate_Principal"];
                    loanApplicationCard.FlatRateInterest = (decimal)config["Flat_Rate_Interest"];
                    loanApplicationCard.InterestRate = (decimal)config["Interest_Rate"];
                    loanApplicationCard.InterestCalculationMethod = (string)config["Interest_Calculation_Method"];
                    loanApplicationCard.PayrollGroup = (string)config["Payroll_Group"];
                    loanApplicationCard.OpeningLoan = (bool)config["Opening_Loan"];
                    loanApplicationCard.TotalRepayment = (decimal)config["Total_Repayment"];
                    //loanApplicationCard.PeriodRepayment = (decimal)config["Period_Repayment"];
                    loanApplicationCard.InterestAmount = (decimal)config["Interest_Amount"];
                    loanApplicationCard.ExternalDocumentNo = (string)config["External_Document_No"];
                    //loanApplicationCard.Receipts = (decimal)config["Receipts"];
                    loanApplicationCard.HELBNo = (string)config["HELB_No"];
                    loanApplicationCard.UniversityName = (string)config["University_Name"];
                    loanApplicationCard.StopLoan = (bool)config["Stop_Loan"];
                    loanApplicationCard.RefinancedFromLoan = (string)config["Refinanced_From_Loan"];
                    loanApplicationCard.DateFilter = (string)config["Date_filter"];
                    loanApplicationCard.BasicPay = (string)config["Basic_Pay"];
                    loanApplicationCard.NetPay = (string)config["Net_Pay"];
                    loanApplicationCard.Instalment = (string)config["Instalment"] + " Month(s)";
                    loanApplicationCard.ApprovalCommittee = (string)config["Salaries_Comments"];
                    loanApplicationCard.DirectorComments = (string)config["Director_Comments"];
                }
            }

            return PartialView("~/Views/DocumentApproval/Document Approval Views/LoanApproval.cshtml",
                loanApplicationCard);
        }

        [HttpPost]
        public JsonResult AddLoanApprovalComments(string DocNo, string HRComments, string DirectorsComments)
        {
            try
            {
                var rtVal = false;
                var msg = "";

                if (Session["Username"] != null)
                {
                    var userID = Session["Username"].ToString();
                    Credentials.ObjNav.UpdateLoanComments(DocNo, HRComments, DirectorsComments);
                    rtVal = true;
                    msg = "Request approved Successfully";
                }
                else
                {
                    rtVal = false;
                    msg = "/Login/Login";
                }

                return Json(new { message = msg, success = true, Redirect = rtVal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerateApprovalOtp(string DocNo, string tableId, string otpType)
        {
            try
            {
                var employee = Session["EmployeeData"] as EmployeeView;
                var userId = employee?.UserID;
                Credentials.ObjNav.GenerateAndSendOTP(userId, DocNo, Convert.ToInt32(tableId), Convert.ToInt32(otpType));

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidateApprovalOtp(string enteredOtp)
        {
            try
            {
                var employee = Session["EmployeeData"] as EmployeeView;
                var userId = employee?.UserID;
                var otpSecure = Credentials.ObjNav.ValidateOTP(enteredOtp, userId);
                if (otpSecure) return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.Replace("'", ""), success = false },
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Imprest and Staff Claim Approvals
        /// </summary>
        /// <param name="docNo"></param>
        /// <returns></returns>
        public ActionResult StandingImprestDocument(string docNo)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");
            try
            {
                var imprest = new StandingImprest();

                var page = "StandingImprest?$filter=No eq '" + docNo + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;
                        imprest.No = (string)config["No"];
                        imprest.Date = DateTime.ParseExact((string)config["Date"], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        imprest.PayMode = (string)config["Pay_Mode"];
                        imprest.ChequeNo = (string)config["Cheque_No"];
                        imprest.ChequeDate = DateTime.ParseExact((string)config["Cheque_Date"], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        imprest.PostingDate = DateTime.ParseExact((string)config["Posting_Date"], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        imprest.StandingImprestType = (string)config["Standing_Imprest_Type"];
                        imprest.PayingBankAccount = (string)config["Paying_Bank_Account"];
                        imprest.BankName = (string)config["Bank_Name"];
                        imprest.PaymentNarration = (string)config["Payment_Narration"];
                        imprest.CurrencyCode = (string)config["Currency_Code"];
                        imprest.TotalAmount = ((decimal)config["Total_Amount"]).ToString("C", new CultureInfo("sw-KE"));
                        imprest.ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"];
                        imprest.DepartmentName = (string)config["Department_Name"];
                        imprest.ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"];
                        imprest.ProjectName = (string)config["Project_Name"];
                        imprest.StrategicPlan = (string)config["Strategic_Plan"];
                        imprest.ReportingYearCode = (string)config["Reporting_Year_Code"];
                        imprest.WorkplanCode = (string)config["Workplan_Code"];
                        imprest.ActivityCode = (string)config["Activity_Code"];
                        imprest.Status = (string)config["Status"];
                        imprest.Posted = (bool)config["Posted"];
                        imprest.PostedBy = (string)config["Posted_By"];
                        imprest.PostedDate = DateTime.ParseExact((string)config["Posted_Date"], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        imprest.DimensionSetId = (string)config["Dimension_Set_ID"];

                    }
                }
                imprest.CourtStation = DimensinValuesList.GetDimensionValueName(imprest.ShortcutDimension1Code);
                imprest.AdminUnit = DimensinValuesList.GetDimensionValueName(imprest.ShortcutDimension2Code);

                return View("Document Approval Views/StandingImprestDocument", imprest);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public ActionResult ImprestWarrantDocument(string docNo)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");
            try
            {
                var imprest = new ImprestWarranties();

                var page = "ImprestWarranties?$filter=No eq '" + docNo + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;
                        imprest.No = (string)config["No"];
                        imprest.Date = DateTime
                            .ParseExact((string)config["Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                        imprest.PostingDate = DateTime
                            .ParseExact((string)config["Posting_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                        imprest.AccountType = (string)config["Account_Type"];
                        imprest.AccountNo = (string)config["Account_No"];
                        imprest.AccountName = (string)config["Account_Name"];
                        imprest.ReferenceNo = (string)config["Reference_No"];
                        imprest.PayMode = (string)config["Pay_Mode"];
                        imprest.ChequeNo = (string)config["Cheque_No"];
                        imprest.PayingBankAccount = (string)config["Paying_Bank_Account"];
                        imprest.BankName = (string)config["Bank_Name"];
                        imprest.TravelDate = DateTime
                            .ParseExact((string)config["Travel_Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                        imprest.PaymentNarration = (string)config["Payment_Narration"];
                        imprest.CreatedBy = (string)config["Created_By"];
                        imprest.Status = (string)config["Status"];
                        imprest.StrategicPlan = (string)config["Strategic_Plan"];
                        imprest.ReportingYearCode = (string)config["Reporting_Year_Code"];
                        imprest.WorkplanCode = (string)config["Workplan_Code"];
                        imprest.ActivityCode = (string)config["Activity_Code"];
                        imprest.ExpenditureRequisitionCode = (string)config["Expenditure_Requisition_Code"];
                        imprest.ImprestMemoNo = (string)config["Imprest_Memo_No"];
                        imprest.ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"];
                        imprest.ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"];
                        imprest.ImprestAmount = ((decimal)config["Imprest_Amount"]).ToString("C", CultureInfo.CurrentCulture);
                        imprest.DimensionSetId = (string)config["Dimension_Set_ID"];

                        imprest.ImprestDeadline = DateTime
                            .ParseExact((string)config["Imprest_Deadline"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyyy");
                    }
                }

                imprest.LocationName = DimensinValuesList.GetDimensionValueName(imprest.ShortcutDimension1Code);
                imprest.AdminUnitName = DimensinValuesList.GetDimensionValueName(imprest.ShortcutDimension2Code);

                return View("Document Approval Views/ImprestWarrantDocument", imprest);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public ActionResult SpecialImprestDocument(string docNo)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "Login");
            try
            {
                var imprest = new SpecialImprest();

                var page = "SpecialImprestWarranty?$filter=No eq '" + docNo + "'&$format=json";

                var httpResponse = Credentials.GetOdataData(page);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var details = JObject.Parse(result);
                    foreach (var jToken in details["value"])
                    {
                        var config = (JObject)jToken;
                        imprest = new SpecialImprest
                        {
                            No = (string)config["No"],
                            Date = (string)config["Date"],
                            PostingDate = (string)config["Posting_Date"],
                            StandingImprestType = (string)config["Standing_Imprest_Type"],
                            ChequeDate = (string)config["Cheque_Date"],
                            PayingBankAccount = (string)config["Paying_Bank_Account"],
                            BankName = (string)config["Bank_Name"],
                            PaymentNarration = (string)config["Payment_Narration"],
                            CurrencyCode = (string)config["Currency_Code"],
                            TotalAmount = ((decimal)config["Total_Amount"]).ToString("C", CultureInfo.CurrentCulture),
                            ShortcutDimension1Code = (string)config["Shortcut_Dimension_1_Code"],
                            ShortcutDimension2Code = (string)config["Shortcut_Dimension_2_Code"],
                            StrategicPlan = (string)config["Strategic_Plan"],
                            ReportingYearCode = (string)config["Reporting_Year_Code"],
                            WorkplanCode = (string)config["Workplan_Code"],
                            ActivityCode = (string)config["Activity_Code"],
                            ExpenditureRequisitionCode = (string)config["Expenditure_Requisition_Code"],
                            Status = (string)config["Status"],
                            Posted = (bool)config["Posted"],
                            PostedBy = (string)config["Posted_By"],
                            PostedDate = (string)config["Posted_Date"],
                            PayMode = (string)config["Pay_Mode"],
                            Payee = (string)config["Payee"],
                            DimensionSetId = (string)config["Dimension_Set_ID"]
                        };
                    }
                }
                return View("Document Approval Views/SpecialImprestDocument", imprest);
            }
            catch (Exception ex)
            {
                var erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return PartialView("~/Views/Shared/Partial Views/ErroMessangeView.cshtml", erroMsg);
            }
        }
        public ActionResult StaffClaimDocumentView(string DocNo)
        {
            try
            {
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                StaffClaims claim = new StaffClaims();
                string page = "StaffClaims?$filter=No eq '" + DocNo + "'&format=json";
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
                            DimensionSetId = (string)config["Dimension_Set_ID"]
                        };
                    }
                }
                return View("Document Approval Views/StaffClaimDocumentView", claim);
            }
            catch (Exception ex)
            {
                Error erroMsg = new Error();
                erroMsg.Message = ex.Message.Replace("'", "");
                return View("~/Views/Common/ErrorMessange.cshtml", erroMsg);
            }
        }
        public JsonResult GenerateVoucherReports(string documentNumber)
        {
            try
            {
                var message = "";
                bool success = false, view = false;

                message = Credentials.ObjNav.GenerateVoucherReport2(documentNumber);
                if (message == "")
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
        public ActionResult PurchaseOrdersDocumentView(string DocNo)
        {
            try
            {
                string userId = Session["UserId"]?.ToString();
                EmployeeView employeeView = Session["EmployeeData"] as EmployeeView;
                List<PurchaseOrder> purchaseOrdersList = new List<PurchaseOrder>();
                string empNo = employeeView.No;
                string page = $"PurchaseOrdersList?$filter=No eq '{DocNo}'&$format=json";
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
                            Posting_Description = (string)config["Posting_Description"],
                        };

                        purchaseOrdersList.Add(purchaseOrder);
                    }
                }

                var purchases = purchaseOrdersList.FirstOrDefault();

                if (purchases == null)
                {
                    return HttpNotFound("Purchase order not found");
                }
                return View("Document Approval Views/PurchaseOrdersDocumentView", purchases);
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(500, "An error occurred while processing your request.");
            }
        }
    }
}