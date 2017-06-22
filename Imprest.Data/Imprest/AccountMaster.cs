namespace Imprest.Data.Imprest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountMaster")]
    public partial class AccountMaster
    {
        [Key]
        public decimal AccountNo { get; set; }

        [Required]
        [StringLength(5)]
        public string InstId { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [StringLength(10)]
        public string CustId { get; set; }

        [StringLength(19)]
        public string CardNo { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        public decimal Amount { get; set; }
    }
}
