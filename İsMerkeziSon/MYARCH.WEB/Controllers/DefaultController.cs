using MYARCH.DATA.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class DefaultController : AdminController
    {

        private readonly IUnitofWork _uow;

        public DefaultController(IUnitofWork uow)
     : base(uow)
        {
            _uow = uow;
         
        }



        public ActionResult Index()
        {
            return View();
        }
    }
}