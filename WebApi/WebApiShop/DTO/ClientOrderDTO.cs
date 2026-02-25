using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record ClientOrderDTO
    (
        int OrderId,
        DateOnly OrderDate,
        int OrderSum,
        int ProductId,
        string ProductName,
        string ImgUrl,
        bool CurrentStatus
    );
}
