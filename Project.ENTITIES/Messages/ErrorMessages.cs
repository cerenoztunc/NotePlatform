using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Messages
{
    public enum ErrorMessages
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153
    }
}
