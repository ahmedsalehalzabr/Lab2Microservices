using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WarehouseService.Controllers;

[ApiController]
[Route("warehouse/items")]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseDbContext _db;

    public WarehouseController(WarehouseDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Items.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Item item)
    {
        item.Id = Guid.NewGuid();
        _db.Items.Add(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }
} 
