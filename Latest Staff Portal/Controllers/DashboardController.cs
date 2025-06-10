using System;
using System.Web.Mvc;
using Latest_Staff_Portal.CustomSecurity;
using Latest_Staff_Portal.ViewModel;

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

        public ActionResult PersonalProfile2()
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
    }
}