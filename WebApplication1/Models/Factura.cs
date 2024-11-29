using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Factura
{
    public int Id { get; set; }

    public int IdPago { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public DateTime? FechaFactura { get; set; }

    public decimal Monto { get; set; }

    public virtual Pago IdPagoNavigation { get; set; } = null!;
}
