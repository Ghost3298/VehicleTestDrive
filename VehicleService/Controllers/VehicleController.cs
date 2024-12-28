using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;


    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }


    [HttpGet]
    public IActionResult GetVehicles()
    {
        var vehicles = _vehicleService.GetVehicles();
        return Ok(vehicles);
    }



    [Authorize(Roles = "admin")]
    [HttpGet("admin-resource")]
    public IActionResult GetAdminResource()
    {
        var claims = User.Claims;
        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        return Ok("This is an admin resource.");
    }



    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult CreateVehicle([FromBody] Vehicle vehicle)
    {
        _vehicleService.AddVehicle(vehicle);
        return CreatedAtAction(nameof(GetVehicles), new { id = vehicle.Id }, vehicle);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public IActionResult UpdateVehicle(int id, [FromBody] Vehicle vehicle)
    {
        _vehicleService.UpdateVehicle(id, vehicle);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteVehicle(int id)
    {
        _vehicleService.DeleteVehicle(id);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public IActionResult GetVehicle(int id)
    {
        var vehicle = _vehicleService.GetVehicleById(id);
        return Ok(vehicle);
    }
}
