using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.System.User
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User is required");
            RuleFor(x => x.PassWord).NotEmpty().WithMessage("PassWord is required")
                .MinimumLength(6).WithMessage("at least 6 characters") ;
            
        }
    }
}
