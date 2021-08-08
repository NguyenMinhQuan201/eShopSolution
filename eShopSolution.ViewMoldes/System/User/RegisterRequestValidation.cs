using eShopSolution.Data.Entities;
using eShopSolution.Date.EF;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eShopSolution.ViewMoldes.System.User
{

    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidation()
        { 
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("Firt Name cant over 200 characters");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(200).WithMessage("Last Name cant over 200 characters");
            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birday cant greater than 100");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email  is required")
                .Matches(@"([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format not match");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("User is required");
           
            
            
            RuleFor(x => x.PassWord).NotEmpty().WithMessage("PassWord is required")
                .MinimumLength(6).WithMessage("at least 6 characters");
            RuleFor(x=>x).Custom((request, context)=>{              
                if (request.PassWord != request.ConFirmPassWord)
                {
                    context.AddFailure("ConFirmPassWord is not match");
                }
            });
        }
    }
}
