
namespace BusinessSystemsApp.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Implements application logic using the BusinessSystemsConnectionString context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class BusinessSystemsDomainService : LinqToEntitiesDomainService<BusinessSystemsConnectionString>
    {
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Attachment' query.
        public IQueryable<Attachment> GetAttachment()
        {
            return this.ObjectContext.Attachment;
        }

        public void InsertAttachment(Attachment attachment)
        {
            if ((attachment.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(attachment, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Attachment.AddObject(attachment);
            }
        }

        public void UpdateAttachment(Attachment currentAttachment)
        {
            this.ObjectContext.Attachment.AttachAsModified(currentAttachment, this.ChangeSet.GetOriginal(currentAttachment));
        }

        public void DeleteAttachment(Attachment attachment)
        {
            if ((attachment.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Attachment.Attach(attachment);
            }
            this.ObjectContext.Attachment.DeleteObject(attachment);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'AttachmenttAssign' query.
        public IQueryable<AttachmenttAssign> GetAttachmenttAssignCsr(Int32 _csrId)
        {
            return this.ObjectContext.AttachmenttAssign.Include("Attachment").Where(c => c.CsrId == _csrId);
        }

        public IQueryable<AttachmenttAssign> GetAttachmenttAssignNote(Int32 _noteId)
        {
            return this.ObjectContext.AttachmenttAssign.Include("Attachment").Where(c => c.NoteId == _noteId);
        }

        public IQueryable<AttachmenttAssign> GetAttachmenttAssignKB(Int32 _kbId)
        {
            return this.ObjectContext.AttachmenttAssign.Include("Attachment").Where(c => c.KBNoteId == _kbId);
        }

        public void InsertAttachmenttAssign(AttachmenttAssign attachmenttAssign)
        {
            if ((attachmenttAssign.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(attachmenttAssign, EntityState.Added);
            }
            else
            {
                this.ObjectContext.AttachmenttAssign.AddObject(attachmenttAssign);
            }
        }

        public void UpdateAttachmenttAssign(AttachmenttAssign currentAttachmenttAssign)
        {
            this.ObjectContext.AttachmenttAssign.AttachAsModified(currentAttachmenttAssign, this.ChangeSet.GetOriginal(currentAttachmenttAssign));
        }

        public void DeleteAttachmenttAssign(AttachmenttAssign attachmenttAssign)
        {
            if ((attachmenttAssign.EntityState == EntityState.Detached))
            {
                this.ObjectContext.AttachmenttAssign.Attach(attachmenttAssign);
            }
            this.ObjectContext.AttachmenttAssign.DeleteObject(attachmenttAssign);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CommunicationChannel' query.
        public IQueryable<CommunicationChannel> GetCommunicationChannel()
        {
            return this.ObjectContext.CommunicationChannel;
        }

        public void InsertCommunicationChannel(CommunicationChannel communicationChannel)
        {
            if ((communicationChannel.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(communicationChannel, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CommunicationChannel.AddObject(communicationChannel);
            }
        }

        public void UpdateCommunicationChannel(CommunicationChannel currentCommunicationChannel)
        {
            this.ObjectContext.CommunicationChannel.AttachAsModified(currentCommunicationChannel, this.ChangeSet.GetOriginal(currentCommunicationChannel));
        }

        public void DeleteCommunicationChannel(CommunicationChannel communicationChannel)
        {
            if ((communicationChannel.EntityState == EntityState.Detached))
            {
                this.ObjectContext.CommunicationChannel.Attach(communicationChannel);
            }
            this.ObjectContext.CommunicationChannel.DeleteObject(communicationChannel);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CompaniesAssignment' query.
        public IQueryable<CompaniesAssignment> GetCompaniesAssignment(Int32 _companyId)
        {
            return this.ObjectContext.CompaniesAssignment.Include("Company1").Where(c => c.CompanyId == _companyId).OrderBy(c => c.Company1.CompanyName);
        }

        public void InsertCompaniesAssignment(CompaniesAssignment companiesAssignment)
        {
            if ((companiesAssignment.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(companiesAssignment, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CompaniesAssignment.AddObject(companiesAssignment);
            }
        }

        public void UpdateCompaniesAssignment(CompaniesAssignment currentCompaniesAssignment)
        {
            this.ObjectContext.CompaniesAssignment.AttachAsModified(currentCompaniesAssignment, this.ChangeSet.GetOriginal(currentCompaniesAssignment));
        }

        public void DeleteCompaniesAssignment(CompaniesAssignment companiesAssignment)
        {
            if ((companiesAssignment.EntityState == EntityState.Detached))
            {
                this.ObjectContext.CompaniesAssignment.Attach(companiesAssignment);
            }
            this.ObjectContext.CompaniesAssignment.DeleteObject(companiesAssignment);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Company' query.
        public IQueryable<Company> GetCompany()
        {
            return this.ObjectContext.Company.OrderBy(c => c.CompanyName);
        }

        public void InsertCompany(Company company)
        {
            if ((company.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(company, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Company.AddObject(company);
            }
        }

        public void UpdateCompany(Company currentCompany)
        {
            this.ObjectContext.Company.AttachAsModified(currentCompany, this.ChangeSet.GetOriginal(currentCompany));
        }

        public void DeleteCompany(Company company)
        {
            if ((company.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Company.Attach(company);
            }
            this.ObjectContext.Company.DeleteObject(company);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CompanyPriorities' query.
        public IQueryable<CompanyPriorities> GetCompanyPriorities(Int32 _companyId)
        {
            return this.ObjectContext.CompanyPriorities.Include("Priority").Where(c => c.CompanyId == _companyId).OrderBy(c => c.Priority.PriorityName);
        }

        public void InsertCompanyPriorities(CompanyPriorities companyPriorities)
        {
            if ((companyPriorities.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(companyPriorities, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CompanyPriorities.AddObject(companyPriorities);
            }
        }

        public void UpdateCompanyPriorities(CompanyPriorities currentCompanyPriorities)
        {
            this.ObjectContext.CompanyPriorities.AttachAsModified(currentCompanyPriorities, this.ChangeSet.GetOriginal(currentCompanyPriorities));
        }

        public void DeleteCompanyPriorities(CompanyPriorities companyPriorities)
        {
            if ((companyPriorities.EntityState == EntityState.Detached))
            {
                this.ObjectContext.CompanyPriorities.Attach(companyPriorities);
            }
            this.ObjectContext.CompanyPriorities.DeleteObject(companyPriorities);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CompanyStatuses' query.
        public IQueryable<CompanyStatuses> GetCompanyStatusesOriginal()
        {
            return this.ObjectContext.CompanyStatuses;
        }

        public IQueryable<CompanyStatuses> GetCompanyStatuses(Int32 _companyId)
        {
            return this.ObjectContext.CompanyStatuses.Include("Csr_Status").Where(c => c.CompanyId == _companyId); ;
        }

        public void InsertCompanyStatuses(CompanyStatuses companyStatuses)
        {
            if ((companyStatuses.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(companyStatuses, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CompanyStatuses.AddObject(companyStatuses);
            }
        }

        public void UpdateCompanyStatuses(CompanyStatuses currentCompanyStatuses)
        {
            this.ObjectContext.CompanyStatuses.AttachAsModified(currentCompanyStatuses, this.ChangeSet.GetOriginal(currentCompanyStatuses));
        }

        public void DeleteCompanyStatuses(CompanyStatuses companyStatuses)
        {
            if ((companyStatuses.EntityState == EntityState.Detached))
            {
                this.ObjectContext.CompanyStatuses.Attach(companyStatuses);
            }
            this.ObjectContext.CompanyStatuses.DeleteObject(companyStatuses);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Contact' query.
        public IQueryable<Contact> GetContact()
        {
            return this.ObjectContext.Contact;
        }

        public IQueryable<Contact> GetContactWithId(Int32 _contactId)
        {
            return this.ObjectContext.Contact.Where(c => c.Id == _contactId);
        }

        public IQueryable<Contact> GetContactWithContactList(List<Int32> _contactList)
        {

            return this.ObjectContext.Contact.Where(c => _contactList.Contains(c.Id));
        }

        public void InsertContact(Contact contact)
        {
            if ((contact.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(contact, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Contact.AddObject(contact);
            }
        }

        public void UpdateContact(Contact currentContact)
        {
            this.ObjectContext.Contact.AttachAsModified(currentContact, this.ChangeSet.GetOriginal(currentContact));
        }

        public void DeleteContact(Contact contact)
        {
            if ((contact.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Contact.Attach(contact);
            }
            this.ObjectContext.Contact.DeleteObject(contact);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Csr' query.
        public IQueryable<Csr> GetCsr()
        {
            return this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").OrderByDescending(c => c.RegisterDate);
        }

        public IQueryable<Csr> GetCsrSupport()
        {
            return this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").OrderByDescending(c => c.RegisterDate);
        }


        public IQueryable<Csr> GetCsrCustomer(Int32 _userId)
        {
            return this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.UserId == _userId).OrderByDescending(c => c.RegisterDate);
        }


        public IQueryable<Csr> GetFilteredCsr(String _csrId, Int32 _companyId, Int32 _productId, DateTime _registerFrom, DateTime _registerTo, DateTime _finishedFrom, DateTime _finishedTo, List<Int32> _statuses, Boolean _onlyMine, String _text, Int32 _userId, bool _customer, List<Int32> _companiesList)
        {
            IQueryable<Csr> csr;

            if (_csrId != "")
            {
                Int32 csrId = Convert.ToInt32(_csrId);

                if (_companyId != 0)
                {
                    if (_productId != 0)
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.Id == csrId && c.CompanyId == _companyId && c.ProductId == _productId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                    else
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.Id == csrId && c.CompanyId == _companyId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                }
                else
                {
                    if (_productId != 0)
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.Id == csrId && c.ProductId == _productId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                    else
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.Id == csrId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                }
            }
            else
            {
                if (_companyId != 0)
                {
                    if (_productId != 0)
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.CompanyId == _companyId && c.ProductId == _productId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                    else
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.CompanyId == _companyId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                }
                else
                {
                    if (_productId != 0)
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => c.ProductId == _productId && (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                    else
                    {
                        csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").Where(c => (c.RegisterDate >= _registerFrom && c.RegisterDate <= _registerTo && (c.FinishDate >= _finishedFrom && c.FinishDate <= _finishedTo || !c.FinishDate.HasValue))).OrderByDescending(c => c.RegisterDate);
                    }
                }
            }

            if (_companiesList != null && _companiesList.Count != 0)
            {
                csr = csr.Where(c => _companiesList.Contains(c.CompanyId));
            }


            if (_statuses != null && _statuses.Count != 0)
            {

                csr = csr.Where(c => _statuses.Contains(c.StatusId));

            }


            if (_onlyMine)
            {
                //Ifuser is customer then look at UserId
                //if(_customer)
                //   csr = csr.Where(c => c.UserId == _userId);
                //else
                csr = csr.Where(c => (c.LastUserId == _userId || c.UserId == _userId || c.CallerId == _userId));
            }

            if (_text != null && _text != String.Empty)
            {
                csr = csr.Where(c => c.Description.Contains(_text));
            }

            return csr;
        }


        public IQueryable<Csr> GetCsrsForAllCompanies(List<Int32> _companiesList)
        {
            IQueryable<Csr> csr;


            csr = this.ObjectContext.Csr.Include("Company").Include("CommunicationChannel").Include("Csr_Status").Include("Priority").Include("User").Include("Product").Include("Site").Include("User1").Include("User2").Include("RequesterType").Include("IssueDomain").OrderByDescending(c => c.RegisterDate);


            if (_companiesList != null && _companiesList.Count != 0)
            {
                csr = csr.Where(c => _companiesList.Contains(c.CompanyId));

            }

            return csr;
        }



        public void InsertCsr(Csr csr)
        {
            if ((csr.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(csr, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Csr.AddObject(csr);
            }
        }

        public void UpdateCsr(Csr currentCsr)
        {
            this.ObjectContext.Csr.AttachAsModified(currentCsr, this.ChangeSet.GetOriginal(currentCsr));
        }

        public void DeleteCsr(Csr csr)
        {
            if ((csr.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Csr.Attach(csr);
            }
            this.ObjectContext.Csr.DeleteObject(csr);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Csr_Status' query.
        public IQueryable<Csr_Status> GetCsr_Status()
        {
            return this.ObjectContext.Csr_Status;
        }

        public void InsertCsr_Status(Csr_Status csr_Status)
        {
            if ((csr_Status.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(csr_Status, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Csr_Status.AddObject(csr_Status);
            }
        }

        public void UpdateCsr_Status(Csr_Status currentCsr_Status)
        {
            this.ObjectContext.Csr_Status.AttachAsModified(currentCsr_Status, this.ChangeSet.GetOriginal(currentCsr_Status));
        }

        public void DeleteCsr_Status(Csr_Status csr_Status)
        {
            if ((csr_Status.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Csr_Status.Attach(csr_Status);
            }
            this.ObjectContext.Csr_Status.DeleteObject(csr_Status);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Frequency' query.
        public IQueryable<Frequency> GetFrequency()
        {
            return this.ObjectContext.Frequency;
        }

        public void InsertFrequency(Frequency frequency)
        {
            if ((frequency.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(frequency, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Frequency.AddObject(frequency);
            }
        }

        public void UpdateFrequency(Frequency currentFrequency)
        {
            this.ObjectContext.Frequency.AttachAsModified(currentFrequency, this.ChangeSet.GetOriginal(currentFrequency));
        }

        public void DeleteFrequency(Frequency frequency)
        {
            if ((frequency.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Frequency.Attach(frequency);
            }
            this.ObjectContext.Frequency.DeleteObject(frequency);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'KB_Notes' query.

        public IQueryable<KB_Notes> GetKB_NotesAll()
        {
            return this.ObjectContext.KB_Notes.Include("Product").Include("User").OrderByDescending(c => c.Date);
        }

        public IQueryable<KB_Notes> GetKB_Notes(Int32 _csrId)
        {
            return this.ObjectContext.KB_Notes.Include("Product").Include("User").Where(c => c.CsrId == _csrId).OrderByDescending(c => c.Date); ;
        }

        public IQueryable<KB_Notes> GetKB_NotesWithParams(String _kbId, Int32 _productId, String _text)
        {
            IQueryable<KB_Notes> kbNotes;

            kbNotes = this.ObjectContext.KB_Notes.Include("Product").Include("User");

            if (_kbId != "")
            {
                Int32 kbId = Convert.ToInt32(_kbId);

                kbNotes = kbNotes.Where(c => c.Id == kbId);
            }

            if (_productId != 0)
            {
                kbNotes = kbNotes.Where(c => c.ProductId == _productId);
            }

            if (_text != "")
            {
                kbNotes = kbNotes.Where(c => c.Note.Contains(_text));
            }


            return kbNotes.OrderByDescending(c => c.Date);
        }

        public void InsertKB_Notes(KB_Notes kB_Notes)
        {
            if ((kB_Notes.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(kB_Notes, EntityState.Added);
            }
            else
            {
                this.ObjectContext.KB_Notes.AddObject(kB_Notes);
            }
        }

        public void UpdateKB_Notes(KB_Notes currentKB_Notes)
        {
            this.ObjectContext.KB_Notes.AttachAsModified(currentKB_Notes, this.ChangeSet.GetOriginal(currentKB_Notes));
        }

        public void DeleteKB_Notes(KB_Notes kB_Notes)
        {
            if ((kB_Notes.EntityState == EntityState.Detached))
            {
                this.ObjectContext.KB_Notes.Attach(kB_Notes);
            }
            this.ObjectContext.KB_Notes.DeleteObject(kB_Notes);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Localization' query.
        public IQueryable<Localization> GetLocalization()
        {
            return this.ObjectContext.Localization;
        }

        public void InsertLocalization(Localization localization)
        {
            if ((localization.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(localization, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Localization.AddObject(localization);
            }
        }

        public void UpdateLocalization(Localization currentLocalization)
        {
            this.ObjectContext.Localization.AttachAsModified(currentLocalization, this.ChangeSet.GetOriginal(currentLocalization));
        }

        public void DeleteLocalization(Localization localization)
        {
            if ((localization.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(localization, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Localization.Attach(localization);
                this.ObjectContext.Localization.DeleteObject(localization);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Notes' query.
        public IQueryable<Notes> GetNotes(Int32 _csrId)
        {
            return this.ObjectContext.Notes.Include("User").Where(c => c.CsrId == _csrId);
        }

        public void InsertNotes(Notes notes)
        {
            if ((notes.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(notes, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Notes.AddObject(notes);
            }
        }

        public void UpdateNotes(Notes currentNotes)
        {
            this.ObjectContext.Notes.AttachAsModified(currentNotes, this.ChangeSet.GetOriginal(currentNotes));
        }

        public void DeleteNotes(Notes notes)
        {
            if ((notes.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Notes.Attach(notes);
            }
            this.ObjectContext.Notes.DeleteObject(notes);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Priority' query.
        public IQueryable<Priority> GetPriority()
        {
            return this.ObjectContext.Priority;
        }

        public void InsertPriority(Priority priority)
        {
            if ((priority.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(priority, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Priority.AddObject(priority);
            }
        }

        public void UpdatePriority(Priority currentPriority)
        {
            this.ObjectContext.Priority.AttachAsModified(currentPriority, this.ChangeSet.GetOriginal(currentPriority));
        }

        public void DeletePriority(Priority priority)
        {
            if ((priority.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Priority.Attach(priority);
            }
            this.ObjectContext.Priority.DeleteObject(priority);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Product' query.
        public IQueryable<Product> GetProduct()
        {
            return this.ObjectContext.Product.OrderBy(c => c.ProductName);
        }

        public void InsertProduct(Product product)
        {
            if ((product.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(product, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Product.AddObject(product);
            }
        }

        public void UpdateProduct(Product currentProduct)
        {
            this.ObjectContext.Product.AttachAsModified(currentProduct, this.ChangeSet.GetOriginal(currentProduct));
        }

        public void DeleteProduct(Product product)
        {
            if ((product.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Product.Attach(product);
            }
            this.ObjectContext.Product.DeleteObject(product);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ProductsInCompany' query.
        public IQueryable<ProductsInCompany> GetProductsInCompany(Int32 _companyId)
        {
            return this.ObjectContext.ProductsInCompany.Include("Product").Where(c => c.CompanyId == _companyId).OrderBy(c => c.Product.ProductName);
        }

        public IQueryable<ProductsInCompany> GetProductsForAllCompanies(List<Int32> _companiesList)
        {
            return this.ObjectContext.ProductsInCompany.Include("Product").Where(c => _companiesList.Contains(c.CompanyId));
        }

        public void InsertProductsInCompany(ProductsInCompany productsInCompany)
        {
            if ((productsInCompany.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(productsInCompany, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ProductsInCompany.AddObject(productsInCompany);
            }
        }

        public void UpdateProductsInCompany(ProductsInCompany currentProductsInCompany)
        {
            this.ObjectContext.ProductsInCompany.AttachAsModified(currentProductsInCompany, this.ChangeSet.GetOriginal(currentProductsInCompany));
        }

        public void DeleteProductsInCompany(ProductsInCompany productsInCompany)
        {
            if ((productsInCompany.EntityState == EntityState.Detached))
            {
                this.ObjectContext.ProductsInCompany.Attach(productsInCompany);
            }
            this.ObjectContext.ProductsInCompany.DeleteObject(productsInCompany);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Severity' query.
        public IQueryable<Severity> GetSeverity()
        {
            return this.ObjectContext.Severity;
        }

        public void InsertSeverity(Severity severity)
        {
            if ((severity.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(severity, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Severity.AddObject(severity);
            }
        }

        public void UpdateSeverity(Severity currentSeverity)
        {
            this.ObjectContext.Severity.AttachAsModified(currentSeverity, this.ChangeSet.GetOriginal(currentSeverity));
        }

        public void DeleteSeverity(Severity severity)
        {
            if ((severity.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Severity.Attach(severity);
            }
            this.ObjectContext.Severity.DeleteObject(severity);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Site' query.
        public IQueryable<Site> GetSite()
        {
            return this.ObjectContext.Site.Include("Company");
        }

        public IQueryable<Site> GetSitesForCompany(Int32 _companyId)
        {
            return this.ObjectContext.Site.Where(c => c.CompanyId == _companyId);
        }

        public void InsertSite(Site site)
        {
            if ((site.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(site, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Site.AddObject(site);
            }
        }

        public void UpdateSite(Site currentSite)
        {
            this.ObjectContext.Site.AttachAsModified(currentSite, this.ChangeSet.GetOriginal(currentSite));
        }

        public void DeleteSite(Site site)
        {
            if ((site.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Site.Attach(site);
            }
            this.ObjectContext.Site.DeleteObject(site);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'User' query.
        public IQueryable<User> GetUserWithUsername(String _userName)
        {
            return this.ObjectContext.User.Include("UserType").Include("Contact").Include("Company").Include("Localization").Where(c => c.UserName.Equals(_userName));
        }

        //[OutputCache(OutputCacheLocation.Any)]
        public IQueryable<User> GetUser()
        {
            return this.ObjectContext.User.Include("UserType").Include("Contact").Include("Company").OrderBy(c => c.FirstName);
        }

        public IQueryable<User> GetUsersForCompany(Int32 _companyId)
        {
            return this.ObjectContext.User.Where(c => c.CompanyId == _companyId).OrderBy(c => c.FirstName);
        }

        public IQueryable<User> GetUserWithId(Int32 _userId)
        {
            return this.ObjectContext.User.Include("Contact").Where(c => c.Id == _userId);
        }


        public IQueryable<User> GetUserWithParams(String _id, String _firstName, String _lastName, Int32 _companyId)
        {
            IQueryable<User> users;

            users = users = this.ObjectContext.User.Include("UserType").Include("Contact").Include("Company").OrderBy(c => c.FirstName);

            if (_id != "")
            {
                Int32 id = Convert.ToInt32(_id);

                users = users.Where(c => c.Id == id);
            }

            if (_firstName != "")
            {
                users = users.Where(c => c.FirstName == _firstName);
            }

            if (_lastName != "")
            {
                users = users.Where(c => c.LastName == _lastName);
            }

            if (_companyId != 0)
            {
                users = users.Where(c => c.CompanyId == _companyId);
            }

            return users;
        }

        public void InsertUser(User user)
        {
            if ((user.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Added);
            }
            else
            {
                this.ObjectContext.User.AddObject(user);
            }
        }

        public void UpdateUser(User currentUser)
        {
            this.ObjectContext.User.AttachAsModified(currentUser, this.ChangeSet.GetOriginal(currentUser));
        }

        public void DeleteUser(User user)
        {
            if ((user.EntityState == EntityState.Detached))
            {
                this.ObjectContext.User.Attach(user);
            }
            this.ObjectContext.User.DeleteObject(user);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UserLogging' query.
        public IQueryable<UserLogging> GetUserLogging()
        {
            return this.ObjectContext.UserLogging;
        }

        public IQueryable<UserLogging> GetUserLoggingForUser(Int32 _userId)
        {
            return this.ObjectContext.UserLogging.Where(c => c.UserId == _userId);
        }

        public void InsertUserLogging(UserLogging userLogging)
        {
            if ((userLogging.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userLogging, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UserLogging.AddObject(userLogging);
            }
        }

        public void UpdateUserLogging(UserLogging currentUserLogging)
        {
            this.ObjectContext.UserLogging.AttachAsModified(currentUserLogging, this.ChangeSet.GetOriginal(currentUserLogging));
        }

        public void DeleteUserLogging(UserLogging userLogging)
        {
            if ((userLogging.EntityState == EntityState.Detached))
            {
                this.ObjectContext.UserLogging.Attach(userLogging);
            }
            this.ObjectContext.UserLogging.DeleteObject(userLogging);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UserNotifications' query.
        public IQueryable<UserNotifications> GetUserNotifications(Int32 _companyId)
        {
            return this.ObjectContext.UserNotifications.Include("User").Where(c => c.CompanyId == _companyId);

        }


        public void InsertUserNotifications(UserNotifications userNotifications)
        {
            if ((userNotifications.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userNotifications, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UserNotifications.AddObject(userNotifications);
            }
        }

        public void UpdateUserNotifications(UserNotifications currentUserNotifications)
        {
            this.ObjectContext.UserNotifications.AttachAsModified(currentUserNotifications, this.ChangeSet.GetOriginal(currentUserNotifications));
        }

        public void DeleteUserNotifications(UserNotifications userNotifications)
        {
            if ((userNotifications.EntityState == EntityState.Detached))
            {
                this.ObjectContext.UserNotifications.Attach(userNotifications);
            }
            this.ObjectContext.UserNotifications.DeleteObject(userNotifications);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UserType' query.
        public IQueryable<UserType> GetUserType()
        {
            return this.ObjectContext.UserType;
        }

        public void InsertUserType(UserType userType)
        {
            if ((userType.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userType, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UserType.AddObject(userType);
            }
        }

        public void UpdateUserType(UserType currentUserType)
        {
            this.ObjectContext.UserType.AttachAsModified(currentUserType, this.ChangeSet.GetOriginal(currentUserType));
        }

        public void DeleteUserType(UserType userType)
        {
            if ((userType.EntityState == EntityState.Detached))
            {
                this.ObjectContext.UserType.Attach(userType);
            }
            this.ObjectContext.UserType.DeleteObject(userType);
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'IssueDomain' query.
        public IQueryable<IssueDomain> GetIssueDomain()
        {
            return this.ObjectContext.IssueDomain;
        }

        public void InsertIssueDomain(IssueDomain issueDomain)
        {
            if ((issueDomain.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(issueDomain, EntityState.Added);
            }
            else
            {
                this.ObjectContext.IssueDomain.AddObject(issueDomain);
            }
        }

        public void UpdateIssueDomain(IssueDomain currentIssueDomain)
        {
            this.ObjectContext.IssueDomain.AttachAsModified(currentIssueDomain, this.ChangeSet.GetOriginal(currentIssueDomain));
        }

        public void DeleteIssueDomain(IssueDomain issueDomain)
        {
            if ((issueDomain.EntityState == EntityState.Detached))
            {
                this.ObjectContext.IssueDomain.Attach(issueDomain);
            }
            this.ObjectContext.IssueDomain.DeleteObject(issueDomain);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'IssueDomainInCompany' query.
        public IQueryable<IssueDomainInCompany> GetIssueDomainInCompany(Int32 _companyId)
        {
            return this.ObjectContext.IssueDomainInCompany.Include("IssueDomain").Where(c => c.CompanyId == _companyId);
        }

        public void InsertIssueDomainInCompany(IssueDomainInCompany issueDomainInCompany)
        {
            if ((issueDomainInCompany.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(issueDomainInCompany, EntityState.Added);
            }
            else
            {
                this.ObjectContext.IssueDomainInCompany.AddObject(issueDomainInCompany);
            }
        }

        public void UpdateIssueDomainInCompany(IssueDomainInCompany currentIssueDomainInCompany)
        {
            this.ObjectContext.IssueDomainInCompany.AttachAsModified(currentIssueDomainInCompany, this.ChangeSet.GetOriginal(currentIssueDomainInCompany));
        }

        public void DeleteIssueDomainInCompany(IssueDomainInCompany issueDomainInCompany)
        {
            if ((issueDomainInCompany.EntityState == EntityState.Detached))
            {
                this.ObjectContext.IssueDomainInCompany.Attach(issueDomainInCompany);
            }
            this.ObjectContext.IssueDomainInCompany.DeleteObject(issueDomainInCompany);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'RequesterType' query.
        public IQueryable<RequesterType> GetRequesterType()
        {
            return this.ObjectContext.RequesterType;
        }

        public void InsertRequesterType(RequesterType requesterType)
        {
            if ((requesterType.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(requesterType, EntityState.Added);
            }
            else
            {
                this.ObjectContext.RequesterType.AddObject(requesterType);
            }
        }

        public void UpdateRequesterType(RequesterType currentRequesterType)
        {
            this.ObjectContext.RequesterType.AttachAsModified(currentRequesterType, this.ChangeSet.GetOriginal(currentRequesterType));
        }

        public void DeleteRequesterType(RequesterType requesterType)
        {
            if ((requesterType.EntityState == EntityState.Detached))
            {
                this.ObjectContext.RequesterType.Attach(requesterType);
            }
            this.ObjectContext.RequesterType.DeleteObject(requesterType);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'RequesterTypeInCompany' query.
        /*public IQueryable<RequesterTypeInCompany> GetRequesterTypeInCompany()
        {
            return this.ObjectContext.RequesterTypeInCompany;
        }*/

        public IQueryable<RequesterTypeInCompany> GetRequesterTypeInCompany(Int32 _companyId)
        {
            return this.ObjectContext.RequesterTypeInCompany.Include("RequesterType").Where(c => c.CompanyId == _companyId);

        }

        public void InsertRequesterTypeInCompany(RequesterTypeInCompany requesterTypeInCompany)
        {
            if ((requesterTypeInCompany.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(requesterTypeInCompany, EntityState.Added);
            }
            else
            {
                this.ObjectContext.RequesterTypeInCompany.AddObject(requesterTypeInCompany);
            }
        }

        public void UpdateRequesterTypeInCompany(RequesterTypeInCompany currentRequesterTypeInCompany)
        {
            this.ObjectContext.RequesterTypeInCompany.AttachAsModified(currentRequesterTypeInCompany, this.ChangeSet.GetOriginal(currentRequesterTypeInCompany));
        }

        public void DeleteRequesterTypeInCompany(RequesterTypeInCompany requesterTypeInCompany)
        {
            if ((requesterTypeInCompany.EntityState == EntityState.Detached))
            {
                this.ObjectContext.RequesterTypeInCompany.Attach(requesterTypeInCompany);
            }
            this.ObjectContext.RequesterTypeInCompany.DeleteObject(requesterTypeInCompany);
        }
    }
}


