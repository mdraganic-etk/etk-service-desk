
namespace BusinessSystemsApp.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;

    // The MetadataTypeAttribute identifies AttachmentMetadata as the class
    // that carries additional metadata for the Attachment class.
    [MetadataTypeAttribute(typeof(Attachment.AttachmentMetadata))]
    public partial class Attachment
    {

        // This class allows you to attach custom attributes to properties
        // of the Attachment class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class AttachmentMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AttachmentMetadata()
            {
            }

            public string AttachmentName { get; set; }

            public EntityCollection<AttachmenttAssign> AttachmenttAssign { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string Url { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies AttachmenttAssignMetadata as the class
    // that carries additional metadata for the AttachmenttAssign class.
    [MetadataTypeAttribute(typeof(AttachmenttAssign.AttachmenttAssignMetadata))]
    public partial class AttachmenttAssign
    {

        // This class allows you to attach custom attributes to properties
        // of the AttachmenttAssign class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class AttachmenttAssignMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AttachmenttAssignMetadata()
            {
            }
            [Include()]
            public Attachment Attachment { get; set; }

            public Nullable<int> AttachmentId { get; set; }

            public Csr Csr { get; set; }

            public Nullable<int> CsrId { get; set; }

            public int Id { get; set; }

            public KB_Notes KB_Notes { get; set; }

            public Nullable<int> KBNoteId { get; set; }

            public Nullable<int> NoteId { get; set; }

            public Notes Notes { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CommunicationChannelMetadata as the class
    // that carries additional metadata for the CommunicationChannel class.
    [MetadataTypeAttribute(typeof(CommunicationChannel.CommunicationChannelMetadata))]
    public partial class CommunicationChannel
    {

        // This class allows you to attach custom attributes to properties
        // of the CommunicationChannel class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CommunicationChannelMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CommunicationChannelMetadata()
            {
            }

            public string CommunicationChannelName { get; set; }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CompaniesAssignmentMetadata as the class
    // that carries additional metadata for the CompaniesAssignment class.
    [MetadataTypeAttribute(typeof(CompaniesAssignment.CompaniesAssignmentMetadata))]
    public partial class CompaniesAssignment
    {

        // This class allows you to attach custom attributes to properties
        // of the CompaniesAssignment class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CompaniesAssignmentMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CompaniesAssignmentMetadata()
            {
            }

            public int AssignedCommpanyId { get; set; }

            public Company Company { get; set; }
            [Include()]
            public Company Company1 { get; set; }

            public int CompanyId { get; set; }

            public string Description { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CompanyMetadata as the class
    // that carries additional metadata for the Company class.
    [MetadataTypeAttribute(typeof(Company.CompanyMetadata))]
    public partial class Company
    {

        // This class allows you to attach custom attributes to properties
        // of the Company class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CompanyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CompanyMetadata()
            {
            }

            public EntityCollection<CompaniesAssignment> CompaniesAssignment { get; set; }

            public EntityCollection<CompaniesAssignment> CompaniesAssignment1 { get; set; }

            public string CompanyName { get; set; }

            public EntityCollection<CompanyPriorities> CompanyPriorities { get; set; }

            public EntityCollection<CompanyStatuses> CompanyStatuses { get; set; }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public EntityCollection<ProductsInCompany> ProductsInCompany { get; set; }

            public EntityCollection<RequesterTypeInCompany> RequesterTypeInCompany { get; set; }

            public EntityCollection<IssueDomainInCompany> IssueDomainInCompany { get; set; }

            public EntityCollection<Site> Site { get; set; }

            public EntityCollection<User> User { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CompanyPrioritiesMetadata as the class
    // that carries additional metadata for the CompanyPriorities class.
    [MetadataTypeAttribute(typeof(CompanyPriorities.CompanyPrioritiesMetadata))]
    public partial class CompanyPriorities
    {

        // This class allows you to attach custom attributes to properties
        // of the CompanyPriorities class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CompanyPrioritiesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CompanyPrioritiesMetadata()
            {
            }

            public Company Company { get; set; }

            public int CompanyId { get; set; }

            public string Description { get; set; }
            [Include()]
            public Priority Priority { get; set; }

            public int PriorityId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CompanyStatusesMetadata as the class
    // that carries additional metadata for the CompanyStatuses class.
    [MetadataTypeAttribute(typeof(CompanyStatuses.CompanyStatusesMetadata))]
    public partial class CompanyStatuses
    {

        // This class allows you to attach custom attributes to properties
        // of the CompanyStatuses class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CompanyStatusesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CompanyStatusesMetadata()
            {
            }

            public Company Company { get; set; }

            public int CompanyId { get; set; }
            [Include()]
            public Csr_Status Csr_Status { get; set; }

            public string Description { get; set; }

            public int StatusId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ContactMetadata as the class
    // that carries additional metadata for the Contact class.
    [MetadataTypeAttribute(typeof(Contact.ContactMetadata))]
    public partial class Contact
    {

        // This class allows you to attach custom attributes to properties
        // of the Contact class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ContactMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ContactMetadata()
            {
            }

            public string Comment { get; set; }

            public string Email { get; set; }

            public string Fix1 { get; set; }

            public string Fix2 { get; set; }

            public int Id { get; set; }

            public string Mobile1 { get; set; }

            public string Mobile2 { get; set; }

            public EntityCollection<User> User { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CsrMetadata as the class
    // that carries additional metadata for the Csr class.
    [MetadataTypeAttribute(typeof(Csr.CsrMetadata))]
    public partial class Csr
    {

        // This class allows you to attach custom attributes to properties
        // of the Csr class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CsrMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CsrMetadata()
            {
            }

            public string Answer { get; set; }

            public Nullable<DateTime> AnswerDate { get; set; }

            public EntityCollection<AttachmenttAssign> AttachmenttAssign { get; set; }

            public Nullable<DateTime> CallDate { get; set; }

            public Nullable<int> CallerId { get; set; }
            [Include()]
            public CommunicationChannel CommunicationChannel { get; set; }

            public int CommunicationId { get; set; }
            [Include()]
            public Company Company { get; set; }

            public int CompanyId { get; set; }
            [Include()]
            public Csr_Status Csr_Status { get; set; }

            public string CSRNumber { get; set; }

            public string Description { get; set; }

            public Nullable<DateTime> FinishDate { get; set; }

            public Frequency Frequency { get; set; }

            public Nullable<int> FrequencyId { get; set; }

            [StringLength(256)]
            public string Heading { get; set; }

            public int Id { get; set; }

            public EntityCollection<KB_Notes> KB_Notes { get; set; }

            public int LastUserId { get; set; }

            public EntityCollection<Notes> Notes { get; set; }
            [Include()]
            public Priority Priority { get; set; }

            public int PriorityId { get; set; }
            [Include()]
            public Product Product { get; set; }

            public int ProductId { get; set; }

            public Nullable<DateTime> RegisterDate { get; set; }

            public bool RiskAnalysisPerformed { get; set; }
            [Include()]
            public Severity Severity { get; set; }

            public Nullable<int> SeverityId { get; set; }
            [Include()]
            public Site Site { get; set; }

            public int SiteId { get; set; }

            public int StatusId { get; set; }

            public string TroubleReport { get; set; }
            [Include()]
            public User User { get; set; }
            [Include()]
            public User User1 { get; set; }
            [Include()]
            public User User2 { get; set; }

            public Nullable<int> UserId { get; set; }

            [Include()]
            public RequesterType RequesterType { get; set; }

            public int RequesterTypeId { get; set; }

            [Include()]
            public IssueDomain IssueDomain { get; set; }

            public int IssueDomainId { get; set; }

            public string Remedy { get; set; }

            public Nullable<DateTime> RemedyTime { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies Csr_StatusMetadata as the class
    // that carries additional metadata for the Csr_Status class.
    [MetadataTypeAttribute(typeof(Csr_Status.Csr_StatusMetadata))]
    public partial class Csr_Status
    {

        // This class allows you to attach custom attributes to properties
        // of the Csr_Status class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class Csr_StatusMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private Csr_StatusMetadata()
            {
            }

            public EntityCollection<CompanyStatuses> CompanyStatuses { get; set; }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string StatusName { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies FrequencyMetadata as the class
    // that carries additional metadata for the Frequency class.
    [MetadataTypeAttribute(typeof(Frequency.FrequencyMetadata))]
    public partial class Frequency
    {

        // This class allows you to attach custom attributes to properties
        // of the Frequency class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class FrequencyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private FrequencyMetadata()
            {
            }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public string FrequencyName { get; set; }

            public int Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies KB_NotesMetadata as the class
    // that carries additional metadata for the KB_Notes class.
    [MetadataTypeAttribute(typeof(KB_Notes.KB_NotesMetadata))]
    public partial class KB_Notes
    {

        // This class allows you to attach custom attributes to properties
        // of the KB_Notes class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class KB_NotesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private KB_NotesMetadata()
            {
            }

            public EntityCollection<AttachmenttAssign> AttachmenttAssign { get; set; }

            public int AutorId { get; set; }

            public Csr Csr { get; set; }

            public int CsrId { get; set; }

            public DateTime Date { get; set; }

            public string Description { get; set; }

            public string Heading { get; set; }

            public int Id { get; set; }

            public string Note { get; set; }
            [Include()]
            public Product Product { get; set; }

            public int ProductId { get; set; }
            [Include()]
            public User User { get; set; }
        }
    }


    // The MetadataTypeAttribute identifies LocalizationMetadata as the class
    // that carries additional metadata for the Localization class.
    [MetadataTypeAttribute(typeof(Localization.LocalizationMetadata))]
    public partial class Localization
    {

        // This class allows you to attach custom attributes to properties
        // of the Localization class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class LocalizationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private LocalizationMetadata()
            {
            }

            public string CultureCode { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public EntityCollection<User> User { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies NotesMetadata as the class
    // that carries additional metadata for the Notes class.
    [MetadataTypeAttribute(typeof(Notes.NotesMetadata))]
    public partial class Notes
    {

        // This class allows you to attach custom attributes to properties
        // of the Notes class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class NotesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private NotesMetadata()
            {
            }

            public EntityCollection<AttachmenttAssign> AttachmenttAssign { get; set; }

            public int AutorId { get; set; }

            public Csr Csr { get; set; }

            public int CsrId { get; set; }

            public DateTime Date { get; set; }

            public string Description { get; set; }

            public string Heading { get; set; }

            public int Id { get; set; }

            public string Note { get; set; }
            [Include()]
            public User User { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies PriorityMetadata as the class
    // that carries additional metadata for the Priority class.
    [MetadataTypeAttribute(typeof(Priority.PriorityMetadata))]
    public partial class Priority
    {

        // This class allows you to attach custom attributes to properties
        // of the Priority class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class PriorityMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private PriorityMetadata()
            {
            }

            public EntityCollection<CompanyPriorities> CompanyPriorities { get; set; }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string PriorityName { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ProductMetadata as the class
    // that carries additional metadata for the Product class.
    [MetadataTypeAttribute(typeof(Product.ProductMetadata))]
    public partial class Product
    {

        // This class allows you to attach custom attributes to properties
        // of the Product class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ProductMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ProductMetadata()
            {
            }

            public string Description { get; set; }

            public int Id { get; set; }

            public EntityCollection<KB_Notes> KB_Notes { get; set; }

            public string ProductName { get; set; }

            public EntityCollection<ProductsInCompany> ProductsInCompany { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ProductsInCompanyMetadata as the class
    // that carries additional metadata for the ProductsInCompany class.
    [MetadataTypeAttribute(typeof(ProductsInCompany.ProductsInCompanyMetadata))]
    public partial class ProductsInCompany
    {

        // This class allows you to attach custom attributes to properties
        // of the ProductsInCompany class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ProductsInCompanyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ProductsInCompanyMetadata()
            {
            }

            public Company Company { get; set; }

            public int CompanyId { get; set; }

            public string Description { get; set; }
            [Include()]
            public Product Product { get; set; }

            public int ProductId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SeverityMetadata as the class
    // that carries additional metadata for the Severity class.
    [MetadataTypeAttribute(typeof(Severity.SeverityMetadata))]
    public partial class Severity
    {

        // This class allows you to attach custom attributes to properties
        // of the Severity class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SeverityMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SeverityMetadata()
            {
            }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string SeverityName { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SiteMetadata as the class
    // that carries additional metadata for the Site class.
    [MetadataTypeAttribute(typeof(Site.SiteMetadata))]
    public partial class Site
    {

        // This class allows you to attach custom attributes to properties
        // of the Site class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SiteMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SiteMetadata()
            {
            }
            [Include()]
            public Company Company { get; set; }

            public int CompanyId { get; set; }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string SiteName { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies sysdiagramsMetadata as the class
    // that carries additional metadata for the sysdiagrams class.
    [MetadataTypeAttribute(typeof(sysdiagrams.sysdiagramsMetadata))]
    public partial class sysdiagrams
    {

        // This class allows you to attach custom attributes to properties
        // of the sysdiagrams class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class sysdiagramsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private sysdiagramsMetadata()
            {
            }

            public byte[] definition { get; set; }

            public int diagram_id { get; set; }

            public string name { get; set; }

            public int principal_id { get; set; }

            public Nullable<int> version { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UserMetadata as the class
    // that carries additional metadata for the User class.
    [MetadataTypeAttribute(typeof(User.UserMetadata))]
    public partial class User
    {

        // This class allows you to attach custom attributes to properties
        // of the User class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserMetadata()
            {
            }
            [Include()]
            public Company Company { get; set; }

            public int CompanyId { get; set; }
            [Include()]
            public Contact Contact { get; set; }

            public int ContactId { get; set; }

            public EntityCollection<Csr> Csr { get; set; }

            public Nullable<int> CultureId { get; set; }

            public string FirstName { get; set; }

            public int Id { get; set; }

            public EntityCollection<KB_Notes> KB_Notes { get; set; }

            public string LastName { get; set; }

            [Include()]
            public Localization Localization { get; set; }

            public EntityCollection<Notes> Notes { get; set; }

            public string Password { get; set; }

            public int TypeId { get; set; }

            public EntityCollection<UserLogging> UserLogging { get; set; }

            public string UserName { get; set; }

            public EntityCollection<UserNotifications> UserNotifications { get; set; }

            [Include()]
            public UserType UserType { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UserLoggingMetadata as the class
    // that carries additional metadata for the UserLogging class.
    [MetadataTypeAttribute(typeof(UserLogging.UserLoggingMetadata))]
    public partial class UserLogging
    {

        // This class allows you to attach custom attributes to properties
        // of the UserLogging class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserLoggingMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserLoggingMetadata()
            {
            }

            public int Id { get; set; }

            public DateTime LogInTime { get; set; }

            public Nullable<DateTime> LogOutTime { get; set; }

            public User User { get; set; }

            public int UserId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UserNotificationsMetadata as the class
    // that carries additional metadata for the UserNotifications class.
    [MetadataTypeAttribute(typeof(UserNotifications.UserNotificationsMetadata))]
    public partial class UserNotifications
    {

        // This class allows you to attach custom attributes to properties
        // of the UserNotifications class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserNotificationsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserNotificationsMetadata()
            {
            }

            public int CompanyId { get; set; }

            public string Description { get; set; }
            [Include()]
            public User User { get; set; }

            public int UserId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UserTypeMetadata as the class
    // that carries additional metadata for the UserType class.
    [MetadataTypeAttribute(typeof(UserType.UserTypeMetadata))]
    public partial class UserType
    {

        // This class allows you to attach custom attributes to properties
        // of the UserType class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserTypeMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserTypeMetadata()
            {
            }

            public string Description { get; set; }

            public int Id { get; set; }

            public string TypeName { get; set; }

            public EntityCollection<User> User { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies IssueDomainMetadata as the class
    // that carries additional metadata for the IssueDomain class.
    [MetadataTypeAttribute(typeof(IssueDomain.IssueDomainMetadata))]
    public partial class IssueDomain
    {

        // This class allows you to attach custom attributes to properties
        // of the IssueDomain class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class IssueDomainMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private IssueDomainMetadata()
            {
            }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public EntityCollection<IssueDomainInCompany> IssueDomainInCompany { get; set; }

            public string Name { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies IssueDomainInCompanyMetadata as the class
    // that carries additional metadata for the IssueDomainInCompany class.
    [MetadataTypeAttribute(typeof(IssueDomainInCompany.IssueDomainInCompanyMetadata))]
    public partial class IssueDomainInCompany
    {

        // This class allows you to attach custom attributes to properties
        // of the IssueDomainInCompany class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class IssueDomainInCompanyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private IssueDomainInCompanyMetadata()
            {
            }

            public Company Company { get; set; }

            public int CompanyId { get; set; }

            public string Description { get; set; }

            [Include()]
            public IssueDomain IssueDomain { get; set; }

            public int IssueDomainId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies RequesterTypeMetadata as the class
    // that carries additional metadata for the RequesterType class.
    [MetadataTypeAttribute(typeof(RequesterType.RequesterTypeMetadata))]
    public partial class RequesterType
    {

        // This class allows you to attach custom attributes to properties
        // of the RequesterType class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RequesterTypeMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private RequesterTypeMetadata()
            {
            }

            public EntityCollection<Csr> Csr { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }

            public EntityCollection<RequesterTypeInCompany> RequesterTypeInCompany { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies RequesterTypeInCompanyMetadata as the class
    // that carries additional metadata for the RequesterTypeInCompany class.
    [MetadataTypeAttribute(typeof(RequesterTypeInCompany.RequesterTypeInCompanyMetadata))]
    public partial class RequesterTypeInCompany
    {

        // This class allows you to attach custom attributes to properties
        // of the RequesterTypeInCompany class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RequesterTypeInCompanyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private RequesterTypeInCompanyMetadata()
            {
            }

            public Company Company { get; set; }

            public int CompanyId { get; set; }

            public string Description { get; set; }
            
            [Include()]
            public RequesterType RequesterType { get; set; }

            public int RequesterTypeId { get; set; }
        }
    }
}
