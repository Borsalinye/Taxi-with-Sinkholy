using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;

namespace Taxi.Models.Persons.Workers
{
    public enum WorkerStates : byte
    {
        Available,
        Busy,
        Absent
    }
    public enum DriverLicenseCategories : byte
    {
        A,
        B,
        C
        //TODO : категории
    }

    public abstract class Worker : Person
    {
        public virtual void OnTripStart(object sender, EventArgs e) => this.State = WorkerStates.Busy;
        public virtual void OnTripEnd(object sender, EventArgs e) => this.State = WorkerStates.Available;
        public virtual void OnTripInicializate(object sender, EventArgs e) { }
        public WorkerStates State { get; private protected set; }
        public Documents Documents { get; private set; }
        public DateTime BirthDate => this.Documents.Passport.BirthDate;
        public int Age => Documents.Passport.Age;
        public int SubjRF { get; private set; }
    }
    public sealed class Documents
    {
        public static bool Validate(XDocument xDoc, XmlSchemaSet XSS, out List<string> errors)
        {
            bool result = true;
            List<string> errorsLocal = new List<string>();
            xDoc.Validate(XSS, (o, e) => 
            {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    errorsLocal.Add(e.Message);
                }
                else
                {
                    result = false;
                    errorsLocal.Add(e.Message);
                }
            });
            errors = errorsLocal;
            return result;
        }
        private static XmlSchemaSet XmlSchemaSet { get; }
        public static bool UpdateXmlSchema(out string errorDescription)
        {
            errorDescription = null;
            return true;
        }
        private XDocument Doc { get; set; }
        public Passport Passport { get; private set; }
        public DriverLicense DriverLicense { get; private set; }
        public string INN { get; private set; }
        public Documents(XDocument xDoc)
        {
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            XmlReaderSettings settings = new XmlReaderSettings();
            xmlSchemaSet.Add(settings.Schemas.Add("DocumentsSchema.xsd", "DocumentsSchema.xsd"));
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidateXml);
            xDoc.Validate(xmlSchemaSet, ValidateXml);
        }
        void ValidateXml(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine($"Error: {e.Message}");
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine($"Warning: {e.Message}");
                    break;
            }
        }
    }
    public abstract class Document
    {
        public XElement Doc { get; private set; }
        public string Number => Doc.Element("number").Value;
        public string IssuedPlace => Doc.Element("issuedPlace").Value;
        public DateTime IssuedDate => DateTime.Parse(Doc.Element("issuedDate").Value);
        public string RegPlace => Doc.Element("regPlace").Value;
        public virtual DateTime ExpirationDate => DateTime.Parse(Doc.Element("expirationDate").Value);
        public Document(XElement docNode)
        {
            
        }
    }
    public sealed class Passport : Document
    {
        public override DateTime ExpirationDate
        {
            get
            {
                int diff = 0;
                if (this.Age < 20)
                {
                    diff = 20 - Age;
                    return this.BirthDate.AddYears(Age + diff);
                }
                else if (this.Age >= 20 && this.Age < 40)
                {
                    diff = 40 - Age;
                    return this.BirthDate.AddYears(Age + diff);
                }
                else
                {
                    return DateTime.MaxValue;
                }
            }
        }
        public int Age => (DateTime.Now - this.BirthDate).Days / 365;
        public DateTime BirthDate => DateTime.Parse(this.Doc.Element("BirthDate").Value);
        public Gender Gender { get; private set; }
        public Passport(XElement node) : base(node) { }
    }
    public sealed class DriverLicense : Document
    {
        public List<DriverLicenseCategories> Categories { get; private set; }
        private void CategoriesInicialize()
        {
            Categories = this.Doc.Element("Categories").Elements().Select(x => 
            {
                switch (x.Value)
                {
                    case "A":
                        return DriverLicenseCategories.A;
                    case "B":
                        return DriverLicenseCategories.B;
                    case "C":
                        return DriverLicenseCategories.C;
                    default:
                        throw new ArgumentException("Невозможно преобразовать категорию ВУ.");
                }
            }).ToList();
        }
        public DriverLicense(XElement node) : base(node) { }
    }
}