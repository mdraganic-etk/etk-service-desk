﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace BusinessSystemsApp.DateTimeServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="DateTimeServiceReference.DateTimeService")]
    public interface DateTimeService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:DateTimeService/GetDateTime", ReplyAction="urn:DateTimeService/GetDateTimeResponse")]
        System.IAsyncResult BeginGetDateTime(System.AsyncCallback callback, object asyncState);
        
        System.DateTime EndGetDateTime(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DateTimeServiceChannel : BusinessSystemsApp.DateTimeServiceReference.DateTimeService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetDateTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetDateTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.DateTime Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.DateTime)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DateTimeServiceClient : System.ServiceModel.ClientBase<BusinessSystemsApp.DateTimeServiceReference.DateTimeService>, BusinessSystemsApp.DateTimeServiceReference.DateTimeService {
        
        private BeginOperationDelegate onBeginGetDateTimeDelegate;
        
        private EndOperationDelegate onEndGetDateTimeDelegate;
        
        private System.Threading.SendOrPostCallback onGetDateTimeCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public DateTimeServiceClient() {
        }
        
        public DateTimeServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DateTimeServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DateTimeServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DateTimeServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetDateTimeCompletedEventArgs> GetDateTimeCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult BusinessSystemsApp.DateTimeServiceReference.DateTimeService.BeginGetDateTime(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetDateTime(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.DateTime BusinessSystemsApp.DateTimeServiceReference.DateTimeService.EndGetDateTime(System.IAsyncResult result) {
            return base.Channel.EndGetDateTime(result);
        }
        
        private System.IAsyncResult OnBeginGetDateTime(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((BusinessSystemsApp.DateTimeServiceReference.DateTimeService)(this)).BeginGetDateTime(callback, asyncState);
        }
        
        private object[] OnEndGetDateTime(System.IAsyncResult result) {
            System.DateTime retVal = ((BusinessSystemsApp.DateTimeServiceReference.DateTimeService)(this)).EndGetDateTime(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetDateTimeCompleted(object state) {
            if ((this.GetDateTimeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetDateTimeCompleted(this, new GetDateTimeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetDateTimeAsync() {
            this.GetDateTimeAsync(null);
        }
        
        public void GetDateTimeAsync(object userState) {
            if ((this.onBeginGetDateTimeDelegate == null)) {
                this.onBeginGetDateTimeDelegate = new BeginOperationDelegate(this.OnBeginGetDateTime);
            }
            if ((this.onEndGetDateTimeDelegate == null)) {
                this.onEndGetDateTimeDelegate = new EndOperationDelegate(this.OnEndGetDateTime);
            }
            if ((this.onGetDateTimeCompletedDelegate == null)) {
                this.onGetDateTimeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetDateTimeCompleted);
            }
            base.InvokeAsync(this.onBeginGetDateTimeDelegate, null, this.onEndGetDateTimeDelegate, this.onGetDateTimeCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override BusinessSystemsApp.DateTimeServiceReference.DateTimeService CreateChannel() {
            return new DateTimeServiceClientChannel(this);
        }
        
        private class DateTimeServiceClientChannel : ChannelBase<BusinessSystemsApp.DateTimeServiceReference.DateTimeService>, BusinessSystemsApp.DateTimeServiceReference.DateTimeService {
            
            public DateTimeServiceClientChannel(System.ServiceModel.ClientBase<BusinessSystemsApp.DateTimeServiceReference.DateTimeService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetDateTime(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetDateTime", _args, callback, asyncState);
                return _result;
            }
            
            public System.DateTime EndGetDateTime(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.DateTime _result = ((System.DateTime)(base.EndInvoke("GetDateTime", _args, result)));
                return _result;
            }
        }
    }
}
