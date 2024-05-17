using System;
using Dominio;

namespace Dominio
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public int Quantidade { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataEsgotado { get; set; }

        public List<Evento<Produto>> Eventos { get; } = [];

        private Produto() {}

        public Produto(string descricao, decimal preco, int quantidade)
        {
            this.AplicaEvento(new ProdutoCadastrado(descricao, preco, quantidade));
        }

        
        public static Produto Reconstroi(List<Evento<Produto>> eventos)
        {
            return eventos.Aggregate(new Produto(), (produto, evento) =>
            {
                produto.AplicaEvento(evento);
                return produto;
            });
        }
        

        public void RegistraVenda(int quantidade)
        {
            this.AplicaEvento(new ProdutoVendido(this, quantidade));

            if (this.Quantidade == 0) {
                this.AplicaEvento(new ProdutoEsgotado(this));
            }
        }

        public void ReajustaPreco(decimal novoPreco) {
            this.AplicaEvento(new PrecoProdutoReajustado(this, novoPreco));
        }

        public void AplicaEvento(Evento<Produto> evento)
        {
            evento.AplicaEm(this);
            this.Eventos.Add(evento);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Descricao: {Descricao}, Preco: {Preco}, Quantidade: {Quantidade}, DataCadastro: {DataCadastro}, DataEsgotado: {DataEsgotado}";
        }
        
    }

   
    public class ProdutoCadastrado : Evento<Produto>
    {
        public Guid EntidadeId { get; }
        public DateTime Data { get; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public ProdutoCadastrado(string descricao, decimal preco, int quantidade)
        {
            this.EntidadeId = Guid.NewGuid();
            this.Data = DateTime.UtcNow;

            this.Preco = preco;
            this.Descricao = descricao;
            this.Quantidade = quantidade;
        }

        public void AplicaEm(Produto produto)
        {
            produto.Id = this.EntidadeId;
            produto.Descricao = this.Descricao;
            produto.Preco = this.Preco;
            produto.DataCadastro = this.Data;
            produto.Quantidade = this.Quantidade;
            produto.DataEsgotado = null;
        }
    }
    
    public class ProdutoVendido : Evento<Produto>
    {
        public Guid EntidadeId { get; }
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }

        public ProdutoVendido(Produto produto, int quantidade)
        {
            this.EntidadeId = produto.Id;
            this.Data = DateTime.UtcNow;
            this.Quantidade = quantidade;
            this.Preco = produto.Preco;
        }

        public void AplicaEm(Produto produto)
        {
            produto.Quantidade -= this.Quantidade;
        }
    }

    public class PrecoProdutoReajustado : Evento<Produto>
    {
        public Guid EntidadeId { get; }
        public decimal PrecoAntigo { get; }
        public decimal NovoPreco { get; set; }
        public DateTime Data { get; set; }

        public PrecoProdutoReajustado(Produto produto, decimal novoPreco)
        {
            this.EntidadeId = produto.Id;
            this.PrecoAntigo = produto.Preco;
            this.NovoPreco = novoPreco;
            this.Data = DateTime.UtcNow;
        }

        public void AplicaEm(Produto produto)
        {
            produto.Preco = this.NovoPreco;
        }
    }

    public class ProdutoEsgotado : Evento<Produto>
    {
        public Guid EntidadeId { get; }
        public DateTime Data { get; set; }

        public ProdutoEsgotado(Produto produto)
        {
            this.EntidadeId = produto.Id;
            this.Data = DateTime.UtcNow;
        }

        public void AplicaEm(Produto produto)
        {
            produto.DataEsgotado = this.Data;
            produto.Quantidade = 0;
        }
    }
    
}