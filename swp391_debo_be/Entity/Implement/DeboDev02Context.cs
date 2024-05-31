using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace swp391_debo_be.Entity.Implement;

public partial class DeboDev02Context : DbContext

{
    public DeboDev02Context()
    {
    }

    public DeboDev02Context(DbContextOptions<DeboDev02Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<ClinicBranch> ClinicBranches { get; set; }

    public virtual DbSet<ClinicTreatment> ClinicTreatments { get; set; }

    public virtual DbSet<CustomerRecord> CustomerRecords { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TreatmentCategory> TreatmentCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=debo_dev_02;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC2741C62A16");

            entity.ToTable("Appointment");

            entity.HasIndex(e => e.BookId, "IX_Appointment_Book_ID");

            entity.HasIndex(e => e.CusRecId, "IX_Appointment_Cus_Rec_ID");

            entity.HasIndex(e => e.TempDent, "IX_Appointment_Temp_Dent");

            entity.HasIndex(e => e.TreatId, "IX_Appointment_Treat_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("Book_ID");
            entity.Property(e => e.CusRecId).HasColumnName("Cus_Rec_ID");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.EstDuration).HasColumnName("Est_Duration");
            entity.Property(e => e.TempDent).HasColumnName("Temp_Dent");
            entity.Property(e => e.TreatId).HasColumnName("Treat_ID");

            entity.HasOne(d => d.Book).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_Appointment_Book_ID");

            entity.HasOne(d => d.CusRec).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CusRecId)
                .HasConstraintName("FK__Appointme__Cus_R__52593CB8");

            entity.HasOne(d => d.TempDentNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.TempDent)
                .HasConstraintName("FK_Appointment_Temp_Dent");

            entity.HasOne(d => d.Treat).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.TreatId)
                .HasConstraintName("FK_Appointment_Treat_ID");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC27E7A736E0");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.CreatorId, "IX_Booking_Creator_ID");

            entity.HasIndex(e => e.CusId, "IX_Booking_Cus_ID");

            entity.HasIndex(e => e.DenId, "IX_Booking_Den_ID");

