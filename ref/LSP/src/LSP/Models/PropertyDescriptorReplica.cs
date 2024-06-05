using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace LSP.Models
{
    #region "Property Descriptor Parse Class"    
    public static class PropertyDescriptorReplica
    {
        public static string[] ParseAttributesName<T>() where T : class
        {
            List<string> attributesName = new List<string>();
            foreach (var property in typeof(T).GetProperties())
            {
                attributesName.Add(property.Name);
            }
            return attributesName.ToArray();
        }

        public static string ParseAttributeDescription<T>(string fieldName)
        {
            string result;            
            try
                {
                    object[] descriptionAttrs = typeof(T).GetProperty(fieldName).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    DescriptionAttribute description = (DescriptionAttribute)descriptionAttrs[0];
                    result = (description.Description);
                }
                catch
                {
                    result = "";
                }

            return result;
        }
    
        public static List<PropertyDescriptors> ParseAttributes<T>() where T : class
        {
            List<PropertyDescriptors> list = new List<PropertyDescriptors>();
            foreach (var property in typeof(T).GetProperties())
            {
                PropertyDescriptors pD = new PropertyDescriptors();
                pD.AttributesName = property.Name;

                var propertyAttributes = Attribute.GetCustomAttributes(property);
                try
                {
                    object[] descriptionAttrs = property.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    DescriptionAttribute description = (DescriptionAttribute)descriptionAttrs[0];
                    pD.Description = (description.Description);

                }
                catch
                {
                    pD.Description = "";
                }
                list.Add(pD);
            }

            return list;
        }

        public static List<PropertyDescriptors> ParseAttributesExistDescriptor<T>() where T : class
        {
            List<PropertyDescriptors> list = new List<PropertyDescriptors>();
            foreach (var property in typeof(T).GetProperties())
            {
                PropertyDescriptors pD = new PropertyDescriptors();
                pD.AttributesName = property.Name;

                var propertyAttributes = Attribute.GetCustomAttributes(property);
                try
                {
                    object[] descriptionAttrs = property.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    DescriptionAttribute description = (DescriptionAttribute)descriptionAttrs[0];
                    pD.Description = (description.Description);

                }
                catch
                {
                    pD.Description = "";
                }
                list.Add(pD);
            }

            return list.Where(d => !d.Description.Equals("")).ToList();
        }
    }
    #endregion

    #region "Property Descriptor Class"
    public class PropertyDescriptors
    {
        [Description("The Field Name")]
        public string AttributesName { get; set; }

        [Description("The Description")]
        public string Description { get; set; }

    }
    #endregion
    
}