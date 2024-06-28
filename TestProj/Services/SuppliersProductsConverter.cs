using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TestProj.Entities;
using TestProj.Interfaces;

namespace TestProj.Services
{
    public class SuppliersProductsConverter : ISuppliersProductsConverter
    {
        public IEnumerable<Product> ConvertToProducts(string xmlContent, string supplierName)
        {
            if (supplierName.Contains("dostawca1"))
            {
                return FirstSupplierXmlToProductsConverter(xmlContent);
            }
            else if (supplierName.Contains("dostawca2"))
            {
                return SecondSupplierXmlToProductsConverter(xmlContent);
            }
            else if (supplierName.Contains("dostawca3"))
            {
                return ThirdSupplierXmlToProductsConverter(xmlContent);
            }
            else
            {
                return new List<Product>();
            }
        }

        private IEnumerable<Product> FirstSupplierXmlToProductsConverter(string xmlContent)
        {
            var products = new List<Product>();
            var xDoc = XDocument.Parse(xmlContent);
            XNamespace xmlNs = "http://www.w3.org/XML/1998/namespace";

            var offerElement = xDoc.Element("offer");
            if (offerElement != null)
            {
                var productsElement = offerElement.Element("products");
                if (productsElement != null)
                {
                    foreach (var productElement in productsElement.Elements("product"))
                    {
                        var product = new Product
                        {
                            Id = productElement.Attribute("id")?.Value ?? string.Empty,
                            ProductCode = productElement.Attribute("code_producer")?.Value ?? string.Empty,
                            Name = productElement.Element("description")?.Elements("name")
                                .FirstOrDefault(n => (string)n.Attribute(xmlNs + "lang") == "pol")?.Value ?? string.Empty,
                            Description = RemoveHtmlTags(productElement.Element("description")?.Elements("long_desc")
                                .FirstOrDefault(ld => (string)ld.Attribute(xmlNs + "lang") == "pol")?.Value ?? string.Empty),
                            Price = decimal.TryParse(productElement.Element("price")?.Attribute("gross")?.Value,
                                NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                            StockQuantity = int.TryParse(productElement.Element("sizes")?.Element("size")?.Element("stock")?
                                .Attribute("quantity")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int qty) ? qty : 0,
                            Photos = productElement.Element("images")?.Element("large")?.Elements("image")
                                .Select(x => x.Attribute("url")?.Value).Where(x => !string.IsNullOrEmpty(x)).ToList() ?? new List<string>(),
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        private IEnumerable<Product> SecondSupplierXmlToProductsConverter(string xmlContent)
        {
            var products = new List<Product>();
            var xDoc = XDocument.Parse(xmlContent);

            foreach (var productElement in xDoc.Descendants("product"))
            {
                var product = new Product
                {
                    Id = productElement.Element("id")?.Value ?? string.Empty,
                    ProductCode = productElement.Element("ean")?.Value ?? string.Empty,
                    Name = productElement.Element("name")?.Value ?? string.Empty,
                    Description = RemoveHtmlTags(productElement.Element("desc")?.Value ?? string.Empty),
                    Price = decimal.TryParse(productElement.Element("retailPriceGross")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                    StockQuantity = int.TryParse(productElement.Element("qty")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int qty) ? qty : 0,
                    Photos = productElement.Element("photos")?.Elements("photo")
                                .Select(p => p.Value).ToList() ?? new List<string>(),
                };

                products.Add(product);
            }

            return products;
        }

        private IEnumerable<Product> ThirdSupplierXmlToProductsConverter(string xmlContent)
        {
            var products = new List<Product>();
            var xDoc = XDocument.Parse(xmlContent);

            foreach (var productElement in xDoc.Descendants("produkt"))
            {
                var product = new Product
                {
                    Id = productElement.Element("id")?.Value.Trim(),
                    ProductCode = productElement.Element("ean")?.Value.Trim(),
                    Name = productElement.Element("nazwa")?.Value.Trim(),
                    Description = RemoveHtmlTags(productElement.Element("dlugi_opis")?.Value.Trim()),
                    Price = decimal.TryParse(productElement.Element("cena_sugerowana")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) ? price : 0,
                    StockQuantity = int.TryParse(productElement.Element("status")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int qty) ? qty : 0,
                    Photos = productElement.Element("zdjecia")?.Elements("zdjecie").Select(x => x.Attribute("url")?.Value).Where(x => !string.IsNullOrEmpty(x)).ToList() ?? new List<string>(),
                };

                products.Add(product);
            }

            return products;
        }

        private static string RemoveHtmlTags(string text)
        {
            return Regex.Replace(text, "<.*?>", string.Empty);
        }
    }
}
