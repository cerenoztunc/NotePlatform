using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
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
                if(user.UserName == data.Username)
                {
                    layerResult.AddError(ErrorMessages.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if(user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessages.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                NaciboUser naciboUser = new NaciboUser();
                naciboUser.UserName = data.Username;
                naciboUser.Email = data.Email;
                naciboUser.Password = data.Password;
                naciboUser.ActivateGuid = Guid.NewGuid();
                naciboUser.IsAdmin = false;
                naciboUser.ModifiedUserName = "system";

                _nUserRep.Add(naciboUser);

                if(naciboUser != null)
                {
                    layerResult.Result = _nUserRep.Where(x => x.Email == data.Email && x.UserName == data.Username).FirstOrDefault();
                    //layerResult.Result.ActivateGuid
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
    }
}
