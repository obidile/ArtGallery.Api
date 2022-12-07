﻿using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models;

public class PaymentModel : BaseModel, IMapFrom<Payment>
{
    public int Amount { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public bool Status { get; set; }
}
