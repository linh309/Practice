namespace Practice.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BillItems
    {
        [Key]
        public int BillItemId { get; set; }

        public int? BillId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public int? ItemTypeId { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual ItemType ItemType { get; set; }
    }
}
