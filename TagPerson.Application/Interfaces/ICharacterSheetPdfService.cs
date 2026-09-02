using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ICharacterSheetPdfService
{
    byte[] Generate(CharacterSheetDto sheet);
}