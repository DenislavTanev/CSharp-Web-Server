namespace SharedTrip.Services
{
    using SharedTrip.Models.Users;
    using System.Collections.Generic;

    public interface IValidator
    {
        ICollection<string> ValidateUser(UserRegisterInputModel model);
    }
}
