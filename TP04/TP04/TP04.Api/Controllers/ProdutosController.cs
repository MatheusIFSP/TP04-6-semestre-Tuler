using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP04.Api.Data;
using TP04.Api.Models;
using TP04.Biblioteca;

namespace TP04.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController(AppDbContext contexto) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Listar() =>
        Ok(await contexto.Produtos.AsNoTracking().OrderBy(p => p.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Produto>> ObterPorId(int id)
    {
        var produto = await contexto.Produtos.FindAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpGet("{id:int}/valor-estoque")]
    public async Task<ActionResult<ValorEstoqueDto>> ObterValorEstoque(int id)
    {
        var produto = await contexto.Produtos.FindAsync(id);
        if (produto is null) return NotFound();

        var total = CalculadoraEstoque.ValorTotal(produto.Preco, produto.Quantidade);
        return Ok(new ValorEstoqueDto(total, CalculadoraEstoque.FormatarMoeda(total)));
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> Criar(ProdutoDto dto)
    {
        var erros = ValidadorProduto.Validar(dto.Nome, dto.Preco, dto.Quantidade);
        if (erros.Count > 0) return BadRequest(new { erros });

        var produto = new Produto
        {
            Nome = dto.Nome!.Trim(),
            Descricao = dto.Descricao?.Trim() ?? string.Empty,
            Preco = dto.Preco,
            Quantidade = dto.Quantidade
        };

        contexto.Produtos.Add(produto);
        await contexto.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Produto>> Atualizar(int id, ProdutoDto dto)
    {
        var produto = await contexto.Produtos.FindAsync(id);
        if (produto is null) return NotFound();

        var erros = ValidadorProduto.Validar(dto.Nome, dto.Preco, dto.Quantidade);
        if (erros.Count > 0) return BadRequest(new { erros });

        produto.Nome = dto.Nome!.Trim();
        produto.Descricao = dto.Descricao?.Trim() ?? string.Empty;
        produto.Preco = dto.Preco;
        produto.Quantidade = dto.Quantidade;

        await contexto.SaveChangesAsync();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var produto = await contexto.Produtos.FindAsync(id);
        if (produto is null) return NotFound();

        contexto.Produtos.Remove(produto);
        await contexto.SaveChangesAsync();

        return NoContent();
    }
}
