using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySkyTask.Core.Entities;
using PaySkyTask.Domain.Enums;

namespace PaySkyTask.Domain.Entities;

public class Vacancy:  AuditableEntity 
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid EmployerId { get; set; }
    public int MaxApplications { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool  IsArchived { get; set; }
    public VacancyStatus Status { get; set; }
    public virtual ICollection<Applications> Applications { get; set; } = new HashSet<Applications>();
}
