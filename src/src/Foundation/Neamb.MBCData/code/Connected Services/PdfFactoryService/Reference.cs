//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Neambc.Neamb.Foundation.MBCData.PdfFactoryService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.neamb.org", ConfigurationName="PdfFactoryService.PdfFactoryPortType")]
    public interface PdfFactoryPortType {
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getPdfUrl", ReplyAction="urn:getPdfUrlResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlResponse getPdfUrl(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getPdfUrl", ReplyAction="urn:getPdfUrlResponse")]
        System.Threading.Tasks.Task<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlResponse> getPdfUrlAsync(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getPdf", ReplyAction="urn:getPdfResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfResponse getPdf(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getPdf", ReplyAction="urn:getPdfResponse")]
        System.Threading.Tasks.Task<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfResponse> getPdfAsync(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getPdfUrl", WrapperNamespace="http://ws.neamb.org", IsWrapped=true)]
    public partial class getPdfUrlRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Product_Item_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Description;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Trans_Date;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_First_Name;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Last_Name;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_DOB;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_MDSID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Address;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_City;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_State;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=10)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Zip;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=11)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_MemberType;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=12)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=13)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_2;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=14)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_3;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=15)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_4;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=16)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_5;
        
        public getPdfUrlRequest() {
        }
        
        public getPdfUrlRequest(
                    string Product_Item_ID, 
                    string PD_Description, 
                    string PD_Trans_Date, 
                    string PD_First_Name, 
                    string PD_Last_Name, 
                    string PD_DOB, 
                    string PD_MDSID, 
                    string PD_Address, 
                    string PD_City, 
                    string PD_State, 
                    string PD_Zip, 
                    string PD_MemberType, 
                    string Custom_1, 
                    string Custom_2, 
                    string Custom_3, 
                    string Custom_4, 
                    string Custom_5) {
            this.Product_Item_ID = Product_Item_ID;
            this.PD_Description = PD_Description;
            this.PD_Trans_Date = PD_Trans_Date;
            this.PD_First_Name = PD_First_Name;
            this.PD_Last_Name = PD_Last_Name;
            this.PD_DOB = PD_DOB;
            this.PD_MDSID = PD_MDSID;
            this.PD_Address = PD_Address;
            this.PD_City = PD_City;
            this.PD_State = PD_State;
            this.PD_Zip = PD_Zip;
            this.PD_MemberType = PD_MemberType;
            this.Custom_1 = Custom_1;
            this.Custom_2 = Custom_2;
            this.Custom_3 = Custom_3;
            this.Custom_4 = Custom_4;
            this.Custom_5 = Custom_5;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getPdfUrlResponse", WrapperNamespace="http://ws.neamb.org", IsWrapped=true)]
    public partial class getPdfUrlResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public getPdfUrlResponse() {
        }
        
        public getPdfUrlResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getPdf", WrapperNamespace="http://ws.neamb.org", IsWrapped=true)]
    public partial class getPdfRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Product_Item_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Description;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Trans_Date;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_First_Name;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Last_Name;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_DOB;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_MDSID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Address;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_City;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_State;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=10)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_Zip;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=11)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PD_MemberType;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=12)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=13)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_2;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=14)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_3;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=15)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_4;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=16)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Custom_5;
        
        public getPdfRequest() {
        }
        
        public getPdfRequest(
                    string Product_Item_ID, 
                    string PD_Description, 
                    string PD_Trans_Date, 
                    string PD_First_Name, 
                    string PD_Last_Name, 
                    string PD_DOB, 
                    string PD_MDSID, 
                    string PD_Address, 
                    string PD_City, 
                    string PD_State, 
                    string PD_Zip, 
                    string PD_MemberType, 
                    string Custom_1, 
                    string Custom_2, 
                    string Custom_3, 
                    string Custom_4, 
                    string Custom_5) {
            this.Product_Item_ID = Product_Item_ID;
            this.PD_Description = PD_Description;
            this.PD_Trans_Date = PD_Trans_Date;
            this.PD_First_Name = PD_First_Name;
            this.PD_Last_Name = PD_Last_Name;
            this.PD_DOB = PD_DOB;
            this.PD_MDSID = PD_MDSID;
            this.PD_Address = PD_Address;
            this.PD_City = PD_City;
            this.PD_State = PD_State;
            this.PD_Zip = PD_Zip;
            this.PD_MemberType = PD_MemberType;
            this.Custom_1 = Custom_1;
            this.Custom_2 = Custom_2;
            this.Custom_3 = Custom_3;
            this.Custom_4 = Custom_4;
            this.Custom_5 = Custom_5;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getPdfResponse", WrapperNamespace="http://ws.neamb.org", IsWrapped=true)]
    public partial class getPdfResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.neamb.org", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] @return;
        
        public getPdfResponse() {
        }
        
        public getPdfResponse(byte[] @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PdfFactoryPortTypeChannel : Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PdfFactoryPortTypeClient : System.ServiceModel.ClientBase<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType>, Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType {
        
        public PdfFactoryPortTypeClient() {
        }
        
        public PdfFactoryPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PdfFactoryPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PdfFactoryPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PdfFactoryPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlResponse Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType.getPdfUrl(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest request) {
            return base.Channel.getPdfUrl(request);
        }
        
        public string getPdfUrl(
                    string Product_Item_ID, 
                    string PD_Description, 
                    string PD_Trans_Date, 
                    string PD_First_Name, 
                    string PD_Last_Name, 
                    string PD_DOB, 
                    string PD_MDSID, 
                    string PD_Address, 
                    string PD_City, 
                    string PD_State, 
                    string PD_Zip, 
                    string PD_MemberType, 
                    string Custom_1, 
                    string Custom_2, 
                    string Custom_3, 
                    string Custom_4, 
                    string Custom_5) {
            Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest inValue = new Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest();
            inValue.Product_Item_ID = Product_Item_ID;
            inValue.PD_Description = PD_Description;
            inValue.PD_Trans_Date = PD_Trans_Date;
            inValue.PD_First_Name = PD_First_Name;
            inValue.PD_Last_Name = PD_Last_Name;
            inValue.PD_DOB = PD_DOB;
            inValue.PD_MDSID = PD_MDSID;
            inValue.PD_Address = PD_Address;
            inValue.PD_City = PD_City;
            inValue.PD_State = PD_State;
            inValue.PD_Zip = PD_Zip;
            inValue.PD_MemberType = PD_MemberType;
            inValue.Custom_1 = Custom_1;
            inValue.Custom_2 = Custom_2;
            inValue.Custom_3 = Custom_3;
            inValue.Custom_4 = Custom_4;
            inValue.Custom_5 = Custom_5;
            Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlResponse retVal = ((Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType)(this)).getPdfUrl(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlResponse> Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType.getPdfUrlAsync(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest request) {
            return base.Channel.getPdfUrlAsync(request);
        }
        
        public System.Threading.Tasks.Task<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlResponse> getPdfUrlAsync(
                    string Product_Item_ID, 
                    string PD_Description, 
                    string PD_Trans_Date, 
                    string PD_First_Name, 
                    string PD_Last_Name, 
                    string PD_DOB, 
                    string PD_MDSID, 
                    string PD_Address, 
                    string PD_City, 
                    string PD_State, 
                    string PD_Zip, 
                    string PD_MemberType, 
                    string Custom_1, 
                    string Custom_2, 
                    string Custom_3, 
                    string Custom_4, 
                    string Custom_5) {
            Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest inValue = new Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfUrlRequest();
            inValue.Product_Item_ID = Product_Item_ID;
            inValue.PD_Description = PD_Description;
            inValue.PD_Trans_Date = PD_Trans_Date;
            inValue.PD_First_Name = PD_First_Name;
            inValue.PD_Last_Name = PD_Last_Name;
            inValue.PD_DOB = PD_DOB;
            inValue.PD_MDSID = PD_MDSID;
            inValue.PD_Address = PD_Address;
            inValue.PD_City = PD_City;
            inValue.PD_State = PD_State;
            inValue.PD_Zip = PD_Zip;
            inValue.PD_MemberType = PD_MemberType;
            inValue.Custom_1 = Custom_1;
            inValue.Custom_2 = Custom_2;
            inValue.Custom_3 = Custom_3;
            inValue.Custom_4 = Custom_4;
            inValue.Custom_5 = Custom_5;
            return ((Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType)(this)).getPdfUrlAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfResponse Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType.getPdf(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest request) {
            return base.Channel.getPdf(request);
        }
        
        public byte[] getPdf(
                    string Product_Item_ID, 
                    string PD_Description, 
                    string PD_Trans_Date, 
                    string PD_First_Name, 
                    string PD_Last_Name, 
                    string PD_DOB, 
                    string PD_MDSID, 
                    string PD_Address, 
                    string PD_City, 
                    string PD_State, 
                    string PD_Zip, 
                    string PD_MemberType, 
                    string Custom_1, 
                    string Custom_2, 
                    string Custom_3, 
                    string Custom_4, 
                    string Custom_5) {
            Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest inValue = new Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest();
            inValue.Product_Item_ID = Product_Item_ID;
            inValue.PD_Description = PD_Description;
            inValue.PD_Trans_Date = PD_Trans_Date;
            inValue.PD_First_Name = PD_First_Name;
            inValue.PD_Last_Name = PD_Last_Name;
            inValue.PD_DOB = PD_DOB;
            inValue.PD_MDSID = PD_MDSID;
            inValue.PD_Address = PD_Address;
            inValue.PD_City = PD_City;
            inValue.PD_State = PD_State;
            inValue.PD_Zip = PD_Zip;
            inValue.PD_MemberType = PD_MemberType;
            inValue.Custom_1 = Custom_1;
            inValue.Custom_2 = Custom_2;
            inValue.Custom_3 = Custom_3;
            inValue.Custom_4 = Custom_4;
            inValue.Custom_5 = Custom_5;
            Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfResponse retVal = ((Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType)(this)).getPdf(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfResponse> Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType.getPdfAsync(Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest request) {
            return base.Channel.getPdfAsync(request);
        }
        
        public System.Threading.Tasks.Task<Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfResponse> getPdfAsync(
                    string Product_Item_ID, 
                    string PD_Description, 
                    string PD_Trans_Date, 
                    string PD_First_Name, 
                    string PD_Last_Name, 
                    string PD_DOB, 
                    string PD_MDSID, 
                    string PD_Address, 
                    string PD_City, 
                    string PD_State, 
                    string PD_Zip, 
                    string PD_MemberType, 
                    string Custom_1, 
                    string Custom_2, 
                    string Custom_3, 
                    string Custom_4, 
                    string Custom_5) {
            Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest inValue = new Neambc.Neamb.Foundation.MBCData.PdfFactoryService.getPdfRequest();
            inValue.Product_Item_ID = Product_Item_ID;
            inValue.PD_Description = PD_Description;
            inValue.PD_Trans_Date = PD_Trans_Date;
            inValue.PD_First_Name = PD_First_Name;
            inValue.PD_Last_Name = PD_Last_Name;
            inValue.PD_DOB = PD_DOB;
            inValue.PD_MDSID = PD_MDSID;
            inValue.PD_Address = PD_Address;
            inValue.PD_City = PD_City;
            inValue.PD_State = PD_State;
            inValue.PD_Zip = PD_Zip;
            inValue.PD_MemberType = PD_MemberType;
            inValue.Custom_1 = Custom_1;
            inValue.Custom_2 = Custom_2;
            inValue.Custom_3 = Custom_3;
            inValue.Custom_4 = Custom_4;
            inValue.Custom_5 = Custom_5;
            return ((Neambc.Neamb.Foundation.MBCData.PdfFactoryService.PdfFactoryPortType)(this)).getPdfAsync(inValue);
        }
    }
}
