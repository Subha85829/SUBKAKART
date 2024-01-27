using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _responseDTO;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                List<Product> productlist = _db.Products.ToList();
                _responseDTO.Result = _mapper.Map<List<ProductDTO>>(productlist);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id) 
        {
            try
            {
                Product? product = _db.Products.Where(x => x.ProductId == id).FirstOrDefault();
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDTO Get(string name)
        {
            try
            {
                Product? product = _db.Products.Where(x => x.Name.ToLower() == name.Trim().ToLower()).FirstOrDefault();
                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody]ProductDTO productDTO) 
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Add(product);
                _db.SaveChanges();

                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch(Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Update(product);
                _db.SaveChanges();

                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Product? product = _db.Products.Where(x => x.ProductId == id).FirstOrDefault();

                _db.Remove(product);
                _db.SaveChanges();

                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }
    }
}
