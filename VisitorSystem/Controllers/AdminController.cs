using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VisitorSystem.Models;
using VisitorSystem.Service;
using VisitorSystem.Util;
using OfficeOpenXml;

namespace VisitorSystem.Controllers
{
    /// <summary>
    /// 김남훈 최초생성
    /// 관리자 웹 Controller
    /// 메뉴별로 Controller를 나눠야 할거같다.
    /// 많아질 경우 컨트롤러 분리시킬것.
    /// </summary>
    public class AdminController : BaseController
    {

        #region Authentication
        /// <summary>
        ///  GET : /Admin/Login
        /// Login View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            //Home/Index 안거치고 들어올 경우 때문에 Session 세팅.
            HomeService service = new HomeService();
            Location location = service.GetLocationAction(Session["ipAddress"].ToString());
            Session["LocationID"] = location.LocationID;
            Session["LocationName"] = location.LocationName;

            return View();
        }

        /// <summary>
        ///  POST : /Admin/Login
        /// Login 프로세스
        /// </summary>
        /// <param name="col">Submit 창</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection col)
        {
            //파라미터 변경 FormColltion -> Dictionary
            Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);

            //모델 유효성 검사. (Jquery.Validate에서 넘어오는데 받는 모델을 변경해야 될듯.
            if (ModelState.IsValid)
            {
                Parameter.TryGetValue("AdminID", out string AdminID);
                Parameter.TryGetValue("AdminPW", out string AdminPW);

                AdminUser adminUser = new AdminUser()
                {
                    AdminID = AdminID,
                    //단방향 해시 함수 SHA256 적용
                    AdminPW = Cipher.EncryptSHA256(AdminPW)
                };

                AdminService service = new AdminService();
                int result = service.Login(adminUser, Session["LocationID"].ToString());
                Session["UserID"] = adminUser.AdminID;

                //성공
                if (result == 2)
                {
                    FormsAuthentication.SetAuthCookie(adminUser.AdminID, false);
                    return RedirectToAction("Main");

                }
                //암호 바꾼지 1개월 지나서 재 변경 요구
                else if (result == 1)
                {
                    
                    return RedirectToAction("ChangePassword");

                }
                else
                {
                    ModelState.AddModelError("AdminPW", "아이디가 없거나 암호가 다릅니다.");
                    return View();

                }
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        ///  GET : /Admin/ChangePassword
        ///  암호 정책 강제 변경 View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            ViewBag.UserID = Session["UserID"];
            return View();
        }

        /// <summary>
        ///  POST : /Admin/ChangePassword
        /// 암호변경 강제 변경 프로세스
        /// </summary>
        /// <param name="col">Submit 데이터</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(FormCollection col)
        {
            
            Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);

            //지금은 들어온 값만 비교하지만 기존 PW 비교 로직이 있어야 하고 
            //PW 정책도 짜야됨. 6자 특수문자 영어 (잊어버릴 가능성도 있음)
            Parameter.TryGetValue("AdminNewPW", out string AdminNewPW);
            Parameter.TryGetValue("AdminNewRePW", out string AdminNewRePW);

