using PismoWebInput.Core.Infrastructure.Models.User;

namespace PismoWebInput.Core.Infrastructure.Models.UserManager
{
    public class EditUserManagerModel
    {
        public EditUserModel User { get; set; }

        public EditUserManagerModel()
        {
            User = new EditUserModel();
        }
    }
}
