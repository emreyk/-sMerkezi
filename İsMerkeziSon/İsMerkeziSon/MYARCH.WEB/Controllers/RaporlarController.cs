using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class RaporlarController : AdminController
    {

        private readonly IRaporlarService _raporService;
        private readonly IMuhtelifIslemlerService _muhtelifServis;
        private readonly IUnitofWork _uow;

        public RaporlarController(IUnitofWork uow, IRaporlarService raporService,IMuhtelifIslemlerService muhtelifServis)
  : base(uow)
        {
            _uow = uow;
            _raporService = raporService;
            _muhtelifServis = muhtelifServis;

        }
     
        public ActionResult VadesiGecenTumu()
        {
            var borcListesi = _raporService.VadesiGecenBorcluListesi();
            return View(borcListesi);
        }

        public ActionResult VadesiGecenAidat()
        {
            var borcListesi = _raporService.AidatVadesiGecenBorcluListesi();
            return View(borcListesi);
        }

        public ActionResult VadesiGecenDemirbas()
        {
            var borcListesi = _raporService.DemirbasVadesiGecenBorcluListesi();
            return View(borcListesi);
        }

        public ActionResult VadesiGecenYakit()
        {
            var borcListesi = _raporService.YakitVadesiGecenBorcluListesi();
            return View(borcListesi);
        }


        public ActionResult VadesiBekleyenTumu()
        {
            var borcListesi = _raporService.VadesiBekleyenBorcluListesi();
            return View(borcListesi);
        }

        public ActionResult VadesiBekleyenAidat()
        {
            var borcListesi = _raporService.AidatVadesiBekleyenBorcluListesi();
            return View(borcListesi);
        }

        public ActionResult VadesiBekleyenDemirbas()
        {
            var borcListesi = _raporService.DemirbasVadesiBekleyenBorcluListesi();
            return View(borcListesi);
        }
        public ActionResult VadesiBekleyenYakit()
        {
            var borcListesi = _raporService.YakitVadesiBekleyenBorcluListesi();
            return View(borcListesi);
        }

        public ActionResult PesinTahsilat()
        {
            var borcListesi = _raporService.AidatVadesiBekleyenBorcluListesi();
            return View(borcListesi);
        }


        public ActionResult BagimsizAlanHesabi()
        {
            return View();
        }

        public ActionResult BagimsizAlanHesabiKaydet(string paraBirimi,string basTarih,string bitTarih)
        {
            double bagimsizAlanTutari = _muhtelifServis.BagimsizAlanHesabı(paraBirimi,basTarih,bitTarih);

            if (bagimsizAlanTutari != 0)
            {
                return Json(bagimsizAlanTutari, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
           
        }

    }
}