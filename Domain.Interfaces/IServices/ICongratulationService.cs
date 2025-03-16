namespace Domain.Interfaces.IServices;

public interface ICongratulationService
{
    Task<IEnumerable<(long? ChatId, string Text)>> GetCongratulationsAsync();

}