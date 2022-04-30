
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


    // Implements application logic using the CSR_NotyfiEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    [RequiresAuthentication]
    [EnableClientAccess()]
    public class SolidusNotifyDomainService : LinqToEntitiesDomainService<CSR_NotyfiEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CSR_list' query.
        public IQueryable<CSR_list> GetCSR_list()
        {
            return this.ObjectContext.CSR_list;
        }

        public void InsertCSR_list(CSR_list cSR_list)
        {
            if ((cSR_list.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(cSR_list, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CSR_list.AddObject(cSR_list);
            }
        }

        public void UpdateCSR_list(CSR_list currentCSR_list)
        {
            this.ObjectContext.CSR_list.AttachAsModified(currentCSR_list, this.ChangeSet.GetOriginal(currentCSR_list));
        }

        public void DeleteCSR_list(CSR_list cSR_list)
        {
            if ((cSR_list.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(cSR_list, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.CSR_list.Attach(cSR_list);
                this.ObjectContext.CSR_list.DeleteObject(cSR_list);
            }
        }
    }
}


