﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

namespace FingerprintAnalyzer.ImportExport
{
    class FingerprintXML<Type>
    {
        public static void Save(Type typeInstance, string fileName)
        {
            var xs = new XmlSerializer(typeof(Type));

            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings() { Indent = true }))
            {
                xs.Serialize(writer, typeInstance);
            }
        }

        public static Type Load(string fileName)
        {
            Type result;

            var xs = new XmlSerializer(typeof(Type));

            using (var reader = XmlReader.Create(fileName))
            {
                result = (Type)xs.Deserialize(reader);
            }

            return result;
        }
    }
}