            entity.HasIndex(e => e.PaymentId, "UQ__Booking__DA6C7FE003D3E611")
                .IsUnique()
                .HasFilter("([Payment_ID] IS NOT NULL)");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatorId).HasColumnName("Creator_ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.DenId).HasColumnName("Den_ID");
            entity.Property(e => e.IsCreatedByStaff).HasColumnName("Is_Created_By_Staff");
            entity.Property(e => e.MedRec).HasColumnName("Med_Rec");
            entity.Property(e => e.PaymentId).HasColumnName("Payment_ID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Creator).WithMany(p => p.BookingCreators)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_Booking_Creator_ID");

            entity.HasOne(d => d.Cus).WithMany(p => p.BookingCus)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Booking_Cus_ID");

            entity.HasOne(d => d.Den).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DenId)
                .HasConstraintName("FK_Booking_Den_ID");

            entity.HasOne(d => d.Payment).WithOne(p => p.Booking)
                .HasForeignKey<Booking>(d => d.PaymentId)
                .HasConstraintName("FK_Booking_Payment_ID");
        });

        modelBuilder.Entity<ClinicBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clinic_B__3214EC2754B82300");

            entity.ToTable("Clinic_Branch");

            entity.HasIndex(e => e.AdminId, "IX_Clinic_Branch_Admin_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.MngId).HasColumnName("Mng_ID");

            entity.HasOne(d => d.Admin).WithMany(p => p.ClinicBranches)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_Clinic_Branch_Admin_ID");
        });

        modelBuilder.Entity<ClinicTreatment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clinic_T__3214EC27D3BB3E55");

            entity.ToTable("Clinic_Treatment");

            entity.HasIndex(e => e.AdminId, "IX_Clinic_Treatment_Admin_ID");

            entity.HasIndex(e => e.Category, "IX_Clinic_Treatment_Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Admin).WithMany(p => p.ClinicTreatments)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_Clinic_Treatment_Admin_ID");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.ClinicTreatments)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("FK_Clinic_Treatment_Category");
        });

        modelBuilder.Entity<CustomerRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC279115F950");

            entity.ToTable("Customer_Record");

            entity.HasIndex(e => e.CusId, "IX_Customer_Record_Cus_ID");

            entity.HasIndex(e => e.DentId, "IX_Customer_Record_Dent_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.DentId).HasColumnName("Dent_ID");
            entity.Property(e => e.Summary).HasMaxLength(2000);

            entity.HasOne(d => d.Cus).WithMany(p => p.CustomerRecords)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Customer_Record_Cus_ID");

            entity.HasOne(d => d.Dent).WithMany(p => p.CustomerRecords)
                .HasForeignKey(d => d.DentId)
                .HasConstraintName("FK_Customer_Record_Dent_ID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27B38DD1FA");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.MngId, "IX_Employee_Mng_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MngId).HasColumnName("Mng_ID");

            entity.HasOne(d => d.Mng).WithMany(p => p.Employees)
                .HasForeignKey(d => d.MngId)
                .HasConstraintName("FK_Employee_Mng_ID");

            entity.HasMany(d => d.Treats).WithMany(p => p.Dents)
                .UsingEntity<Dictionary<string, object>>(
                    "DentistMajor",
                    r => r.HasOne<ClinicTreatment>().WithMany()
                        .HasForeignKey("TreatId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Dentist_Major_Treat_ID"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("DentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Dentist_Major_Dent_ID"),
                    j =>
                    {
                        j.HasKey("DentId", "TreatId").HasName("PK__Dentist___7EA26274A7A5F0FE");
                        j.ToTable("Dentist_Major");
                        j.HasIndex(new[] { "TreatId" }, "IX_Dentist_Major_Treat_ID");
                        j.IndexerProperty<Guid>("DentId").HasColumnName("Dent_ID");
                        j.IndexerProperty<int>("TreatId").HasColumnName("Treat_ID");
                    });
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC2719078489");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.CusId, "IX_Feedback_Cus_ID");

            entity.HasIndex(e => e.TreatId, "IX_Feedback_Treat_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.TreatId).HasColumnName("Treat_ID");

            entity.HasOne(d => d.Cus).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Feedback_Cus_ID");

            entity.HasOne(d => d.Treat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TreatId)
                .HasConstraintName("FK_Feedback_Treat_ID");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3214EC27A5342A08");

            entity.ToTable("Manager");

            entity.HasIndex(e => e.BrId, "UQ_Manager_Br_ID")
                .IsUnique()
                .HasFilter("([Br_ID] IS NOT NULL)");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BrId).HasColumnName("Br_ID");

            entity.HasOne(d => d.Br).WithOne(p => p.Manager)
                .HasForeignKey<Manager>(d => d.BrId)
                .HasConstraintName("FK_Manager_Br_ID_Clinic_Branch_ID");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC2741530AD8");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.CusId, "IX_Payment_Cus_ID");

            entity.HasIndex(e => e.MethodId, "IX_Payment_Method_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.MethodId).HasColumnName("Method_ID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Cus).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Payment_Cus_ID");

            entity.HasOne(d => d.Method).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MethodId)
                .HasConstraintName("FK_Payment_Method_ID");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment___3214EC27A63F48AB");

            entity.ToTable("Payment_Method");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IpnListenerLink).HasColumnName("Ipn_Listener_Link");
            entity.Property(e => e.PaymentInfo)
                .HasMaxLength(256)
                .HasColumnName("Payment_Info");
            entity.Property(e => e.PrivateKey)
                .HasMaxLength(256)
                .HasColumnName("Private_Key");
            entity.Property(e => e.ProviderId).HasColumnName("Provider_ID");
            entity.Property(e => e.PublicKey)
                .HasMaxLength(256)
                .HasColumnName("Public_Key");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB49B9D6A0592");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("Role_ID");
            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<TreatmentCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Treatmen__3214EC270769FCB0");

            entity.ToTable("Treatment_Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC2717C4EBC6");

            entity.ToTable("User");

            entity.HasIndex(e => e.Role, "IX_User_Role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DateOfBirthday).HasColumnName("Date Of Birthday");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
