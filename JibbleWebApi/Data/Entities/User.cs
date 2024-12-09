using System.Security.Policy;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Index(nameof(UserName), Name = "IDX_UserName", IsUnique = true)]
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
