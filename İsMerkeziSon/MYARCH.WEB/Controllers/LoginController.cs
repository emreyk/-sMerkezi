using MYARCH.CORE.Entities;
using MYARCH.DATA.UnitofWork;
using MYARCH.DTO.EEntity;
using MYARCH.SERVICES.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class LoginController : AdminController
    {
     
        private readonly IUserService _userService;
        private readonly IUnitofWork _uow;
        private SessionContext _sessionContext;
        public LoginController(IUnitofWork uow,
            IUserService userService)
            : base(uow)
        {
            _uow = uow;
            _userService = userService;
            _sessionContext = new SessionContext();
        }


        public ActionResult Index() {

            return View();

        }

        [HttpPost]
        public ActionResult LoginControl(EUserDTO login)
        {
            List<bagimsiz_bolum_kisiler> bagimsizKisiKontrol=new List<bagimsiz_bolum_kisiler>();

            var result = _userService.KullaniciKontrol(login);
            if (result!= null)
            {
                bagimsizKisiKontrol = _userService.BagimsisKisiKontrol(result.kisi_id);
                if (result != null && bagimsizKisiKontrol.Count > 0 && result.rutbe == "user")
                {
                    AutoMapper.Mapper.DynamicMap(result, _sessionContext);
                    Session["SessionContext"] = _sessionContext;

                    return Json("/kisiler/kisi/" + result.kisi_id, JsonRequestBehavior.AllowGet);
                }
                else if (result.rutbe == "admin" && result != null)
                {
                    AutoMapper.Mapper.DynamicMap(result, _sessionContext);
                    Session["SessionContext"] = _sessionContext;
                    return Json("/default", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
         
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult SessionKontrolEt()
        {
            if (Session["SessionContext"]!= null)
            {
                Session["SessionContext"] = _sessionContext;
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            Response.Redirect("/login");
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}