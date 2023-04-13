using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MissionApp.Entities.Models;

namespace MissionApp.Entities.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsPage> CmsPages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FavouriteMission> FavouriteMissions { get; set; }

    public virtual DbSet<GoalMission> GoalMissions { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<MissionApplication> MissionApplications { get; set; }

    public virtual DbSet<MissionDocument> MissionDocuments { get; set; }

    public virtual DbSet<MissionInvite> MissionInvites { get; set; }

    public virtual DbSet<MissionMedium> MissionMedia { get; set; }

    public virtual DbSet<MissionRating> MissionRatings { get; set; }

    public virtual DbSet<MissionSkill> MissionSkills { get; set; }

    public virtual DbSet<MissionTheme> MissionThemes { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<StoryInvite> StoryInvites { get; set; }

    public virtual DbSet<StoryMedium> StoryMedia { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PCA32\\SQL2019;DataBase=MissionAppDb;Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__4A3006F7943C810C");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "UQ__Admin__A9D105344D468D91").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("Admin_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("First_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__8177AC04BE61A694");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("Banner_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Image)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.SortOrder).HasColumnName("Sort_Order");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__DE9DE000D21DC136");

            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("City_Id");
            entity.Property(e => e.CountryId).HasColumnName("Country_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__City__Country_Id__2F10007B");
        });

        modelBuilder.Entity<CmsPage>(entity =>
        {
            entity.HasKey(e => e.CmsPageId).HasName("PK__CMS_Page__DFA80DBFE3F68C7F");

            entity.ToTable("CMS_Page");

            entity.Property(e => e.CmsPageId).HasColumnName("CMS_Page_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__99FC14DBB95436EB");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("Comment_Id");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("Approval_Status");
            entity.Property(e => e.CommentText).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Mission__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__User_Id__4316F928");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Country__8036CBAEDEA1DA14");

            entity.ToTable("Country");

            entity.Property(e => e.CountryId).HasColumnName("Country_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Iso)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ISO");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<FavouriteMission>(entity =>
        {
            entity.HasKey(e => e.FavouriteMissionId).HasName("PK__Favourit__4AA4FEF190950221");

            entity.ToTable("Favourite_Mission");

            entity.Property(e => e.FavouriteMissionId).HasColumnName("Favourite_Mission_id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Mission).WithMany(p => p.FavouriteMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favourite__Missi__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.FavouriteMissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favourite__User___4BAC3F29");
        });

        modelBuilder.Entity<GoalMission>(entity =>
        {
            entity.HasKey(e => e.GoalMissionId).HasName("PK__Goal_Mis__3552730160410670");

            entity.ToTable("Goal_Mission");

            entity.Property(e => e.GoalMissionId).HasColumnName("Goal_Mission_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.GoalObjectiveText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Goal_Objective_Text");
            entity.Property(e => e.GoalValue).HasColumnName("Goal_Value");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.TotalValue).HasColumnName("Total_Value");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.GoalMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Goal_Miss__Missi__5070F446");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__Mission__93DB38B2A87A0B48");

            entity.ToTable("Mission");

            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.CityId).HasColumnName("City_Id");
            entity.Property(e => e.CountryId).HasColumnName("Country_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndDate)
                .HasPrecision(0)
                .HasColumnName("End_Date");
            entity.Property(e => e.MissionAvailability)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Mission_Availability");
            entity.Property(e => e.MissionStatus).HasColumnName("Mission_Status");
            entity.Property(e => e.MissionThemeId).HasColumnName("Mission_Theme_Id");
            entity.Property(e => e.MissionType)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Mission_Type");
            entity.Property(e => e.OrganizationDetail)
                .HasColumnType("text")
                .HasColumnName("Organization_Detail");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Organization_Name");
            entity.Property(e => e.ShortDescription)
                .HasColumnType("text")
                .HasColumnName("Short_Description");
            entity.Property(e => e.StartDate)
                .HasPrecision(0)
                .HasColumnName("Start_Date");
            entity.Property(e => e.Title)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.City).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission__City_Id__3E52440B");

            entity.HasOne(d => d.Country).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission__Country__3F466844");

            entity.HasOne(d => d.MissionTheme).WithMany(p => p.Missions)
                .HasForeignKey(d => d.MissionThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission__Mission__3D5E1FD2");
        });

        modelBuilder.Entity<MissionApplication>(entity =>
        {
            entity.HasKey(e => e.MissionApplicationId).HasName("PK__Mission___4D66C19C29E7F63C");

            entity.ToTable("Mission_Application");

            entity.Property(e => e.MissionApplicationId).HasColumnName("Mission_Application_Id");
            entity.Property(e => e.AppliedAt)
                .HasColumnType("datetime")
                .HasColumnName("Applied_at");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("Approval_status");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_A__Missi__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_A__User___5535A963");
        });

        modelBuilder.Entity<MissionDocument>(entity =>
        {
            entity.HasKey(e => e.MissionDocumentId).HasName("PK__Mission___5E814220028A9035");

            entity.ToTable("Mission_Document");

            entity.Property(e => e.MissionDocumentId).HasColumnName("Mission_Document_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Document_Name");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Document_Path");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Document_Type");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionDocuments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_D__Missi__59FA5E80");
        });

        modelBuilder.Entity<MissionInvite>(entity =>
        {
            entity.HasKey(e => e.MissionInviteId).HasName("PK__Mission___F42BB8D1F69295CB");

            entity.ToTable("Mission_Invite");

            entity.Property(e => e.MissionInviteId).HasColumnName("Mission_Invite_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("From_User_Id");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.ToUserId).HasColumnName("To_User_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MissionInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_I__From___5EBF139D");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_I__Missi__5DCAEF64");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MissionInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_I__To_Us__5FB337D6");
        });

        modelBuilder.Entity<MissionMedium>(entity =>
        {
            entity.HasKey(e => e.MissionMediaId).HasName("PK__Mission___395AE427626AEEAE");

            entity.ToTable("Mission_Media");

            entity.Property(e => e.MissionMediaId).HasColumnName("Mission_Media_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MediaName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("Media_Name");
            entity.Property(e => e.MediaPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Media_Path");
            entity.Property(e => e.MediaType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("Media_Type");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionMedia)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_M__Missi__6383C8BA");
        });

        modelBuilder.Entity<MissionRating>(entity =>
        {
            entity.HasKey(e => e.MissionRatingId).HasName("PK__Mission___193BE1C146CFDA48");

            entity.ToTable("Mission_Rating");

            entity.Property(e => e.MissionRatingId).HasColumnName("Mission_Rating_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_R__Missi__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_R__User___68487DD7");
        });

        modelBuilder.Entity<MissionSkill>(entity =>
        {
            entity.HasKey(e => e.MissionSkillId).HasName("PK__Mission___CF5C1E4B0D6A7217");

            entity.ToTable("Mission_Skill");

            entity.Property(e => e.MissionSkillId).HasColumnName("Mission_Skill_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.SkillId).HasColumnName("Skill_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_S__Missi__72C60C4A");

            entity.HasOne(d => d.Skill).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mission_S__Skill__71D1E811");
        });

        modelBuilder.Entity<MissionTheme>(entity =>
        {
            entity.HasKey(e => e.MissionThemeId).HasName("PK__MissionT__57A3193B5F278D6C");

            entity.ToTable("MissionTheme");

            entity.Property(e => e.MissionThemeId).HasColumnName("Mission_Theme_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.HasKey(e => e.Token);

            entity.ToTable("Password_Reset");

            entity.HasIndex(e => e.Email, "UQ__Password__A9D10534C47F6EC0").IsUnique();

            entity.Property(e => e.Token)
                .HasMaxLength(191)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__B4A9E290EA8DC10D");

            entity.Property(e => e.SkillId).HasColumnName("Skill_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.SkillName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("Skill_Name");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("PK__Story__C43D7E48A30A0AE2");

            entity.ToTable("Story");

            entity.Property(e => e.StoryId).HasColumnName("Story_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.PublishedAt)
                .HasPrecision(0)
                .HasColumnName("Published_at");
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('DRAFT')");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Stories)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Story__Mission_I__7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.Stories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Story__User_Id__7A672E12");
        });

        modelBuilder.Entity<StoryInvite>(entity =>
        {
            entity.HasKey(e => e.StoryInviteId).HasName("PK__Stoty_In__BE13AAD2DA7D2F4F");

            entity.ToTable("Story_Invite");

            entity.Property(e => e.StoryInviteId).HasColumnName("Story_Invite_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("From_User_Id");
            entity.Property(e => e.StoryId).HasColumnName("Story_Id");
            entity.Property(e => e.ToUserId).HasColumnName("To_User_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.StoryInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stoty_Inv__From___01142BA1");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryInvites)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stoty_Inv__Story__00200768");

            entity.HasOne(d => d.ToUser).WithMany(p => p.StoryInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stoty_Inv__To_Us__02084FDA");
        });

        modelBuilder.Entity<StoryMedium>(entity =>
        {
            entity.HasKey(e => e.StoryMediaId).HasName("PK__Story_Me__E32A73378D5DE2B9");

            entity.ToTable("Story_Media");

            entity.Property(e => e.StoryMediaId).HasColumnName("Story_Media_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Path).HasColumnType("text");
            entity.Property(e => e.StoryId).HasColumnName("Story_Id");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryMedia)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Story_Med__Story__05D8E0BE");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__Timeshee__9E52341032D2D7D0");

            entity.ToTable("Timesheet");

            entity.Property(e => e.TimesheetId).HasColumnName("Timesheet_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateVolunteered)
                .HasPrecision(0)
                .HasColumnName("Date_Volunteered");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("Mission_Id");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Timesheet__Missi__0A9D95DB");

            entity.HasOne(d => d.User).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Timesheet__User___09A971A2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206D917057DF79ED");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534F11BD120").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(2048)
                .IsUnicode(false);
            entity.Property(e => e.CityId).HasColumnName("City_Id");
            entity.Property(e => e.CountryId).HasColumnName("Country_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Employee_Id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("First_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Last_name");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Linked_In_Url");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber).HasColumnName("Phone_number");
            entity.Property(e => e.ProfileText)
                .HasColumnType("text")
                .HasColumnName("Profile_Text");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.WhyIVolunteer)
                .HasColumnType("text")
                .HasColumnName("Why_I_Volunteer");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Users__City_Id__33D4B598");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__Users__Country_I__34C8D9D1");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserSkillId).HasName("PK__User_Ski__5EEE510640DF088E");

            entity.ToTable("User_Skill");

            entity.Property(e => e.UserSkillId).HasColumnName("User_Skill_Id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("Created_at");
            entity.Property(e => e.DeletedAt)
                .HasPrecision(0)
                .HasColumnName("Deleted_at");
            entity.Property(e => e.SkillId).HasColumnName("Skill_Id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasColumnName("Updated_at");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Skil__Skill__10566F31");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Skil__User___0F624AF8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
