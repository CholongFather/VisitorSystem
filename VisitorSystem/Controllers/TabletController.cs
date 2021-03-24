using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.IO;
using VisitorSystem.Models;
using VisitorSystem.Util;
using VisitorSystem.Service;


namespace VisitorSystem.Controllers
{
    [AllowAnonymous]
    public class TabletController : BaseController
    {
        /// <summary>
        /// GET: /Tablet/Intro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Intro()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
            }

            return View();
        }

        /// <summary>
        /// GET: /Tablet/Intro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult NoActionIntro()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
            }

            return View();
        }

        /// <summary>
        /// GET: /Tablet/VisitorInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorInfo()
        {
            try
            {
                //Submit 중 필수값 체크시 에러 나타날 경우 데이터를 보존하기 위한 TempData 처리.
                //TempData는 Url을 한번만 이동할 수있는 Stream과 같은 데이터 형태.
                ViewBag.Error = TempData["error"];
                VisitorInfo Visitor = (VisitorInfo)TempData["Visitor"];

                return View(Visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }
            return View();
        }


        /// <summary>
        /// POST: /Tablet/VisitorInfoProcess
        /// </summary>
        /// <param name="col">폼에서 Submit된 데이터</param>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VisitorInfo(FormCollection col)
        {
            try
            {
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                //필수값, 옵션값을 나누게끔 설계. 서버에서 필수값을 체크안하면 웹 브라우저에서 Js 변조해서 들어올 경우에 보안에 취약해진다.

                TabletService service = new TabletService();
                
                bool error = false;

                //필수값 체크
                if (!Parameter.TryGetValue("VisitorName", out string name) || string.IsNullOrEmpty(name))
                {
                    TempData["error"] = "방문객 이름이 없습니다.";
                    error = true;
                }

                if (!Parameter.TryGetValue("VisitorMobile", out string mobile) || string.IsNullOrEmpty(mobile))
                {
                    TempData["error"] = "연락처 정보가 없습니다.";
                    error = true;
                }
                else
                {
                    if(service.ChackMobileVaildation(mobile) == 1)
                    {
                        TempData["error"] = "연락처 형식을 맞춰주세요 000-0000-0000.";
                        error = true;
                    }
                }

                if (!Parameter.TryGetValue("VisitorCompany", out string company) || string.IsNullOrEmpty(company))
                {
                    
                }

                //옵션값은 체크 없이 Parameter 전달.
                if (error)
                {
                    TempData["Visitor"] = new VisitorInfo()
                    {
                        VisitorName = Parameter["VisitorName"],
                        VisitorMobile = Parameter["VisitorMobile"],
                        VisitorCompany = Parameter["VisitorCompany"],
                        HostCompany = Parameter["HostCompany"],
                        HostName = Parameter["HostName"]
                    };
                    return RedirectToAction("VisitorInfo");

                }
                else
                {
                    VisitorInfo Visitor = new VisitorInfo()
                    {
                        VisitorName = Parameter["VisitorName"],
                        VisitorMobile = Parameter["VisitorMobile"],
                        VisitorCompany = Parameter["VisitorCompany"],
                        HostCompany = Parameter["HostCompany"],
                        HostName = Parameter["HostName"]
                    };

                    Session["visitor"] = Visitor;

                    return RedirectToAction("VisitorInfoAgree");
                }
            }
            catch (Exception ex)
            {
                //핸들링 하지 않은 에러가 날 경우 Intro로 날려보냄.
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
                return RedirectToAction("intro");
            }
        }


        /// <summary>
        /// GET: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorInfoAgree()
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];
                return View(visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }

            return View();
        }

        /// <summary>
        /// Post: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VisitorInfoAgree(FormCollection col)
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                //bool error = false;

                visitor.AgreeFirst = Convert.ToBoolean(Parameter["AgreeFirst"]);
                visitor.AgreeFirstFlag = visitor.AgreeFirst == true ? 'Y' : 'N';

                visitor.AgreeSecond = Convert.ToBoolean(Parameter["AgreeSecond"]);
                visitor.AgreeSecondFlag = visitor.AgreeSecond == true ? 'Y' : 'N';

                //SignPath 저장하시길
                string jsonSign = Parameter["SignPad"];
                string SignPath = Server.MapPath("~/Sign/" + visitor.VisitorName + "_" + visitor.VisitorMobile + "_" + DateTime.Now.ToString("yyyyMMddhh") + ".json");

                visitor.SignPath = "C:\\";

                if(!Directory.Exists(Server.MapPath("~/Sign")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Sign"));
                }

                using (StreamWriter file = new StreamWriter(SignPath))
                {
                    file.Write(jsonSign);
                }

                visitor.SignPath = SignPath;
                Session["visitor"] = visitor;

                return RedirectToAction("VisitorSmsAuth");
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
                return RedirectToAction("intro");
            }
        }

        /// <summary>
        /// GET: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PopupVisitorInfoAgree(int VisitorHistorySeq)
        {
            try
            {
                ViewBag.Process = "S";
                ViewBag.VisitorHistorySeq = VisitorHistorySeq;

                TabletService service = new TabletService();
                VisitorInfo visitor = service.GetVisitorHistoryInfo(VisitorHistorySeq);

                return View(visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }

            return View();
        }



        /// <summary>
        /// GET: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopupVisitorInfoAgree(FormCollection col)
        {
            try
            {
                TabletService service = new TabletService();
                
                VisitorInfo visitor = new VisitorInfo();
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                //bool error = false;

                visitor.AgreeFirst = Convert.ToBoolean(Parameter["AgreeFirst"]);
                visitor.AgreeFirstFlag = visitor.AgreeFirst == true ? 'Y' : 'N';

                visitor.AgreeSecond = Convert.ToBoolean(Parameter["AgreeSecond"]);
                visitor.AgreeSecondFlag = visitor.AgreeSecond == true ? 'Y' : 'N';

                visitor.VisitorHistorySeq = Convert.ToInt32(Parameter["VisitorHistorySeq"]);

                //SignPath 저장하시길
                string jsonSign = Parameter["SignPad"];
                string SignPath = Server.MapPath("~/Sign/" + visitor.VisitorName + "_" + visitor.VisitorMobile + "_" + DateTime.Now.ToString("yyyyMMddhh") + ".json");

                visitor.SignPath = "C:\\";

                if (!Directory.Exists(Server.MapPath("~/Sign")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Sign"));
                }

                using (StreamWriter file = new StreamWriter(SignPath))
                {
                    file.Write(jsonSign);
                }

                visitor.SignPath = SignPath;
                visitor.VisitDateTime = DateTime.Now;
                service.UpdateVisitorHistoryAgree(visitor);
                ViewBag.Process = "E";
                return View();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }

            return View();
        }

        /// <summary>
        /// Get: /Tablet/VisitorSmsAuth
        /// </summary>
        /// <returns>Tablet/VisitorSmsAuth</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorSmsAuth()
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];

                return View(visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }
            return View();
        }

        /// <summary>
        /// Post: /Tablet/VisitorSmsAuth
        /// </summary>
        /// <returns>Tablet/VisitorSmsAuth</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VisitorSmsAuth(FormCollection col)
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                bool error = false;

                //내방 정보 다 가지고 있으므로 해당 내방정보로 Database 세팅할것
                TabletService Service = new TabletService();

                //인증번호 비교.
                if(Parameter.TryGetValue("AuthNumber", out string AuthNumber) && !string.IsNullOrEmpty(AuthNumber))
                {
                    if(Parameter.TryGetValue("AuthSendNumber", out string AuthSendNumber) && AuthSendNumber == AuthNumber)
                    {

                    }
                    else
                    {
                        error = true;
                    }

                }


                if(error)
                {
                    return View(visitor);
                }

                if(Service.SetVisitorInfo(visitor))
                {
                    //Result 성공


                }
                else
                {
                    //Result 실패
                }

                return RedirectToAction("VisitorProcess");
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
                return RedirectToAction("intro");
            }
           
        }

        /// <summary>
        /// Sms Agent Ajax Call
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult SendSMS()
        {
            Dictionary<string, string> Parameter = RequestParameter.QuerystringToDictionary(this.Request);
            //SMS 인증 및 SMS 보내는 Service 만들어서 열기. Visitor의 번호를 받아서 세팅

            return Json(new { AuthNumber = "1111"  }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorProcess()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorComplete()
        {
            return View();
        }


        /// <summary>
        /// GET: /Tablet/VisitorInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorInfoENG()
        {
            try
            {
                //Submit 중 필수값 체크시 에러 나타날 경우 데이터를 보존하기 위한 TempData 처리.
                //TempData는 Url을 한번만 이동할 수있는 Stream과 같은 데이터 형태.
                ViewBag.Error = TempData["error"];
                VisitorInfo Visitor = (VisitorInfo)TempData["Visitor"];

                return View(Visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }
            return View();
        }


        /// <summary>
        /// POST: /Tablet/VisitorInfoProcess
        /// </summary>
        /// <param name="col">폼에서 Submit된 데이터</param>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VisitorInfoENG(FormCollection col)
        {
            try
            {
                TabletService service = new TabletService();
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                //필수값, 옵션값을 나누게끔 설계. 서버에서 필수값을 체크안하면 웹 브라우저에서 Js 변조해서 들어올 경우에 보안에 취약해진다.
                bool error = false;

                //필수값 체크
                if (!Parameter.TryGetValue("VisitorName", out string name) || string.IsNullOrEmpty(name))
                {
                    TempData["error"] = "Please enter your name";
                    error = true;
                }

                if (!Parameter.TryGetValue("VisitorMobile", out string mobile) || string.IsNullOrEmpty(mobile))
                {
                    TempData["error"] = "Please enter your CellPhone number";
                    error = true;
                }
                if (service.ChackMobileVaildation(mobile) == 1)
                {
                    TempData["error"] = "Your CellPhone number is not valid. 000-0000-0000.";
                    error = true;
                }

                //옵션값은 체크 없이 Parameter 전달.
                if (error)
                {
                    TempData["Visitor"] = new VisitorInfo()
                    {
                        VisitorName = Parameter["VisitorName"],
                        VisitorMobile = Parameter["VisitorMobile"],
                        VisitorCompany = Parameter["VisitorCompany"],
                        HostCompany = Parameter["HostCompany"],
                        HostName = Parameter["HostName"]
                    };
                    return RedirectToAction("VisitorInfoENG");

                }
                else
                {
                    VisitorInfo Visitor = new VisitorInfo()
                    {
                        VisitorName = Parameter["VisitorName"],
                        VisitorMobile = Parameter["VisitorMobile"],
                        VisitorCompany = Parameter["VisitorCompany"],
                        HostCompany = Parameter["HostCompany"],
                        HostName = Parameter["HostName"]
                    };

                    Session["visitor"] = Visitor;

                    return RedirectToAction("VisitorInfoAgreeENG");
                }
            }
            catch (Exception ex)
            {
                //핸들링 하지 않은 에러가 날 경우 Intro로 날려보냄.
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
                return RedirectToAction("intro");
            }
        }



        /// <summary>
        /// GET: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorInfoAgreeENG()
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];
                return View(visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }

            return View();
        }

        /// <summary>
        /// Post: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VisitorInfoAgreeENG(FormCollection col)
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                //bool error = false;

                visitor.AgreeFirst = Convert.ToBoolean(Parameter["AgreeFirst"]);
                visitor.AgreeFirstFlag = visitor.AgreeFirst == true ? 'Y' : 'N';

                visitor.AgreeSecond = Convert.ToBoolean(Parameter["AgreeSecond"]);
                visitor.AgreeSecondFlag = visitor.AgreeSecond == true ? 'Y' : 'N';

                //SignPath 저장하시길
                string jsonSign = Parameter["SignPad"];
                string SignPath = Server.MapPath("~/Sign/" + visitor.VisitorName + "_" + visitor.VisitorMobile + "_" + DateTime.Now.ToString("yyyyMMddhh") + ".json");

                visitor.SignPath = "C:\\";

                if (!Directory.Exists(Server.MapPath("~/Sign")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Sign"));
                }

                using (StreamWriter file = new StreamWriter(SignPath))
                {
                    file.Write(jsonSign);
                }

                visitor.SignPath = SignPath;
                Session["visitor"] = visitor;

                return RedirectToAction("VisitorSmsAuthENG");
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
                return RedirectToAction("intro");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorProcessENG()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorSmsAuthENG()
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];

                return View(visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }
            return View();
        }

        /// <summary>
        /// Post: /Tablet/VisitorSmsAuth
        /// </summary>
        /// <returns>Tablet/VisitorSmsAuthENG</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VisitorSmsAuthENG(FormCollection col)
        {
            try
            {
                VisitorInfo visitor = (VisitorInfo)Session["visitor"];
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                bool error = false;

                //내방 정보 다 가지고 있으므로 해당 내방정보로 Database 세팅할것
                TabletService Service = new TabletService();

                //인증번호 비교.
                if (Parameter.TryGetValue("AuthNumber", out string AuthNumber) && !string.IsNullOrEmpty(AuthNumber))
                {
                    if (Parameter.TryGetValue("AuthSendNumber", out string AuthSendNumber) && AuthSendNumber == AuthNumber)
                    {

                    }
                    else
                    {
                        error = true;
                    }

                }


                if (error)
                {
                    return View(visitor);
                }

                if (Service.SetVisitorInfo(visitor))
                {
                    //Result 성공


                }
                else
                {
                    //Result 실패
                }

                return RedirectToAction("VisitorProcessENG");
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());
                return RedirectToAction("intro");
            }

        }

        /// <summary>
        /// GET: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PopupVisitorInfoAgreeENG(int VisitorHistorySeq)
        {
            try
            {
                ViewBag.Process = "S";
                ViewBag.VisitorHistorySeq = VisitorHistorySeq;

                TabletService service = new TabletService();
                VisitorInfo visitor = service.GetVisitorHistoryInfo(VisitorHistorySeq);

                return View(visitor);
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }

            return View();
        }



        /// <summary>
        /// GET: /Tablet/VisitorInfoAgree
        /// </summary>
        /// <returns>Tablet/VisitorInfoAgree</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PopupVisitorInfoAgreeENG(FormCollection col)
        {
            try
            {
                TabletService service = new TabletService();

                VisitorInfo visitor = new VisitorInfo();
                Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);
                //bool error = false;

                visitor.AgreeFirst = Convert.ToBoolean(Parameter["AgreeFirst"]);
                visitor.AgreeFirstFlag = visitor.AgreeFirst == true ? 'Y' : 'N';

                visitor.AgreeSecond = Convert.ToBoolean(Parameter["AgreeSecond"]);
                visitor.AgreeSecondFlag = visitor.AgreeSecond == true ? 'Y' : 'N';

                visitor.VisitorHistorySeq = Convert.ToInt32(Parameter["VisitorHistorySeq"]);

                //SignPath 저장하시길
                string jsonSign = Parameter["SignPad"];
                string SignPath = Server.MapPath("~/Sign/" + visitor.VisitorName + "_" + visitor.VisitorMobile + "_" + DateTime.Now.ToString("yyyyMMddhh") + ".json");

                visitor.SignPath = "C:\\";

                if (!Directory.Exists(Server.MapPath("~/Sign")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Sign"));
                }

                using (StreamWriter file = new StreamWriter(SignPath))
                {
                    file.Write(jsonSign);
                }

                visitor.SignPath = SignPath;
                visitor.VisitDateTime = DateTime.Now;
                service.UpdateVisitorHistoryAgree(visitor);
                ViewBag.Process = "E";
                return View();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(" Position : " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Error : " + ex.ToString());

            }

            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult VisitorCompleteENG()
        {
            return View();
        }

    }
}
