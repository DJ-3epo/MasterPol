﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MasterPol
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        
        public virtual DbSet<Material_type> Material_type { get; set; }
        public virtual DbSet<Partner_products> Partner_products { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<Partners_type> Partners_type { get; set; }
        public virtual DbSet<Product_type> Product_type { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
