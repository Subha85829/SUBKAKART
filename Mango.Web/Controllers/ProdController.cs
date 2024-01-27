using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProdController : Controller
    {
        public readonly IProdService _prodService;
        public ProdController(IProdService prodService)
        {
            _prodService = prodService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? prodList = new();

            ResponseDTO? response = await _prodService.GetAllProducts();
            if (response != null && response.IsSuccess)
            {
                prodList = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(prodList);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View(null);
        }

		[HttpPost]
		public async Task<IActionResult> ProductCreate(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _prodService.CreateProduct(productDTO);

                if(responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Product Created Successfully!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
					TempData["error"] = responseDTO?.Message;
				}               
            }
			return View(productDTO);
		}

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDTO? response = await _prodService.GetProductsById(productId);
            if(response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _prodService.UpdateProduct(productDTO);
                if(responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDTO?.Message;
                }               
            }
            return View(productDTO);
        }
    }
}
