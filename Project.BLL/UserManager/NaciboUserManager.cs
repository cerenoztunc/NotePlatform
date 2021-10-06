using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Helpers;
using Project.ENTITIES.Messages;
using Project.ENTITIES.Models;
using Project.ENTITIES.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.UserManager
{
    public class NaciboUserManager
    {
        NaciboUserRep _nUserRep;
        public NaciboUserManager()
        {
            _nUserRep = new NaciboUserRep();
        }
        public BussinessLayerResult<NaciboUser> RegisterUser(RegisterVM data)
        {
            NaciboUser user = _nUserRep.Where(x => x.UserName == data.Username || x.Email == data.Email).FirstOrDefault();
            BussinessLayerResult<NaciboUser> layerResult = new BussinessLayerResult<NaciboUser>();
            if (user != null)
            {
                if (user.UserName == data.Username)
                {
                    layerResult.AddError(ErrorMessages.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessages.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                NaciboUser naciboUser = new NaciboUser()
                {
                    UserName = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsAdmin = false,
                    ModifiedUserName = "system"

                };

                _nUserRep.Add(naciboUser);

                if (naciboUser != null)
                {
                    layerResult.Result = _nUserRep.Where(x => x.Email == data.Email && x.UserName == data.Username).FirstOrDefault();
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");

                    string activateUri = $"{siteUri}/Home/UserActivate/{naciboUser.ActivateGuid}";
                    string body = $"Merhaba {naciboUser.UserName}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank' > tıklayınız.</a>";
                    MailHelper.SendMail(body, naciboUser.Email,"NaciboNotes Hesap Aktifleştirme");
                }

            }

            return layerResult;
        }

        public BussinessLayerResult<NaciboUser> LoginUser(LoginVM data)
        {

            BussinessLayerResult<NaciboUser> layerResult = new BussinessLayerResult<NaciboUser>();
            layerResult.Result = _nUserRep.Where(x => x.UserName == data.Username && x.Password == data.Password).FirstOrDefault();

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessages.UserIsNotActive, "Kullanıcı aktifleştirilmemiş.");
                    layerResult.AddError(ErrorMessages.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }

            }
            else
            {
                layerResult.AddError(ErrorMessages.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
            }
            return layerResult;
        }
        public BussinessLayerResult<NaciboUser> ActivateUser(Guid activateID)
        {
            BussinessLayerResult<NaciboUser> layerResult = new BussinessLayerResult<NaciboUser>();
            layerResult.Result = _nUserRep.Where(x => x.ActivateGuid == activateID).FirstOrDefault();

            if(layerResult.Result != null)
            {
                if (layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessages.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return layerResult;
                }

                layerResult.Result.IsActive = true;
                _nUserRep.Update(layerResult.Result);
            }
            else
            {
                layerResult.AddError(ErrorMessages.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı");
            }

            return layerResult;
        }
    }
}
