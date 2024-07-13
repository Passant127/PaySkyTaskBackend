using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySkyTask.Core.Entities;

public class AuditableEntity : BaseEntity
{
	public string CreatedBy { get; set; } = "";
	public string ModifiedBy { get; set; } = "";
	public DateTime CreatedDate { get; set; } = DateTime.Now;
	public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
