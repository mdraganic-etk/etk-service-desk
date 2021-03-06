//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace BusinessSystemsApp.Web
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class CSR_NotyfiEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new CSR_NotyfiEntities object using the connection string found in the 'CSR_NotyfiEntities' section of the application configuration file.
        /// </summary>
        public CSR_NotyfiEntities() : base("name=CSR_NotyfiEntities", "CSR_NotyfiEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new CSR_NotyfiEntities object.
        /// </summary>
        public CSR_NotyfiEntities(string connectionString) : base(connectionString, "CSR_NotyfiEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new CSR_NotyfiEntities object.
        /// </summary>
        public CSR_NotyfiEntities(EntityConnection connection) : base(connection, "CSR_NotyfiEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<CSR_list> CSR_list
        {
            get
            {
                if ((_CSR_list == null))
                {
                    _CSR_list = base.CreateObjectSet<CSR_list>("CSR_list");
                }
                return _CSR_list;
            }
        }
        private ObjectSet<CSR_list> _CSR_list;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the CSR_list EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCSR_list(CSR_list cSR_list)
        {
            base.AddObject("CSR_list", cSR_list);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="CSR_NotyfiModel", Name="CSR_list")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CSR_list : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new CSR_list object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="source">Initial value of the Source property.</param>
        /// <param name="cSR">Initial value of the CSR property.</param>
        /// <param name="priority">Initial value of the Priority property.</param>
        /// <param name="company">Initial value of the Company property.</param>
        /// <param name="product">Initial value of the Product property.</param>
        /// <param name="registrationTime">Initial value of the RegistrationTime property.</param>
        /// <param name="subject">Initial value of the Subject property.</param>
        public static CSR_list CreateCSR_list(global::System.Int32 id, global::System.String source, global::System.String cSR, global::System.String priority, global::System.String company, global::System.String product, global::System.DateTime registrationTime, global::System.String subject)
        {
            CSR_list cSR_list = new CSR_list();
            cSR_list.Id = id;
            cSR_list.Source = source;
            cSR_list.CSR = cSR;
            cSR_list.Priority = priority;
            cSR_list.Company = company;
            cSR_list.Product = product;
            cSR_list.RegistrationTime = registrationTime;
            cSR_list.Subject = subject;
            return cSR_list;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Source
        {
            get
            {
                return _Source;
            }
            set
            {
                OnSourceChanging(value);
                ReportPropertyChanging("Source");
                _Source = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Source");
                OnSourceChanged();
            }
        }
        private global::System.String _Source;
        partial void OnSourceChanging(global::System.String value);
        partial void OnSourceChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CSR
        {
            get
            {
                return _CSR;
            }
            set
            {
                OnCSRChanging(value);
                ReportPropertyChanging("CSR");
                _CSR = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("CSR");
                OnCSRChanged();
            }
        }
        private global::System.String _CSR;
        partial void OnCSRChanging(global::System.String value);
        partial void OnCSRChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                OnPriorityChanging(value);
                ReportPropertyChanging("Priority");
                _Priority = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Priority");
                OnPriorityChanged();
            }
        }
        private global::System.String _Priority;
        partial void OnPriorityChanging(global::System.String value);
        partial void OnPriorityChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Company
        {
            get
            {
                return _Company;
            }
            set
            {
                OnCompanyChanging(value);
                ReportPropertyChanging("Company");
                _Company = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Company");
                OnCompanyChanged();
            }
        }
        private global::System.String _Company;
        partial void OnCompanyChanging(global::System.String value);
        partial void OnCompanyChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Product
        {
            get
            {
                return _Product;
            }
            set
            {
                OnProductChanging(value);
                ReportPropertyChanging("Product");
                _Product = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Product");
                OnProductChanged();
            }
        }
        private global::System.String _Product;
        partial void OnProductChanging(global::System.String value);
        partial void OnProductChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Code
        {
            get
            {
                return _Code;
            }
            set
            {
                OnCodeChanging(value);
                ReportPropertyChanging("Code");
                _Code = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Code");
                OnCodeChanged();
            }
        }
        private global::System.String _Code;
        partial void OnCodeChanging(global::System.String value);
        partial void OnCodeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime RegistrationTime
        {
            get
            {
                return _RegistrationTime;
            }
            set
            {
                OnRegistrationTimeChanging(value);
                ReportPropertyChanging("RegistrationTime");
                _RegistrationTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("RegistrationTime");
                OnRegistrationTimeChanged();
            }
        }
        private global::System.DateTime _RegistrationTime;
        partial void OnRegistrationTimeChanging(global::System.DateTime value);
        partial void OnRegistrationTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String RegistrationPerson
        {
            get
            {
                return _RegistrationPerson;
            }
            set
            {
                OnRegistrationPersonChanging(value);
                ReportPropertyChanging("RegistrationPerson");
                _RegistrationPerson = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("RegistrationPerson");
                OnRegistrationPersonChanged();
            }
        }
        private global::System.String _RegistrationPerson;
        partial void OnRegistrationPersonChanging(global::System.String value);
        partial void OnRegistrationPersonChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                OnSubjectChanging(value);
                ReportPropertyChanging("Subject");
                _Subject = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Subject");
                OnSubjectChanged();
            }
        }
        private global::System.String _Subject;
        partial void OnSubjectChanging(global::System.String value);
        partial void OnSubjectChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                OnDescriptionChanging(value);
                ReportPropertyChanging("Description");
                _Description = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Description");
                OnDescriptionChanged();
            }
        }
        private global::System.String _Description;
        partial void OnDescriptionChanging(global::System.String value);
        partial void OnDescriptionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Status
        {
            get
            {
                return _Status;
            }
            set
            {
                OnStatusChanging(value);
                ReportPropertyChanging("Status");
                _Status = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Status");
                OnStatusChanged();
            }
        }
        private global::System.String _Status;
        partial void OnStatusChanging(global::System.String value);
        partial void OnStatusChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> NotificationTime
        {
            get
            {
                return _NotificationTime;
            }
            set
            {
                OnNotificationTimeChanging(value);
                ReportPropertyChanging("NotificationTime");
                _NotificationTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("NotificationTime");
                OnNotificationTimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _NotificationTime;
        partial void OnNotificationTimeChanging(Nullable<global::System.DateTime> value);
        partial void OnNotificationTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Agent
        {
            get
            {
                return _Agent;
            }
            set
            {
                OnAgentChanging(value);
                ReportPropertyChanging("Agent");
                _Agent = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Agent");
                OnAgentChanged();
            }
        }
        private global::System.String _Agent;
        partial void OnAgentChanging(global::System.String value);
        partial void OnAgentChanged();

        #endregion

    
    }

    #endregion

    
}
