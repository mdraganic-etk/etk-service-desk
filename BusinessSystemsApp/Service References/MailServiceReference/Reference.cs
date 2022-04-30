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
namespace BusinessSystemsApp.MailServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="MailServiceReference.MailService")]
    public interface MailService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:MailService/SendMail", ReplyAction="urn:MailService/SendMailResponse")]
        System.IAsyncResult BeginSendMail(System.Collections.ObjectModel.ObservableCollection<string> emailTo, string emailFrom, string msgSubject, string msgBody, System.AsyncCallback callback, object asyncState);
        
        bool EndSendMail(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MailServiceChannel : BusinessSystemsApp.MailServiceReference.MailService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendMailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SendMailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MailServiceClient : System.ServiceModel.ClientBase<BusinessSystemsApp.MailServiceReference.MailService>, BusinessSystemsApp.MailServiceReference.MailService {
        
        private BeginOperationDelegate onBeginSendMailDelegate;
        
        private EndOperationDelegate onEndSendMailDelegate;
        
        private System.Threading.SendOrPostCallback onSendMailCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public MailServiceClient() {
        }
        
        public MailServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MailServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<SendMailCompletedEventArgs> SendMailCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult BusinessSystemsApp.MailServiceReference.MailService.BeginSendMail(System.Collections.ObjectModel.ObservableCollection<string> emailTo, string emailFrom, string msgSubject, string msgBody, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSendMail(emailTo, emailFrom, msgSubject, msgBody, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        bool BusinessSystemsApp.MailServiceReference.MailService.EndSendMail(System.IAsyncResult result) {
            return base.Channel.EndSendMail(result);
        }
        
        private System.IAsyncResult OnBeginSendMail(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Collections.ObjectModel.ObservableCollection<string> emailTo = ((System.Collections.ObjectModel.ObservableCollection<string>)(inValues[0]));
            string emailFrom = ((string)(inValues[1]));
            string msgSubject = ((string)(inValues[2]));
            string msgBody = ((string)(inValues[3]));
            return ((BusinessSystemsApp.MailServiceReference.MailService)(this)).BeginSendMail(emailTo, emailFrom, msgSubject, msgBody, callback, asyncState);
        }
        
        private object[] OnEndSendMail(System.IAsyncResult result) {
            bool retVal = ((BusinessSystemsApp.MailServiceReference.MailService)(this)).EndSendMail(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSendMailCompleted(object state) {
            if ((this.SendMailCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SendMailCompleted(this, new SendMailCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SendMailAsync(System.Collections.ObjectModel.ObservableCollection<string> emailTo, string emailFrom, string msgSubject, string msgBody) {
            this.SendMailAsync(emailTo, emailFrom, msgSubject, msgBody, null);
        }
        
        public void SendMailAsync(System.Collections.ObjectModel.ObservableCollection<string> emailTo, string emailFrom, string msgSubject, string msgBody, object userState) {
            if ((this.onBeginSendMailDelegate == null)) {
                this.onBeginSendMailDelegate = new BeginOperationDelegate(this.OnBeginSendMail);
            }
            if ((this.onEndSendMailDelegate == null)) {
                this.onEndSendMailDelegate = new EndOperationDelegate(this.OnEndSendMail);
            }
            if ((this.onSendMailCompletedDelegate == null)) {
                this.onSendMailCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSendMailCompleted);
            }
            base.InvokeAsync(this.onBeginSendMailDelegate, new object[] {
                        emailTo,
                        emailFrom,
                        msgSubject,
                        msgBody}, this.onEndSendMailDelegate, this.onSendMailCompletedDelegate, userState);
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
        
        protected override BusinessSystemsApp.MailServiceReference.MailService CreateChannel() {
            return new MailServiceClientChannel(this);
        }
        
        private class MailServiceClientChannel : ChannelBase<BusinessSystemsApp.MailServiceReference.MailService>, BusinessSystemsApp.MailServiceReference.MailService {
            
            public MailServiceClientChannel(System.ServiceModel.ClientBase<BusinessSystemsApp.MailServiceReference.MailService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginSendMail(System.Collections.ObjectModel.ObservableCollection<string> emailTo, string emailFrom, string msgSubject, string msgBody, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = emailTo;
                _args[1] = emailFrom;
                _args[2] = msgSubject;
                _args[3] = msgBody;
                System.IAsyncResult _result = base.BeginInvoke("SendMail", _args, callback, asyncState);
                return _result;
            }
            
            public bool EndSendMail(System.IAsyncResult result) {
                object[] _args = new object[0];
                bool _result = ((bool)(base.EndInvoke("SendMail", _args, result)));
                return _result;
            }
        }
    }
}