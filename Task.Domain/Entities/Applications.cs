using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySkyTask.Core.Entities;
using PaySkyTask.Domain.Enums;

namespace PaySkyTask.Domain.Entities;

public class Applications : AuditableEntity
{

    public Guid VacancyId { get; set; }
    public virtual Vacancy Vacancy { get; set; }
    public Guid ApplicantId { get; set; }
    public string ApplicantName { get; set; } = "";

    public string EmployerName { get; set; } = "";
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime ApplicationDate { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

}
