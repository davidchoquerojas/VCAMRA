using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidaCamara.DIS.Modelo.Enumerado
{
    public enum EstadoPago : int
    {
        Ingresado = 1,
        Validado = 2,
        Aprobado = 3,
        Informado = 5,
        Efectuado = 6,
        Contabilizado = 4,
        Rechazado = 0
    }
}