            if (AdminNewPW != AdminNewRePW)
            {
                ModelState.AddModelError("AdminNewRePW", "암호 재확인과 다릅니다.");
                ViewBag.Error = "암호 재확인과 다릅니다.";
            }
            //모델 유효성 검증
            if (ModelState.IsValid)
            {
                AdminService service = new AdminService();

                //암호 알고리즘 확인 service
                int Result = service.ChackPasswordVaildation(Session["UserID"].ToString(), AdminNewRePW);

                if (Result == 1)
                {
                    ModelState.AddModelError("AdminNewPW", "암호 최소 최대 길이가 충족하지 않는 패스워드 입니다. 최소 10 ~ 최대 12.");
                    ViewBag.Error = "암호 최소 최대 길이가 충족하지 않는 패스워드 입니다. 최소 10 ~ 최대 12.";
                }
                else if(Result == 2)
                {
                    ModelState.AddModelError("AdminNewPW", "영문, 특수문자, 숫자의 2종류 이상의 조합이여야 합니다.");
                    ViewBag.Error = "영문, 특수문자, 숫자의 2종류 이상의 조합이여야 합니다.";
                }
                else if(Result == 3)
                {
                    ModelState.AddModelError("AdminNewPW", "숫자, 문자의 3자 이상이 동일 합니다.");
                    ViewBag.Error = "숫자, 문자의 3자 이상이 동일 합니다.";
                }
                else if (Result == 4)
                {
                    ModelState.AddModelError("AdminNewPW", "숫자 문자가 3자이상 연속되어 있는 패턴입니다.");
                    ViewBag.Error = "숫자 문자가 3자이상 연속되어 있는 패턴입니다.";
                }
                else if(Result == 5)
                {
                    ModelState.AddModelError("AdminNewPW", "ID와 같은 문자는 암호에 사용하실수 없습니다.");
                    ViewBag.Error = "ID와 같은 문자는 암호에 사용하실수 없습니다.";
                }

                if(!ModelState.IsValid)
                {
                    ViewBag.UserID = Session["UserID"];
                    return View();
                }

                AdminUser adminUser = new AdminUser()
                {
                    AdminID = Session["UserID"].ToString(),
                    AdminPW = Cipher.EncryptSHA256(AdminNewPW)
                };

                if(service.SetAdminPassword(adminUser, Session["LocationID"].ToString()))
                {
                    FormsAuthentication.SetAuthCookie(Session["UserID"].ToString(), false);
                    //변경 후 Main 리턴
                    return RedirectToAction("Main");
                }
            }

