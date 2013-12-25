using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RandomStudent.Utilities
{
    public class XmlHelper
    {
        /// <summary>
        /// 保存到xml
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        /// <param name="xmlRootName"></param>
        public static void SaveToXml(string filePath, object sourceObj, string xmlRootName)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                Type type = sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(xmlRootName) ?
                        new System.Xml.Serialization.XmlSerializer(type) :
                        new System.Xml.Serialization.XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
        }
        /// <summary>
        /// 从xml加载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object LoadFromXml(string filePath, Type type)
        {
            object result = null;
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                        result = xmlSerializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        //添加一个xml节点
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"> xml path</param>
        /// <param name="type">根节点的 类型</param>
        /// <param name="item">要加入的子节点</param>
        /// <returns></returns>
        public static bool AddToXml(string filePath, Type type, object item)
        {
            bool res = false;
            if (File.Exists(filePath) && item != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath.Trim());
                string name = item.GetType().Name;
                XmlElement xmlElement = xmlDocument.DocumentElement;
                XmlElement collNodes = xmlElement.FirstChild as XmlElement;
                ///用反射的方式生成一个新的xmlelement 并加入
                XmlElement studentElement = xmlDocument.CreateElement(name, collNodes.GetNamespaceOfPrefix(""));
                PropertyInfo[] propInfos = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in propInfos)
                {
                    string tname = prop.Name;
                    XmlElement tempElement = xmlDocument.CreateElement(tname, studentElement.GetNamespaceOfPrefix(""));
                    tempElement.InnerText = prop.GetValue(item, null).ToString();
                    studentElement.AppendChild(tempElement);
                }
                collNodes.AppendChild(studentElement);
                xmlDocument.Save(filePath);
            }
            return res;
        }
        //
        /// <summary>
        /// 删除一个xml节点 没有用attribute来定位node 用的 比较的方式
        /// </summary>
        /// <param name="filePath"> xml path</param>
        /// <param name="type">根节点的 类型</param>
        /// <param name="item">要加入的子节点</param>
        /// <returns></returns>
        public static bool RemoveFromXml(string filePath, Type type, object item)
        {
            bool res = false;
            if (File.Exists(filePath) && item != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath.Trim());
                string name = item.GetType().Name;
                XmlElement xmlElement = xmlDocument.DocumentElement;
                XmlElement collNodes = xmlElement.FirstChild as XmlElement;
                ///用反射的方式生成一个新的xmlelement 并加入
                XmlElement studentElement = xmlDocument.CreateElement(name);
                XmlElement delElement = xmlDocument.CreateElement(name);
                PropertyInfo[] propInfos = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in propInfos)
                {
                    string tname = prop.Name;
                    XmlElement tempElement = xmlDocument.CreateElement(tname, studentElement.GetNamespaceOfPrefix(""));
                    tempElement.InnerText = prop.GetValue(item, null).ToString();
                    studentElement.AppendChild(tempElement);
                }

                for (int i = 0; i < collNodes.ChildNodes.Count; i++)
                {
                    for (int j = 0; j < (int)collNodes.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        string tname = propInfos[j].Name;
                        if (collNodes.ChildNodes[i].ChildNodes[j].InnerText != studentElement.ChildNodes[j].InnerText)
                            continue;
                        else
                        {
                            if (j == collNodes.ChildNodes[i].ChildNodes.Count - 1)
                            {
                                res = true;
                                delElement = collNodes.ChildNodes[i] as XmlElement;
                                break;
                            }
                        }
                    }
                }
                collNodes.RemoveChild(delElement);
                xmlDocument.Save(filePath);
            }
            return res;
        }
    }
}
