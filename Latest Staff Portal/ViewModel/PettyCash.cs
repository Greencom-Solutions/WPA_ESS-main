using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Latest_Staff_Portal.ViewModel
{
    public class PettyCashVouchers
    {
        public string No { get; set; }
        public string Date { get; set; }
        public string Pay_Mode { get; set; }
        public string Cheque_No { get; set; }
        public string Cheque_Date { get; set; }
        public string Paying_Bank_Account { get; set; }
        public string Payee { get; set; }
        public int Total_Amount_LCY { get; set; }
        public string Currency_Code { get; set; }
        public string Created_By { get; set; }
        public string Status { get; set; }
        public bool Posted { get; set; }
        public string Posted_By { get; set; }
        public string Posted_Date { get; set; }
        public string Shortcut_Dimension_1_Code { get; set; }
        public string Shortcut_Dimension_2_Code { get; set; }
    }



    public class PettyCashVoucherLines
    {

        public string No { get; set; }
        public int Line_No { get; set; }
        public string Bounced_Pv_No { get; set; }
        public string Type { get; set; }
        public string Account_Type { get; set; }
        public string Account_No { get; set; }
        public string Account_Name { get; set; }
        public int Amount { get; set; }
        public int Net_Amount { get; set; }
        public int Remaining_Amount { get; set; }
        public int VAT_Rate { get; set; }
        public int VAT_Six_Percent_Rate { get; set; }
        public string VAT_Withheld_Code { get; set; }
        public int VAT_Withheld_Amount { get; set; }
        public bool Budgetary_Control_A_C { get; set; }
        public int Advance_Recovery { get; set; }
        public int Retention_Amount { get; set; }
        public string Shortcut_Dimension_1_Code { get; set; }
        public string Shortcut_Dimension_2_Code { get; set; }
        public string Claim_Doc_No { get; set; }
        public string VAT_Code { get; set; }
        public string W_Tax_Code { get; set; }
        public string W_T_VAT_Code { get; set; }
        public int VAT_Amount { get; set; }
        public int W_Tax_Amount { get; set; }
        public int Total_Net_Pay { get; set; }
        public string Status { get; set; }
    }

    public class NewPettyCashRequisition
    {
        public string Dim1 { get; set; }
        public string Dim2 { get; set; }
        public string Dim3 { get; set; }
        public string Dim4 { get; set; }
        public string Dim5 { get; set; }
        public string Dim6 { get; set; }
        public string Dim7 { get; set; }
        public string Dim8 { get; set; }
        public string RespC { get; set; }
        public string Dim1Name { get; set; }
        public string Dim2Name { get; set; }
        public string Dim3Name { get; set; }
        public string Dim4Name { get; set; }
        public string Dim5Name { get; set; }
        public string Dim6Name { get; set; }
        public string Dim7Name { get; set; }
        public string Dim8Name { get; set; }
        public string UoM { get; set; }
        public string PettyCashDueType { get; set; }
        public List<SelectListItem> ListOfDim1 { get; set; }
        public List<SelectListItem> ListOfDim2 { get; set; }
        public List<SelectListItem> ListOfDim3 { get; set; }
        public List<SelectListItem> ListOfDim4 { get; set; }
        public List<SelectListItem> ListOfDim5 { get; set; }
        public List<SelectListItem> ListOfDim6 { get; set; }
        public List<SelectListItem> ListOfDim7 { get; set; }
        public List<SelectListItem> ListOfDim8 { get; set; }

        public List<SelectListItem> ListOfResponsibility { get; set; }
        public List<SelectListItem> ListOfPettyCashDue { get; set; }
    }

    public class PettyCashTypes
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class PettyCashTypesList
    {
        public string Code { get; set; }
        public List<SelectListItem> ListOfPettyCashTypes { get; set; }
        public List<SelectListItem> ListOfUnitMeasure { get; set; }
        public List<SelectListItem> ListOfDestination { get; set; }
    }

    public class PettyCashHeader
    {
        public string No { get; set; }
        public string DateNeeded { get; set; }
        public string Remarks { get; set; }
        public NewPettyCashRequisition DocD { get; set; }
        public string Status { get; set; }
        public string TotalAmount { get; set; }
        public string RequestorNo { get; set; }
        public string RequestorName { get; set; }
        public string Dim1 { get; set; }
        public string Dim2 { get; set; }
        public string Dim3 { get; set; }
        public string Dim4 { get; set; }
        public string Dim5 { get; set; }
        public string Dim6 { get; set; }
        public string Dim7 { get; set; }
        public string Dim8 { get; set; }
        public string Dim1Name { get; set; }
        public string Dim2Name { get; set; }
        public string Dim3Name { get; set; }
        public string Dim4Name { get; set; }
        public string Dim5Name { get; set; }
        public string Dim6Name { get; set; }
        public string Dim7Name { get; set; }
        public string Dim8Name { get; set; }
        public string PettyCashDueType { get; set; }
        public string RespC { get; set; }
    }

    public class PettyCashLines
    {
        public string DocNo { get; set; }
        public string AdvanceType { get; set; }
        public string Item { get; set; }
        public string ItemDesc { get; set; }
        public string ItemDesc2 { get; set; }
        public string Quantity { get; set; }
        public string UnitAmount { get; set; }
        public string Amount { get; set; }
        public string UoN { get; set; }
        public string Desination { get; set; }
        public string LnNo { get; set; }
    }

    public class PettyCashLinesList
    {
        public string Status { get; set; }
        public List<PettyCashLines> ListOfPettyCashLines { get; set; }
    }

    public class PettyCashItemDetails
    {
        public string Code { get; set; }
        public List<SelectListItem> ListOfPettyCashTypes { get; set; }
        public List<SelectListItem> ListOfUoM { get; set; }
        public List<SelectListItem> ListOfDestination { get; set; }
        public PettyCashLines ItemDetails { get; set; }
    }

    public class PettyCashDocument
    {
        public PettyCashHeader DocHeader { get; set; }
        public List<PettyCashLines> ListOfPettyCashLines { get; set; }
    }
}