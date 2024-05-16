using Dominio;

var produto = new Produto("Relógio" , (decimal) 500.0, 50);
Console.WriteLine(produto.ToString());

produto.RegistraVenda(40);
Console.WriteLine(produto.ToString()); // quantidade 10

produto.ReajustaPreco((decimal) 650.0);
Console.WriteLine(produto.ToString()); // preço 650

produto.RegistraVenda(10);
Console.WriteLine(produto.ToString()); // quantidade 0 esgotado hoje

var repositorio = new ProdutoRepositorio();
repositorio.Salva(produto);

var produtoSalvo = repositorio.BuscaPorId(produto.Id);
Console.WriteLine("\n===========================");
Console.WriteLine(produtoSalvo?.ToString());

Console.WriteLine("\n===========================");
Console.WriteLine($"Mesma instância? {produto == produtoSalvo}");
Console.WriteLine($"Mesmo estado? {produto.ToString() == produtoSalvo?.ToString()}");