            ViewBag.UserID = Session["UserID"];
            return View();
        }

        /// <summary>
        /// GET : Admin/LogOut View
        /// 로그아웃 VIew
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult LogOut()
        {
            //인증 쿠키 삭제
            FormsAuthentication.SignOut();

            //로그아웃 AdminLogging
            AdminService service = new AdminService();
            string UserID = Session["UserID"].ToString();
            string LocationID = Session["LocationID"].ToString();

            service.SetAdminLog(UserID, LocationID, "관리자 로그아웃");

            return RedirectToAction("login");
        }

        #endregion

        #region Main
        /// <summary>
        /// GET : /Admin/Main View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Main(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            

            AdminService service = new AdminService();
            List<VisitorInfo> visitors = service.GetTodayVisitorInfo();
            ViewBag.Hosts = service.GetHostRanked();

            //partial view 던져줄 인자값.
            ViewBag.UserID = Session["UserID"];
            ViewBag.LocationName = Session["LocationName"];

            //검색 영역
            if(!string.IsNullOrEmpty(SearchString))
            {
                switch(SearchColumn)
                {
                    case "VisitorName" :
                        visitors = visitors.Where(m => m.VisitorName.Contains(SearchString)).ToList();
                        break;
                    case "HostName":
                        visitors = visitors.Where(m => m.HostName.Contains(SearchString)).ToList();
                        break;
                    case "Mobile":
                        visitors = visitors.Where(m => m.VisitorMobile.Contains(SearchString)).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = visitors.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;
            

            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            visitors = visitors.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝


            return View(visitors);
        }
        #endregion

        #region Visitor
        /// <summary>
        /// GET Admin/VisitorList
        /// 내방객 리스트 조회 화면
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult VisitorList(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            AdminService service = new AdminService();
            List<Visitor> visitors = service.GetVisitorList();


            //검색 영역
            if (!string.IsNullOrEmpty(SearchString))
            {
                switch (SearchColumn)
                {
                    case "VisitorName":
                        visitors = visitors.Where(m => m.VisitorName.Contains(SearchString)).ToList();
                        break;
                    case "Company":
                        visitors = visitors.Where(m => m.Company.Contains(SearchString)).ToList();
                        break;
                    case "Mobile":
                        visitors = visitors.Where(m => m.Mobile.Contains(SearchString)).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = visitors.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;


            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            visitors = visitors.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝

            return View(visitors);
        }

        [HttpGet]
        [Authorize]
        public ActionResult VisitorDetail(int VisitorID)
        {
            AdminService service = new AdminService();
            List<Visitor> visitors = service.GetVisitorList();

            Visitor visitor = visitors.Where(m => m.VisitorID == VisitorID).FirstOrDefault();
            return View(visitor);
        }


        [HttpPost]
        [Authorize]
        public ActionResult VisitorDetail(Visitor Visitor)
        {
            AdminService service = new AdminService();
            if (Visitor.VisitorID > 0)
            {
                service.SetVisitor(Visitor);
            }
            else
            {
                service.AddVisitor(Visitor);
            }

            return RedirectToAction("VisitorList");
        }

        [HttpGet]
        [Authorize]
        public ActionResult VisitorDelete(int VisitorID)
        {
            AdminService service = new AdminService();
            service.DeleteVisitor(VisitorID);

            return RedirectToAction("VisitorList");
        }
        #endregion

        #region VisitorHistory
        [HttpGet]
        [Authorize]
        public ActionResult VisitorHistoryAdd()
        {
            //개인정보 동의없는 동의자 넣기 페이지

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult VisitorHistoryAdd(FormCollection col)
        {
            //개인정보 동의없는 동의자 넣기 페이지 입력 작업


            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult VisitorHistoryList(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            AdminService service = new AdminService();
            List<VisitorInfo> visitors = service.GetVisitorInfoList();

            //검색 영역
            if (!string.IsNullOrEmpty(SearchString))
            {
                switch (SearchColumn)
                {
                    case "VisitorName":
                        visitors = visitors.Where(m => m.VisitorName.Contains(SearchString)).ToList();
                        break;
                    case "Company":
                        visitors = visitors.Where(m => m.VisitorCompany.Contains(SearchString)).ToList();
                        break;
                    case "Mobile":
                        visitors = visitors.Where(m => m.VisitorMobile.Contains(SearchString)).ToList();
                        break;
                    case "Host":
                        visitors = visitors.Where(m => !string.IsNullOrEmpty(m.HostName) && m.HostName.Contains(SearchString)).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = visitors.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;


            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            visitors = visitors.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝


            return View(visitors);
        }
        [HttpGet]
        [Authorize]
        public ActionResult VisitorHistoryDetail(int VisitorHistorySeq)
        {
            AdminService service = new AdminService();
            List<VisitorInfo> visitors = service.GetVisitorInfoList();

            VisitorInfo visitor = visitors.Where(m => m.VisitorHistorySeq == VisitorHistorySeq).FirstOrDefault();
            return View(visitor);
        }

        [HttpPost]
        [Authorize]
        public ActionResult VisitorHistoryDetail(VisitorInfo visitorInfo)
        {
            AdminService service = new AdminService();

            return RedirectToAction("VisitorHistoryList");
        }
#endregion

        #region Host

        [HttpGet]
        [Authorize]
        public ActionResult HostList(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            AdminService service = new AdminService();
            List<Host> hosts = service.GetHostList();

            //검색 영역
            if (!string.IsNullOrEmpty(SearchString))
            {
                switch (SearchColumn)
                {
                    case "HostName":
                        hosts = hosts.Where(m => m.HostName.Contains(SearchString)).ToList();
                        break;
                    case "Company":
                        hosts = hosts.Where(m => m.Company.Contains(SearchString)).ToList();
                        break;
                    case "Mobile":
                        hosts = hosts.Where(m => m.Mobile.Contains(SearchString)).ToList();
                        break;
                    case "Tel":
                        hosts = hosts.Where(m => m.Tel.Contains(SearchString)).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = hosts.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;


            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            hosts = hosts.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝

            return View(hosts);
        }

        [HttpGet]
        [Authorize]
        public ActionResult HostDetail(int HostID)
        {
            AdminService service = new AdminService();
            List<Host> hosts = service.GetHostList();

            Host host = hosts.Where(m => m.HostID == HostID).FirstOrDefault();
            return View(host);
        }


        [HttpPost]
        [Authorize]
        public ActionResult HostDetail(Host Host)
        {
            AdminService service = new AdminService();
            if (Host.HostID > 0)
            {
                service.SetHost(Host);
            }
            else
            {
                service.AddHost(Host);
            }

            return Redirect("HostList");
        }

        [HttpGet]
        [Authorize]
        public ActionResult HostDelete(int HostID)
        {
            AdminService service = new AdminService();
            service.DeleteHost(HostID);

            return Redirect("HostList");
        }

        #endregion

        #region Card
        [HttpGet]
        [Authorize]
        public ActionResult CardList(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            AdminService service = new AdminService();
            List<Card> cards = service.GetCardList();

            //검색 영역
            if (!string.IsNullOrEmpty(SearchString))
            {
                switch (SearchColumn)
                {
                    case "CardID":
                        cards = cards.Where(m => m.CardID.Contains(SearchString)).ToList();
                        break;
                    case "CardFlag":
                        cards = cards.Where(m => m.CardFlag == SearchString[0]).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = cards.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;


            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            cards = cards.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝

            return View(cards);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CardDetail(string CardID)
        {
            AdminService service = new AdminService();
            List<Card> cards = service.GetCardList();

            Card card = cards.Where(m => m.CardID == CardID).FirstOrDefault();
            ViewBag.Error = "";
            return View(card);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CardDetail(Card Card)
        {
            AdminService service = new AdminService();
            List<Card> cards = service.GetCardList();

            if(service.ChackLengthVaildation(Card.CardNo.ToString(), 13, "fix") == 1)
            {
                ViewBag.Error = "카드번호가 13자리가 아닙니다.";
                return View(Card);
            }

            Card card = cards.Where(m => m.CardID == Card.CardID).FirstOrDefault();

            if (card != null)
            {
                service.SetCard(Card);
            }
            else
            {
                service.AddCard(Card);
            }

            return Redirect("CardList");
        }

        [HttpGet]
        [Authorize]
        public ActionResult CardDelete(string CardID)
        {
            AdminService service = new AdminService();
            service.DeleteCard(CardID);

            return Redirect("CardList");
        }
        #endregion

        #region VisitorBlackList
        [HttpGet]
        [Authorize]
        public ActionResult VisitorBlackList(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            AdminService service = new AdminService();
            List<VisitorBlackList> visitorBlackLists =  service.GetVisitorBlackList();


            //검색 영역
            if (!string.IsNullOrEmpty(SearchString))
            {
                switch (SearchColumn)
                {
                    case "VisitorName":
                        visitorBlackLists = visitorBlackLists.Where(m => m.VisitorName.Contains(SearchString)).ToList();
                        break;
                    case "Company":
                        visitorBlackLists = visitorBlackLists.Where(m => m.Company.Contains(SearchString)).ToList();
                        break;
                    case "Mobile":
                        visitorBlackLists = visitorBlackLists.Where(m => m.Mobile.Contains(SearchString)).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = visitorBlackLists.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;


            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            visitorBlackLists = visitorBlackLists.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝

            return View(visitorBlackLists);
        }

        [HttpGet]
        [Authorize]
        public ActionResult VisitorBlackListDetail(int VisitorID)
        {
            AdminService service = new AdminService();
            List<VisitorBlackList> visitorBlackLists = service.GetVisitorBlackList();

            VisitorBlackList visitorBlackList = visitorBlackLists.Where(m => m.VisitorID == VisitorID).FirstOrDefault();
            return View(visitorBlackList);
        }

        [HttpPost]
        [Authorize]
        public ActionResult VisitorBlackListDetail(FormCollection col)
        {
            Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);

            Parameter.TryGetValue("VisitorID", out string visitorID);
            Parameter.TryGetValue("desc", out string Desc);

            VisitorBlackList visitorBlackList = new VisitorBlackList()
            {
                VisitorID = Convert.ToInt32(visitorID),
                Desc = Desc
            };


            AdminService service = new AdminService();
            service.SetVisitorBlackList(visitorBlackList);

            return RedirectToAction("VisitorBlackList");
        }

        [HttpGet]
        [Authorize]
        public ActionResult VisitorBlackListDelete(int visitorID)
        {
            AdminService service = new AdminService();
            service.DeleteVisitorBlackList(visitorID);

            return RedirectToAction("VisitorBlackList");
        }

        [HttpGet]
        [Authorize]
        public ActionResult VisitorBlackListAdd(int visitorID)
        {
            AdminService service = new AdminService();
            service.InsertVisitorBlackList(visitorID,Session["UserID"].ToString());

            return RedirectToAction("VisitorBlackList");
        }

        #endregion

        #region Location
        [HttpGet]
        [Authorize]
        public ActionResult LocationList(string SearchColumn, string SearchString, int CurrentPage = 1)
        {
            AdminService service = new AdminService();
            List<Location> locations = service.GetLocationList();


            //검색 영역
            if (!string.IsNullOrEmpty(SearchString))
            {
                switch (SearchColumn)
                {
                    case "LocationName":
                        locations = locations.Where(m => m.LocationName.Contains(SearchString)).ToList();
                        break;
                    case "LocationIP":
                        locations = locations.Where(m => m.LocationIP.Contains(SearchString)).ToList();
                        break;
                    default:
                        break;
                }

            }

            //검색조건 유지를 위한 ViewBag
            ViewBag.SearchColumn = SearchColumn;
            ViewBag.SearchString = SearchString;
            //검색 영역

            //페이징 영역
            int pagePerData = 10;
            //Data 총 갯수
            ViewBag.TotalCount = locations.Count();
            //Page당 데이터 수 변경 원하면 해당 ViewBag 수정.
            ViewBag.PagePerData = pagePerData;
            //전달 받은 Page 값.
            ViewBag.CurrentPage = CurrentPage;


            //균일하게 뽑히게 Ordering 되어 있어야 함. PK 잡히므로 균일
            //계산식 : 페이지 * 페이지당 데이터수 Skip After Take 페이지당 데이터 수
            locations = locations.Skip((CurrentPage - 1) * pagePerData).Take(pagePerData).ToList();
            //페이징 영역끝

            return View(locations);
        }


        [HttpGet]
        [Authorize]
        public ActionResult LocationDetail(int LocationID)
        {
            AdminService service = new AdminService();
            List<Location> locations = service.GetLocationList();

            Location location = locations.Where(m => m.LocationID == LocationID).FirstOrDefault();
            ViewBag.Error = "";
            return View(location);
        }


        [HttpPost]
        [Authorize]
        public ActionResult LocationDetail(Location location)
        {
            AdminService service = new AdminService();

            if(service.ChackIPVaildation(location.LocationIP) == 1)
            {
                ViewBag.Error = "아이피 형식이 맞지 않습니다.";
                return View(location);
            }

            if (location.LocationID > 0)
                service.SetLocation(location);
            else
                service.AddLocation(location);

            return RedirectToAction("Locationlist");
        }

        [HttpGet]
        [Authorize]
        public ActionResult LocationDelete(string LocationID)
        {
            AdminService service = new AdminService();
            service.DeleteLocation(LocationID);

            return RedirectToAction("Locationlist");
        }
        #endregion

        #region Popup
        [HttpGet]
        [Authorize]
        public ActionResult PopupSignature(int VisitorHistorySeq)
        {
            //개인정보 동의없는 동의자 넣기 페이지 입력 작업
            AdminService service = new AdminService();
            VisitorInfo visitor = service.GetVisitorInfo(VisitorHistorySeq);

            if(System.IO.File.Exists(visitor.SignPath))
            {
                using (StreamReader sr = new StreamReader(visitor.SignPath))
                {
                    ViewBag.Sig = sr.ReadToEnd();
                }
            }

            return View(visitor);
        }

        [HttpGet]
        [Authorize]
        public ActionResult PopupExcelImport()
        {
            //개인정보 동의없는 동의자 넣기 페이지 입력 작업
            return View();
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult PopupExcelImport(HttpPostedFileBase file)
        {
            //개인정보 동의없는 동의자 넣기 페이지 입력 작업
            AdminService service = new AdminService();

            if (file != null && file.ContentLength > 0)
                try
                {
                    List<VisitorInfo> visitorInfos = new List<VisitorInfo>();

                    string path = Path.Combine(Server.MapPath("~/Files"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);

                    FileInfo excelFile = new FileInfo(path);
                    //Excel로 데이터 밀어넣는 VisitorInfoList 생성 구문
                    using (ExcelPackage ex = new ExcelPackage(excelFile))
                    {
                        

                        ExcelWorkbook wb = ex.Workbook;
                        ExcelWorksheet ws = ex.Workbook.Worksheets[1];
                        int passCount = 0;


                        for (int i = ws.Dimension.Start.Row + 2;i <= ws.Dimension.End.Row;i++)
                        {
                            VisitorInfo visitorInfo = new VisitorInfo();
                            for (int j = ws.Dimension.Start.Column +1 ;j <= ws.Dimension.End.Column; j++)
                            {
                                if (ws.Cells[i, j].Value == null)
                                    continue;

                                switch (j)
                                {
                                    //방문일자
                                    case 2:
                                        DateTime dt = Convert.ToDateTime(ws.Cells[i, j].Value);

                                        if((dt.Date - DateTime.Now.Date).TotalSeconds < 0)
                                        {
                                            passCount++;
                                            goto Pass;

                                        }

                                        visitorInfo.VisitDateTime = dt;
                                        break;
                                    //성명
                                    case 3:
                                        visitorInfo.VisitorName = ws.Cells[i, j].Value.ToString();
                                        break;
                                    //연락처
                                    case 4:
                                        visitorInfo.VisitorMobile = ws.Cells[i, j].Value.ToString();
                                        break;
                                    //회사명
                                    case 5:
                                        visitorInfo.VisitorCompany = ws.Cells[i, j].Value.ToString();
                                        break;
                                    //접견자
                                    case 6:
                                        visitorInfo.HostName = ws.Cells[i, j].Value.ToString();
                                        break;
                                    //접견회사
                                    case 7:
                                        visitorInfo.HostCompany = ws.Cells[i, j].Value.ToString();
                                        visitorInfos.Add(visitorInfo);
                                        break;
                                    default:
                                        continue;
                                    
                                }
                                
                            }
                            Pass:;
                        }

                        ViewBag.Message = string.Format("파일 업로드 성공하였습니다. 등록 {0} 명, 등록안됨 {1} 명",visitorInfos.Count, passCount);
                        ViewBag.Process = "E";
                    }

                    excelFile.Delete();

                    if(visitorInfos.Count > 0)
                        service.SetVisitorHistoryList(visitorInfos);

                }
                catch (Exception)
                {
                    ViewBag.Message = "양식에 맞지 않는 데이터가 있습니다.";
                }
                finally
                {
                    //양식에 맞지 않을 경우 File 지워내기
                    string path = Path.Combine(Server.MapPath("~/Files"),
                           Path.GetFileName(file.FileName));

                    if(System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                }
            else
            {
                ViewBag.Message = "파일 등록을 먼저 해주세요";
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult PopupCardAdd(int VisitorHistorySeq)
        {
            //개인정보 동의없는 동의자 넣기 페이지 입력 작업
            AdminService service = new AdminService();

            //카드 정보 받아서 Post 처리
            ViewBag.VisitorHistorySeq = VisitorHistorySeq;
            ViewBag.Process = "S";
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult PopupCardAdd(FormCollection col)
        {
            Dictionary<string, string> Parameter = RequestParameter.FormCollectionToDictionary(col);

            //카드 발급 절차. Database에 Location 데이터까지 update해야 프린트 내용에서 프린트 작업 할꺼임.
            AdminService service = new AdminService();
            Parameter.TryGetValue("VisitorID", out string visitorID);
            Parameter.TryGetValue("CardID", out string cardID);

            if(string.IsNullOrEmpty(cardID))
            {
                return Content("<script language='javascript' type='text/javascript'>alert('카드번호는 없을 수 없습니다.');</script>");
            }

            int locationID = Convert.ToInt32(Session["LocationID"]);
            VisitorInfo visitorInfo = new VisitorInfo()
            {
                VisitorID = Convert.ToInt32(visitorID),
                LocationID = locationID,
                CardID = cardID,
                CardFlag = "I"

            };

            service.SetCardData(visitorInfo);
            ViewBag.Process = "E";
            return View();
        }
        #endregion 
    }
}
