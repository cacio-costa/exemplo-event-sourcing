using System;

namespace Dominio
{
    public interface Evento
    {
        Guid EntidadeId { get; }
        DateTime Data  { get; }
    }
}