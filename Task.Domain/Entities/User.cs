using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PaySkyTask.Core.Entities;
using PaySkyTask.Domain.Enums;

namespace PaySkyTask.Domain.Entities;

public class User: IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = "";

    public UserRole UserRole { get; set; }
    public  virtual ICollection<Vacancy> Vacancies { get; set; }
    public virtual ICollection<Applications> Applications { get; set; }
}
