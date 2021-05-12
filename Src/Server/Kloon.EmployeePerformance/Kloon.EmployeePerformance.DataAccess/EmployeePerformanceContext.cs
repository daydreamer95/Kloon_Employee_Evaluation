using Kloon.EmployeePerformance.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.DataAccess
{
    public class EmployeePerformanceContext : DbContext
    {
        public EmployeePerformanceContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region ConfigBuilder

            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FirstName).IsRequired();
                entity.Property(x => x.LastName).IsRequired();
                entity.Property(x => x.Email).IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(x => x.Position).WithMany(x => x.Users).HasForeignKey(x => x.PositionId).HasConstraintName("FK_User_Position");
            });

            modelBuilder.Entity<ProjectUser>().ToTable("ProjectUser");
            modelBuilder.Entity<ProjectUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(x => x.User).WithMany(x => x.ProjectUsers).HasForeignKey(x => x.UserId).HasConstraintName("FK_ProjectUser_User");
                entity.HasOne(x => x.Project).WithMany(x => x.ProjectUsers).HasForeignKey(x => x.ProjectId).HasConstraintName("FK_ProjectUser_Project");
            });

            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();
            });


            modelBuilder.Entity<CriteriaType>().ToTable("CriteriaType");
            modelBuilder.Entity<CriteriaType>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Criteria>().ToTable("Criteria");
            modelBuilder.Entity<Criteria>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(x => x.CriteriaType).WithMany(x => x.Criterias).HasForeignKey(x => x.CriteriaTypeId).HasConstraintName("FK_Criteria_CriteriaType");
            });

            modelBuilder.Entity<CriteriaQuarterEvaluation>().ToTable("CriteriaQuarterEvaluation");
            modelBuilder.Entity<CriteriaQuarterEvaluation>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(x => x.Criteria).WithMany(x => x.CriteriaQuarterEvaluations).HasForeignKey(x => x.CriteriaId).HasConstraintName("FK_CriteriaQuarterEvaluation_Criteria");
                entity.HasOne(x => x.QuarterEvaluation).WithMany(x => x.CriteriaQuarterEvaluations).HasForeignKey(x => x.QuarterEvaluationId).HasConstraintName("FK_CriteriaQuarterEvaluation_QuarterEvaluation");
            });

            modelBuilder.Entity<CriteriaTypeQuarterEvaluation>().ToTable("CriteriaTypeQuarterEvaluation");
            modelBuilder.Entity<CriteriaTypeQuarterEvaluation>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(x => x.CriteriaType).WithMany(x => x.CriteriaTypeQuarterEvaluations).HasForeignKey(x => x.CriteriaTypeId).HasConstraintName("FK_CriteriaTypeQuarterEvaluation_CriteriaType");
                entity.HasOne(x => x.QuarterEvaluation).WithMany(x => x.CriteriaTypeQuarterEvaluations).HasForeignKey(x => x.QuarterEvaluationId).HasConstraintName("FK_CriteriaTypeQuarterEvaluation_QuarterEvaluation");
            });

            modelBuilder.Entity<QuarterEvaluation>().ToTable("QuarterEvaluation");
            modelBuilder.Entity<QuarterEvaluation>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

            });

            modelBuilder.Entity<UserQuarterEvaluation>().ToTable("UserQuarterEvaluation");
            modelBuilder.Entity<UserQuarterEvaluation>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .IsRequired();

                entity.Property(x => x.RowVersion)
                    .IsRequired(true)
                    .HasColumnType("timestamp")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(y => y.QuarterEvaluation).WithOne(x => x.UserQuarterEvaluation).HasForeignKey<UserQuarterEvaluation>(y => y.QuarterEvaluationId);
            });

            #endregion
        }

        public virtual DbSet<Criteria> Criterias { get; set; }
        public virtual DbSet<CriteriaType> CriteriaTypes { get; set; }
        public virtual DbSet<CriteriaQuarterEvaluation> CriteriaQuarterEvaluations { get; set; }
        public virtual DbSet<CriteriaTypeQuarterEvaluation> CriteriaTypeQuarterEvaluations { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }
        public virtual DbSet<QuarterEvaluation> QuarterEvaluations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserQuarterEvaluation> UserQuarterEvaluations { get; set; }
    }
}
