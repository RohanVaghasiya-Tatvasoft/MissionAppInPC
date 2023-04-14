using System;
using System.Collections.Generic;

namespace MissionApp.Entities.Models;

public partial class ContactUs
{
    public int ContactUsId { get; set; }

    public int UserId { get; set; }

    public string? Message { get; set; }

    public string? Subject { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
