using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewMoldes.Catalog.ProductImages;
using eShopSolution.ViewMoldes.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }
        //https://localhost:44393/Product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
        }
        //https://localhost:44393/Product?pugeIndex=1&pageSize=10&CategoryId=
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId,[FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllbyCategoryId( languageId,request);
            return Ok(products);
        }
        //https://localhost:44393/Product/1
        [HttpGet("{Id}/{langugeId}")]
        public async Task<IActionResult> GetById(int productId,string langugeId)
        {
            var products = await _manageProductService.GetById(productId, langugeId);
            if (products == null) return NotFound("can't find");
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.Create(request);
            if (result == 0) return BadRequest();
            var product = await _manageProductService.GetById(result,request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id=result},product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedresult = await _manageProductService.Update(request);
            if (affectedresult == 0) return BadRequest();
            return Ok();
        }
        [HttpDelete("{productid}")]
        public async Task<IActionResult> Delete(int productid)
        {
            var affectedresult = await _manageProductService.Delete(productid);
            if (affectedresult == 0) return BadRequest();
            return Ok();
        }
        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var issuccess = await _manageProductService.UpdatePrice( id, newPrice);
            if (issuccess) return Ok();
            return BadRequest();
        }



        //image
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageByID(int imageId)
        {
            var image = await _manageProductService.GetImageByID(imageId);
            if (image == null) return NotFound("can't find");
            return Ok(image);
        }
         
        [HttpPost("{productId}/image")]
        public async Task<IActionResult> AddImage( int productid, [FromForm] ProductIamgeCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageid = await _manageProductService.AddImage(productid,request);
            if (imageid == 0) return BadRequest();
            var image = await _manageProductService.GetImageByID(imageid);
            return CreatedAtAction(nameof(GetImageByID), new { id = imageid }, image);
        }
        [HttpPut("{productId}/image/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductIamgeUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.UpdateImage(imageId, request);
            if (result == 0) return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}/image/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.RemoveImage(imageId);
            if (result == 0) return BadRequest();
            return Ok();
        }

        
    }
}
