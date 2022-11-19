using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Services.Protocols;
using Neambc.Neamb.Foundation.MBCData.org.neambc.neamb.memberdata;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
    public interface IMemberDataService : IDisposable {
        SoapProtocolVersion SoapVersion { get; set; }
        bool AllowAutoRedirect { get; set; }
        CookieContainer CookieContainer { get; set; }
        X509CertificateCollection ClientCertificates { get; }
        bool EnableDecompression { get; set; }
        string UserAgent { get; set; }
        IWebProxy Proxy { get; set; }
        bool UnsafeAuthenticatedConnectionSharing { get; set; }
        ICredentials Credentials { get; set; }
        bool UseDefaultCredentials { get; set; }
        string ConnectionGroupName { get; set; }
        bool PreAuthenticate { get; set; }
        string Url { get; set; }
        Encoding RequestEncoding { get; set; }
        int Timeout { get; set; }
        ISite Site { get; set; }
        IContainer Container { get; }
        /// <remarks/>
        event currentMembershipFlagCompletedEventHandler currentMembershipFlagCompleted;
        /// <remarks/>
        event newMemberCheckCompletedEventHandler newMemberCheckCompleted;
        /// <remarks/>
        event mdsid2iaidCompletedEventHandler mdsid2iaidCompleted;
        /// <remarks/>
        event checkSiteRegistrationCompletedEventHandler checkSiteRegistrationCompleted;
        /// <remarks/>
        event matchOnlyUserCompletedEventHandler matchOnlyUserCompleted;
        /// <remarks/>
        event matchUserCompletedEventHandler matchUserCompleted;
        /// <remarks/>
        event mdsid2userdataCompletedEventHandler mdsid2userdataCompleted;
        /// <remarks/>
        event mdsid2mdsuserdataCompletedEventHandler mdsid2mdsuserdataCompleted;
        /// <remarks/>
        event iaid2mdsidCompletedEventHandler iaid2mdsidCompleted;
        /// <remarks/>
        string currentMembershipFlag([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string mdsid, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void currentMembershipFlagAsync(string mdsid, string partner, string key);
        /// <remarks/>
        void currentMembershipFlagAsync(string mdsid, string partner, string key, object userState);
        /// <remarks/>
        newMemberObject newMemberCheck([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string mdsid, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void newMemberCheckAsync(string mdsid, string partner, string key);
        /// <remarks/>
        void newMemberCheckAsync(string mdsid, string partner, string key, object userState);
        /// <remarks/>
        string mdsid2iaid([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string mdsid, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void mdsid2iaidAsync(string mdsid, string partner, string key);
        /// <remarks/>
        void mdsid2iaidAsync(string mdsid, string partner, string key, object userState);
        /// <remarks/>
        checkSiteRegistrationObject checkSiteRegistration([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string email, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void checkSiteRegistrationAsync(string email, string partner, string key);
        /// <remarks/>
        void checkSiteRegistrationAsync(string email, string partner, string key, object userState);
        /// <remarks/>
        matchUserObject matchOnlyUser([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string firstName, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string lastName, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string streetAddress, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string city, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string stateCode, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string zipCode, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string email, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string dob, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string phone, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void matchOnlyUserAsync(string firstName, string lastName, string streetAddress, string city, string stateCode, string zipCode, string email, string dob, string phone, string partner, string key);
        /// <remarks/>
        void matchOnlyUserAsync(string firstName, string lastName, string streetAddress, string city, string stateCode, string zipCode, string email, string dob, string phone, string partner, string key, object userState);
        /// <remarks/>
        matchUserObject matchUser([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string firstName, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string lastName, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string streetAddress, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string city, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string stateCode, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string zipCode, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string email, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string dob, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string phone, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void matchUserAsync(string firstName, string lastName, string streetAddress, string city, string stateCode, string zipCode, string email, string dob, string phone, string partner, string key);
        /// <remarks/>
        void matchUserAsync(string firstName, string lastName, string streetAddress, string city, string stateCode, string zipCode, string email, string dob, string phone, string partner, string key, object userState);
        /// <remarks/>
        userDataObject mdsid2userdata([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string mdsid, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void mdsid2userdataAsync(string mdsid, string partner, string key);
        /// <remarks/>
        void mdsid2userdataAsync(string mdsid, string partner, string key, object userState);
        /// <remarks/>
        mdsUserDataObject mdsid2mdsuserdata([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string mdsid, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void mdsid2mdsuserdataAsync(string mdsid, string partner, string key);
        /// <remarks/>
        void mdsid2mdsuserdataAsync(string mdsid, string partner, string key, object userState);
        /// <remarks/>
        string iaid2mdsid([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string iaid, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string partner, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key);
        /// <remarks/>
        void iaid2mdsidAsync(string iaid, string partner, string key);
        /// <remarks/>
        void iaid2mdsidAsync(string iaid, string partner, string key, object userState);
        /// <remarks/>
        void CancelAsync(object userState);
        void Discover();
        void Abort();
        string ToString();
        event EventHandler Disposed;
        object GetLifetimeService();
        object InitializeLifetimeService();
        ObjRef CreateObjRef(Type requestedType);
    }
}