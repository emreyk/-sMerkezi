using MYARCH.CORE;
using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IUserService
    {
        EUserDTO KullaniciKontrol(EUserDTO model);

        List<bagimsiz_bolum_kisiler> BagimsisKisiKontrol(int kisiId);
    }
}
