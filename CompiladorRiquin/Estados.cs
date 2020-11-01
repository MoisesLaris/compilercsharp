using System;
namespace CompiladorRiquin
{
    public enum Estados
    {
        Inicio,
        Fin,
        Id,
        Entero,
        Punto,
        Flotante,
        Mas,
        Menos,
        Mayor,
        Menor,
        DosPuntos,
        Diagonal,
        CometarioLinea,
        AsteriscoAbrir,
        AsteriscoCerrar,
        Diferente,
        Comparacion
    }

    public enum TipoToken
    {
        ID,
        ID_RESERVED,
        NUM,
        OTHER
    }
}
