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

    public virtual DbSet<ClinicBranch> ClinicBranches { get; set; }

    public virtual DbSet<ClinicTreatment> ClinicTreatments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentProvider> PaymentProviders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TreatmentCategory> TreatmentCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=debo_dev_02;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC270942E22D");

            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC277B6C5D7B");


            entity.ToTable("Appointment");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_Date");
            entity.Property(e => e.CreatorId).HasColumnName("Creator_ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.DentId).HasColumnName("Dent_ID");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsCreatedByStaff).HasColumnName("Is_Created_By_Staff");
            entity.Property(e => e.Note).HasMaxLength(2000);
            entity.Property(e => e.PaymentId).HasColumnName("Payment_ID");
            entity.Property(e => e.StartDate).HasColumnName("Start_Date");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TempDentId).HasColumnName("Temp_Dent_ID");
            entity.Property(e => e.TimeSlot).HasColumnName("Time_Slot");
            entity.Property(e => e.TreatId).HasColumnName("Treat_ID");

            entity.HasOne(d => d.Creator).WithMany(p => p.AppointmentCreators)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_Appointment.Creator_ID");

            entity.HasOne(d => d.Cus).WithMany(p => p.AppointmentCus)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Appointment.Cus_ID");

            entity.HasOne(d => d.Dent).WithMany(p => p.AppointmentDents)
                .HasForeignKey(d => d.DentId)
                .HasConstraintName("FK_Appointment.Dent_ID");

            entity.HasOne(d => d.TempDent).WithMany(p => p.AppointmentTempDents)
                .HasForeignKey(d => d.TempDentId)
                .HasConstraintName("FK_Appointment.Temp_Dent_ID");

            entity.HasOne(d => d.Treat).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.TreatId)
                .HasConstraintName("FK_Appointment.Treat_ID");
        });

        modelBuilder.Entity<ClinicBranch>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Clinic_B__3214EC274B196EB8");

            entity.HasKey(e => e.Id).HasName("PK__Clinic_B__3214EC27D1C78800");


            entity.ToTable("Clinic_Branch");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.MngId).HasColumnName("Mng_ID");

            entity.HasOne(d => d.Admin).WithMany(p => p.ClinicBranchAdmins)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_Clinic Branch.Admin_ID");

            entity.HasOne(d => d.Mng).WithMany(p => p.ClinicBranchMngs)
                .HasForeignKey(d => d.MngId)
                .HasConstraintName("FK_Clinic Branch.Mng_ID");
        });

        modelBuilder.Entity<ClinicTreatment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clinic_T__3214EC277F791234");


            entity.ToTable("Clinic_Treatment");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Admin).WithMany(p => p.ClinicTreatments)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_Clinic Treatment.Admin_ID");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.ClinicTreatments)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("FK_Clinic Treatment.Category");
        });

        modelBuilder.Entity<Employee>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC277B8624C8");

            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC274AA2C74F");


            entity.ToTable("Employee");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BrId).HasColumnName("Br_ID");

            entity.HasOne(d => d.Br).WithMany(p => p.Employees)
                .HasForeignKey(d => d.BrId)
                .HasConstraintName("FK_Employee.Br_ID");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee.UserID");

            entity.HasMany(d => d.Treats).WithMany(p => p.Dents)
                .UsingEntity<Dictionary<string, object>>(
                    "DentistMajor",
                    r => r.HasOne<ClinicTreatment>().WithMany()
                        .HasForeignKey("TreatId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Dentist Major.Treat_ID"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("DentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Dentist Major.Dent_ID"),
                    j =>
                    {

                        j.HasKey("DentId", "TreatId").HasName("PK__Dentist___7EA2627449EC37B8");

                        j.HasKey("DentId", "TreatId").HasName("PK__Dentist___7EA262746BE0065F");

                        j.ToTable("Dentist_Major");
                        j.IndexerProperty<Guid>("DentId").HasColumnName("Dent_ID");
                        j.IndexerProperty<int>("TreatId").HasColumnName("Treat_ID");
                    });
        });

        modelBuilder.Entity<Feedback>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC272C2CC90F");

            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC271E80042A");


            entity.ToTable("Feedback");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.TreatId).HasColumnName("Treat_ID");

            entity.HasOne(d => d.Cus).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Feedback.Cus_ID");

            entity.HasOne(d => d.Treat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TreatId)
                .HasConstraintName("FK_Feedback.Treat_ID");
        });

        modelBuilder.Entity<Payment>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC2717BA771E");

            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC27805006BC");


            entity.ToTable("Payment");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CusId).HasColumnName("Cus_ID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.MethodId).HasColumnName("Method_ID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Cus).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK_Payment.Cus_ID");

            entity.HasOne(d => d.Method).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MethodId)
                .HasConstraintName("FK_Payment.Method_ID");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Payment___3214EC278212B1EA");

            entity.HasKey(e => e.Id).HasName("PK__Payment___3214EC2778F5B051");


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

            entity.HasOne(d => d.Provider).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK_Payment Method.Provider_ID");
        });

        modelBuilder.Entity<PaymentProvider>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Payment___3214EC27B80867AD");

            entity.HasKey(e => e.Id).HasName("PK__Payment___3214EC272707E7F8");


            entity.ToTable("Payment_Provider");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Desciption).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {

            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB49BC8DF745B");

            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB49BB8E7C484");


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

            entity.HasKey(e => e.Id).HasName("PK__Treatmen__3214EC27FF88C2AC");

            entity.HasKey(e => e.Id).HasName("PK__Treatmen__3214EC27AC47EE49");

            entity.ToTable("Treatment_Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__User__3214EC270A790CA6");

            entity.HasKey(e => e.Id).HasName("PK__User__3214EC27259B5854");


            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DateOfBirthday).HasColumnName("Date Of Birthday");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.MedRec).HasColumnName("Med_Rec");

            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK_User.Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
