using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Latest_Staff_Portal.ViewModel
{
    public class TrainingRequisition
    {
       
        public string Code { get; set; }
        public string Training_Plan_No { get; set; }
        public string Employee_Department { get; set; }
        public string Course_Title { get; set; }
        public string Description { get; set; }
        public string Start_DateTime { get; set; }
        public string End_DateTime { get; set; }
        public int Duration { get; set; }
        public string Duration_Type { get; set; }
        public string Training_Region { get; set; }
        public int Cost { get; set; }
        public string Training_Venue_Region_Code { get; set; }
        public string Training_Venue_Region { get; set; }
        public string Training_Responsibility_Code { get; set; }
        public string Training_Responsibility { get; set; }
        public string Location { get; set; }
        public string Provider { get; set; }
        public string Provider_Name { get; set; }
        public string Training_Type { get; set; }
        public int Training_Duration_Hrs { get; set; }
        public int Planned_Budget { get; set; }
        public int Planned_No_to_be_Trained { get; set; }
        public int No_of_Participants { get; set; }
        public int Total_Procurement_Cost { get; set; }
        public int Other_Costs { get; set; }
        public string Employee_No { get; set; }
        public string Employee_Name { get; set; }
        public string Created_By { get; set; }
        public string Created_On { get; set; }
        public string Status { get; set; }
        public bool Bonded { get; set; }
        public string Bonding_Period { get; set; }
        public string Expected_Bond_End { get; set; }
        public string Expected_Bond_Start { get; set; }
        public int Bond_Penalty { get; set; }

        public List<SelectListItem> ListOfTrainingPlan { get; set; }


        public List<SelectListItem> ListOfResponsibilityCenters { get; set; }
        public List<SelectListItem> ListOfTrainingCourses { get; set; }
        public List<SelectListItem> ListOfLocations { get; set; }
        public List<SelectListItem> ListOfRegions { get; set; }
        public List<SelectListItem> ListOfProviders { get; set; }
    }


    public class TrainingParticipantsList
    {
        
        public int Line_No { get; set; }
        public string Training_Code { get; set; }
        public string Employee_Code { get; set; }
        public string Training_Responsibility_Code { get; set; }
        public string Employee_Name { get; set; }
        public string Type { get; set; }
        public string Destination { get; set; }
        public int No_of_Days { get; set; }
        public int Total_Amount { get; set; }
        public string Training_Responsibility { get; set; }
        public string Global_Dimension_1_Code { get; set; }
        public string Witness { get; set; }
        public string Witness_Name { get; set; }
        public bool Charge_Levy { get; set; }
        public string Requestor { get; set; }

        public List<SelectListItem> ListOfEmployees { get; set; }
        public List<SelectListItem> ListOfExpenseTypes { get; set; }
        public List<SelectListItem> ListOfRegions { get; set; }
    }


}