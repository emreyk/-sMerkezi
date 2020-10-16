using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using MYARCH.CORE;
using MYARCH.DATA.GenericRepository;
using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using MYARCH.SERVICES.Services;

namespace MYARCH.IoC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.BindInRequestScope<IUnitofWork, UnitofWork>();
            container.BindInRequestScope<IUserService, UserService>();
            container.BindInRequestScope<IBlokService, BlokService>();
            container.BindInRequestScope<ITipService, TipService>();
            container.BindInRequestScope<IPersonelService, PersonelService>();
            container.BindInRequestScope<IAnasayacService, AnasayacService>();
            container.BindInRequestScope<ISayacTipleriService, SayacTipleriService>();
            container.BindInRequestScope<IAnaSayacOrtakDagitimService, AnaSayacOrtakDagitimService>();
            container.BindInRequestScope<IBagimsizBolumlerService, BagimsizBolumlerService>();
            container.BindInRequestScope<IBagimsizSayacService, BagimsizSayacService>();
            container.BindInRequestScope<IKisilerService, KisilerService>();
            container.BindInRequestScope<IBorclandirmaService, BorclandirmaService>();
            container.BindInRequestScope<IKasaService, KasaService>();
            container.BindInRequestScope<IBankaService, BankaService>();
            container.BindInRequestScope<IFirmaService, FirmaService>();
            container.BindInRequestScope<ITopluBorclandirService, TopluBorclandirService>();
            container.BindInRequestScope<IAidatGunService, AidatGunService>();
            container.BindInRequestScope<IVadeGunSayisiService, VadeGunSayisiService>();
            container.BindInRequestScope<IRaporlarService, RaporlarService>();
            container.BindInRequestScope<IBorcTipleri, BorcTipleriService>();
            container.BindInRequestScope<ISmsService, SmsService>();
            container.BindInRequestScope<IMuhtelifBasliklarService, MuhtelifBasliklarService>();
            container.BindInRequestScope<IMuhtelifIslemlerService, MuhtelifIslemlerService>();
            container.BindInRequestScope<IDosyaService, DosyaService>();
         
        }


        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

    }
}