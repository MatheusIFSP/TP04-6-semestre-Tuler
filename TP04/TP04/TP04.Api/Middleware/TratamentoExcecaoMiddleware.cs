namespace TP04.Api.Middleware;

public class TratamentoExcecaoMiddleware(RequestDelegate proximo, ILogger<TratamentoExcecaoMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await proximo(contexto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado em {Metodo} {Caminho}", contexto.Request.Method, contexto.Request.Path);
            contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await contexto.Response.WriteAsJsonAsync(new { erro = "Ocorreu um erro interno ao processar a requisição." });
        }
    }
}
