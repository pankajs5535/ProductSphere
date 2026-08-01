using AutoMapper;
using ProductSphere.Application.DTOs.ItemDtos;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Application.Interfaces.IServices;
using ProductSphere.Domain.Entities;
using ProductSphere.Domain.Exceptions; // Glo_Exc

namespace ProductSphere.Infrastructure.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Items.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<ItemDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"Item with Id {id} was not found."); // Glo_Exc

            return _mapper.Map<ItemDto>(item);
        }

        public async Task CreateAsync(CreateItemDto dto)
        {
            var item = _mapper.Map<Item>(dto);

            await _unitOfWork.Items.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateItemDto dto)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(dto.Id);

            if (item == null)
                throw new NotFoundException($"Item with Id {dto.Id} was not found."); // Glo_Exc

            _mapper.Map(dto, item);

            _unitOfWork.Items.Update(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"Item with Id {id} was not found."); // Glo_Exc

            _unitOfWork.Items.Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}