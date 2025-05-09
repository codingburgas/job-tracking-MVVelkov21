using System.ComponentModel.DataAnnotations;
using JobTracking.DataAccess.Data.Base;
namespace JobTracking.DataAccess.Data.Models;

public class Temp : IEntity
{
    [Key]
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    [Required]
    public int PainerId { get; set; }
    public virtual Temporary Temporary { get; set; }
}