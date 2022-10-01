using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Common.Models
{
    public class CategoryModel : BaseModel, IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}
