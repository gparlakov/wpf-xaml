using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PhonesStore.ViewModels
{
    public static class DataPersister
    {
        public static IEnumerable<PhonesStoreViewModel> GetAllStores(string documentPath)
        {
            var allPhones = GetPhones(documentPath);
            var storesDocumentRoot = XDocument.Load(documentPath + "stores.xml").Root;

            var stores = from storeElement in storesDocumentRoot.Elements("store")
                         select new PhonesStoreViewModel()
                         {
                             Name = storeElement.Element("name").Value,
                             Phones = from phoneElement in storeElement.Element("phones").Elements("phoneId")
                                      select allPhones.FirstOrDefault(p => p.Id == phoneElement.Value)
                         };
            return stores;
        }

        public static IEnumerable<PhoneViewModel> GetPhones(string documentPath)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);
            
            var phonesDocumentRoot = XDocument.Load(documentPath + "phones.xml").Root;

            var oses = GetAllOSes(documentPath);

            var phonesVM =
                           from phoneElement in phonesDocumentRoot.Elements("phone")
                           select new PhoneViewModel()
                                       {
                                           Id = phoneElement.Element("id").Value,
                                           Vendor = phoneElement.Element("vendor").Value,
                                           Model = phoneElement.Element("model").Value,
                                           Year = int.Parse(phoneElement.Element("year").Value),
                                           ImagePath = Path.Combine(path, phoneElement.Element("image").Value),
                                           OS = oses.FirstOrDefault(os => os.Id == phoneElement.Element("os").Value)
                                       };
            return phonesVM;
        }

        internal static IEnumerable<OperatingSystemViewModel> GetAllOSes(string phonesDocumentPath)
        {
            var osRoot = XDocument.Load(phonesDocumentPath + "os.xml").Root;
            var osVms =
                    from osElement in osRoot.Elements("os")
                    select new OperatingSystemViewModel()
                    {
                        Name = osElement.Element("name").Value,
                        Manufacturer = osElement.Element("manufacturer").Value,
                        Version = osElement.Element("version").Value,
                        Id = osElement.Element("id").Value
                    };
            return osVms;
        }

        internal static void AddPhone(string documenPath, PhoneViewModel phone, string storeName)
        {
            var root = XDocument.Load(documenPath + "phones.xml").Root;
            var newPhoneId = Guid.NewGuid();
            root.Add(
                new XElement("phone",
                    new XElement("id", newPhoneId),
                    new XElement("vendor", phone.Vendor),
                    new XElement("model", phone.Model),
                    new XElement("year", phone.Year),
                    new XElement("image", phone.ImagePath),
                    new XElement("os",
                            new XElement("id", phone.OS.Id)
                            //new XElement("name", phone.OS.Name),
                            //new XElement("version", phone.OS.Name),
                            //new XElement("manufacturer", phone.OS.Manufacturer)))
                            )
                )
            );
            root.Document.Save(documenPath + "phones.xml");
        }

        public static void SavePhoneToStore(string documenPath, string storeName, string newPhoneId)
        {
            var storesRoot = XDocument.Load(documenPath + "stores.xml").Root;
            var store = storesRoot.Elements("store").FirstOrDefault(e => e.Element("name").Value == storeName);
            store.Element("phones").Add(new XElement("phoneId", newPhoneId));
            storesRoot.Save(documenPath + "stores.xml");
        }

        public static IEnumerable<PhoneViewModel> GetPhonesByStoreName(string documentPath, string storeName)
        {
            var allStores = GetAllStores(documentPath);
            var store = allStores.FirstOrDefault(s => s.Name == storeName);
            return store.Phones;
        }

        internal static void AddNewStore(PhonesStoreViewModel phonesStoreViewModel, string path)
        {
            var storesPath = path + "stores.xml";
            var stores = XDocument.Load(storesPath).Root;
            stores.Add(
                new XElement("store",
                    new XElement("name", phonesStoreViewModel.Name),
                    new XElement("phones")));
            stores.Save(storesPath);
        }
    }
}