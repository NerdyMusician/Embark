using Embark.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Embark.Toolbox
{
    public static class XmlMethods
    {
        // Public Methods
        public static XElement ListToXml(IEnumerable itemList, string enumName = "")
        {
            string elementName = "";
            List<XElement> items = new();
            foreach (object item in itemList)
            {
                elementName = item.GetType().ToString().Split('.').Last();
                items.Add(new XElement(elementName));
                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
                {
                    foreach (CustomAttributeData attr in propertyInfo.CustomAttributes)
                    {
                        if (attr.AttributeType.Name == "XmlSaveMode")
                        {
                            if (attr.ConstructorArguments[0].Value.ToString() == "0")
                            {
                                items.Last().Add(new XAttribute(propertyInfo.Name, (propertyInfo.GetValue(item, null) != null) ? propertyInfo.GetValue(item, null).ToString() : ""));
                            }
                            if (attr.ConstructorArguments[0].Value.ToString() == "1")
                            {
                                items.Last().Add(ListToXml(propertyInfo.GetValue(item, null) as IEnumerable, propertyInfo.Name));
                            }
                        }
                    }
                }
            }

            if (items.Count() == 0) { return null; }
            return new XElement(elementName + "Set", items, new XAttribute("Name", enumName));

        }

        // Node to Object
        public static void NodeToObject(XmlNode jobNode, out JobLead job)
        {
            JobLead newJob = SetObjectPropertiesFromNode(jobNode, new JobLead()) as JobLead;

            foreach (XmlNode childNode in jobNode.ChildNodes)
            {
                if (childNode.Attributes.GetNamedItem("Name").InnerText == "Links")
                {
                    foreach (XmlNode node in childNode.ChildNodes)
                    {
                        NodeToObject(node, out WebLink obj);
                        newJob.Links.Add(obj);
                    }
                }
                if (childNode.Attributes.GetNamedItem("Name").InnerText == "Contacts")
                {
                    foreach (XmlNode node in childNode.ChildNodes)
                    {
                        NodeToObject(node, out Contact obj);
                        newJob.Contacts.Add(obj);
                    }
                }
                if (childNode.Attributes.GetNamedItem("Name").InnerText == "Events")
                {
                    foreach (XmlNode node in childNode.ChildNodes)
                    {
                        NodeToObject(node, out Event obj);
                        newJob.Events.Add(obj);
                    }
                }
            }

            job = newJob;

        }
        public static void NodeToObject(XmlNode linkNode, out WebLink link)
        {
            link = SetObjectPropertiesFromNode(linkNode, new WebLink()) as WebLink;
        }
        public static void NodeToObject(XmlNode contactNode, out Contact contact)
        {
            contact = SetObjectPropertiesFromNode(contactNode, new Contact()) as Contact;
        }
        public static void NodeToObject(XmlNode eventNode, out Event xEvent)
        {
            xEvent = SetObjectPropertiesFromNode(eventNode, new Event()) as Event;
        }

        // XML to List
        public static void XmlToList(string filePath, out List<JobLead> items, XmlDocument xmlDoc = null)
        {
            List<JobLead> newItems = new();

            foreach (XmlNode itemNode in GetXmlNodeListFromXmlDoc(filePath, "JobLeadSet", xmlDoc))
            {
                NodeToObject(itemNode, out JobLead item);
                newItems.Add(item);
            }

            items = newItems;

        }

        // Private Methods
        private static object SetObjectPropertiesFromNode(XmlNode node, object newObject)
        {
            foreach (PropertyInfo propertyInfo in newObject.GetType().GetProperties())
            {
                if (node.Attributes[propertyInfo.Name] != null)
                {
                    string value = node.Attributes[propertyInfo.Name].InnerText;
                    Type propType = propertyInfo.PropertyType;
                    if (propType == typeof(int)) { propertyInfo.SetValue(newObject, Convert.ToInt32(value)); }
                    if (propType == typeof(decimal)) { propertyInfo.SetValue(newObject, Convert.ToDecimal(value)); }
                    if (propType == typeof(bool)) { propertyInfo.SetValue(newObject, Convert.ToBoolean(value)); }
                    if (propType == typeof(long)) { propertyInfo.SetValue(newObject, Convert.ToInt64(value)); }
                    if (propType == typeof(string)) { propertyInfo.SetValue(newObject, value); }
                }
            }
            return newObject;
        }
        private static XmlNodeList GetXmlNodeListFromXmlDoc(string filePath, string setName, XmlDocument xmlDoc)
        {
            XmlNodeList xmlNodes;
            if (xmlDoc == null)
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                if (xmlDoc.ChildNodes[1].Name != setName)
                {
                    AuxMethods.WriteToLogFile("Invalid XML for Import: " + filePath, true);
                    return null;
                }
                xmlNodes = xmlDoc.ChildNodes[1].ChildNodes;
            }
            else
            {
                xmlNodes = xmlDoc.ChildNodes[0].ChildNodes;
            }
            return xmlNodes;
        }

    }
}
