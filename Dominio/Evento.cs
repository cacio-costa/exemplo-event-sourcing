using System;

namespace Dominio
{
    public interface Evento<T>
    {
        Guid EntidadeId { get; }
        DateTime Data  { get; }

        void AplicaEm(T entidade) {}
    }
}