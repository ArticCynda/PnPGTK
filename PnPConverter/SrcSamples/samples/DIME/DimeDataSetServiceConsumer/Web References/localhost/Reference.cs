﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.3705.288
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 1.0.3705.288.
// 
namespace DimeDataSetServiceConsumer.localhost {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Service1Soap", Namespace="http://tempuri.org/")]
    public class Service1 : Microsoft.Web.Services.WebServicesClientProtocol { // base class is changed here
        
        /// <remarks/>
        public Service1() {
            this.Url = "http://localhost/DimeDataSetService/Service1.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetDataSet", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void GetDataSet() {
            this.Invoke("GetDataSet", new object[0]);
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetDataSet(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetDataSet", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public void EndGetDataSet(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
    }
}
