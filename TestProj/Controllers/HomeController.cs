using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;
using TestProj.Interfaces;
using TestProj.Models;

namespace TestProj.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ISuppliersProductsConverter _suppliersProductsConverter;

    public HomeController(IProductService productService, ISuppliersProductsConverter suppliersProductsConverter)
    {
        _productService = productService;
        _suppliersProductsConverter = suppliersProductsConverter;
    }

    #region Get

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAll();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var product = await _productService.GetById(id);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetFlaggedProducts()
    {
        var flaggedProducts = await _productService.GetFlaggedProducts();
        return View(flaggedProducts);
    }

    [HttpGet]
    public IActionResult UploadFileView()
    {
        return View("UploadFileView");
    }

    #endregion

    #region Post

    [HttpPost]
    public async Task<IActionResult> ConvertSupplierXmlFileToProduct(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            try
            {
                if (file.ContentType == "text/xml" ||
                    Path.GetExtension(file.FileName).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    await using var stream = file.OpenReadStream();
                    var xmlDoc = XDocument.Load(stream);
                    string xmlContent = xmlDoc.ToString();
                    var products = _suppliersProductsConverter.ConvertToProducts(xmlContent, file.FileName);
                    await _productService.SaveOrUpdateProducts(products);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The file must be an XML file.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        return View("UploadFileView");
    }

    [HttpPost]
    public async Task<IActionResult> FlagProduct(string id)
    {
        var product = await _productService.FlagProduct(id);
        return RedirectToAction("GetAll");
    }

    #endregion

}
