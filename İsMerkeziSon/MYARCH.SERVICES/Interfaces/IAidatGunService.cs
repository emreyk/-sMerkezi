using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IAidatGunService
    {
        aidat_gun AidatGunGetir();

        bool AidatGunuGuncelle(string aidatGunu);


    }
}
