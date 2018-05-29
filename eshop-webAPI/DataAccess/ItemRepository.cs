﻿using eshopAPI.Models;
using eshopAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshopAPI.DataAccess
{
    public interface IItemRepository : IBaseRepository
    {
        Task<Item> FindByID(long itemID);
        Task ArchiveByIDs(List<long> ids);
        Task UnarchiveByIDs(List<long> ids);
        Task<Item> Insert(Item item);
        Task<IQueryable<AdminItemVM>> GetAllAdminItemVMAsQueryable();
        Task<IQueryable<ItemVM>> GetAllItemsForFirstPageAsQueryable();
    }

    public class ItemRepository : BaseRepository, IItemRepository
    {
        public ItemRepository(ShopContext context) : base(context)
        {
        }

        public Task<IQueryable<AdminItemVM>> GetAllAdminItemVMAsQueryable()
        {
            var query =
                Context.Items
                .Include(x => x.SubCategory)
                .Select(x => new AdminItemVM()
                {
                    Category = x.SubCategory.Name,
                    Name = x.Name,
                    ID = x.ID,
                    Description = x.Description,
                    Price = x.Price,
                    SKU = x.SKU,
                    IsDeleted = x.IsDeleted
                });
            return Task.FromResult(query);
        }

        public Task<Item> FindByID(long itemID)
        {
            return Context.Items.Where(i => i.ID == itemID)
                .Include(i => i.Pictures)
                .Include(i => i.Attributes).ThenInclude(a => a.Attribute)
                .FirstOrDefaultAsync();
        }

        public Task<Item> Insert(Item item)
        {
            return Task.FromResult(Context.Items.Add(item).Entity);
        }
        
        public Task<IQueryable<ItemVM>> GetAllItemsForFirstPageAsQueryable()
        {
            var query = Context.Items
                .Select(i => new ItemVM
                {
                    ID = i.ID,
                    SKU = i.SKU,
                    Name = i.Name,
                    Price = i.Price,
                    Description = i.Description,
                    Pictures = i.Pictures.Select(p => new ItemPictureVM { ID = p.ID, URL = p.URL }),
                    Attributes = i.Attributes.Select(a => new ItemAttributesVM
                    {
                        ID = a.ID,
                        AttributeID = a.AttributeID,
                        Name = a.Attribute.Name,
                        Value = a.Value
                    }),
                    ItemCategory = new ItemCategoryVM
                    {
                        Name = i.SubCategory.Category.Name,
                        ID = i.SubCategory.CategoryID,
                        SubCategory = new ItemSubCategoryVM
                        {
                            ID = i.SubCategoryID.Value,
                            Name = i.SubCategory.Name
                        }
                    }
                });
            return Task.FromResult(query);
        }

        public async Task ArchiveByIDs(List<long> ids)
        {
            await Context.Items
                .Where(x => ids.Contains(x.ID))
                .ForEachAsync(x =>
                {
                    x.IsDeleted = true;
                    x.DeleteDate = DateTime.Now;
                });
        }

        public async Task UnarchiveByIDs(List<long> ids)
        {
            await Context.Items
                .Where(x => ids.Contains(x.ID))
                .ForEachAsync(x =>
                {
                    x.IsDeleted = false;
                });
        }
    }
}
