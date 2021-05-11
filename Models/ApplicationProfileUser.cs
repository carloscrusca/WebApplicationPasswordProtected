using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPasswordProtected.Models
{
    public class ApplicationProfileUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public byte[] ProfilePicture { get; set; }
    }
}
