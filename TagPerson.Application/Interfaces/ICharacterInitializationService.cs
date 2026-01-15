using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces;

/// <summary>
/// Interface para o serviço de inicialização de personagens.
/// </summary>
public interface ICharacterInitializationService
{
    /// <summary>
    /// Inicializa um personagem com atributos, habilidades e magias baseado na raça e profissão.
    /// </summary>
    /// <param name="character">O personagem a ser inicializado</param>
    /// <param name="ct">Token de cancelamento</param>
    /// <returns>Uma tarefa que representa a operação assíncrona</returns>
    Task InitializeCharacterAsync(Character character, CancellationToken ct);
}
