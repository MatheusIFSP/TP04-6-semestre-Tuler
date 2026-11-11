using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP04.Api.Controllers;
using TP04.Api.Data;
using TP04.Api.Models;

namespace TP04.Tests;

public class ProdutosControllerTests
{
    private static ProdutosController CriarController()
    {
        var opcoes = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ProdutosController(new AppDbContext(opcoes));
    }

    private static ProdutoDto ProdutoValido() => new("Teclado", "Teclado mecânico", 250.90m, 10);

    private static async Task<Produto> Cadastrar(ProdutosController controller)
    {
        var resultado = await controller.Criar(ProdutoValido());
        return (Produto)((CreatedAtActionResult)resultado.Result!).Value!;
    }

    [Fact]
    public async Task Criar_DadosValidos_RetornaCreatedComIdGerado()
    {
        var controller = CriarController();

        var resultado = await controller.Criar(ProdutoValido());

        var created = Assert.IsType<CreatedAtActionResult>(resultado.Result);
        var produto = Assert.IsType<Produto>(created.Value);
        Assert.True(produto.Id > 0);
        Assert.Equal("Teclado", produto.Nome);
    }

    [Fact]
    public async Task Criar_NomeVazio_RetornaBadRequest()
    {
        var controller = CriarController();

        var resultado = await controller.Criar(new ProdutoDto("", "", 10m, 1));

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Criar_PrecoZero_RetornaBadRequest()
    {
        var controller = CriarController();

        var resultado = await controller.Criar(new ProdutoDto("Mouse", "", 0m, 1));

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Criar_QuantidadeNegativa_RetornaBadRequest()
    {
        var controller = CriarController();

        var resultado = await controller.Criar(new ProdutoDto("Mouse", "", 10m, -1));

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Listar_RetornaProdutosCadastrados()
    {
        var controller = CriarController();
        await Cadastrar(controller);
        await Cadastrar(controller);

        var resultado = await controller.Listar();

        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        var produtos = Assert.IsAssignableFrom<IEnumerable<Produto>>(ok.Value);
        Assert.Equal(2, produtos.Count());
    }

    [Fact]
    public async Task ObterPorId_Existente_RetornaOk()
    {
        var controller = CriarController();
        var cadastrado = await Cadastrar(controller);

        var resultado = await controller.ObterPorId(cadastrado.Id);

        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        Assert.Equal(cadastrado.Id, ((Produto)ok.Value!).Id);
    }

    [Fact]
    public async Task ObterPorId_Inexistente_RetornaNotFound()
    {
        var controller = CriarController();

        var resultado = await controller.ObterPorId(999);

        Assert.IsType<NotFoundResult>(resultado.Result);
    }

    [Fact]
    public async Task Atualizar_Existente_AlteraDados()
    {
        var controller = CriarController();
        var cadastrado = await Cadastrar(controller);

        var resultado = await controller.Atualizar(cadastrado.Id, new ProdutoDto("Teclado Gamer", "RGB", 400m, 5));

        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        var atualizado = Assert.IsType<Produto>(ok.Value);
        Assert.Equal("Teclado Gamer", atualizado.Nome);
        Assert.Equal(400m, atualizado.Preco);
        Assert.Equal(5, atualizado.Quantidade);
    }

    [Fact]
    public async Task Atualizar_Inexistente_RetornaNotFound()
    {
        var controller = CriarController();

        var resultado = await controller.Atualizar(999, ProdutoValido());

        Assert.IsType<NotFoundResult>(resultado.Result);
    }

    [Fact]
    public async Task Atualizar_DadosInvalidos_RetornaBadRequest()
    {
        var controller = CriarController();
        var cadastrado = await Cadastrar(controller);

        var resultado = await controller.Atualizar(cadastrado.Id, new ProdutoDto(" ", "", 10m, 1));

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Excluir_Existente_RetornaNoContentERemove()
    {
        var controller = CriarController();
        var cadastrado = await Cadastrar(controller);

        var resultado = await controller.Excluir(cadastrado.Id);

        Assert.IsType<NoContentResult>(resultado);
        Assert.IsType<NotFoundResult>((await controller.ObterPorId(cadastrado.Id)).Result);
    }

    [Fact]
    public async Task Excluir_Inexistente_RetornaNotFound()
    {
        var controller = CriarController();

        var resultado = await controller.Excluir(999);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact]
    public async Task ObterValorEstoque_CalculaPrecoVezesQuantidade()
    {
        var controller = CriarController();
        var cadastrado = await Cadastrar(controller);

        var resultado = await controller.ObterValorEstoque(cadastrado.Id);

        var ok = Assert.IsType<OkObjectResult>(resultado.Result);
        var valor = Assert.IsType<ValorEstoqueDto>(ok.Value);
        Assert.Equal(2509.00m, valor.ValorTotal);
    }

    [Fact]
    public async Task ObterValorEstoque_Inexistente_RetornaNotFound()
    {
        var controller = CriarController();

        var resultado = await controller.ObterValorEstoque(999);

        Assert.IsType<NotFoundResult>(resultado.Result);
    }
}
