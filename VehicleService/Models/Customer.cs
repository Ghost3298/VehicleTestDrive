using System;
using System.Collections.Generic;

namespace VehicleServiceAPI.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? MobileNumber { get; set; }
}
