using System;
using System.Collections.Generic;
using Dominio;

namespace Dominio
{
    public class ProdutoRepositorio
    {
        private readonly Dictionary<Guid, Produto> produtos = [];

        private readonly Dictionary<Guid, List<Evento>> EventosSalvos = [];


        public List<Produto> ListaTodos()
        {
            return EventosSalvos
                .Select(entry => Produto.Reconstroi(entry.Value))
                .ToList();
        }

        public Produto? BuscaPorId(Guid id)
        {
            if (!EventosSalvos.ContainsKey(id))
            {
                return null;
            }

            return Produto.Reconstroi(EventosSalvos[id]);
        }

        public void Salva(Produto produto)
        {
            EventosSalvos[produto.Id] = produto.Eventos;
        }
    }
}