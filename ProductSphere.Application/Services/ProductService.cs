using AutoMapper;
using ProductSphere.Application.DTOs.ProductDtos;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Application.Interfaces.IServices;
using ProductSphere.Domain.Entities;

namespace ProductSphere.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task CreateAsync(CreateProductDto createProductDto)
        {
            if (await _unitOfWork.Products.ExistsByNameAsync(createProductDto.ProductName))
                throw new Exception("Product already exists.");

            var product = _mapper.Map<Product>(createProductDto);
            product.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(updateProductDto.Id);

            if (product == null)
                throw new Exception("Product not found.");

            _mapper.Map(updateProductDto, product);
            product.ModifiedOn = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found.");

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}