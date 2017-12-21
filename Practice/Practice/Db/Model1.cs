namespace Practice.Db
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=SchoolEntities")
        {
        }

        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<BillItems> BillItems { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<BillItems>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ItemType>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}
