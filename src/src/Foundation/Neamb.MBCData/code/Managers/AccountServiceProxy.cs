using System;
using System.Net;
using System.Reflection;
using AutoMapper;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.org.neambc.neamb.accountmng;
using Neambc.Neamb.Foundation.MBCData.org.neambc.neamb.compintrolife;
using Neambc.Neamb.Foundation.MBCData.org.neambc.neamb.encryptmercer;
using Neambc.Seiumb.Foundation.Sitecore;
using compIntroObject = Neambc.Neamb.Foundation.MBCData.org.neambc.neamb.compintrolife.compIntroObject;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{

    /// <summary>
    /// Calls IAccountService methods and maps to local types
    /// </summary>
    [Service(typeof(IAccountServiceProxy))]
    public class AccountServiceProxy : IAccountServiceProxy
    {

        #region Fields
        private readonly IGlobalConfigurationManager _config;
        private readonly int _defaultTimeoutMs;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        private readonly IResourcesService _resources;
       
        #endregion

        #region Constructors
        public AccountServiceProxy(
            IGlobalConfigurationManager globalConfigurationManager,
            IResourcesService resources,
            ILog log
        )
        {
            _config = globalConfigurationManager ?? throw new ArgumentNullException(nameof(globalConfigurationManager));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _mapper = BuildMapper();
            _defaultTimeoutMs = _config.ServiceTimeout * 1000;
          
        }
        #endregion

        #region Private Methods
        private static IMapper BuildMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<LoginUserObject, LoginUserDto>();
                cfg.CreateMap<ValidateResetTokenObject, ValidateResetTokenDto>();
            });
            return config.CreateMapper();
        }
        #endregion

        #region Protected Methods
        protected virtual string ExtractEnrollmentQueryXml(string mdsId, Assembly asm = null)
        {
            string ret = null;
            var payload = _resources.ReadTextResourceFromAssembly(
                "Neambc.Neamb.Foundation.MBCData.Managers.EnrollmentQuery.xml"
                , asm ?? GetType().Assembly);
            if (null != payload)
            {
                ret = payload.Replace("{mdsid}", mdsId).Replace("\n", string.Empty);
            }
            return ret;
        }
        protected virtual string ExtractEnrollmentUpdateXml(
            string address,
            string city,
            string statecode,
            string zipcode,
            string loginuserid,
            string firstname,
            string lastname,
            string birthdate,
            string mdsIndvId,
            Assembly asm = null
        )
        {
            string ret = null;
            var payload = _resources.ReadTextResourceFromAssembly(
                "Neambc.Neamb.Foundation.MBCData.Managers.EnrollmentUpdate.xml"
                , asm ?? GetType().Assembly);
            if (null != payload)
            {
                var birthdateStr = $"{birthdate.Substring(4, 4)}-{birthdate.Substring(0, 2)}-{birthdate.Substring(2, 2)}";
                ret = payload
                    .Replace("{address}", address)
                    .Replace("{city}", city)
                    .Replace("{statecode}", statecode)
                    .Replace("{zipcode}", zipcode)
                    .Replace("{loginuserid}", loginuserid)
                    .Replace("{firstname}", firstname)
                    .Replace("{lastname}", lastname)
                    .Replace("{birthdate}", birthdateStr)
                    .Replace("{mdsIndvId}", mdsIndvId)
                    .Replace("\n", string.Empty);
            }
            return ret;
        }
        #endregion

        #region Public Methods

        public string ExecuteEnrollmentQuery(string mdsid)
        {
            using (var client = new EnrollmentService.EnrollmentSoapClient())
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, _config.ServiceTimeout);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                       | SecurityProtocolType.Tls12
                                                       | SecurityProtocolType.Tls11
                                                       | SecurityProtocolType.Tls;
                return client.Query(
                    _config.EnrollmentServiceUser,
                    _config.EnrollmentServicePassword,
                    ExtractEnrollmentQueryXml(mdsid)
                );
            }
        }

        public Guid ExecuteEnrollmentGetLogin(Guid uniqueId)
        {
            using (var client = new EnrollmentService.EnrollmentSoapClient())
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, _config.ServiceTimeout);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                       | SecurityProtocolType.Tls12
                                                       | SecurityProtocolType.Tls11
                                                       | SecurityProtocolType.Tls;
                var response = client.GetLoginGUID(_config.EnrollmentServiceUser,
                    _config.EnrollmentServicePassword,
                    new Guid("D05B0402-88C2-4327-9704-2E2040526E0E"), uniqueId);
                return response;
            }
        }

        public string ExecuteUpdateEnrollment(
            string address,
            string city,
            string statecode,
            string zipcode,
            string loginuserid,
            string firstname,
            string lastname,
            string birthdate,
            string mdsIndvId
        )
        {
            using (var client = new EnrollmentService.EnrollmentSoapClient())
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, _config.ServiceTimeout);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                       | SecurityProtocolType.Tls12
                                                       | SecurityProtocolType.Tls11
                                                       | SecurityProtocolType.Tls;
                var payload = ExtractEnrollmentUpdateXml(
                    address,
                    city,
                    statecode,
                    zipcode,
                    loginuserid,
                    firstname,
                    lastname,
                    birthdate,
                    mdsIndvId
                );
                //var birthdateStr = $"{birthdate.Substring(4, 4)}-{birthdate.Substring(0, 2)}-{birthdate.Substring(2, 2)}";
                //var valueString = string.Format(
                //	"<Transmittal xmlns:xsi={0} xmlns:xsd={1} SenderID={2} Type={3}><Group><GroupName>NEA Member Benefits</GroupName></Group><Applicants><Applicant  ID={4} ForceChangePIN={5}> <AsOfDate>2015-01-01T21:24:40</AsOfDate> <Address Type={6}> <Line1>{7}</Line1> <City>{8}</City> <State>{9}</State> <Zip>{10}</Zip></Address> <Email>{11}</Email> <FirstName>{12}</FirstName> <LastName>{13}</LastName> <Sex>Female</Sex> <PersonalEmail>{14}</PersonalEmail> <Employment Status={15}> <HireDate>2015-01-01T00:00:00</HireDate> <Department>NEA</Department> <Location>NEA</Location> <JobClass>NEA</JobClass> <PayGroup>NEA</PayGroup> <PayrollFrequency>26</PayrollFrequency> <DeductionFrequency>26</DeductionFrequency> <Salary>0000.0000</Salary></Employment><LegalStatus>Employee</LegalStatus> <BirthDate>{16}</BirthDate><EmployeeIdent>{17}</EmployeeIdent><Location>NEA</Location> </Applicant></Applicants><TransmittalResult Status={18} /></Transmittal>",
                //	"\"http://www.w3.org/2001/XMLSchema-instance\"", 
                //	"\"http://www.w3.org/2001/XMLSchema\"",
                //	"\"D05B0402-88C2-4327-9704-2E2040526E0E\"", 
                //	"\"UploadApplicants\"", 
                //	"\"1\"", 
                //	"\"false\"",
                //	"\"Personal\"", 
                //	address, 
                //	city, 
                //	statecode, 
                //	zipcode, 
                //	loginuserid, 
                //	firstname, 
                //	lastname, 
                //	loginuserid,
                //	"\"Active\"",
                //	birthdateStr, 
                //	mdsIndvId, 
                //	"\"OK\""
                //);
                return client.Upload(
                    _config.EnrollmentServiceUser,
                    _config.EnrollmentServicePassword,
                    payload
                );
            }
        }


        /// <summary>
        /// Encrypt mdsid with service
        /// </summary>
        /// <param name="mdsid">Required.</param>
        /// <param name="partner">Optional.  If "afinium" then use the configuration Afinium password</param>
        /// <returns></returns>
        public string EncryptPartner(string mdsid, string partner)
        {
            mdsid = mdsid ?? throw new ArgumentNullException(nameof(mdsid));
            var password = _config.MercerEncryptDecryptPassword;
            if (string.Equals(partner, "afinium"))
            {
                password = _config.AfiniumEncryptDecryptPassword;
            }
            using (var client = new aes256EncryptDecrypt { Timeout = _defaultTimeoutMs })
            {
                return client.encrypt(mdsid, password,
                     _config.MercerEncryptDecryptSalt,
                     _config.AESPasswordInteractions, true,
                     _config.AESKeySize, true
                );
            }
        }
        #endregion
    }
}
