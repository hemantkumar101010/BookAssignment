using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookSellingApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUsers class
public class AppUsers : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Country { get; set; }
}

