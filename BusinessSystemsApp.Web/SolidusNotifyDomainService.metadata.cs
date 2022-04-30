
namespace BusinessSystemsApp.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies CSR_listMetadata as the class
    // that carries additional metadata for the CSR_list class.
    [MetadataTypeAttribute(typeof(CSR_list.CSR_listMetadata))]
    public partial class CSR_list
    {

        // This class allows you to attach custom attributes to properties
        // of the CSR_list class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CSR_listMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CSR_listMetadata()
            {
            }

            public string Agent { get; set; }

            public string Code { get; set; }

            public string Company { get; set; }

            public string CSR { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public Nullable<DateTime> NotificationTime { get; set; }

            public string Priority { get; set; }

            public string Product { get; set; }

            public string RegistrationPerson { get; set; }

            public Nullable<DateTime> RegistrationTime { get; set; }

            public string Source { get; set; }

            public string Status { get; set; }

            public string Subject { get; set; }
        }
    }
}
