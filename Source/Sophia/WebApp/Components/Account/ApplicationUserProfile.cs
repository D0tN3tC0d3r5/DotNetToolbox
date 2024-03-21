namespace Sophia.WebApp.Components.Account;

[Table("UserProfiles")]
[EntityTypeConfiguration(typeof(ApplicationUserProfile))]
public class ApplicationUserProfile
    : UserProfile,
      IEntityTypeConfiguration<ApplicationUserProfile> {
    [Key]
    [MaxLength(450)]
    public string Id { get; set; } = default!;

    public UserProfileData ToDto() => new() {
        Name = Name,
        Language = Language,
    };

    public void Configure(EntityTypeBuilder<ApplicationUserProfile> builder) {
        builder.HasKey(p => p.Id);
        builder.HasOne<ApplicationUser>()
               .WithOne(u => u.Profile)
               .HasForeignKey<ApplicationUserProfile>(w => w.Id)
               .HasPrincipalKey<ApplicationUser>(p => p.Id)
               .IsRequired();
    }
}
