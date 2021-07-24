using FluentValidation;
using module_10.Models;

namespace module_10.Utils.Validation
{
    internal class UserValidator : AbstractValidator<UserInput>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Length(3, 10);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Role).NotEmpty().Equal("student");
            RuleFor(x => x.PhoneNumber).Matches(@"\+?[0-9\s]+(?:\([0-9\s]+\)|-[0-9\s]+)?(?:[0-9\s]+(?:-[0-9\s]+)*)");
        }
    }
}
