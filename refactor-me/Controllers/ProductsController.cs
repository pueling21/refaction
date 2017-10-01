using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [Route]
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            List<Product> products = new Product().GetProducts();

            if (products == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return products;
        }

        [Route]
        [HttpGet]
        public IEnumerable<Product> SearchByName(string name)
        {
            List<Product> products = new Product().GetProducts();
            var product = products.Where((p) => p.Name.ToLower().Contains(name.ToLower()));

            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return product;
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            List<Product> products = new Product().GetProducts();
            var product = products.FirstOrDefault((p) => p.Id == id);

            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Ok(product);
        }

        [Route]
        [HttpPost]
        public string Create(Product product)
        {
            try
            {
                product.Save();

                return "Saved successfully.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
            
        }

        [Route("{id}")]
        [HttpPut]
        public string Update(Guid id, Product product)
        {
            try
            {
                var orig = new Product(id)
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    DeliveryPrice = product.DeliveryPrice
                };

                if (!orig.IsNew)
                    orig.Save();

                return "Updated successfully.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
            
        }

        [Route("{id}")]
        [HttpDelete]
        public string Delete(Guid id)
        {
            try
            {
                var product = new Product(id);
                product.Delete();

                return "Deleted successfully.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
            
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            var options = new ProductOptions(productId);
            if(options == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(options);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Ok(option);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public string CreateOption(Guid productId, ProductOption option)
        {
            try
            {
                option.ProductId = productId;
                option.Save();

                return "Saved successfully.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
            
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public string UpdateOption(Guid id, ProductOption option)
        {
            try
            {
                var orig = new ProductOption(id)
                {
                    Name = option.Name,
                    Description = option.Description
                };

                if (!orig.IsNew)
                    orig.Save();

                return "Updated successfully.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
            
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public string DeleteOption(Guid id)
        {
            try
            {
                var opt = new ProductOption(id);
                opt.Delete();

                return "Deleted successfully.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
            
        }
    }
}
