using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.Results;
using Project.COMMON.Helpers;
using Project.ENTITIES.Messages;
using Project.ENTITIES.Models;
using Project.ENTITIES.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers
{
    public class NaciboUserManager : BaseManager<NaciboUser>
    {
       
        public BussinessLayerResult<NaciboUser> RegisterUser(RegisterVM data)
        {
            NaciboUser user = FirstOrDefault(x => x.UserName == data.Username || x.Email == data.Email);
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
                    ProfileImageFileName = "profile.jpg",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsAdmin = false,
                    ModifiedUserName = "system"

                };

                Add(naciboUser);

                if (naciboUser != null)
                {
                    layerResult.Result = FirstOrDefault(x => x.Email == data.Email && x.UserName == data.Username);
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");

                    string activateUri = $"{siteUri}/Home/UserActivate/{naciboUser.ActivateGuid}";
                    string body = $"Merhaba {naciboUser.UserName}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank' > tıklayınız.</a>";
                    MailHelper.SendMail(body, naciboUser.Email,"NaciboNotes Hesap Aktifleştirme");
                }

            }

            return layerResult;
        }

        public BussinessLayerResult<NaciboUser> GetUserById(int id)
        {
            BussinessLayerResult<NaciboUser> res = new BussinessLayerResult<NaciboUser>();
            res.Result = Find(id);

            if(res.Result == null)
            {
                res.AddError(ErrorMessages.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
            
        }

        public BussinessLayerResult<NaciboUser> LoginUser(LoginVM data)
        {

            BussinessLayerResult<NaciboUser> layerResult = new BussinessLayerResult<NaciboUser>();
            layerResult.Result = FirstOrDefault(x => x.UserName == data.Username && x.Password == data.Password && 
            (x.Status == ENTITIES.Enums.DataStatus.Inserted ||
            x.Status == ENTITIES.Enums.DataStatus.Updated));


            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessages.UserIsNotActive, "Kullanıcı aktifleştirilmemiş.");
                    layerResult.AddError(ErrorMessages.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }
                if (layerResult.Result.Status == ENTITIES.Enums.DataStatus.Deleted)
                {
                    layerResult.AddError(ErrorMessages.UserCouldNotFind, "Böyle bir kullanıcı bulunmamaktadır");
                }

            }
            else
            {
                layerResult.AddError(ErrorMessages.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
            }
            return layerResult;
        }

        public BussinessLayerResult<NaciboUser> UpdateProfile(NaciboUser data)
        {
            NaciboUser naciboUser =FirstOrDefault(x => x.UserName == data.UserName || x.Email == data.Email);
            BussinessLayerResult<NaciboUser> res = new BussinessLayerResult<NaciboUser>();
            if(naciboUser != null && naciboUser.ID != data.ID)
            {
                if(naciboUser.UserName == data.UserName)
                {
                    res.AddError(ErrorMessages.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if(naciboUser.Email == data.Email)
                {
                    res.AddError(ErrorMessages.EmailAlreadyExists, "E-posta adresi kayıtlı");
                }
                return res;
            }
            
            res.Result = FirstOrDefault(x =>x.ID == data.ID);
            res.Result.Email = data.Email;
            res.Result.FirstName = data.FirstName;
            res.Result.LastName = data.LastName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;

            if(string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            
            if (Update(res.Result) == 0)
            {
                res.AddError(ErrorMessages.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }
            return res;
        }

        public BussinessLayerResult<NaciboUser> DeleteUserByID(int id)
        {
            BussinessLayerResult<NaciboUser> res = new BussinessLayerResult<NaciboUser>();
            NaciboUser user = FirstOrDefault(x => x.ID == id);

            if (user != null)
            {
                if(Delete(user) == 0)
                {
                    res.AddError(ErrorMessages.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
                
            }
            else
            {
                res.AddError(ErrorMessages.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BussinessLayerResult<NaciboUser> ActivateUser(Guid activateID)
        {
            BussinessLayerResult<NaciboUser> layerResult = new BussinessLayerResult<NaciboUser>();
            layerResult.Result = FirstOrDefault(x => x.ActivateGuid == activateID);

            if(layerResult.Result != null)
            {
                if (layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessages.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return layerResult;
                }

                layerResult.Result.IsActive = true;
                Update(layerResult.Result);
            }
            else
            {
                layerResult.AddError(ErrorMessages.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı");
            }

            return layerResult;
        }
    }
}
