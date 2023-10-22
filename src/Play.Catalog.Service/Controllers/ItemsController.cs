using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;



namespace Play.Catalog.Service.Controllers {

   [ApiController]
   [Route("items")]
    public class ItemsController : ControllerBase 
    {
      private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Id: Guid.NewGuid(), Name: "Potion", Description: "Restores a small amount of HP", Price: 5, CreatedDate: DateTimeOffset.UtcNow),
            new ItemDto(Id: Guid.NewGuid(), Name: "Bronze sword", Description: "Deals a small amount of damage", Price: 5, CreatedDate: DateTimeOffset.UtcNow),
            new ItemDto(Id: Guid.NewGuid(), Name: "Bronze Shield", Description: "Adds a small shield bonus", Price: 3, CreatedDate: DateTimeOffset.UtcNow),
        };


      [HttpGet]
      public IEnumerable<ItemDto> Get() 
      {
        return items;
      }

      [HttpGet("{id}")]
      public ActionResult<ItemDto> GetById(Guid id) 
      {
        var item = items.Where(item => item.Id == id).SingleOrDefault();
        if (item == null) {
          return NotFound();
        }
        return item;
      }

      [HttpPost]
      public ActionResult<ItemDto> Post(CreateItemDto CreateItemDto) {
        var item = new ItemDto(Guid.NewGuid(), CreateItemDto.Name, CreateItemDto.Description, CreateItemDto.Price, DateTimeOffset.UtcNow);
        items.Add(item);
        return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
      }

      [HttpPut("{id}")]
      public ActionResult Put(Guid id, UpdateItemDto updateItemDto) {
        var existingItem = items.Where(item => item.Id == id).FirstOrDefault();
        if (existingItem == null) {
          return NotFound();
        }
        var updatedItem = existingItem with {
          Name = updateItemDto.Name,
          Description = updateItemDto.Description,
          Price = updateItemDto.Price,
        };

        if (updatedItem == null) {
          return NotFound();
        }

        var index = items.FindIndex(item => item.Id == id);

        items[index] = updatedItem;

        return NoContent();

      }

      [HttpDelete("{id}")]
      public ActionResult Delete(Guid id) {
        var index = items.FindIndex(existingItem => existingItem.Id == id);
        if (index == -1) {
          return NotFound();
        }

        items.RemoveAt(index);
        return NoContent();
      }
    }
 }