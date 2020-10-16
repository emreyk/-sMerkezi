using MYARCH.CORE.Entities;
using MYARCH.DATA.UnitofWork;
using MYARCH.DTO.EEntity;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WIA;

namespace MYARCH.WEB.Controllers
{
    public class DosyaController : AdminController
    {

        private readonly IUnitofWork _uow;
        private readonly IKisilerService _kisilerServis;
        private readonly IDosyaService _dosyaServis;
        public DosyaController(IUnitofWork uow, IKisilerService kisilerServis, IDosyaService dosyaServis)
   : base(uow)
        {
            _uow = uow;
            _kisilerServis = kisilerServis;
            _dosyaServis = dosyaServis;

        }

        // GET: Dosya
        public ActionResult Index()
        {
            var kisiler = _kisilerServis.KisiListesiModal();
            return View(kisiler);
        }

        bool durum;
        [HttpPost]
        public ActionResult DosyaKaydet(dosyalar model)
        {
           
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase httpPostedFile = Request.Files[file];
                string dosyaYolu = "/Uploads/" + httpPostedFile.FileName;

                string contentTipi = httpPostedFile.ContentType;
                string tip = contentTipi.Split('/')[1];
                model.dosya_tipi = tip;
                model.dosya_adi = httpPostedFile.FileName;
                try
                {
                    Request.Files[file].SaveAs(Server.MapPath(dosyaYolu));
                    model.dosya_yolu = dosyaYolu;
                    durum=  _dosyaServis.DosyaKaydet(model);



                }
                catch (Exception)
                {

                    throw;

                }
            }

            if (durum ==true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult TarayiciIslemleri()
        {
            var deviceManager = new DeviceManager();
            object test;
            // Loop through the list of devices and add the name to the listbox
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                // Add the device only if it's a scanner
                if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                {
                    continue;
                }

                // Add the Scanner device to the listbox (the entire DeviceInfos object)
                // Important: we store an object of type scanner (which ToString method returns the name of the scanner)

                List<string> tara = new List<string>();

                //tara.Add();
                tara.Add(deviceManager.DeviceInfos[i].Properties["Name"].get_Value().ToString());
                ViewBag.YaziAdlari = tara;
            }
            return View();

        }

        public ActionResult Yazdir(dosyalar model)
        {
            var deviceManager = new DeviceManager();

            DeviceInfo AvailableScanner = null;

            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++) // tarayıcı listesi
            {
                if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType) // tarayacı yoksa atla
                {
                    continue;
                }

                AvailableScanner = deviceManager.DeviceInfos[i];

                break;
            }

            var device = AvailableScanner.Connect(); //tarayıcıya bağlan

            var ScanerItem = device.Items[1];

            var imgFile = (ImageFile)ScanerItem.Transfer(FormatID.wiaFormatJPEG);


            //var Path = @"C:\Users\HP\Desktop\ScannerTest\ScanImg.png"; 
            Random rnd = new Random();
            int sayi = rnd.Next(100, 999);

            string dosyaAdi = "TY" + DateTime.Now.ToString("yyyy-MM-dd-HH-'" + sayi + "'") + ".png";
            string path = ControllerContext.HttpContext.Server.MapPath("~/Uploads/" + dosyaAdi);

            model.dosya_yolu = "/Uploads/" + dosyaAdi;

            model.dosya_tipi = "png";
            model.dosya_adi = dosyaAdi;

            imgFile.SaveFile(path);

            if (_dosyaServis.DosyaKaydet(model) == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }
}