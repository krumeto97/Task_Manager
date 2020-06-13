﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Task.Manager.Entities;

namespace Task.Manager.Api.Data
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>()
                .HasIndex(a => a.Name)
                .IsUnique();


            modelBuilder.Entity<Assignment>(t =>
            {
                t.HasMany(x => x.Comments).WithOne(x => x.Assignment).HasForeignKey(x => x.AssignmentId);
                t.HasOne(t => t.Project).WithMany(t => t.Assignments).HasForeignKey(p => p.ProjectId);
            });

            modelBuilder.Entity<Worker>(w =>
            {
                w.HasIndex(w => w.Name)
                    .IsUnique();
                w.HasOne(r => r.Role).WithMany(w => w.Workers).HasForeignKey(w => w.RoleId);
                w.HasOne(p => p.Project).WithMany(p => p.Workers).HasForeignKey(w => w.ProjectId).OnDelete(DeleteBehavior.NoAction);
                
            });
            
            modelBuilder.Entity<Project>(p =>
            {
                p.HasMany(w => w.Workers).WithOne(p => p.Project).HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.NoAction);
                p.HasMany(p => p.Assignments).WithOne(p => p.Project).HasForeignKey(p => p.ProjectId);
            });

            modelBuilder.Entity<Comment>(c =>
            {
                c.HasOne(c => c.Assignment).WithMany(c => c.Comments).HasForeignKey(t => t.AssignmentId);
                
            });

            modelBuilder.Entity<Role>(r =>
            {
                r.HasMany(w => w.Workers).WithOne(r => r.Role).HasForeignKey(r => r.RoleId);
            });
        }


        public DbSet<Project> Projects { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Worker> Workers { get; set; }

    }
}
