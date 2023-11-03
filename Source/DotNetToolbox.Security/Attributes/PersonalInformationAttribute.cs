namespace DotNetToolbox.Security.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PersonalInformationAttribute : Attribute
{
    public bool IsEncrypted { get; set; }
}
