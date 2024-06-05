using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Master
{
    [Table("MstEsignActiveDirectory")]
    public class MstEsignActiveDirectory : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        public long SNo { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string DisplayName { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [StringLength(50)]
        public string EmailAlias { get; set; }
        [StringLength(50)]
        public string SamAccountName { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Division { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string Manager { get; set; }
        [StringLength(50)]
        public string Office { get; set; }
        [StringLength(20)]
        public string WorkPhone { get; set; }
        [StringLength(50)]
        public string Company { get; set; }
        [StringLength(150)]
        public string Notes { get; set; }
        [StringLength(50)]
        public string AccountExpiryTime { get; set; }
        [StringLength(50)]
        public string AccountStatus { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string CommonName { get; set; }
        [StringLength(50)]
        public string EmployeeNumber { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string LogonName { get; set; }
        public int? DivisionId { get; set; }
        public int? DepartmentId { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
    }
}
