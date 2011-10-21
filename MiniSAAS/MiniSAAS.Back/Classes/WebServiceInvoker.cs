﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml;


namespace MiniSAAS.Back.Classes
{
    public class WebServiceInvoker
    {
        Dictionary<string, Type> availabletypes;
        Assembly webserviceassembly;
        List<string> services;

        public List<string> AvailableServices { get { return this.services; } }

        public WebServiceInvoker(Uri webserviceuri)
        {
            try
            {
                this.services = new List<string>();
                this.availabletypes = new Dictionary<string, Type>();
                this.webserviceassembly = BuildAssemblyFromWSDL(webserviceuri);
                Type[] types = this.webserviceassembly.GetExportedTypes();
                foreach (Type type in types)
                {
                    services.Add(type.FullName);
                    availabletypes.Add(type.FullName, type);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ServiceMethod> EnumerateServiceMethods(string servicename)
        {
            List<ServiceMethod> servicemethods = new List<ServiceMethod>();
            ServiceMethod method = null;
            List<string> parameters = null;
            try
            {
                if (!this.availabletypes.ContainsKey(servicename))
                {
                    throw new Exception("Service not available");
                }
                else
                {
                    Type type = this.availabletypes[servicename];
                    foreach (MethodInfo minfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        method = new ServiceMethod();
                        parameters = new List<string>();
                        method.MethodName = minfo.Name;
                        foreach (ParameterInfo pinfo in minfo.GetParameters())
                        {
                            parameters.Add(pinfo.ParameterType.FullName);
                        }
                        method.Parameters = parameters;
                        method.ReturnType = minfo.ReturnParameter.ParameterType.FullName;
                        servicemethods.Add(method);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return servicemethods;
        }

        /*public List<string> EnumerateServiceMethods(string servicename)
        {
            try
            {
                List<string> methods = new List<string>();
                if (!this.availabletypes.ContainsKey(servicename))
                {
                    throw new Exception("Service Not Available");
                }
                else
                {
                    Type type = this.availabletypes[servicename];
                    foreach (MethodInfo minfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        methods.Add(minfo.Name);
                    }
                    return methods;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }*/

        public T InvokeMethod<T>(string servicename, string methodname, params object[] args)
        {
            try
            {
                object obj = this.webserviceassembly.CreateInstance(servicename);
                Type type = obj.GetType();
                return (T)type.InvokeMember(methodname, BindingFlags.InvokeMethod, null, obj, args);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ServiceDescriptionImporter BuildServiceDescriptionImporter(XmlTextReader xmlreader)
        {
            try
            {
                if (!ServiceDescription.CanRead(xmlreader))
                {
                    throw new Exception("Invalid Web Service Description");
                }
                ServiceDescription servicedescription = ServiceDescription.Read(xmlreader);
                ServiceDescriptionImporter descriptionimporter = new ServiceDescriptionImporter();
                descriptionimporter.ProtocolName = "Soap";
                descriptionimporter.AddServiceDescription(servicedescription, null, null);
                descriptionimporter.Style = ServiceDescriptionImportStyle.Client;
                descriptionimporter.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
                return descriptionimporter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Assembly CompileAssembly(ServiceDescriptionImporter descriptionimporter)
        {
            try
            {
                CodeNamespace codenamespace = new CodeNamespace();
                CodeCompileUnit codeunit = new CodeCompileUnit();
                codeunit.Namespaces.Add(codenamespace);
                ServiceDescriptionImportWarnings importwarnings = descriptionimporter.Import(codenamespace, codeunit);
                if (importwarnings == 0)
                {
                    CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");
                    string[] references = new string[2] { "System.Web.Services.dll", "System.Xml.dll" };
                    CompilerParameters parameters = new CompilerParameters(references);
                    CompilerResults results = compiler.CompileAssemblyFromDom(parameters, codeunit);
                    foreach (CompilerError oops in results.Errors)
                    {
                        throw new Exception("Compilation Error Creating Assembly");
                    }
                    return results.CompiledAssembly;
                }
                else
                {
                    throw new Exception("Invalid WSDL");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Assembly BuildAssemblyFromWSDL(Uri webserviceuri)
        {
            try
            {
                if (String.IsNullOrEmpty(webserviceuri.ToString()))
                {
                    throw new Exception("Web Service Not Found");
                }
                XmlTextReader xmlreader = new XmlTextReader(webserviceuri.ToString() + "?wsdl");
                ServiceDescriptionImporter descriptionimporter = BuildServiceDescriptionImporter(xmlreader);
                return CompileAssembly(descriptionimporter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}